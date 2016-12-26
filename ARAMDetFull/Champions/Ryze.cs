using System.Collections.Generic;
using EloBuddy;
using EloBuddy.SDK;
using EloBuddy.SDK.Enumerations;

namespace ARAMDetFull.Champions
{
    class Ryze : Champion
    {
        public Ryze()
        {
            ARAMSimulator.champBuild = new Build
            {
                coreItems = new List<ConditionalItem>
                {
                    new ConditionalItem(ItemId.Rod_of_Ages),
                    new ConditionalItem(ItemId.Sorcerers_Shoes),
                    new ConditionalItem(ItemId.Archangels_Staff),
                    new ConditionalItem(ItemId.Spirit_Visage,ItemId.Frozen_Heart,ItemCondition.ENEMY_AP),
                    new ConditionalItem(ItemId.Rabadons_Deathcap),
                    new ConditionalItem(ItemId.Banshees_Veil),
                },
                startingItems = new List<ItemId>
                {
                    ItemId.Tear_of_the_Goddess,ItemId.Ruby_Crystal
                }
            };
        }


        public override void useQ(Obj_AI_Base target)
        {
            if (Q.IsReady())
                Q.Cast(target);
        }

        public override void useW(Obj_AI_Base target)
        {
            if (W.IsReady())
                W.Cast(target);
        }

        public override void useE(Obj_AI_Base target)
        {
            if (E.IsReady())
                E.CastOnUnit(target);
        }


        public override void useR(Obj_AI_Base target)
        {
            if (!R.IsReady() || target == null)
                return;
            R.Cast();
        }

        public override void useSpells()
        {
            var tar = ARAMTargetSelector.getBestTarget(Q.Range);
            if (tar != null) useQ(tar);
            tar = ARAMTargetSelector.getBestTarget(W.Range);
            if (tar != null) useW(tar);
            tar = ARAMTargetSelector.getBestTarget(E.Range);
            if (tar != null) useE(tar);
            //tar = ARAMTargetSelector.getBestTarget(R.Range);
            //if (tar != null) useR(tar);


        }

        public override void setUpSpells()
        {
            //Create the spells
            Q = new Spell.Skillshot(SpellSlot.Q, 1000, SkillShotType.Linear, 250, 1700, 60);
            W = new Spell.Targeted(SpellSlot.W, 615);
            E = new Spell.Targeted(SpellSlot.E, 615);
            R = new Spell.Skillshot(SpellSlot.R, 1750, SkillShotType.Circular, 2250, int.MaxValue, 475);
        }

        public override void farm()
        {
            if (player.ManaPercent <= 60)
                return;
            var minionCount = EntityManager.MinionsAndMonsters.GetLaneMinions(EntityManager.UnitTeam.Enemy, player.ServerPosition, Q.Range);
            {
                foreach (var minion in minionCount)
                {
                    if (Q.IsReady()
                        && minion.IsValidTarget(Q.Range)
                        && minion.Health > Q.GetDamage(minion))
                    {
                        Q.Cast(minion);
                    }

                    if (W.IsReady()
                        && minion.IsValidTarget(W.Range)
                        && minion.Health > W.GetDamage(minion))
                    {
                        W.CastOnUnit(minion);
                    }

                    if (E.IsReady()
                        && minion.IsValidTarget(E.Range)
                        && minion.Health > E.GetDamage(minion))
                    {
                        E.CastOnUnit(minion);
                    }

                    if (Q.IsReady()
                        && minion.IsValidTarget(Q.Range))
                    {
                        Q.Cast(minion);
                    }

                    if (E.IsReady()
                        && minion.IsValidTarget(E.Range))
                    {
                        E.CastOnUnit(minion);
                    }
                    if (W.IsReady()
                        && minion.IsValidTarget(W.Range))
                    {
                        W.CastOnUnit(minion);
                    }
                }
            }
        }
    }
}