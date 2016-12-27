using System.Collections.Generic;
using System.Linq;
using SharpDX;
using EloBuddy;
using EloBuddy.SDK;
using EloBuddy.SDK.Enumerations;
using System;

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
            if (!W.IsReady() || target == null)
                return;
            //HealLogic(target);

            foreach (var ally in EntityManager.Heroes.Allies.Where(x => x.HealthPercent < 20 && EnemyInRange(x)).OrderBy(x => !x.IsMe).ThenBy(x => x.MaxHealth))
            {
                if (!ally.IsInShopRange() && !ally.IsRecalling())
                    W.Cast(ally);
            }

            WCombo((AIHeroClient) target);
        }

        public override void useE(Obj_AI_Base target)
        {
            if (!E.IsReady() || target == null)
                return;
            if ((target.HasBuffOfType(BuffType.Poison)))
            {
                E.Cast(target);
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
                W.Cast(ally);
                return;
            }
            if (target == null)
                return;
            if (player.Distance(target) > W.Range) // target out of range try bounce
            {
                var bounceTarget = EntityManager.Heroes.AllHeroes.SingleOrDefault(hero => hero.IsValidTarget(W.Range) && hero.Distance(target) < W.Range);
                if (bounceTarget != null && bounceTarget.MaxHealth - bounceTarget.Health > WHeal) // use bounce & heal
                {
                    W.Cast(bounceTarget);
                }
            }
            else // target in range
            {
                W.Cast(target);
            }
        }

        bool EnemyInRange(AIHeroClient ally)
        {
            return EntityManager.Heroes.Enemies.Any(x => x.Distance(ally) <= ally.AttackRange && x.IsValid);
        }

        private void WCombo(AIHeroClient target)
        {
            if (!W.IsReady()) return;
            int enemyHitVal = 5, targetHitVal = 2, allyHealBounceVal = 4, lowAllyVal = 10, allyFullHp = -3, enemyKill = int.MaxValue;

            float bestValue = -1;
            AIHeroClient bestTarget = null;

            foreach (var hero in EntityManager.Heroes.AllHeroes.Where(x => x.IsValid && x.Distance(player) <= W.Range))
            {
                float currentValue = 0;
                float flyTime = W.CastDelay + (player.Distance(hero) / 2000) * 1000;//2000 = W speed

                if (hero.IsAlly)
                {
                    if (hero.IsRecalling() || hero.IsInShopRange())
                        continue;

                    if (isEnemyHit(hero, flyTime))
                        currentValue += enemyHitVal;

                    if (hero.HealthPercent <= 30)
                        currentValue += lowAllyVal;

                    if (hero.HealthPercent >= 75)
                        currentValue += allyFullHp;
                }
                else
                {
                    if (isAllyHit(hero, flyTime))
                        currentValue += allyHealBounceVal;

                    if (hero == target)
                        currentValue += targetHitVal;

                    if (player.GetSpellDamage(hero, SpellSlot.W) > hero.Health)
                        currentValue += enemyKill;
                    else if (player.GetSpellDamage(hero, SpellSlot.W) > hero.Health + player.GetAutoAttackDamage(hero))
                        currentValue += enemyKill * 0.75f;

                    if (EntityManager.Heroes.Allies.All(x => x.Distance(hero) > x.AttackRange) &&
                        player.Distance(hero) <= W.Range)
                        currentValue += 100;
                }

                if (currentValue > bestValue)
                {
                    bestValue = currentValue;
                    bestTarget = hero;
                }
                else if (Math.Abs(currentValue - bestValue) <= 0.1f)
                {
                    if (hero.IsAlly)
                    {
                        bestValue = currentValue;
                        bestTarget = hero;
                    }
                }
            }

            bool canKill = bestTarget.IsAlly ? true : player.GetSpellDamage(bestTarget, SpellSlot.W) > bestTarget.Health;

            if (bestTarget.IsValid)
                W.Cast(bestTarget);
            //else Chat.Print("no w target");
        }

        bool isEnemyHit(AIHeroClient source, float delay)
        {
            return
                EntityManager.Heroes.Enemies.Where(x => x.IsValid && x.Distance(source) <= 1500).
                    Select(enemy => Prediction.Position.PredictUnitPosition(enemy, (int)delay)).
                        Any(movePrd => movePrd.Distance(source) <= W.Range - 100);
        }

        bool isAllyHit(AIHeroClient enemy, float delay)
        {
            return
                EntityManager.Heroes.Allies.Where(x => x.IsValid && x.Distance(enemy) <= 1500).
                    Select(ally => Prediction.Position.PredictUnitPosition(ally, (int)delay)).
                        Any(movePrd => movePrd.Distance(enemy) <= W.Range - 100);
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
