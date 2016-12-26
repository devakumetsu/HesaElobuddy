using System;
using System.Collections.Generic;
using System.Linq;
using EloBuddy;
using EloBuddy.SDK;
using EloBuddy.SDK.Enumerations;

namespace ARAMDetFull.Champions
{
    class Mordekaiser : Champion
    {

        public Mordekaiser()
        {
            //DeathWalker.AfterAttack += AfterAttack;
            Orbwalker.OnPostAttack += AfterAttack;

            ARAMSimulator.champBuild = new Build
            {
                coreItems = new List<ConditionalItem>
                {
                    //new ConditionalItem(ItemId.Will_of_the_Ancients),
                    new ConditionalItem(ItemId.Sorcerers_Shoes),
                    new ConditionalItem(ItemId.Rabadons_Deathcap),
                    new ConditionalItem(ItemId.Lich_Bane),
                    new ConditionalItem(ItemId.Void_Staff),
                    new ConditionalItem(ItemId.Zhonyas_Hourglass),
                },
                startingItems = new List<ItemId>
                {
                    ItemId.Hextech_Revolver
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
            if (!E.IsReady() || target == null)
                return;
            E.Cast(target);
        }


        public override void useR(Obj_AI_Base target)
        {
            if (!R.IsReady() || target == null)
                return;
            if (target.HealthPercent < 35)
                R.CastOnUnit(target);
        }

        public override void useSpells()
        {
            var tar = ARAMTargetSelector.getBestTarget(E.Range);
            if (tar != null) useE(tar);
            tar = ARAMTargetSelector.getBestTarget(W.Range);
            if (tar != null) useW(tar);

            if (R.IsReady())
            {
                foreach (var enem in ObjectManager.Get<AIHeroClient>()
                    .Where(ene => ene.IsEnemy && ene.Distance(player, true) < R.Range * R.Range).Where(enem => enem.HealthPercent < 35))
                {
                    R.CastOnUnit(enem);
                    return;
                }
            }


            if (W.IsReady())
            {
                foreach (var enem in ObjectManager.Get<AIHeroClient>().Where(ene => ene.Distance(player, true) < W.Range * W.Range))
                {
                    if (enem.CountEnemiesInRange(330) > 1)
                    {
                        W.CastOnUnit(enem);
                        return;
                    }
                }

            }

        }

        public override void setUpSpells()
        {
            //Create the spells
            Q = new Spell.Active(SpellSlot.Q, (uint)Player.Instance.GetAutoAttackRange());
            W = new Spell.Targeted(SpellSlot.W, 750);
            E = new Spell.Skillshot(SpellSlot.E, 700, SkillShotType.Cone, 250, 2000, 12 * 2 * (int)Math.PI / 180);
            R = new Spell.Targeted(SpellSlot.R, 850);
        }
    }
}