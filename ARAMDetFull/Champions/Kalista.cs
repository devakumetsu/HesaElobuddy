using System;
using System.Collections.Generic;
using System.Linq;
using EloBuddy;
using EloBuddy.SDK;
using EloBuddy.SDK.Enumerations;

namespace ARAMDetFull.Champions
{
    class Kalista : Champion
    {
        public AIHeroClient CoopStrikeAlly;
        public float CoopStrikeAllyRange = 1250f;

        public Kalista()
        {
            Orbwalker.OnPostAttack += onAfterAttack;

            ARAMSimulator.champBuild = new Build
            {
                coreItems = new List<ConditionalItem>
                        {
                            new ConditionalItem(ItemId.Runaans_Hurricane_Ranged_Only),
                            new ConditionalItem(ItemId.Berserkers_Greaves),
                            new ConditionalItem(ItemId.Blade_of_the_Ruined_King),
                            new ConditionalItem(ItemId.The_Bloodthirster),
                            new ConditionalItem(ItemId.Guinsoos_Rageblade),
                            new ConditionalItem(ItemId.Infinity_Edge),
                        },
                startingItems = new List<ItemId>
                        {
                            ItemId.Vampiric_Scepter,ItemId.Boots_of_Speed
                        }
            };
        }

        private void onAfterAttack(AttackableUnit unit, EventArgs args)
        {
            if (unit.IsMe && ARAMSimulator.awayTo.IsValid())
            {
                Player.IssueOrder(GameObjectOrder.MoveTo, ARAMSimulator.awayTo.To3D());
            }
        }


        public override void useQ(Obj_AI_Base target)
        {
            if (!Q.IsReady())
                return;
            if (!Sector.inTowerRange(target.Position.To2D()) &&
                (MapControl.balanceAroundPoint(target.Position.To2D(), 700) >= -1 ||
                 (MapControl.fightIsOn() != null && MapControl.fightIsOn().NetworkId == target.NetworkId)))
                Q.Cast(target);
        }

        public override void useW(Obj_AI_Base target)
        {
            if (!W.IsReady())
                return;
            W.Cast();
        }

        public override void useE(Obj_AI_Base target)
        {
            if (!E.IsReady() || W.IsReady())
                return;
            E.Cast();
        }

        public override void useR(Obj_AI_Base target)
        {
            if (!R.IsReady())
                return;
            R.Cast(target);
        }

        public override void setUpSpells()
        {
            Q = new Spell.Skillshot(SpellSlot.Q, 1150, SkillShotType.Linear, 250, 2100, 40);
            W = new Spell.Targeted(SpellSlot.W, 5000);
            E = new Spell.Active(SpellSlot.E, 1000);
            R = new Spell.Active(SpellSlot.R, 1100);
        }

        public override void useSpells()
        {
            if (CoopStrikeAlly == null)
            {
                if (Item.HasItem(ItemId.The_Black_Spear))
                {
                    var targ = ObjectManager.Get<AIHeroClient>()
                        .Where(h => h.IsAlly && h.IsValid)
                        .OrderBy(h => player.Distance(h, true))
                        .FirstOrDefault();
                    if (targ != null)
                        Item.UseItem(ItemId.The_Black_Spear, targ);
                }

                foreach (
                    var ally in
                        from ally in ObjectManager.Get<AIHeroClient>().Where(tx => tx.IsAlly && !tx.IsDead && !tx.IsMe)
                        where ObjectManager.Player.Distance(ally) <= CoopStrikeAllyRange
                        from buff in ally.Buffs
                        where buff.Name.Contains("kalistacoopstrikeally")
                        select ally)
                {
                    CoopStrikeAlly = ally;
                }
            }

            AIHeroClient t;

            if (Q.IsReady())
            {
                t = ARAMTargetSelector.getBestTarget(Q.Range);
                if (t != null)
                    Q.Cast(t);
            }

            if (E.IsReady())
            {
                foreach (var targ in EntityManager.Heroes.Enemies.Where(o => o.IsValidTarget(E.Range) && !o.IsDead))
                {
                    if (targ.Health < player.GetSpellDamage(targ, SpellSlot.E))
                    {
                        E.Cast();
                    }
                }
                var deadMins =
                    EntityManager.MinionsAndMonsters.GetLaneMinions(EntityManager.UnitTeam.Enemy, Player.Instance.Position, E.Range)
                        .Count(o => o.IsValidTarget(E.Range) && !o.IsDead && E.GetDamage(o) > o.Health);

                if (deadMins > 1)
                    E.Cast();
            }

            if (!R.IsReady()) return;
            t = ARAMTargetSelector.getBestTarget(R.Range);
            if (t != null)
                R.Cast(t);
        }



        public int KalistaMarkerCount
        {
            get
            {
                var xbuffCount = 0;
                foreach (
                    var buff in from enemy in ObjectManager.Get<AIHeroClient>().Where(tx => tx.IsEnemy && !tx.IsDead)
                                where ObjectManager.Player.Distance(enemy) < E.Range
                                from buff in enemy.Buffs
                                where buff.Name.Contains("kalistaexpungemarker")
                                select buff)
                {
                    xbuffCount = buff.Count;
                }
                return xbuffCount;
            }
        }

        private float GetEDamage(Obj_AI_Base t)
        {
            if (!E.IsReady())
                return 0f;
            return ObjectManager.Player.GetSpellDamage(t, SpellSlot.E);

            /* I think this calculation working good but i cant check now. after I'll do */
            var buff = t.Buffs.FirstOrDefault(xBuff => xBuff.DisplayName.ToLower() == "kalistaexpungemarker");
            if (buff.Count == 0)
                return 0f;

            float damage = ObjectManager.Player.FlatPhysicalDamageMod + ObjectManager.Player.BaseAttackDamage;
            double eDmg = damage * 0.60 + new double[] { 0, 20, 30, 40, 50, 60 }[E.Level];

            if (buff.Count == 1)
                return (float)eDmg;

            damage += buff.Count * 0.003f * damage + (float) eDmg;
            return ObjectManager.Player.CalculateDamageOnUnit(t, DamageType.Physical, damage);
        }
    }
}