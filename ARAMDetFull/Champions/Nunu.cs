using System.Collections.Generic;
using System.Linq;
using EloBuddy;
using EloBuddy.SDK;

namespace ARAMDetFull.Champions
{
    class Nunu : Champion
    {

        public Nunu()
        {
            ARAMSimulator.champBuild = new Build
            {
                coreItems = new List<ConditionalItem>
                {
                    new ConditionalItem(ItemId.Rod_of_Ages),
                    new ConditionalItem(ItemId.Sorcerers_Shoes),
                    new ConditionalItem(ItemId.Liandrys_Torment),
                    new ConditionalItem(ItemId.Rabadons_Deathcap),
                    new ConditionalItem(ItemId.Void_Staff),
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
        }

        public override void useW(Obj_AI_Base target)
        {
            if (!W.IsReady() || target == null)
                return;
            W.Cast();
        }

        public override void useE(Obj_AI_Base target)
        {
            if (!E.IsReady() || target == null)
                return;
            E.CastOnUnit(target);
        }

        public override void useR(Obj_AI_Base target)
        {
            if (target == null || !R.IsReady())
                return;
            if (player.Position.CountEnemiesInRange(500) > 1)
            {
                R.Cast();
            }
        }

        public override void setUpSpells()
        {
            Q = new Spell.Targeted(SpellSlot.Q, 185);
            W = new Spell.Targeted(SpellSlot.W, 125);
            E = new Spell.Active(SpellSlot.E, 550);
            R = new Spell.Chargeable(SpellSlot.R, 650, 650, 3000, 0, 0, 650);
        }

        public override void useSpells()
        {
            if (Q.IsReady() &&
                    player.HealthPercent < 80)
            {
                var minion = EntityManager.MinionsAndMonsters.GetLaneMinions(EntityManager.UnitTeam.Enemy, player.Position, Q.Range).FirstOrDefault();
                if (minion.IsValidTarget(Q.Range))
                {
                    Q.CastOnUnit(minion);
                }
            }

            var tar = ARAMTargetSelector.getBestTarget(W.Range);
            useW(tar);
            tar = ARAMTargetSelector.getBestTarget(E.Range);
            useE(tar);
            tar = ARAMTargetSelector.getBestTarget(R.Range);
            useR(tar);
        }
    }
}