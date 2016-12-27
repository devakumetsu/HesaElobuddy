using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EloBuddy;
using EloBuddy.SDK;

namespace ARAMDetFull.Champions
{
    class Kayle : Champion
    {
        public Kayle()
        {
            Obj_AI_Base.OnSpellCast += HealUltTrigger;

            ARAMSimulator.champBuild = new Build
            {
                coreItems = new List<ConditionalItem>
                {
                    new ConditionalItem(ItemId.Nashors_Tooth),
                    new ConditionalItem(ItemId.Berserkers_Greaves),
                    new ConditionalItem(ItemId.Guinsoos_Rageblade),
                    new ConditionalItem(ItemId.Rabadons_Deathcap),
                    new ConditionalItem(ItemId.Lich_Bane),
                    new ConditionalItem(ItemId.Zhonyas_Hourglass),
                },
                startingItems = new List<ItemId>
                {
                    ItemId.Fiendish_Codex,ItemId.Boots_of_Speed
                }
            };
        }

        void HealUltTrigger(Obj_AI_Base sender, GameObjectProcessSpellCastEventArgs args)
        {
            var target = args.Target as AIHeroClient;
            var senderhero = sender as AIHeroClient;
            var senderturret = sender as Obj_AI_Turret;

            if (sender.IsAlly || (target == null) || !target.IsAlly)
            {
                return;
            }
            float setvaluehealth = 55;
            float setvalueult = 26;

            bool triggered = false;

            if (W.IsReady() && (target.HealthPercent <= setvaluehealth))
            {
                HealUltManager(true, false, target);
                triggered = true;
            }
            if (R.IsReady() && (target.HealthPercent <= setvaluehealth))
            {
                HealUltManager(false, true, target);
                triggered = true;
            }

            if (triggered)
            {
                return;
            }
            
            var damage = player.GetSpellDamage(target, args.Slot);
            var afterdmg = ((target.Health - damage) / (target.MaxHealth)) * 100f;

            if (W.IsReady() && player.Distance(target) <= W.Range && (target.HealthPercent <= setvaluehealth || (afterdmg <= setvaluehealth)))
            {
                HealUltManager(true, false, target);
            }

            if (R.IsReady() && player.Distance(target) <= R.Range && (target.HealthPercent <= setvalueult || (afterdmg <= setvalueult)) && (senderhero != null || senderturret != null || target.HealthPercent < 5f))
            {
                HealUltManager(false, true, target);
            }

        }

        void HealUltManager(bool forceheal = false, bool forceult = false, AIHeroClient target = null)
        {
            if (forceheal && target != null && W.IsReady() && player.Distance(target) <= W.Range)
            {
                W.CastOnUnit(target);
                return;
            }
            if (forceult && target != null && R.IsReady() && player.Distance(target) <= R.Range)
            {
                R.CastOnUnit(target);
                return;
            }

            var herolistheal = ObjectManager.Get<AIHeroClient>()
                            .Where(
                                h =>
                                    (h.IsAlly || h.IsMe) && !h.IsZombie && !h.IsDead &&
                                    h.HealthPercent <= 55 && player.Distance(h) <= R.Range).OrderByDescending(i => i == player).ThenBy(i => i);

            if (W.IsReady())
            {
                if (herolistheal.Contains(player) && !player.IsRecalling())// && !player.InFountain())
                {
                    W.CastOnUnit(player);
                    return;
                }
                else if (herolistheal.Any())
                {
                    var hero = herolistheal.FirstOrDefault();

                    if (player.Distance(hero) <= R.Range && !player.IsRecalling() && !hero.IsRecalling())// && !hero.InFountain())
                    {
                        W.CastOnUnit(hero);
                        return;
                    }
                }
            }

            var herolist = ObjectManager.Get<AIHeroClient>()
                .Where(
                    h =>
                        (h.IsAlly || h.IsMe) && !h.IsZombie && !h.IsDead &&
                        h.HealthPercent <= 35 &&
                        player.Distance(h) <= R.Range && player.CountEnemiesInRange(500) > 0).OrderByDescending(i => i == player).ThenBy(i => i);

            if (R.IsReady())
            {
                if (herolist.Contains(player))
                {
                    R.CastOnUnit(player);
                    return;
                }

                else if (herolist.Any())
                {
                    var hero = herolist.FirstOrDefault();

                    if (player.Distance(hero) <= R.Range)
                    {
                        R.CastOnUnit(hero);
                        return;
                    }
                }
            }
        }

        public override void useQ(Obj_AI_Base target)
        {
            Q.CastOnUnit(target);
        }

        public override void useW(Obj_AI_Base target)
        {
        }

        public override void useE(Obj_AI_Base target)
        {
            if (E.IsReady())
                E.Cast();
        }


        public override void useR(Obj_AI_Base target)
        {
            if (!R.IsReady() || target == null) return;
        }

        public override void useSpells()
        {
            if (E.IsReady() && player.CountEnemiesInRange(650) > 0)
                E.Cast();
            var tarq = ARAMTargetSelector.getBestTarget(Q.Range);
            if (tarq != null) useQ(tarq);
            var tarw = ARAMTargetSelector.getBestTarget(W.Range);
            if (tarw != null) useW(tarw);
            //var tare = ARAMTargetSelector.getBestTarget(E.Range);
            //if (tare != null) useE(tare);
            var tarr = ARAMTargetSelector.getBestTarget(R.Range);
            if (tarr != null) useR(tarr);


        }

        public override void setUpSpells()
        {
            //Create the spells
            Q = new Spell.Targeted(SpellSlot.Q, 650)
            {
                CastDelay = 5
            };

            W = new Spell.Targeted(SpellSlot.W, 900);
            E = new Spell.Active(SpellSlot.E, 650);
            R = new Spell.Targeted(SpellSlot.R, 900);
        }

        public override void farm()
        {
            if (E.IsReady() && EntityManager.MinionsAndMonsters.EnemyMinions.Where(x => player.IsInRange(x, 700)).Count() > 2)
                E.Cast();
        }
    }
}