using System;
using System.Collections.Generic;
using System.Linq;
using EloBuddy;
using EloBuddy.SDK;
using EloBuddy.SDK.Enumerations;
using SharpDX;

namespace ARAMDetFull.Champions
{
    class Evelynn : Champion
    {
        public Evelynn()
        {
            Spellbook.OnCastSpell += Spellbook_OnCastSpell;
            Orbwalker.OnPostAttack += AfterAttack;
            ARAMSimulator.champBuild = new Build
            {
                coreItems = new List<ConditionalItem>
                        {
                            new ConditionalItem(ItemId.Ludens_Echo),
                            new ConditionalItem(ItemId.Ionian_Boots_of_Lucidity),
                            new ConditionalItem(ItemId.Rabadons_Deathcap),
                            new ConditionalItem(ItemId.Rylais_Crystal_Scepter),
                            new ConditionalItem(ItemId.Lich_Bane),
                            new ConditionalItem(ItemId.Void_Staff),
                        },
                startingItems = new List<ItemId>
                        {
                            ItemId.Needlessly_Large_Rod
                        }
            };
        }

        private void AfterAttack(AttackableUnit unit, EventArgs args)
        {
            if (unit is AIHeroClient && Q.IsReady())
            {
                Q.Cast();
                Player.IssueOrder(GameObjectOrder.AutoAttack, unit);
            }
        }

        public override void useQ(Obj_AI_Base target)
        {

        }

        public override void useW(Obj_AI_Base target)
        {

        }

        public override void useE(Obj_AI_Base target)
        {

        }


        public override void useR(Obj_AI_Base target)
        {
        }

        public override void useSpells()
        {
            var target = ARAMTargetSelector.getBestTarget(Q.Range);

            if (target != null)
            {
                Q.Cast();

                if (ObjectManager.Player.HasBuffOfType(BuffType.Slow))
                    W.Cast();

                if (E.IsReady())
                    E.CastOnUnit(target);

                if (R.IsReady() && GetComboDamage(target) > target.Health)
                    R.Cast(target);
            }

        }

        private void Spellbook_OnCastSpell(Spellbook sender, SpellbookCastSpellEventArgs args)
        {
            if (sender.Owner.IsMe && args.Slot == SpellSlot.R)
            {
                if (ObjectManager.Get<AIHeroClient>()
                .Count(
                    hero =>
                        hero.IsValidTarget() &&
                        hero.Distance(new Vector2(args.EndPosition.X, args.EndPosition.Y)) <= R.Range) == 0)
                    args.Process = false;
            }
        }


        public override void setUpSpells()
        {
            //Create the spells
            Q = new Spell.Active(SpellSlot.Q, 500);
            W = new Spell.Targeted(SpellSlot.W, Q.Range);
            E = new Spell.Active(SpellSlot.E, 225 + 2 * 65);
            R = new Spell.Skillshot(SpellSlot.R, 650, SkillShotType.Circular, 250, 350, 300);
            //R.SetSkillshot(0.25f, 350f, float.MaxValue, false, SkillshotType.SkillshotCircle);
        }

        public override void farm()
        {
            if (player.ManaPercent < 45)
                return;
            var minions = EntityManager.MinionsAndMonsters.GetLaneMinions(EntityManager.UnitTeam.Enemy, ObjectManager.Player.ServerPosition, Q.Range);

            foreach (var minion in minions.Where(minion => minion.IsValidTarget(Q.Range)))
            {
                if (Q.IsReady())
                    Q.Cast();

                if (E.IsReady())
                    E.CastOnUnit(minion);
            }
        }

        private float GetComboDamage(Obj_AI_Base target)
        {
            float comboDamage = 0;

            if ((ObjectManager.Player.Spellbook.GetSpell(SpellSlot.Q).Level) > 0)
                comboDamage += Q.GetDamage(target) * 3;
            if (E.IsReady())
                comboDamage += E.GetDamage(target);
            if (R.IsReady())
                comboDamage += R.GetDamage(target);

            return comboDamage;
        }
    }
}