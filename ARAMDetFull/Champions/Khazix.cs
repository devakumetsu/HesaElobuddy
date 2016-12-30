using System.Collections.Generic;
using EloBuddy;
using EloBuddy.SDK;
using EloBuddy.SDK.Enumerations;

namespace ARAMDetFull.Champions
{
    class Khazix : Champion
    {
        private static bool _qevolved = false, _wevolved = false, _eevolved = false;
        
        public Khazix()
        {
            ARAMSimulator.champBuild = new Build
            {
                coreItems = new List<ConditionalItem>
                {
                    new ConditionalItem(ItemId.Ravenous_Hydra_Melee_Only),
                    new ConditionalItem(ItemId.Mercurys_Treads),
                    new ConditionalItem(ItemId.The_Black_Cleaver),
                    new ConditionalItem(ItemId.Spirit_Visage, ItemId.Randuins_Omen, ItemCondition.ENEMY_AP),
                    new ConditionalItem(ItemId.The_Bloodthirster),
                    new ConditionalItem(ItemId.Banshees_Veil),
                },
                startingItems = new List<ItemId>
                {
                    ItemId.Vampiric_Scepter,ItemId.Boots_of_Speed
                }
            };
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
            if (W.IsReady())
            W.Cast(target);
        }

        public override void useE(Obj_AI_Base target)
        {
            if (!E.IsReady() || target == null)
                return;
            if (target.Health > E.GetDamage(target) && target.Distance(player, true) > 350 * 350)
                return;
            if (safeGap(target))
                E.Cast(target);
        }


        public override void useR(Obj_AI_Base target)
        {
            if (!R.IsReady() || target == null)
                return;
            //R.Cast();
        }

        public override void useSpells()
        {
            if (E.IsReady() && player.HealthPercent < 35 && Player.Instance.CountEnemiesInRange(600) > 1/*.Count(ene => !ene.IsDead) > 1*/)
                E.Cast(ARAMSimulator.fromNex.Position);
            //CheckSpells();
            var tar = ARAMTargetSelector.getBestTarget(Q.Range);
            if (tar != null) useQ(tar);
            tar = ARAMTargetSelector.getBestTarget(W.Range);
            if (tar != null) useW(tar);
            //tar = ARAMTargetSelector.getBestTarget(E.Range);
            //if (tar != null) useE(tar);
            tar = ARAMTargetSelector.getBestTarget(R.Range);
            //if (tar != null) useR(tar);

        }

        public override void setUpSpells()
        {
            Q = new Spell.Skillshot(SpellSlot.Q, 325, SkillShotType.Linear, 250, int.MaxValue, 85)
            {
                AllowedCollisionCount = int.MaxValue
            };
            W = new Spell.Skillshot(SpellSlot.W, 1000, SkillShotType.Circular, 250, int.MaxValue, 20)
            {
                AllowedCollisionCount = int.MaxValue
            };
            E = new Spell.Skillshot(SpellSlot.E, 700, SkillShotType.Linear, 250, 1150, 70)
            {
                AllowedCollisionCount = int.MaxValue
            };
            R = new Spell.Skillshot(SpellSlot.R, 300, SkillShotType.Circular, 250, 1200, 500)
            {
                AllowedCollisionCount = int.MaxValue
            };
            /*Q = new Spell(SpellSlot.Q, 325f);
            W = new Spell(SpellSlot.W, 1000f);
            E = new Spell(SpellSlot.E, 600f);
            R = new Spell(SpellSlot.R, 300f);

            W.SetSkillshot(0.225f, 80f, 828.5f, true, SkillshotType.SkillshotLine);

            E.SetSkillshot(0.25f, 100f, 1000f, false, SkillshotType.SkillshotCircle);*/
        }

        private void CheckSpells()
        {

            //check for evolutions
            if (ObjectManager.Player.HasBuff("khazixqevo") && !_qevolved)
            {
                Q.Range = 375;
                _qevolved = true;
            }
            if (ObjectManager.Player.HasBuff("khazixwevo") && !_wevolved)
            {
                W.Range = 1000;
                _wevolved = true;
                //should be fixed
                //W.IsSkillShot(250, 20, int.MaxValue, SkillShotType.Linear);
            }

            if (ObjectManager.Player.HasBuff("khazixeevo") && !_eevolved)
            {
               E.Range = 900;
                _eevolved = true;
            }
            if (player.EvolvePoints > 0)
            {
                if (!_eevolved)
                {                     //DISABLE Jumpl until crash fixed.
                   //player.Spellbook.EvolveSpell(SpellSlot.E);
                   //return;
                }
                if (!_qevolved)
                {
                    player.Spellbook.EvolveSpell(SpellSlot.Q);
                    return;
                }
                if (!_wevolved)
                {
                    player.Spellbook.EvolveSpell(SpellSlot.W);
                    //return;
                }
            }
        }
    }
}