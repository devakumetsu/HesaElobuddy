using EloBuddy;
using EloBuddy.SDK;
using EloBuddy.SDK.Enumerations;
using System.Collections.Generic;
using System.Linq;
using static EloBuddy.SDK.Spell;

namespace ARAMDetFull.Champions
{
    class RekSai2 : Champion
    {
        public static Active Q1 { get; private set; }
        public static Active W { get; private set; }
        public static Targeted E1 { get; private set; }

        public static Skillshot Q2 { get; private set; }
        public static Skillshot E2 { get; private set; }


        public List<SpellBase> BurrowedSpells = new List<SpellBase>();
        public List<SpellBase> UnburrowedSpells = new List<SpellBase>();

        public RekSai2()
        {
            ARAMSimulator.champBuild = new Build
            {
                coreItems = new List<ConditionalItem>
                {
                    new ConditionalItem(ItemId.Ludens_Echo),
                    new ConditionalItem(ItemId.Mercurys_Treads,ItemId.Ninja_Tabi,ItemCondition.ENEMY_AP),
                    new ConditionalItem(ItemId.The_Black_Cleaver),
                    new ConditionalItem(ItemId.Spirit_Visage, ItemId.Randuins_Omen, ItemCondition.ENEMY_AP),
                    new ConditionalItem(ItemId.Ravenous_Hydra_Melee_Only,(ItemId)3748),
                    new ConditionalItem(ItemId.Banshees_Veil, ItemId.Randuins_Omen, ItemCondition.ENEMY_AP),
                },
                startingItems = new List<ItemId>
                {
                    ItemId.Phage
                }
            };
            
            //DeathWalker.AfterAttack += afterAttack;
            Obj_AI_Base.OnProcessSpellCast += OnProcessSpell;
        }

        private void OnProcessSpell(Obj_AI_Base sender, GameObjectProcessSpellCastEventArgs args)
        {
            if (!sender.IsMe)
                return;
        }

        private void afterAttack(AttackableUnit unit, AttackableUnit target)
        {
            if (unit.IsMe && !player.IsInvulnerable
                && E1.IsReady()
                && player.ManaPercent > 99f)
            {
                E1.Cast((Obj_AI_Base)target);
            }
        }

        public override void useQ(Obj_AI_Base target)
        {
            if (!Q1.IsReady() || target == null || player.IsInvulnerable)
                return;
            Q1.Cast();
        }

        public void useQ2(Obj_AI_Base target)
        {
            if (!Q2.IsReady() || target == null || !player.IsInvulnerable)
                return;
            Q2.Cast(target);
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
            //if (!E.IsReady() || target == null || !player.IsInvulnerable || !safeGap(target))
            //    return;
           // E2.Cast(target);
        }

        public void useE2(Obj_AI_Base target)
        {
            if (!E2.IsReady() || target == null || !player.IsInvulnerable || !safeGap(target))
                return;
            E2.Cast(target);
        }

        public override void useR(Obj_AI_Base target)
        {
            
        }

        public override void useSpells()
        {
            //if (player.IsChannelingImportantSpell())
                //return;

            var tar = ARAMTargetSelector.getBestTarget(Q1.Range);
            if (tar != null) useQ(tar);
            tar = ARAMTargetSelector.getBestTarget(Q2.Range);
            if (tar != null) useQ2(tar);
            manageW();
            tar = ARAMTargetSelector.getBestTarget(E2.Range);
            if (tar != null) useE2(tar);


        }


        public override void setUpSpells()
        {
            Q1 = new Spell.Active(SpellSlot.Q, 325);
            UnburrowedSpells.Add(Q1);
            W = new Spell.Active(SpellSlot.W);
            UnburrowedSpells.Add(W);
            BurrowedSpells.Add(W);

            E1 = new Spell.Targeted(SpellSlot.E, 250);
            UnburrowedSpells.Add(E1);

            Q2 = new Spell.Skillshot(SpellSlot.Q, 1450, SkillShotType.Linear, 500, 1950, 60);
            BurrowedSpells.Add(Q2);
            E2 = new Spell.Skillshot(SpellSlot.E, 750, SkillShotType.Linear);
            BurrowedSpells.Add(E2);
        }
        
        public override void farm()
        {
            var minions = ObjectManager.Get<Obj_AI_Minion>().Where(x => x.IsEnemy && x.Distance(player.Position) <= 250f);
            if (minions.Count() >= 2
                && !player.IsInvulnerable
                && Q1.IsReady())
            {
                Q1.Cast();
            }
        }

        public void Combo()
        {
            var target = ARAMTargetSelector.getBestTarget(1400f);
            manageW();
            if (!player.IsInvulnerable
                && target.IsValidTarget(Q1.Range)
                && Q1.IsReady())
            {
                Q1.Cast();
            }
            if (player.IsInvulnerable
                && target.IsValidTarget(Q2.Range)
                && Q2.IsReady())
            {
                    (Q2 as Skillshot).CastMinimumHitchance(target, HitChance.High);
            }
            if (player.IsInvulnerable
                && target.IsValidTarget(E2.Range)
                && E2.IsReady())
            {
                (E2 as Skillshot).CastMinimumHitchance(target, HitChance.High);
            }

        }

        public void manageW()
        {
            var target = ARAMTargetSelector.getBestTarget(1400f);
            if (target == null) return;
            if (!player.IsInvulnerable)
            {
                if ( W.IsReady()
                        && player.HealthPercent < 9
                        && player.Mana > 0)
                {
                    W.Cast();
                }

                    if (W.IsReady() && !Q.IsOnCooldown && Q.IsReady())
                    {
                        W.Cast();
                    }

                    if (!E1.IsInRange(target)
                        && player.Distance(target.Position) < E2.Range
                        && W.IsReady())
                    {
                        W.Cast();
                   }
                
            }
            else if (player.IsInvulnerable)
            {
                if ( W.IsReady() && player.IsInAutoAttackRange(target))
                {
                    W.Cast();
                }
            }
        }
    }
}
