using System.Collections.Generic;
using EloBuddy;
using EloBuddy.SDK;
using EloBuddy.SDK.Enumerations;

namespace ARAMDetFull.Champions
{
    class Singed : Champion
    {

        public Singed()
        {

            ARAMSimulator.champBuild = new Build
            {
                coreItems = new List<ConditionalItem>
                {
                    new ConditionalItem(ItemId.Rod_of_Ages),
                    new ConditionalItem(ItemId.Mercurys_Treads),
                    new ConditionalItem(ItemId.Randuins_Omen),
                    new ConditionalItem(ItemId.Rylais_Crystal_Scepter),
                    new ConditionalItem(ItemId.Liandrys_Torment),
                    new ConditionalItem(ItemId.Banshees_Veil),
                },
                startingItems = new List<ItemId>
                {
                    ItemId.Catalyst_of_Aeons
                }
            };
        }

        public override void useQ(Obj_AI_Base target)
        {
            if (!Q.IsReady())
                return;
            if (!player.HasBuff("Poison Trail"))
            {
                Q.Cast();
            }
        }

        public override void useW(Obj_AI_Base target)
        {
            if (!W.IsReady() || player.IsUnderTurret() || MapControl.fightIsOn() == null)
                return;
            W.Cast(target.Position);
        }

        public override void useE(Obj_AI_Base target)
        {
            if (!E.IsReady())
                return;
            E.CastOnUnit(target);

        }

        public override void useR(Obj_AI_Base target)
        {
            if (!R.IsReady() || player.HealthPercent > 70)
                return;
            R.Cast();
        }

        public override void setUpSpells()
        {
            Q = new Spell.Active(SpellSlot.Q);
            W = new Spell.Skillshot(SpellSlot.W, 1000, SkillShotType.Circular, 500, 700, 350)
            {
                MinimumHitChance = HitChance.High,
                AllowedCollisionCount = int.MaxValue
            };
            E = new Spell.Targeted(SpellSlot.E, 125);
            R = new Spell.Active(SpellSlot.R);
        }

        public override void useSpells()
        {
            var tar = ARAMTargetSelector.getBestTarget(550);
            if (tar != null) useQ(tar); else if (player.HasBuff("Poison Trail")) { if (Q.IsReady()) Q.Cast(); }
            tar = ARAMTargetSelector.getBestTarget(E.Range);
            if (tar != null) useE(tar);
            tar = ARAMTargetSelector.getBestTarget(W.Range);
            if (tar != null) useW(tar);
            tar = ARAMTargetSelector.getBestTarget(300);
            if (tar != null) useR(tar);
        }

        public static bool EnemyInRange(int numOfEnemy, float range)
        {
            return Player.Instance.CountEnemiesInRange((int)range) >= numOfEnemy;
        }

    }
}