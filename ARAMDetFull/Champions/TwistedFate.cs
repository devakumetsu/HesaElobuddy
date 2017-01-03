using System;
using System.Collections.Generic;
using EloBuddy;
using EloBuddy.SDK;
using EloBuddy.SDK.Enumerations;
using EloBuddy.SDK.Menu.Values;

namespace ARAMDetFull.Champions
{
    class TwistedFate : Champion
    {
        private static Spell.Active W;
        public TwistedFate()
        {
            ARAMSimulator.champBuild = new Build
            {
                coreItems = new List<ConditionalItem>
                {
                    new ConditionalItem(ItemId.Lich_Bane),
                    new ConditionalItem(ItemId.Sorcerers_Shoes),
                    new ConditionalItem(ItemId.Ludens_Echo),
                    new ConditionalItem(ItemId.Rabadons_Deathcap),
                    new ConditionalItem(ItemId.Morellonomicon),
                    new ConditionalItem(ItemId.Zhonyas_Hourglass),
                },
                startingItems = new List<ItemId>
                {
                    ItemId.Sheen,ItemId.Boots_of_Speed
                }
            };
            Orbwalker.OnPreAttack += DeathWalkerOnBeforeAttack;
        }

        private void DeathWalkerOnBeforeAttack(AttackableUnit unit, Orbwalker.PreAttackArgs args)
        {
            if (!W.IsReady() || !(args.Target is AIHeroClient))
                return;
            W.Cast();
        }

        public override void useQ(Obj_AI_Base target)
        {
            if (!Q.IsReady() || target == null)
                return;
            Q.Cast(target);
        }

        public override void useW(Obj_AI_Base target)
        {
            if (W.IsReady())
                W.Cast();
        }

        public override void useE(Obj_AI_Base target)
        {
            if (!E.IsReady() || target == null || !safeGap(target))
                return;
            // E.Cast(target.Position);
            //  Utility.DelayAction.Add(250, () => E.Cast(target.Position));
        }


        public override void useR(Obj_AI_Base target)
        {
            if (!R.IsReady() || target == null || target.Distance(player) < 1600)
                return;
            if (target.HasBuff("dianamoonlight"))
                R.Cast(target.Position);
            else if (safeGap(target))
                R.Cast();
        }

        public override void useSpells()
        {
            var tar = ARAMTargetSelector.getBestTarget(Q.Range);
            if (tar != null) useQ(tar);
            tar = ARAMTargetSelector.getBestTarget(W.Range);
            if (tar != null) useW(tar);
        }

        public override void setUpSpells()
        {
            //Create the spells
            Q = new Spell.Skillshot(SpellSlot.Q, 1450, SkillShotType.Linear, 0, 1000, 40);
            W = new Spell.Active(SpellSlot.W);
            E = new Spell.Active(SpellSlot.E);
            R = new Spell.Active(SpellSlot.R, 5500);
        }


        public override void farm()
        {
            if (player.ManaPercent < 55 || !Q.IsReady())
                return;

            foreach (var minion in EntityManager.MinionsAndMonsters.GetLaneMinions(EntityManager.UnitTeam.Enemy, player.Position, Q.Range - 100))
            {
                if (minion.Health > ObjectManager.Player.GetAutoAttackDamage(minion) && minion.Health < Q.GetDamage(minion))
                {
                    Q.Cast(minion);
                    return;
                }
            }
        }
    }
}