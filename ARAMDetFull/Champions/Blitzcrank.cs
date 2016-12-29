using EloBuddy;
using EloBuddy.SDK;
using EloBuddy.SDK.Enumerations;
using System.Collections.Generic;
using System.Linq;
using EloBuddy.SDK.Events;

namespace ARAMDetFull.Champions
{
    class Blitzcrank : Champion
    {
        public Blitzcrank()
        {
            ARAMSimulator.champBuild = new Build
            {
                coreItems = new List<ConditionalItem>
                {
                    new ConditionalItem(ItemId.Face_of_the_Mountain),
                    new ConditionalItem(ItemId.Boots_of_Mobility),
                    new ConditionalItem(ItemId.Locket_of_the_Iron_Solari),
                    new ConditionalItem(ItemId.Banshees_Veil),
                    new ConditionalItem(ItemId.Mikaels_Crucible),
                    new ConditionalItem(ItemId.Righteous_Glory),
                },
                startingItems = new List<ItemId>
                {
                    ItemId.Relic_Shield
                }
            };
            Interrupter.OnInterruptableSpell += Interrupter_OnInterruptableSpell;
            //DeathWalker.AfterAttack += OnAfterAttack;
        }
        
        public override void useW(Obj_AI_Base target)
        {
            if (!W.IsReady())
                return;
            if (Shop.CanShop && !player.IsDead)
                W.Cast();
        }

        public override void useE(Obj_AI_Base target)
        {
            if (!E.IsReady() || W.IsReady())
                return;
            E.Cast();
            if (!Orbwalker.IsAutoAttacking || Orbwalker.GetTarget() != target)
                Player.IssueOrder(GameObjectOrder.AttackUnit, target);
        }

        public override void useR(Obj_AI_Base target)
        {
            if (!R.IsReady())
                return;
                R.Cast();


        }

        public override void setUpSpells()
        {
            Q = new Spell.Skillshot(SpellSlot.Q, 980, SkillShotType.Linear, (int)250f, (int)1800f, (int)70f);
            W = new Spell.Active(SpellSlot.W, 0);
            E = new Spell.Active(SpellSlot.E, 150);
            R = new Spell.Active(SpellSlot.R, 550);
        }

        public override void useSpells()
        {
            var tar = ARAMTargetSelector.getBestTarget(Q.Range);
                if(tar != null)useQ(tar);
            tar = ARAMTargetSelector.getBestTarget(W.Range);
                if (tar != null) useW(tar);
            tar = ARAMTargetSelector.getBestTarget(R.Range);
                if (tar != null) useR(tar);
        }

        public override void useQ(Obj_AI_Base target)
        {
            if (!Q.IsReady())
            {
                return;
            }

            if (!target.IsValidTarget())
            {
                return;
            }

            if (target.HasBuff("BlackShield"))
            {
                return;
            }

            if (AllyInRange(1200)
                .Any(ally => ally.Distance(target) < ally.AttackRange + ally.BoundingRadius))
            {
                return;
            }
            Q.Cast(target);

        }

        public override void farm()
        {

        }

        public List<AIHeroClient> AllyInRange(float range)
        {
            return ObjectManager.Get<AIHeroClient>().Where(h => ObjectManager.Player.Distance(h) < range && h.IsAlly && !h.IsMe && h.IsValid && !h.IsDead).OrderBy(h => ObjectManager.Player.Distance(h)).ToList();
        }

        public void OnAfterAttack(AttackableUnit unit, AttackableUnit target)
        {
            if (!unit.IsMe)
            {
                return;
            }

            if (!target.IsValidTarget() && !target.Name.ToLower().Contains("ward"))
            {
                return;
            }

            if (!E.IsReady())
            {
                return;
            }

            if (E.Cast())
            {
                if (!Orbwalker.IsAutoAttacking || Orbwalker.GetTarget() != target)
                    Player.IssueOrder(GameObjectOrder.AttackUnit, target);
            }
        }

        private void Interrupter_OnInterruptableSpell(Obj_AI_Base unit, Interrupter.InterruptableSpellEventArgs spell)
        {
            if (spell.DangerLevel < DangerLevel.High || unit.IsAlly)
            {
                return;
            }
            if (E.IsReady() && E.Cast())
            {
                if (!Orbwalker.IsAutoAttacking || Orbwalker.GetTarget() != unit)
                    Player.IssueOrder(GameObjectOrder.AttackUnit, unit);
            }
            if (Q.IsReady())
            {
                Q.Cast(unit);
            }
            if (R.IsReady())
            {
                R.Cast();
            }
        }
    }
}
