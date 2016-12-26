using System.Collections.Generic;
using System.Linq;
using SharpDX;
using EloBuddy;
using EloBuddy.SDK;
using EloBuddy.SDK.Enumerations;
using static EloBuddy.SDK.Spell;

namespace ARAMDetFull.Champions
{
    class Zyra : Champion
    {

        public Zyra()
        {
            ARAMSimulator.champBuild = new Build
            {
                coreItems = new List<ConditionalItem>
                {
                    new ConditionalItem(ItemId.Morellonomicon),
                    new ConditionalItem(ItemId.Sorcerers_Shoes),
                    new ConditionalItem(ItemId.Liandrys_Torment),
                    new ConditionalItem(ItemId.Ludens_Echo),
                    new ConditionalItem(ItemId.Void_Staff, ItemId.Rylais_Crystal_Scepter, ItemCondition.ENEMY_AP),
                    new ConditionalItem(ItemId.Rabadons_Deathcap),
                },
                startingItems = new List<ItemId>
                {
                    ItemId.Needlessly_Large_Rod
                }
            };
        }

        public override void useQ(Obj_AI_Base target)
        {
            if (!Q.IsReady() || target == null)
                return;
            if (Q.Cast(target))
            {
                 CastW(Q.GetPrediction(target).CastPosition);
            }
            Core.DelayAction(() => Player.IssueOrder(GameObjectOrder.AttackUnit, target), 100);
        }

        public override void useW(Obj_AI_Base target)
        {
           
        }

        public override void useE(Obj_AI_Base target)
        {
            if (!E.IsReady() || target == null)
                return;
            if (E.Cast(target))
            {
                  CastW(Q.GetPrediction(target).CastPosition);
            }
            Core.DelayAction(() => Player.IssueOrder(GameObjectOrder.AttackUnit, target), 100);
        }

        public override void useR(Obj_AI_Base target)
        {
            if (target == null)
                return;
            if (target.IsValidTarget(R.Range) && R.IsReady())
            {
                R.CastIfWillHit(target, 2);
            }
            Core.DelayAction(() => Player.IssueOrder(GameObjectOrder.AttackUnit, target), 100);
        }

        public override void useSpells()
        {
            var tar = ARAMTargetSelector.getBestTarget(Passive.Range);
            if (ZyraisZombie())
            {
                if(tar != null)
                    CastPassive(tar);
                return;
            }

            tar = ARAMTargetSelector.getBestTarget(Q.Range);
            useQ(tar);
            tar = ARAMTargetSelector.getBestTarget(W.Range);
            useW(tar);
            tar = ARAMTargetSelector.getBestTarget(E.Range);
            useE(tar);
            tar = ARAMTargetSelector.getBestTarget(R.Range);
            useR(tar);
        }

        public override void setUpSpells()
        {
            Q = new Spell.Skillshot(SpellSlot.Q, 800, SkillShotType.Cone);
            W = new Spell.Skillshot(SpellSlot.W, 820, SkillShotType.Circular, 500, int.MaxValue, 80);
            E = new Spell.Skillshot(SpellSlot.E, 1100, SkillShotType.Linear, 250, 1150, 80)
            {
                AllowedCollisionCount = 0
            };
            R = new Spell.Skillshot(SpellSlot.R, 700, SkillShotType.Circular, 250, 1200, 150);

            Passive = new Spell.Skillshot(SpellSlot.Q, 1470, SkillShotType.Linear);

        }

        private bool ZyraisZombie()
        {
            return player.Spellbook.GetSpell(SpellSlot.Q).Name == player.Spellbook.GetSpell(SpellSlot.E).Name || player.Spellbook.GetSpell(SpellSlot.W).Name == player.Spellbook.GetSpell(SpellSlot.R).Name;
        }

        private void CastPassive(Obj_AI_Base target)
        {
            if (!Passive.IsReady())
            {
                return;
            }
            if (!target.IsValidTarget(E.Range))
            {
                return;
            }
            Passive.CastIfHitchanceEquals(target, HitChance.High, true);
        }


        private SpellBase Passive { get; set; }

        private int WCount
        {
            get { return W.Level > 0 ? W.AmmoQuantity : 0; }
        }

        private void CastW(Vector3 v)
        {
            if (!W.IsReady())
            {
                return;
            }

            if (WCount == 1)
            {
                Core.DelayAction(() => W.Cast(new Vector2(v.X - 5, v.Y - 5).To3D()), 50);
            }

            if (WCount == 2)
            {
                Core.DelayAction(() => W.Cast(new Vector2(v.X - 5, v.Y - 5).To3D()), 50);
                Core.DelayAction(() => W.Cast(new Vector2(v.X - 5, v.Y - 5).To3D()), 180);
            }
        }
    }
}
