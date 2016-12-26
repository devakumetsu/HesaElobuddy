using System;
using System.Collections.Generic;
using EloBuddy;
using EloBuddy.SDK;
using EloBuddy.SDK.Enumerations;

namespace ARAMDetFull.Champions
{
    class Galio : Champion
    {

        public Galio()
        {
            ARAMSimulator.champBuild = new Build
            {
                coreItems = new List<ConditionalItem>
                    {
                        new ConditionalItem(ItemId.Mercurys_Treads),
                        new ConditionalItem(ItemId.Athenes_Unholy_Grail),
                        new ConditionalItem(ItemId.Abyssal_Scepter),
                        new ConditionalItem(ItemId.Spirit_Visage),
                        new ConditionalItem(ItemId.Rabadons_Deathcap),
                        new ConditionalItem(ItemId.Void_Staff),
                    },
                startingItems = new List<ItemId>
                    {
                        ItemId.Chalice_of_Harmony,ItemId.Boots_of_Speed
                    }
            };
            
            Obj_AI_Base.OnProcessSpellCast += OnProcessSpell;
        }

        private void OnProcessSpell(Obj_AI_Base sender, GameObjectProcessSpellCastEventArgs args)
        {
            if (!W.IsReady() || sender.IsAlly || !(sender is AIHeroClient) || !(args.Target is AIHeroClient) || args.Target.IsEnemy)
                return;
            if (W.IsInRange((AIHeroClient) args.Target))
                W.CastOnUnit((AIHeroClient)args.Target);

            //26386025
        }
        
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
            if (!Q.IsReady() && player.Mana > 200)
                W.Cast();
        }

        public override void useE(Obj_AI_Base target)
        {
            if (!E.IsReady() || target == null)
                return;
            E.Cast(target);
        }

        public override void useR(Obj_AI_Base target)
        {
            if (target == null || !R.IsReady())
                return;
            if (player.CountEnemiesInRange(450) > 1)
            {
                if (W.IsReady())
                    W.Cast();
                R.Cast();
            }
        }

        public override void useSpells()
        {
            if(ObjectManager.Player.Spellbook.IsChanneling)
                return;
            
            var tar = ARAMTargetSelector.getBestTarget(Q.Range);
            if (tar != null) useQ(tar);
            tar = ARAMTargetSelector.getBestTarget(E.Range);
            if (tar != null) useE(tar);
            tar = ARAMTargetSelector.getBestTarget(R.Range);
            if (tar != null) useR(tar);
        }


        public override void setUpSpells()
        {
            Q = new Spell.Skillshot(SpellSlot.Q, 940, SkillShotType.Linear, 500, 1300, 120);
            W = new Spell.Targeted(SpellSlot.W, 800);
            E = new Spell.Skillshot(SpellSlot.E, 1180, SkillShotType.Linear, 500, 1200, 140);
            R = new Spell.Active(SpellSlot.R, 450);

            //  R.SetSkillshot(0.2f, 320, float.MaxValue, false, SkillshotType.SkillshotCircle);

        }
        
        public override void farm()
        {
            if (player.ManaPercent < 65)
                return;

            var lanemonster = EntityManager.MinionsAndMonsters.GetLaneMinions(EntityManager.UnitTeam.Enemy, player.ServerPosition, Q.Range);
            foreach (var minion in lanemonster)
            {
                if (Q.IsReady() && Q.GetDamage(minion) > minion.Health)
                {
                    Q.Cast(minion);
                    return;
                }
                if (E.IsReady() && E.GetDamage(minion) > minion.Health)
                {
                    E.Cast(minion);
                }
            }
        }
    }
}