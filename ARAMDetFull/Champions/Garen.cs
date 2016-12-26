using System.Collections.Generic;
using System.Linq;

using EloBuddy;
using EloBuddy.SDK;

namespace ARAMDetFull.Champions
{
    class Garen : Champion
    {

        public Garen()
        {
            //DeathWalker.AfterAttack += AfterAttack;

            ARAMSimulator.champBuild = new Build
            {
                coreItems = new List<ConditionalItem>
                {
                    new ConditionalItem(ItemId.The_Black_Cleaver),
                    new ConditionalItem(ItemId.Mercurys_Treads,ItemId.Ninja_Tabi,ItemCondition.ENEMY_AP),
                    new ConditionalItem(ItemId.Banshees_Veil,ItemId.Sunfire_Cape,  ItemCondition.ENEMY_AP),
                    new ConditionalItem(ItemId.Frozen_Mallet),
                    new ConditionalItem(ItemId.Spirit_Visage, ItemId.Randuins_Omen, ItemCondition.ENEMY_AP),
                    new ConditionalItem(ItemId.Last_Whisper),
                },
                startingItems = new List<ItemId>
                {
                    ItemId.Phage
                }
            };
        }

        private static bool GarenE
        {
            get
            {
                return player.Buffs.Any(buff => buff.Name == "GarenE");
            }
        }

        private static bool GarenQ
        {
            get { return player.Buffs.Any(buff => buff.Name == "GarenQ"); }
        }

        private void AfterAttack(AttackableUnit unit, AttackableUnit target)
        {
            if (unit.IsMe && target is AIHeroClient && Q.IsReady() && !GarenE)
            {
                Q.Cast();
                Aggresivity.addAgresiveMove(new AgresiveMove(1000, 2500, true));
                Player.IssueOrder(GameObjectOrder.AttackUnit, target);
            }
        }

        public override void useQ(Obj_AI_Base target)
        {
            if (Q.IsReady() && !GarenE)
            {
                Q.Cast();
                Aggresivity.addAgresiveMove(new AgresiveMove(100, 2500, true));
                Core.DelayAction(() => Player.IssueOrder(GameObjectOrder.AttackUnit, target), 100);
            }
        }

        public override void useW(Obj_AI_Base target)
        {
            if (W.IsReady())
            {
                W.Cast();
                Aggresivity.addAgresiveMove(new AgresiveMove(50, 2500, true));
            }
        }

        public override void useE(Obj_AI_Base target)
        {
            if (!E.IsReady() || target == null || GarenE)
                return;
            E.Cast();
            Aggresivity.addAgresiveMove(new AgresiveMove(150, 3000, true));
        }


        public override void useR(Obj_AI_Base target)
        {
            if (!R.IsReady() || target == null)
                return;
            if (R.IsKillable(target))
                R.Cast(target);
        }

        public override void useSpells()
        {
            var tar = ARAMTargetSelector.getBestTarget(Q.Range);
            if (tar != null) useQ(tar);
            tar = ARAMTargetSelector.getBestTarget(E.Range);
            if (tar != null) useE(tar);
            tar = ARAMTargetSelector.getBestTarget(W.Range);
            if (tar != null) useW(tar);

            if (R.IsReady())
            {
                foreach (var enem in MapControl.enemy_champions.Where(ene => ene.hero.Distance(player) < R.Range))
                {
                    if (R.IsKillable(enem.hero))
                    {
                        R.Cast(enem.hero);
                        return;
                    }
                }

            }
        }

        public override void setUpSpells()
        {
            //Create the spells
            Q = new Spell.Active(SpellSlot.Q);
            W = new Spell.Active(SpellSlot.W);
            E = new Spell.Active(SpellSlot.E, 300);
            R = new Spell.Targeted(SpellSlot.R, 400);
        }
    }
}