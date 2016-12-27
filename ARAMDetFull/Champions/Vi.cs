using System.Collections.Generic;
using EloBuddy;
using EloBuddy.SDK;

namespace ARAMDetFull.Champions
{
    class Vi : Champion
    {
        public static Spell.Chargeable Q;

        public Vi()
        {
            Orbwalker.OnPreAttack += DeathWalkerOnBeforeAttack;
            ARAMSimulator.champBuild = new Build
            {
                coreItems = new List<ConditionalItem>
                {
                    new ConditionalItem(ItemId.Trinity_Force),
                    new ConditionalItem(ItemId.Mercurys_Treads),
                    new ConditionalItem((ItemId)3053),
                    new ConditionalItem(ItemId.Locket_of_the_Iron_Solari,(ItemId)3742,ItemCondition.ENEMY_AP),
                    new ConditionalItem(ItemId.Banshees_Veil,ItemId.Randuins_Omen,ItemCondition.ENEMY_AP),
                    new ConditionalItem(ItemId.Warmogs_Armor),
                },
                startingItems = new List<ItemId>
                {
                    ItemId.Phage
                }
            };
        }

        private void DeathWalkerOnBeforeAttack(AttackableUnit unit, Orbwalker.PreAttackArgs args)
        {
            if (E.IsReady() && args.Target is AIHeroClient)
                E.Cast();
        }

        public override void useQ(Obj_AI_Base target)
        {
            if (!Q.IsReady() || target == null)
                return;
            if (safeGap(target))
            {
                if (target.IsValidTarget(Q.Range))
                {
                    if (Q.IsCharging)
                    {
                        Q.Cast(target);
                    }
                    else
                    {
                        Q.StartCharging();
                    }
                }
            }
        }

        public override void useW(Obj_AI_Base target)
        {
        }

        public override void useE(Obj_AI_Base target)
        {
            if (!E.IsReady() || target == null)
                return;
            if (target.IsValidTarget(Player.Instance.GetAutoAttackRange(target)))
            {
                E.Cast();
            }
        }


        public override void useR(Obj_AI_Base t)
        {
            if (!R.IsReady() || t == null)
                return;
            if (t.HealthPercent < 60 && safeGap(t))
                R.CastOnUnit(t);
            var qDamage = player.GetSpellDamage(t, SpellSlot.Q);
            var eDamage = player.GetSpellDamage(t, SpellSlot.E);
            var rDamage = player.GetSpellDamage(t, SpellSlot.R);

            if (Q.IsReady() && t.Health < qDamage)
                return;

            if (E.IsReady() && Player.Instance.IsInAutoAttackRange(t) && t.Health < eDamage)
                return;

            if (Q.IsReady() && E.IsReady() && t.Health < qDamage + eDamage)
                return;

            if (t.Health > rDamage)
            {
                if (Q.IsReady() && E.IsReady() && t.Health < rDamage + qDamage + eDamage)
                    R.CastOnUnit(t);
                else if (E.IsReady() && t.Health < rDamage + eDamage)
                    R.CastOnUnit(t);
                else if (Q.IsReady() && t.Health < rDamage + qDamage)
                    R.CastOnUnit(t);
            }
            else
            {
                if (!Player.Instance.IsInAutoAttackRange(t))
                    R.CastOnUnit(t);
            }
        }

        public override void useSpells()
        {
            var tar = ARAMTargetSelector.getBestTarget(Q.Range);
            if (tar != null) useQ(tar);
            tar = ARAMTargetSelector.getBestTarget(R.Range);
            if (tar != null) useR(tar);
        }

        public override void setUpSpells()
        {
            Q = new Spell.Chargeable(SpellSlot.Q, 250, 725, 4, 250, 1250, 55, DamageType.Physical)
            {
                AllowedCollisionCount = int.MaxValue,
            };
            W = new Spell.Active(SpellSlot.W, 0);
            E = new Spell.Active(SpellSlot.E, 175, DamageType.Physical);
            R = new Spell.Targeted(SpellSlot.R, 800, DamageType.Physical);
        }
    }
}