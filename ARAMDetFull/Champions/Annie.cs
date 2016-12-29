using EloBuddy;
using EloBuddy.SDK;
using EloBuddy.SDK.Enumerations;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ARAMDetFull.Champions
{
    class Annie : Champion
    {
        public Obj_AI_Base Tibbers { get; private set; }

        private bool haveStun
        {
            get
            {
                var buffs = player.Buffs.Where(buff => (buff.Name.ToLower() == "pyromania" || buff.Name.ToLower() == "pyromania_particle"));
                var buffInstances = buffs as BuffInstance[] ?? buffs.ToArray();
                if (buffInstances.Any())
                {
                    var buff = buffInstances.First();
                    if (buff.Name.ToLower() == "pyromania_particle")
                        return true;
                    else
                        return false;
                }
                return false;
            }
        }

        public Annie()
        {
            GameObject.OnCreate += NewTibbers;
            GameObject.OnDelete += DeleteTibbers;

            ARAMSimulator.champBuild = new Build
            {
                coreItems = new List<ConditionalItem>
                        {
                            new ConditionalItem(ItemId.Abyssal_Scepter),
                            new ConditionalItem(ItemId.Ionian_Boots_of_Lucidity),
                            new ConditionalItem(ItemId.Zhonyas_Hourglass),
                            new ConditionalItem(ItemId.Void_Staff),
                            new ConditionalItem(ItemId.Ludens_Echo),
                            new ConditionalItem(ItemId.Rabadons_Deathcap),
                        },
                startingItems = new List<ItemId>
                        {
                            ItemId.Fiendish_Codex
                        }
            };
            Chat.Print("Annie loaded.");
        }

        private void NewTibbers(GameObject sender, EventArgs args)
        {
            if (IsTibbers(sender))
            {
                Tibbers = (Obj_AI_Base)sender;
            }
        }

        private void DeleteTibbers(GameObject sender, EventArgs args)
        {
            if (IsTibbers(sender))
            {
                Tibbers = null;
            }
        }

        private static bool IsTibbers(GameObject sender)
        {
            return ((sender != null) && (sender.IsValid) && (sender.Name.ToLowerInvariant().Equals("tibbers")) && (sender.IsAlly));
        }

        public override void useQ(Obj_AI_Base target)
        {
            if (!Q.IsReady() || target == null)
                return;
            Q.Cast(target);
        }

        public override void useW(Obj_AI_Base target)
        {
            if (!W.IsReady())
                return;
            W.Cast(target);
        }

        public override void useE(Obj_AI_Base target)
        {
            if (!E.IsReady() || target == null || haveStun)
                return;
            E.Cast();
        }


        public override void useR(Obj_AI_Base target)
        {
            if (!R.IsReady() || target == null)
                return;
            var prediction = (R as Spell.Skillshot).GetPrediction(target);
            if(prediction.HitChance == HitChance.High)
            {
                if(haveStun || player.HealthPercent < 20)
                {
                    R.Cast(prediction.CastPosition);
                }
            }
        }

        public override void useSpells()
        {
            var tar = ARAMTargetSelector.getBestTarget(W.Range);
            if (tar != null) useW(tar);
            tar = ARAMTargetSelector.getBestTarget(Q.Range);
            if (tar != null) useQ(tar);
            tar = ARAMTargetSelector.getBestTarget(E.Range);
            if (tar != null) useE(tar);
            tar = ARAMTargetSelector.getBestTarget(R.Range);
            if (tar != null) useR(tar);
            if (Tibbers != null && tar != null)
            {
                tar = ARAMTargetSelector.getBestTarget(1500);
                if (Tibbers.Distance(tar.Position) > 200)
                {
                    Player.IssueOrder(GameObjectOrder.MovePet, tar);
                }
                else
                {
                    if (!Orbwalker.IsAutoAttacking || Orbwalker.GetTarget() != tar)
                        Player.IssueOrder(GameObjectOrder.AttackUnit, tar);
                }
            }

        }

        public override void setUpSpells()
        {
            //Create the spells
            Q = new Spell.Targeted(SpellSlot.Q, 625);
            W = new Spell.Skillshot(SpellSlot.W, 625, SkillShotType.Cone, 250, int.MaxValue, 210);
            E = new Spell.Active(SpellSlot.E);
            R = new Spell.Skillshot(SpellSlot.R, 600, SkillShotType.Circular, 50, int.MaxValue, 250);
        }
        
        public override void farm()
        {
            if(player.ManaPercent < 55 || !Q.IsReady())
                return;
            var minions = EntityManager.MinionsAndMonsters.EnemyMinions.Where(x => x.Type == GameObjectType.obj_AI_Minion && player.Distance(x) <= Q.Range);
            foreach (var minion in minions)
            {
                if (minion.Health > ObjectManager.Player.GetAutoAttackDamage(minion) && minion.Health < player.GetSpellDamage(minion, SpellSlot.Q))
                {
                    Q.Cast(minion);
                    return;
                }
            }
        }
    }
}