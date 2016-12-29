using System;
using System.Collections.Generic;
using System.Linq;
using SharpDX;
using EloBuddy;
using EloBuddy.SDK;
using EloBuddy.SDK.Enumerations;
using static EloBuddy.SDK.Spell;

namespace ARAMDetFull.Champions
{
    class Azir : Champion
    {
        public static List<Obj_AI_Minion> MySoldiers = new List<Obj_AI_Minion>();
        
        public Azir()
        {
            GameObject.OnCreate += OnCreateObject;
            GameObject.OnDelete += OnDeleteObject;

            ARAMSimulator.champBuild = new Build
            {
                coreItems = new List<ConditionalItem>
                        {
                            new ConditionalItem(ItemId.Nashors_Tooth),
                            new ConditionalItem(ItemId.Sorcerers_Shoes),
                            new ConditionalItem(ItemId.Rylais_Crystal_Scepter),
                            new ConditionalItem(ItemId.Rabadons_Deathcap),
                            new ConditionalItem(ItemId.Void_Staff),
                            new ConditionalItem(ItemId.Abyssal_Scepter,ItemId.Zhonyas_Hourglass,ItemCondition.ENEMY_AP),
                        },
                startingItems = new List<ItemId>
                        {
                            ItemId.Fiendish_Codex
                        }
            };
        }

        private static void OnCreateObject(GameObject sender, EventArgs args)
        {
            if (sender.Name == "AzirSoldier" && sender.IsAlly)
            {
                Obj_AI_Minion myMin = sender as Obj_AI_Minion;
                if (myMin.BaseSkinName == "AzirSoldier")
                    MySoldiers.Add(myMin);
            }

        }

        private static void OnDeleteObject(GameObject sender, EventArgs args)
        {
            int i = 0;
            foreach (var sol in MySoldiers)
            {
                if (sol.NetworkId == sender.NetworkId)
                {
                    MySoldiers.RemoveAt(i);
                    return;
                }
                i++;
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
        }

        public override void useR(Obj_AI_Base target)
        {
            if (target == null)
                return;
            if (R.IsReady())
            {
                R.Cast(target);
            }
        }

        public override void useSpells()
        {
            var targ = ARAMTargetSelector.getBestTarget(1000);
            if (targ == null)
                return;
            doAttack();
            useR(targ);
            castWTarget(targ);
            castQTarget(targ);
            if (targ.IsValidTarget(E.Range) && (targ.Health < E.GetDamage(targ) + Q.GetDamage(targ) + 100 || targ.Distance(ARAMSimulator.fromNex.Position, true) < player.Distance(ARAMSimulator.fromNex.Position, true)))
                castETarget(targ);

        }

        public override void setUpSpells()
        {
            Q = new Spell.Skillshot(SpellSlot.Q, 825, SkillShotType.Linear, 250, 1000, 70);
            W = new Spell.Skillshot(SpellSlot.W, 450, SkillShotType.Circular);
            E = new Spell.Skillshot(SpellSlot.E, 1200, SkillShotType.Linear, 250, 1600, 100);
            R = new Spell.Skillshot(SpellSlot.R, 300, SkillShotType.Linear, 500, 1000, 532);
        }

        public void doAttack()
        {
            List<AIHeroClient> enes = getEnemiesInSolRange();
            if (enes != null)
            {
                foreach (var ene in enes)
                {
                    if (Orbwalker.CanMove && Orbwalker.CanAutoAttack && solisAreStill())
                    {
                        //DeathWalker. = DeathWalker.now;
                        EloBuddy.Player.IssueOrder(GameObjectOrder.AttackUnit, ene);
                    }
                }
            }
        }

        public void castQTarget(Obj_AI_Base target)
        {
            if (!Q.IsReady())
                return;

            try
            {
                var midDel = getMiddleDelay(target);

                if (midDel != -1)
                {
                    PredictionResult po2 = Q.GetPrediction(target);//, midDel * 1.1f);
                    if (po2.HitChance > HitChance.Low)
                    {
                        Q.Cast(po2.UnitPosition);
                    }
                }
            }
            catch (Exception)
            {

            }
        }

        public void castWTarget(AIHeroClient target)
        {
            if (!W.IsReady() || (W as Skillshot).AmmoQuantity == 0)
                return;
            PredictionResult po = W.GetPrediction(target);//, 0.2f);
            W.Cast(po.UnitPosition);
        }

        public void castETarget(AIHeroClient target)
        {
            if (!E.IsReady())
                return;
            if (player.HealthPercent > 25 && !safeGap(target))
                return;

            List<Obj_AI_Minion> solis = getUsableSoliders().Where(sol => !sol.IsMoving).ToList();
            if (solis.Count == 0)
                return;
            foreach (var sol in solis)
            {
                float toSol = player.Distance(sol.Position);

                //Collision.GetCollision(new List<Vector3>{sol.Position},getMyEPred(sol));
                PredictionResult po = E.GetPrediction(target);//, toSol / 1500f);
                
                if (sol.Distance(po.UnitPosition) < 325 && interact(player.Position.To2D(), sol.Position.To2D(), po.UnitPosition.To2D(), 65)
                    && interactsOnlyWithTarg(target, sol, player.Distance(po.UnitPosition)))
                {
                    E.Cast(sol.Position);
                    return;
                }


                /*if (po.CollisionObjects.Count == 0)
                    continue;
                Console.WriteLine(po.CollisionObjects.Count);
                Obj_AI_Base col = po.CollisionObjects.OrderBy(obj => obj.Distance(Player.Position)).First();
                if (col.NetworkId == target.NetworkId)
                {
                    E.Cast(sol);
                    return;
                }*/

            }
        }

        public List<Obj_AI_Minion> getUsableSoliders()
        {
            return MySoldiers.Where(sol => !sol.IsDead).ToList();
        }

        public List<AIHeroClient> getEnemiesInSolRange()
        {
            List<Obj_AI_Minion> solis = getUsableSoliders();
            List<AIHeroClient> enemies = ObjectManager.Get<AIHeroClient>().Where(ene => ene.IsEnemy && ene.IsVisible && !ene.IsDead).ToList();
            List<AIHeroClient> inRange = new List<AIHeroClient>();

            if (solis.Count == 0)
                return null;
            foreach (var ene in enemies)
            {
                foreach (var sol in solis)
                {
                    if (ene.Distance(sol) < 350)
                    {
                        inRange.Add(ene);
                        break;
                    }
                }
            }
            return inRange;
        }

        public bool solisAreStill()
        {
            List<Obj_AI_Minion> solis = getUsableSoliders();
            foreach (var sol in solis)
            {
                if (sol.Spellbook.IsAutoAttacking)
                {
                    // Console.WriteLine("isAuta awdawdAWD");
                    return false;
                }
            }
            return true;
        }


        public bool interactsOnlyWithTarg(AIHeroClient target, Obj_AI_Base sol, float distColser)
        {
            foreach (var hero in EntityManager.Enemies.Where(obj => obj != null && obj.IsValid && obj.NetworkId != target.NetworkId))
            {
                float myDistToIt = player.Distance(hero);
                PredictionResult po = E.GetPrediction(hero);//, myDistToIt / 1500f);
                if (myDistToIt < distColser && interact(sol.Position.To2D(), player.Position.To2D(), po.UnitPosition.To2D(), 65))
                {
                    return false;
                }
            }
            return true;
        }

        public bool interact(Vector2 p1, Vector2 p2, Vector2 pC, float radius)
        {

            Vector2 p3 = new Vector2();
            p3.X = pC.X + radius;
            p3.Y = pC.Y + radius;
            float m = ((p2.Y - p1.Y) / (p2.X - p1.X));
            float Constant = (m * p1.X) - p1.Y;

            float b = -(2f * ((m * Constant) + p3.X + (m * p3.Y)));
            float a = (1 + (m * m));
            float c = ((p3.X * p3.X) + (p3.Y * p3.Y) - (radius * radius) + (2f * Constant * p3.Y) + (Constant * Constant));
            float D = ((b * b) - (4f * a * c));
            if (D > 0)
            {
                return true;
            }
            else
                return false;

        }

        public float getMiddleDelay(Obj_AI_Base target)
        {
            float allRange = 0;
            List<Obj_AI_Minion> solis = getUsableSoliders().Where(sol => (sol.Distance(target.ServerPosition) > 325
                || sol.Distance(Q.GetPrediction(target/*, 0.7f*/).UnitPosition) > 325)).ToList();
            if (solis.Count == 0)
                return -1;
            foreach (var sol in solis)
            {
                float dist = sol.Distance(target.ServerPosition);
                allRange += dist;
            }
            return (allRange / (1500f * solis.Count));
        }
    }
}