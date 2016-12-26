using System.Collections.Generic;
using EloBuddy;
using EloBuddy.SDK;
using EloBuddy.SDK.Constants;
using EloBuddy.SDK.Enumerations;

namespace ARAMDetFull.Champions
{
    class Nocturne : Champion
    {
        public Spell.Targeted R1 { get; private set; }

        public Nocturne()
        {
            ARAMSimulator.champBuild = new Build
            {
                coreItems = new List<ConditionalItem>
                {
                    new ConditionalItem(ItemId.Ravenous_Hydra_Melee_Only),
                    new ConditionalItem(ItemId.Mercurys_Treads, ItemId.Ninja_Tabi, ItemCondition.ENEMY_AP),
                    new ConditionalItem(ItemId.Spirit_Visage, ItemId.Frozen_Heart, ItemCondition.ENEMY_AP),
                    new ConditionalItem(ItemId.Last_Whisper),
                    new ConditionalItem(ItemId.Sunfire_Cape),
                    new ConditionalItem(ItemId.Banshees_Veil),
                },
                startingItems = new List<ItemId>
                {
                    ItemId.Vampiric_Scepter,ItemId.Boots_of_Speed
                }
            };

            //Orbwalker.OnPostAttack += afterAttack;
            Obj_AI_Base.OnProcessSpellCast += OnProcessSpell;
        }

        private void OnProcessSpell(Obj_AI_Base sender, GameObjectProcessSpellCastEventArgs args)
        {
            if (!W.IsReady() || sender == null || args.Target == null || sender.IsAlly || !(sender is AIHeroClient) || !args.Target.IsMe || args.SData.IsAutoAttack())
                return;
            W.Cast();


        }

        /*private void afterAttack(AttackableUnit unit, EventArgs args)
        {

        }*/

        public override void useQ(Obj_AI_Base target)
        {
            if (!Q.IsReady() || target == null)
                return;
            Q.Cast(target);
        }

        public override void useW(Obj_AI_Base target)
        {
            if (!W.IsReady() || target == null)
                return;
            //if (!Q.IsReady(4500) && player.Mana > 200)
            //      W.Cast();
        }

        public override void useE(Obj_AI_Base target)
        {
            if (!E.IsReady() || target == null)
                return;
            E.CastOnUnit(target);
        }

        public override void useR(Obj_AI_Base target)
        {
            if (target == null || !R.IsReady() || !safeGap(target))
                return;
            R.Cast();
            R.CastOnUnit(target);
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
            Q = new Spell.Skillshot(SpellSlot.Q, 1125, SkillShotType.Linear);
            W = new Spell.Active(SpellSlot.W);
            E = new Spell.Targeted(SpellSlot.E, 425);
            R = new Spell.Active(SpellSlot.R, 2500);
            R1 = new Spell.Targeted(SpellSlot.R, R.Range);
            /*   Q = new Spell(SpellSlot.Q, 1150);
               Q.SetSkillshot(
                   0.25f, Q.Instance.SData.LineWidth, 1350, false, SkillshotType.SkillshotLine);
               W = new Spell(SpellSlot.W);
               E = new Spell(SpellSlot.E, 400);
               R = new Spell(SpellSlot.R, 2000);*/

        }


        public override void farm()
        {
            if (player.ManaPercent < 65)
                return;

            var AllMinions = EntityManager.MinionsAndMonsters.GetLaneMinions(EntityManager.UnitTeam.Enemy, player.ServerPosition, Q.Range);
            foreach (var minion in AllMinions)
            {
                if (Q.IsReady() && Q.GetDamage(minion) > minion.Health)
                {
                    Q.Cast(minion);
                    return;
                }
            }
        }
    }
}