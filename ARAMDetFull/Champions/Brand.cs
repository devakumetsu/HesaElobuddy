using EloBuddy;
using EloBuddy.SDK;
using EloBuddy.SDK.Enumerations;
using SharpDX;
using System.Collections.Generic;
using System.Linq;
using static EloBuddy.SDK.Spell;

namespace ARAMDetFull.Champions
{
    internal class Brand : Champion
    {
        public readonly List<SpellBase> spellList = new List<SpellBase>();

        private const int BOUNCE_RADIUS = 450;

        public Brand()
        {
            ARAMSimulator.champBuild = new Build
            {
                coreItems = new List<ConditionalItem>
                {
                    new ConditionalItem(ItemId.Morellonomicon),
                    new ConditionalItem(ItemId.Sorcerers_Shoes),
                    new ConditionalItem(ItemId.Rylais_Crystal_Scepter),
                    new ConditionalItem(ItemId.Liandrys_Torment),
                    new ConditionalItem(ItemId.Rabadons_Deathcap),
                    new ConditionalItem(ItemId.Void_Staff),
                },
                startingItems = new List<ItemId>
                {
                    ItemId.Boots_of_Speed,
                    ItemId.Lost_Chapter
                }
            };
        }

        public override void useQ(Obj_AI_Base target)
        {
        }

        public override void useW(Obj_AI_Base target)
        {
        }

        public override void useE(Obj_AI_Base target)
        {
        }

        public override void useR(Obj_AI_Base target)
        {
        }

        public override void setUpSpells()
        {
            Q = new Skillshot(SpellSlot.Q, 1100, SkillShotType.Linear, 250, 60, 60);
            W = new Skillshot(SpellSlot.W, 900, SkillShotType.Circular, 100, 240, 240);
            E = new Targeted(SpellSlot.E, 625, DamageType.Magical);
            R = new Targeted(SpellSlot.R, 750, DamageType.Magical);
        }

        public override void useSpells()
        {
            var tar = ARAMTargetSelector.getBestTarget(W.Range + 240);
            if (tar == null || tar.Type != GameObjectType.AIHeroClient)
            {
                OnWaveClear();
            }
            else
            {
                OnCombo();
            }
        }

        private void OnWaveClear()
        {
            if (player.ManaPercent < 43)
                return;

            // Minions around
            var minions = EntityManager.MinionsAndMonsters.EnemyMinions.Where(x => Player.Instance.IsInRange(x, W.Range + 240 / 2));

            // Spell usage
            bool useQ = Q.IsReady();
            bool useW = W.IsReady();
            bool useE = E.IsReady();

            if (useQ)
            {
                // Loop through all minions to find a target, preferred a killable one
                Obj_AI_Base target = null;
                foreach (var minion in minions)
                {
                    var prediction = (Q as Spell.Skillshot).GetPrediction(minion);
                    if (prediction.HitChance == HitChance.High)
                    {
                        // Set target
                        target = minion;

                        // Break if killlable
                        if (minion.Health > player.GetAutoAttackDamage(minion) && IsSpellKillable(minion, SpellSlot.Q))
                            break;
                    }
                }

                // Cast if target found
                if (target != null)
                    Q.Cast(target);
            }

            if (useW)
            {
                // Get farm location
                //var farmLocation = MinionManager.GetBestCircularFarmLocation(minions.Select(minion => minion.ServerPosition.To2D()).ToList(), W.Width, W.Range);

                // Check required hitnumber and cast
                //if (farmLocation.MinionsHit >= 2)
                //W.Cast(farmLocation.Position);
            }

            if (useE)
            {
                // Loop through all minions to find a target
                foreach (var minion in minions)
                {
                    // Distance check
                    if (minion.ServerPosition.Distance(player.Position, true) < E.Range * E.Range)
                    {
                        // E only on targets that are ablaze or killable
                        if (IsAblazed(minion) || minion.Health > player.GetAutoAttackDamage(minion) && IsSpellKillable(minion, SpellSlot.E))
                        {
                            E.Cast(minion);
                            break;
                        }
                    }
                }
            }
        }

        private void OnCombo()
        {
            // Target aquireing
            var target = ARAMTargetSelector.getBestTarget(W.Range + 240);

            // Target validation
            if (target == null)
            {
                OnWaveClear();
                return;
            }

            if (!Orbwalker.IsAutoAttacking || Orbwalker.GetTarget() != target)
                Player.IssueOrder(GameObjectOrder.AttackUnit, target);

            // Spell usage
            bool useQ = true;
            bool useW = true;
            bool useE = true;
            bool useR = true;
            // Add to spell list
            spellList.AddRange(new[] { Q, W, E, R });

            // Killable status
            bool mainComboKillable = IsMainComboKillable(target);
            bool bounceComboKillable = IsBounceComboKillable(target);
            bool inMinimumRange = target.ServerPosition.Distance(player.Position, true) < E.Range * E.Range;

            // Ignite auto cast if killable, bitch please

            // Continue if spell not ready

            // Q
            if (Q.IsReady() && useQ)
            {
                if ((mainComboKillable && inMinimumRange) || // Main combo killable
                    (!useW && !useE) || // Casting when not using W and E
                    (IsAblazed(target)) || // Ablazed
                    (player.GetSpellDamage(target, SpellSlot.Q) > target.Health) || // Killable
                    (useW && !useE && !W.IsReady() && W.IsReady((uint)(player.Spellbook.GetSpell(SpellSlot.Q).Cooldown * 1000))) || // Cooldown substraction W ready
                    ((useE && !useW || useW && useE) && !E.IsReady() && E.IsReady((uint)(player.Spellbook.GetSpell(SpellSlot.Q).Cooldown * 1000)))) // Cooldown substraction E ready
                {
                    // Cast Q on high hitchance
                    var pred = (Q as Skillshot).GetPrediction(target);
                    //if (pred.HitChance == HitChance.High)
                    {
                        Q.Cast(pred.CastPosition);
                    }
                }
            }
            // W
            if (W.IsReady() && useW)
            {
                Chat.Print("Check if we can cast W on " + target.Name);
                if ((mainComboKillable && inMinimumRange) || // Main combo killable
                    (!useE) || // Casting when not using E
                    (IsAblazed(target)) || // Ablazed
                    (player.GetSpellDamage(target, SpellSlot.W) > target.Health) || // Killable
                    (target.ServerPosition.Distance(player.Position, true) > E.Range * E.Range) ||
                    (!E.IsReady() && E.IsReady((uint)(player.Spellbook.GetSpell(SpellSlot.W).Cooldown * 1000)))) // Cooldown substraction E ready
                {
                    // Cast W on high hitchance
                    if (player.Mana < 130)
                    {
                        if (target.Type == GameObjectType.AIHeroClient)
                        {
                            var pred = (W as Skillshot).GetPrediction(target);
                            if (pred.HitChance == HitChance.High)
                            {
                                W.Cast(pred.CastPosition);
                            }
                        }
                    }
                    else
                    {
                        (W as Skillshot).CastMinimumHitchance(target, HitChance.High);
                    }
                }
            }
            // E
            if (E.IsReady() && useE)
            {
                // Distance check
                if (Vector2.DistanceSquared(target.ServerPosition.To2D(), player.Position.To2D()) < E.Range * E.Range)
                {
                    if ((mainComboKillable) || // Main combo killable
                        (!useQ && !useW) || // Casting when not using Q and W
                        (E.Level >= 4) || // E level high, damage output higher
                        (useQ && (Q.IsReady() || player.Spellbook.GetSpell(SpellSlot.Q).Cooldown < 5)) || // Q ready
                        (useW && W.IsReady())) // W ready
                    {
                        // Cast E on target
                        E.Cast(target);
                    }
                }
            }
            // R
            if (R.IsReady() && useR)
            {
                // Distance check
                if (target.Type == GameObjectType.AIHeroClient && target.ServerPosition.Distance(player.Position, true) < R.Range * R.Range)
                {
                    if (GoodBounceTarget(target))
                    {
                        R.Cast(target);
                        return;
                    }

                    // Logic prechecks
                    if ((useQ && Q.IsReady() && (Q as Skillshot).GetPrediction(target).HitChance == HitChance.High || useW && W.IsReady()) && player.Health / player.MaxHealth > 0.4f)
                        return;

                    // Single hit
                    if (mainComboKillable && inMinimumRange || player.GetSpellDamage(target, SpellSlot.R) > target.Health)
                        R.Cast(target);
                    // Double bounce combo
                    else if (bounceComboKillable && inMinimumRange || player.GetSpellDamage(target, SpellSlot.R) * 2 > target.Health)
                    {
                        if (ObjectManager.Get<Obj_AI_Base>().Count(enemy => (enemy.Type == GameObjectType.obj_AI_Minion || enemy.NetworkId != target.NetworkId && enemy.Type == GameObjectType.AIHeroClient) && enemy.IsValidTarget() && enemy.ServerPosition.Distance(target.ServerPosition, true) < BOUNCE_RADIUS * BOUNCE_RADIUS) > 0)
                            R.Cast(target);
                    }
                }
            }
        }

        public bool GoodBounceTarget(Obj_AI_Base target)
        {
            return target.CountEnemiesInRange(380) > 1;
        }

        // TODO: DFG handling and so on :P
        public double GetMainComboDamage(Obj_AI_Base target)
        {
            double damage = player.GetAutoAttackDamage(target);

            if (Q.IsReady())
                damage += player.GetSpellDamage(target, SpellSlot.Q);

            if (W.IsReady())
                damage += player.GetSpellDamage(target, SpellSlot.W) * (IsAblazed(target) ? 2 : 1);

            if (E.IsReady())
                damage += player.GetSpellDamage(target, SpellSlot.E);

            if (R.IsReady())
                damage += player.GetSpellDamage(target, SpellSlot.R);

            return damage;
        }

        public bool IsMainComboKillable(Obj_AI_Base target)
        {
            return GetMainComboDamage(target) > target.Health;
        }

        public double GetBounceComboDamage(Obj_AI_Base target)
        {
            double damage = GetMainComboDamage(target);

            if (R.IsReady())
                damage += player.GetSpellDamage(target, SpellSlot.R);

            return damage;
        }

        public bool IsBounceComboKillable(Obj_AI_Base target)
        {
            return GetBounceComboDamage(target) > target.Health;
        }

        public static bool IsAblazed(Obj_AI_Base target)
        {
            return target.HasBuff("brandablaze");
        }

        public static bool IsSpellKillable(Obj_AI_Base target, SpellSlot spellSlot)
        {
            return player.GetSpellDamage(target, spellSlot) > target.Health;
        }

        public static bool HasIgnite(AIHeroClient target)
        {
            if (target.IsMe)
            {
                var ignite = player.Spellbook.GetSpell(ObjectManager.Player.GetSpellSlotFromName("summonerdot"));
                return ignite != null && ignite.Slot != SpellSlot.Unknown;
            }
            return false;
        }
    }
}