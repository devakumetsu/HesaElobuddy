using System;
using System.Collections.Generic;
using System.Linq;
using SharpDX;
using EloBuddy;
using EloBuddy.SDK;
using EloBuddy.SDK.Enumerations;

namespace ARAMDetFull.Champions
{
    class Zed : Champion
    {

        public float lastW = 0;

        public override void useQ(Obj_AI_Base target)
        {
            if (!Q.IsReady())
                return;
            Q.Cast(target);
        }

        public override void useW(Obj_AI_Base target)
        {
            if (!W.IsReady() || shadowW != null || lastW+1000> Game.Time)
                return;
            lastW = Game.Time;
            W.Cast(target.Position);
        }

        public override void useE(Obj_AI_Base target)
        {
            if (!E.IsReady() || W.IsReady())
                return;
            E.Cast();
        }

        public override void useR(Obj_AI_Base target)
        {
            return;
            if (!R.IsReady())
                return;
            if (player.Path.Length > 0 && player.Path[player.Path.Length - 1].Distance(player.Position) > 2500)
            {
                R.Cast(player.Path[player.Path.Length - 1]);
            }


        }

        public override void setUpSpells()
        {
            //Initialize spells.
            Q = new Spell.Skillshot(SpellSlot.Q, 925, SkillShotType.Linear, 250, 1700, 50, DamageType.Physical);
            W = new Spell.Skillshot(SpellSlot.W, 700, SkillShotType.Linear, 250, 1750, 60, DamageType.Physical);
            E = new Spell.Active(SpellSlot.E, 280, DamageType.Physical);
            R = new Spell.Targeted(SpellSlot.R, 625, DamageType.Physical);
        }

        public override void useSpells()
        {
            var tar = ARAMTargetSelector.getBestTarget(600);
            if (tar == null)
                return;

            if (!Sector.inTowerRange(tar.Position.To2D()) && (getFullComboDmg(tar) > tar.Health || player.HealthPercent < 25))
            {
                if (tFocus == null) tFocus = tar;
                doLineCombo(tFocus);
            }
            else
            {
                tFocus = null;
                tar = ARAMTargetSelector.getBestTarget(Q.Range);
                if (tar != null) useQ(tar);
                tar = ARAMTargetSelector.getBestTarget(W.Range);
                if (tar != null) useW(tar);
                tar = ARAMTargetSelector.getBestTarget(E.Range);
                if (tar != null) useE(tar);
            }
        }

        public override void farm()
        {
            if (player.ManaPercent < 75 || !Q.IsReady())
                return;

            foreach (var minion in EntityManager.MinionsAndMonsters.EnemyMinions.Where(x => !x.IsDead && x.IsInRange(player, Q.Range - 50)).ToList())
            {
                if (minion.Health < Q.GetDamage(minion))
                {
                    Q.Cast(minion);
                    return;
                }
            }
        }

        public static SummonerItems sumItems;

        public static AIHeroClient tFocus = null;

        public static Obj_AI_Minion shadowW;
        public static bool getRshad;
        public static bool getWshad;
        public static Obj_AI_Minion shadowR;
        public static float LastWCast;

        public static bool wIsCasted = false;
        public static bool serverTookWCast = false;
        public static double recast = 0;

        public Zed()
        {
            sumItems = new SummonerItems(ObjectManager.Player);
            GameObject.OnCreate += OnCreateObject;
            GameObject.OnDelete += OnDeleteObject;

            ARAMSimulator.champBuild = new Build
            {
                coreItems = new List<ConditionalItem>
                {
                    new ConditionalItem(ItemId.Blade_of_the_Ruined_King),
                    new ConditionalItem(ItemId.Mercurys_Treads),
                    new ConditionalItem(ItemId.Last_Whisper),
                    new ConditionalItem(ItemId.Ravenous_Hydra_Melee_Only),
                    new ConditionalItem(ItemId.Maw_of_Malmortius, ItemId.The_Bloodthirster, ItemCondition.ENEMY_AP),
                    new ConditionalItem(ItemId.Banshees_Veil),
                },
                startingItems = new List<ItemId>
                {
                    ItemId.Vampiric_Scepter,ItemId.Boots_of_Speed
                }
            };
        }

        private static void OnDeleteObject(GameObject sender, EventArgs args) {
            if (Zed.shadowR != null && sender.NetworkId == Zed.shadowR.NetworkId) {
                Zed.shadowR = null;
                Zed.getRshad = false;
            }

            if (Zed.shadowW != null && sender.NetworkId == Zed.shadowW.NetworkId) {
                Zed.shadowW = null;
                Zed.getWshad = false;
            }
        }

        private static void OnCreateObject(GameObject sender, EventArgs args)
        {
            /* if (sender.Name.Equals("Zed_ShadowIndicatorNEARBloop.troy") && Zed.isSafeSwap(Zed.shadowR) &&
                menu.Item("SwapRKill").GetValue<bool>()) {
                if (Zed.canGoToShadow("R")) {
                    Zed.R.Cast(); TODO fixerino
                }
            }*/
            if (sender is Obj_AI_Minion)
            {
                var min = sender as Obj_AI_Minion;
                if (min.IsAlly && min.BaseSkinName == "ZedShadow")
                {
                    if (Zed.getRshad)
                    {
                        // Game.PrintChat("R Create");
                        Zed.shadowR = min;
                        Zed.getRshad = false;
                    }
                    if (Zed.getWshad)
                    {
                        //Game.PrintChat("W Created");
                        Zed.shadowW = min;
                        Zed.getWshad = false;
                    }
                }
            }
        }

        public float getFullComboDmg(AIHeroClient target)
        {
            if (target == null)
                return 0;
            float dmg = 0;
            var po = Q.GetPrediction(target);
            float dist = player.Distance(po.UnitPosition);
            float gapDist = ((W.IsReady()) ? W.Range : 0);
            float distAfterGap = dist - gapDist;

            if (distAfterGap < player.AttackRange)
                dmg += (float)player.GetAutoAttackDamage(target);
            if (Q.IsReady() && distAfterGap < Q.Range)
                dmg += Q.GetDamage(target);
            if (Q.IsReady() && W.IsReady() && distAfterGap < Q.Range && dist < Q.Range)
                dmg += Q.GetDamage(target) / 2;
            if (distAfterGap < E.Range)
                dmg += E.GetDamage(target);
            if (R.IsReady() && distAfterGap < R.Range)
            {
                dmg += R.GetDamage(target);
                dmg += (float)player.CalculateDamageOnUnit(target, DamageType.Physical, (dmg * (5 + 15 * R.Level) / 100));
            }
            if (Item.HasItem(ItemId.Blade_of_the_Ruined_King)) // botrk
                dmg += (float)player.GetItemDamage(target, ItemId.Blade_of_the_Ruined_King);
            if (Item.HasItem(ItemId.Titanic_Hydra))
                dmg += (float)player.GetItemDamage(target, ItemId.Titanic_Hydra);
            if (Item.HasItem(ItemId.Tiamat))
                dmg += (float)player.GetItemDamage(target, ItemId.Tiamat);
            if (Item.HasItem(ItemId.Bilgewater_Cutlass))
                dmg += (float)player.GetItemDamage(target, ItemId.Bilgewater_Cutlass);

            return dmg;
        }

        private bool canDoCombo(IEnumerable<SpellSlot> sp)
        {
            float delay = sp.Sum(sp1 => player.Spellbook.GetSpell(sp1).SData.SpellCastTime); //Hope it is correct
            float totalCost = sp.Sum(sp1 => player.Spellbook.GetSpell(sp1).SData.Mana);
            return player.Mana + delay * 5 >= totalCost;
        }

        public void doLineCombo(Obj_AI_Base target)
        {
            if (target == null)
                return;

            try
            {
                //Tried to Add shadow Coax
                float dist = player.Distance(target);
                if (R.IsReady() && shadowR == null && dist < R.Range &&
                    canDoCombo(new[] { SpellSlot.Q, SpellSlot.W, SpellSlot.E, SpellSlot.R }))
                {
                    R.Cast(target);
                    Aggresivity.addAgresiveMove(new AgresiveMove(25,3000,true));
                }
                //eather casts 2 times or 0 get it to cast 1 time TODO
                // Game.PrintChat("W2 "+ZedSharp.W2);
                /*foreach (
                AIHeroClient newtarget in
                ObjectManager.Get<AIHeroClient>().Where(hero => hero.IsValidTarget(Q.Range)).Where(
                enemy => enemy.HasBuff("zedulttargetmark") && enemy.IsEnemy && !enemy.IsMinion)) {
                target = newtarget;
                }*/
                //PredictionOutput p1o = Prediction.GetPrediction(target, 0.350f);
                Vector3 shadowPos = target.Position + Vector3.Normalize(target.Position - shadowR.Position) * E.Range;
                if (W.IsReady() && shadowW == null &&
                    ((!getWshad && recast < Game.Time && !serverTookWCast)))
                {
                    //V2E(shadowR.Position, po.UnitPosition, E.Range)
                    Console.WriteLine("cast WWW");
                    W.Cast(shadowPos);
                    serverTookWCast = false;
                    wIsCasted = true;
                    recast = Game.Time + 300;
                }
                if (E.IsReady() && shadowW != null || shadowR != null)
                {
                    E.Cast();
                }
                if (Q.IsReady() && shadowW != null && shadowR != null)
                {
                    float midDist = dist;
                    midDist += target.Distance(shadowR);
                    midDist += target.Distance(shadowW);
                    float delay = midDist / (Q.Speed() * 3);
                    var po = Q.GetPrediction(target);
                    if (po.HitChance > HitChance.Low)
                    {
                        // Console.WriteLine("Cast QQQQ");
                        Q.Cast(po.UnitPosition);
                    }
                }
                if (shadowR != null)
                {
                    castItemsFull(target);
                }
            }
            catch (Exception ex)
            {
                //Console.WriteLine(ex);
            }
        }

        private void castItemsFull(Obj_AI_Base target)
        {
            if (target.Distance(player) < 500)
            {
                sumItems.cast(SummonerItems.ItemIds.Ghostblade);
                sumItems.castIgnite((AIHeroClient)target);
            }
            if (target.Distance(player) < 500)
            {
                sumItems.cast(SummonerItems.ItemIds.BotRK, target);
                sumItems.cast(SummonerItems.ItemIds.Cutlass, target);
            }
            if (target.Distance(player.ServerPosition) < (400 + target.BoundingRadius - 20))
            {
                sumItems.cast(SummonerItems.ItemIds.Tiamat);
                sumItems.cast(SummonerItems.ItemIds.Hydra);
            }
        }
    }
}
