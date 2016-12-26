using System;
using System.Collections.Generic;
using EloBuddy;
using EloBuddy.SDK;
using EloBuddy.SDK.Enumerations;

namespace ARAMDetFull.Champions
{
    class Fizz : Champion
    {
        public Fizz()
        {
            ARAMSimulator.champBuild = new Build
            {
                coreItems = new List<ConditionalItem>
                {
                    new ConditionalItem(ItemId.Lich_Bane),
                    new ConditionalItem(ItemId.Sorcerers_Shoes),
                    new ConditionalItem(ItemId.Rabadons_Deathcap),
                    new ConditionalItem(ItemId.Rylais_Crystal_Scepter),
                    new ConditionalItem(ItemId.Void_Staff,ItemId.Liandrys_Torment, ItemCondition.ENEMY_MR),
                    new ConditionalItem(ItemId.Zhonyas_Hourglass),
                },
                startingItems = new List<ItemId>
                {
                    ItemId.Sheen
                }
            };
            Console.WriteLine("Fizz i");
        }
        public override void useQ(Obj_AI_Base target)
        {
            if (!Q.IsReady() || target == null || !safeGap(player.Position.Extend(target,475)))
                return;
            Q.CastOnUnit(target);
        }

        public override void useW(Obj_AI_Base target)
        {
            if (!W.IsReady())
                return;
            W.Cast();
        }

        public override void useE(Obj_AI_Base target)
        {
            if (!E.IsReady() || target == null || !safeGap(target))
                return;
            if (player.Spellbook.GetSpell(SpellSlot.E).Name == "fizzjumptwo" /*&& Environment.TickCount - E. > 150*/)
                E.Cast(target.Position);
            if (player.Spellbook.GetSpell(SpellSlot.E).Name == "FizzJump")
                E.Cast(target.Position);

        }


        public override void useR(Obj_AI_Base target)
        {
            if (!R.IsReady() || target == null)
                return;
            if (!safeGap(target) || target.HealthPercent < 28)
            {
                if (R.CastIfHitchanceEquals(target, HitChance.High, true))
                    Aggresivity.addAgresiveMove(new AgresiveMove(25, 4000, true));
            }
        }

        public override void useSpells()
        {
            var tar = ARAMTargetSelector.getBestTarget(Q.Range);
            if (tar != null) useQ(tar);
            tar = ARAMTargetSelector.getBestTarget(W.Range);
            if (tar != null) useW(tar);
            tar = ARAMTargetSelector.getBestTarget(E.Range);
            if (tar != null) useE(tar);
            tar = ARAMTargetSelector.getBestTarget(R.Range);
            if (tar != null) useR(tar);
        }

        public override void setUpSpells()
        {
            Q = new Spell.Targeted(SpellSlot.Q, 550);
            W = new Spell.Active(SpellSlot.W, 240);
            E = new Spell.Skillshot(SpellSlot.E, 725, SkillShotType.Circular, 500, 270, 1300);
            R = new Spell.Skillshot(SpellSlot.R, 1300, SkillShotType.Linear, 250, 1350, 120);
        }


        public override void farm()
        {
            if (player.ManaPercent < 55 || !E.IsReady())
                return;
            var lanemonster = EntityManager.MinionsAndMonsters.GetLaneMinions(EntityManager.UnitTeam.Enemy, Player.Instance.ServerPosition, E.Range - 300);
            foreach (var minion in lanemonster)
            {
                if (minion.Health > ObjectManager.Player.GetAutoAttackDamage(minion) && minion.Health < E.GetDamage(minion) && safeGap(minion))
                {
                    E.Cast(minion);
                    return;
                }
            }
        }
    }
}