using System.Collections.Generic;
using System.Linq;
using SharpDX;
using EloBuddy;
using EloBuddy.SDK;
using EloBuddy.SDK.Enumerations;

namespace ARAMDetFull.Champions
{
    class Nami : Champion
    {
        public Nami()
        {
            Orbwalker.OnPreAttack += Orbwalker_OnPreAttack;
            ARAMSimulator.champBuild = new Build
            {
                coreItems = new List<ConditionalItem>
                {
                    new ConditionalItem(ItemId.Athenes_Unholy_Grail),
                    new ConditionalItem(ItemId.Sorcerers_Shoes),
                    new ConditionalItem(ItemId.Rabadons_Deathcap),
                    new ConditionalItem(ItemId.Morellonomicon),
                    new ConditionalItem(ItemId.Rylais_Crystal_Scepter,ItemId.Locket_of_the_Iron_Solari,ItemCondition.ENEMY_LOSING),
                    new ConditionalItem(ItemId.Ludens_Echo,ItemId.Ardent_Censer,ItemCondition.ENEMY_LOSING),
                },
                startingItems = new List<ItemId>
                {
                    ItemId.Chalice_of_Harmony,ItemId.Boots_of_Speed
                }
            };
        }

        private void Orbwalker_OnPreAttack(AttackableUnit target, Orbwalker.PreAttackArgs args)
        {
            if (E.IsReady() && args.Target is AIHeroClient && args.Target.IsAlly && args.Target is AIHeroClient && E.IsInRange((Obj_AI_Base) args.Target))
            {
                E.Cast((Obj_AI_Base) args.Target);
            }
        }

        public override void useQ(Obj_AI_Base target)
        {
            if (!Q.IsReady() || target == null)
                return;
            if (target.IsValidTarget(Q.Range + 100))
                Q.Cast(target);
        }

        public override void useW(Obj_AI_Base target)
        {
            //if (!W.IsReady() || target == null)
           //     return;
            HealLogic(target);
        }

        public override void useE(Obj_AI_Base target)
        {
            if (!E.IsReady() || target == null)
                return;
            if ((target.HasBuffOfType(BuffType.Poison)))
            {
                E.CastOnUnit(target);
            }
        }

        public override void useR(Obj_AI_Base target)
        {
            if (target == null)
                return;
            if (target.IsValidTarget(R.Range) && R.IsReady())
            {
                if(R.CastIfWillHitReturn(target, 2))
                    Aggresivity.addAgresiveMove(new AgresiveMove(55, 4000, true));
            }
        }

        public override void useSpells()
        {
            var tar = ARAMTargetSelector.getBestTarget(Q.Range);
            useQ(tar);
            tar = ARAMTargetSelector.getBestTarget(W.Range+400);
            useW(tar);
            tar = ARAMTargetSelector.getBestTarget(E.Range);
            useE(tar);
            tar = ARAMTargetSelector.getBestTarget(R.Range);
            useR(tar);
        }

        
        public override void setUpSpells()
        {
            //Initialize spells.
            Q = new Spell.Skillshot(SpellSlot.Q, 875, SkillShotType.Circular, 500, int.MaxValue, 150);
            W = new Spell.Targeted(SpellSlot.W, 725);
            E = new Spell.Targeted(SpellSlot.E, 800);
            R = new Spell.Skillshot(SpellSlot.R, 2750, SkillShotType.Linear, 250, 500, 160);
        }

        private double WHeal
        {
            get
            {
                int[] heal = { 0, 65, 95, 125, 155, 185 };
                return heal[W.Level] + player.FlatMagicDamageMod * 0.3;
            }
        }

        private void HealLogic(Obj_AI_Base target)
        {
            var ally = AllyBelowHp((player.ManaPercent>70)?70:35, W.Range);
            if (ally != null) // force heal low ally
            {
                W.CastOnUnit(ally);
                return;
            }
            if (target == null)
                return;
            if (player.Distance(target) > W.Range) // target out of range try bounce
            {
                var bounceTarget = EntityManager.Heroes.AllHeroes.SingleOrDefault(hero => hero.IsValidTarget(W.Range) && hero.Distance(target) < W.Range);

                if (bounceTarget != null && bounceTarget.MaxHealth - bounceTarget.Health > WHeal) // use bounce & heal
                {
                    W.CastOnUnit(bounceTarget);
                }
            }
            else // target in range
            {
                W.CastOnUnit(target);
            }
        }

        public static AIHeroClient AllyBelowHp(int percentHp, float range)
        {
            foreach (var ally in ObjectManager.Get<AIHeroClient>())
            {
                if (ally.IsMe)
                {
                    if (((ObjectManager.Player.Health / ObjectManager.Player.MaxHealth) * 100) < percentHp)
                    {
                        return ally;
                    }
                }
                else if (ally.IsAlly)
                {
                    if (Vector3.Distance(ObjectManager.Player.Position, ally.Position) < range &&
                        ((ally.Health / ally.MaxHealth) * 100) < percentHp)
                    {
                        return ally;
                    }
                }
            }

            return null;
        }
    }
}
