using System;
using System.Collections.Generic;
using System.Linq;
using SharpDX;
using EloBuddy;
using EloBuddy.SDK;
using EloBuddy.SDK.Enumerations;

namespace ARAMDetFull.Champions
{
    class Gangplank : Champion
    {
        public const int BarrelExplosionRange = 345;
        public const int BarrelConnectionRange = 640;
        public bool justE = false, justQ = false;
        public List<Barrel> savedBarrels = new List<Barrel>();
        public Vector3 ePos;

        internal class Barrel
        {
            public Obj_AI_Minion barrel;
            public float time;

            public Barrel(Obj_AI_Minion objAiBase, int tickCount)
            {
                barrel = objAiBase;
                time = tickCount;
            }
        }

        public Gangplank()
        {
            GameObject.OnCreate += GameObjectOnOnCreate;
            GameObject.OnDelete += GameObject_OnDelete;
            Obj_AI_Base.OnProcessSpellCast += processSpells;

            ARAMSimulator.champBuild = new Build
            {
                coreItems = new List<ConditionalItem>
                {
                    new ConditionalItem(ItemId.Trinity_Force),
                    new ConditionalItem(ItemId.Ionian_Boots_of_Lucidity),
                    new ConditionalItem(ItemId.Youmuus_Ghostblade),
                    new ConditionalItem(ItemId.Spirit_Visage,ItemId.Randuins_Omen,ItemCondition.ENEMY_AP),
                    new ConditionalItem(ItemId.Infinity_Edge),
                    new ConditionalItem(ItemId.Banshees_Veil),
                },
                startingItems = new List<ItemId>
                {
                    ItemId.Phage
                }
            };
        }

        private void processSpells(Obj_AI_Base sender, GameObjectProcessSpellCastEventArgs args)
        {
            if (sender.IsMe)
            {
                if (args.SData.Name == "GangplankQWrapper")
                {
                    if (!justQ)
                    {
                        justQ = true;
                        Core.DelayAction(() => justQ = false, 200);
                    }
                }
                if (args.SData.Name == "GangplankE")
                {
                    ePos = args.End;
                    if (!justE)
                    {
                        justE = true;
                        Core.DelayAction(() => justE = false, 500);
                    }
                }
            }
        }

        private void GameObject_OnDelete(GameObject sender, EventArgs args)
        {
            for (int i = 0; i < savedBarrels.Count; i++)
            {
                if (savedBarrels[i].barrel.NetworkId == sender.NetworkId)
                {
                    savedBarrels.RemoveAt(i);
                    return;
                }
            }
        }

        private float getEActivationDelay()
        {
            if (player.Level >= 13)
            {
                return 0.5f;
            }
            if (player.Level >= 7)
            {
                return 1f;
            }
            return 2f;
        }

        private void GameObjectOnOnCreate(GameObject sender, EventArgs args)
        {
            if (sender.Name == "Barrel")
            {
                savedBarrels.Add(new Barrel(sender as Obj_AI_Minion, System.Environment.TickCount));
            }
        }

        private float GetQTime(Obj_AI_Base targetB)
        {
            return player.Distance(targetB) / 2800f + Q.CastDelay;
        }

        private bool KillableBarrel(Obj_AI_Base targetB)
        {
            if (targetB.Health < 2)
            {
                return true;
            }
            var barrel = savedBarrels.FirstOrDefault(b => b.barrel.NetworkId == targetB.NetworkId);
            if (barrel != null)
            {
                var time = targetB.Health * getEActivationDelay() * 1000;
                if (Environment.TickCount - barrel.time + GetQTime(targetB) * 1000 > time)
                {
                    return true;
                }
            }
            return false;
        }


        public override void useQ(Obj_AI_Base target)
        {
            if (!Q.IsReady() || target == null)
                return;
            Q.Cast(target);
        }

        public override void useW(Obj_AI_Base target)
        {
            if (!W.IsReady() || target == null)
                return;
            if (!Q.IsReady(3500) && player.Mana > 150)
                W.Cast();
        }

        public override void useE(Obj_AI_Base target)
        {
            if (!E.IsReady() || target == null)
                return;
            E.Cast(target);
        }

        public override void useR(Obj_AI_Base target)
        {
            if (target == null || !R.IsReady())
                return;
            R.CastIfWillHit(target, 3);
        }

        public override void useSpells()
        {
            try
            {

                Combo();
                if (player.HealthPercent < 40 && W.IsReady())
                    W.Cast();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
        }


        public override void setUpSpells()
        {
            //Initialize Spells.
            Q = new Spell.Targeted(SpellSlot.Q, 625, DamageType.Physical)
            {
                CastDelay = 250,
            };
            W = new Spell.Active(SpellSlot.W);
            E = new Spell.Skillshot(SpellSlot.E, 1000, SkillShotType.Circular, 250, null, 690);
            R = new Spell.Skillshot(SpellSlot.R, 20000, SkillShotType.Circular, 250, null, 1050, DamageType.Physical);
        }

        public override void farm()
        {
            if (Q.IsReady())
            {
                
                var mini =
                    EntityManager.MinionsAndMonsters.GetLaneMinions(EntityManager.UnitTeam.Enemy, ObjectManager.Player.ServerPosition, Q.Range)
                        .Where(m => m.Health < Q.GetDamage(m) && m.BaseSkinName != "GangplankBarrel")
                        .OrderByDescending(m => m.MaxHealth)
                        .ThenByDescending(m => m.Distance(player))
                        .FirstOrDefault();

                if (mini != null)
                {
                    Q.Cast(mini);
                }
            }
        }

        private IEnumerable<Obj_AI_Minion> GetBarrels()
        {
            return savedBarrels.Select(b => b.barrel).Where(b => b.IsValid);
        }

        public static List<Vector3> PointsAroundTheTargetOuterRing(Vector3 pos, float dist, float width = 15)
        {
            if (!pos.IsValid())
            {
                return new List<Vector3>();
            }
            List<Vector3> list = new List<Vector3>();
            var max = 2 * dist / 2 * Math.PI / width / 2;
            var angle = 360f / max * Math.PI / 180.0f;
            for (int i = 0; i < max; i++)
            {
                list.Add(
                    new Vector3(
                        pos.X + (float)(Math.Cos(angle * i) * dist), pos.Y + (float)(Math.Sin(angle * i) * dist),
                        pos.Z));
            }

            return list;
        }

        private List<Vector3> GetBarrelPoints(Vector3 point)
        {
            return PointsAroundTheTargetOuterRing(point, BarrelConnectionRange, 20f);
        }

        private void Combo()
        {
            AIHeroClient target = ARAMTargetSelector.getBestTarget(
                E.Range);
            if (target == null)
            {
                return;
            }

            if (40 < player.HealthPercent &&
                player.CountEnemiesInRange(500) > 0)
            {
                W.Cast();
            }
            if (R.IsReady())
            {
                var Rtarget =
                    EntityManager.Enemies.FirstOrDefault(e => e.HealthPercent < 50 && e.CountAlliesInRange(660) > 0);
                if (Rtarget != null)
                {
                    R.CastIfWillHit(Rtarget, 2);
                }
            }
            var barrels =
                GetBarrels()
                    .Where(
                        o =>
                            o.IsValid && !o.IsDead && o.Distance(player) < 1600 && o.BaseSkinName == "GangplankBarrel" &&
                            o.GetBuff("gangplankebarrellife").Caster.IsMe)
                    .ToList();

            if (Q.IsReady() &&
                E.IsReady())
            {
                var Qbarrels = GetBarrels().Where(o => o.Distance(player) < Q.Range && KillableBarrel(o));
                foreach (var Qbarrel in Qbarrels)
                {
                    if (Qbarrel.Distance(target) < BarrelExplosionRange)
                    {
                        continue;
                    }
                    var point =
                        GetBarrelPoints(Qbarrel.Position)
                            .Where(
                                p =>
                                    p.IsValid() && !p.IsWall() && p.Distance(player.Position) < E.Range &&
                                    p.Distance(Q.GetPrediction(target).UnitPosition) <
                                    BarrelExplosionRange && savedBarrels.Count(b => b.barrel.Position.Distance(p) < BarrelExplosionRange) < 1)
                             .OrderBy(p => p.Distance(target.Position))
                             .FirstOrDefault();
                    if (point != null && !justE)
                    {
                        E.Cast(point);
                        Core.DelayAction(() => Q.Cast(Qbarrel), 1);
                        return;
                    }
                }
            }

            if (E.IsReady() && player.Distance(target) < E.Range &&
                target.Health > Q.GetDamage(target) + player.GetAutoAttackDamage(target) && Orbwalker.CanMove &&
                0 < E.AmmoQuantity)
            {
                CastE(target, barrels);
            }
            var meleeRangeBarrel =
                barrels.FirstOrDefault(
                    b =>
                        b.Health < 2 && b.Distance(player) < player.GetAutoAttackDamage(b) &&
                        b.CountEnemiesInRange(BarrelExplosionRange) > 0);
            if (meleeRangeBarrel != null)
            {
                Orbwalker.ForcedTarget = meleeRangeBarrel;
            }
            if (Q.IsReady())
            {
                if (barrels.Any())
                {
                    var detoneateTargetBarrels = barrels.Where(b => b.Distance(player) < Q.Range);

                    if (detoneateTargetBarrels.Any())
                    {
                        foreach (var detoneateTargetBarrel in detoneateTargetBarrels)
                        {
                            if (!KillableBarrel(detoneateTargetBarrel))
                            {
                                continue;
                            }
                            if (
                                detoneateTargetBarrel.Distance(
                                    Q.GetPrediction(target).UnitPosition) <
                                BarrelExplosionRange &&
                                target.Distance(detoneateTargetBarrel.Position) < BarrelExplosionRange)
                            {
                                Q.Cast(detoneateTargetBarrel);
                                return;
                            }
                            var detoneateTargetBarrelSeconds =
                                barrels.Where(b => b.Distance(detoneateTargetBarrel) < BarrelConnectionRange);
                            if (detoneateTargetBarrelSeconds.Any())
                            {
                                foreach (var detoneateTargetBarrelSecond in detoneateTargetBarrelSeconds)
                                {
                                    if (
                                        detoneateTargetBarrelSecond.Distance(
                                            Q.GetPrediction(
                                                target).UnitPosition) <
                                        BarrelExplosionRange &&
                                        target.Distance(detoneateTargetBarrelSecond.Position) < BarrelExplosionRange)
                                    {
                                        Q.Cast(detoneateTargetBarrel);
                                        return;
                                    }
                                }
                            }
                        }
                    }

                    if (2 > 1)
                    {
                        var enemies =
                            EntityManager.Enemies.Where(e => e.IsValidTarget() && e.Distance(player) < 600)
                                .Select(e => Q.GetPrediction(e));
                        var enemies2 =
                            EntityManager.Enemies.Where(e => e.IsValidTarget() && e.Distance(player) < 600)
                                .Select(e => Q.GetPrediction(e));
                        if (detoneateTargetBarrels.Any())
                        {
                            foreach (var detoneateTargetBarrel in detoneateTargetBarrels)
                            {
                                if (!KillableBarrel(detoneateTargetBarrel))
                                {
                                    continue;
                                }
                                var enemyCount =
                                    enemies.Count(
                                        e =>
                                            e.UnitPosition.Distance(detoneateTargetBarrel.Position) <
                                            BarrelExplosionRange);
                                if (enemyCount >= 1 &&
                                    detoneateTargetBarrel.CountEnemiesInRange(BarrelExplosionRange) >=
                                    1)
                                {
                                    Q.CastOnUnit(detoneateTargetBarrel);
                                    return;
                                }
                                var detoneateTargetBarrelSeconds =
                                    barrels.Where(b => b.Distance(detoneateTargetBarrel) < BarrelConnectionRange);
                                if (detoneateTargetBarrelSeconds.Any())
                                {
                                    foreach (var detoneateTargetBarrelSecond in detoneateTargetBarrelSeconds)
                                    {
                                        if (enemyCount +
                                            enemies2.Count(
                                                e =>
                                                    e.UnitPosition.Distance(detoneateTargetBarrelSecond.Position) <
                                                    BarrelExplosionRange) >=
                                            1 &&
                                            detoneateTargetBarrelSecond.CountEnemiesInRange(BarrelExplosionRange) >=
                                            1)
                                        {
                                            Q.CastOnUnit(
                                                detoneateTargetBarrel);
                                            return;
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
                if (Q.CanCast(target))
                {
                    CastQonHero(target, barrels);
                }
            }
        }

        private void CastQonHero(AIHeroClient target, List<Obj_AI_Minion> barrels)
        {
            if (barrels.FirstOrDefault(b => b.Health == 2 && Q.GetPrediction(target).UnitPosition.Distance(b.Position) < BarrelExplosionRange) != null && target.Health > Q.GetDamage(target))
            {
                return;
            }
            Q.Cast(target);
        }

        private void CastE(AIHeroClient target, List<Obj_AI_Minion> barrels)
        {
            if (barrels.Count < 1)
            {
                CastEtarget(target);
                return;
            }
            var enemies = EntityManager.Enemies.Where(e => e.IsValidTarget() && e.Distance(player) < E.Range).Select(e => E.GetPrediction(e));
            List<Vector3> points = new List<Vector3>();
            foreach (var barrel in barrels.Where(b => b.Distance(player) < Q.Range && KillableBarrel(b)))
            {
                if (barrel != null)
                {
                    var newP = GetBarrelPoints(barrel.Position).Where(p => !p.IsWall());
                    if (newP.Any())
                    {
                        points.AddRange(newP.Where(p => p.Distance(player.Position) < E.Range));
                    }
                }
            }
            var bestPoint =
                points.Where(b => enemies.Count(e => e.UnitPosition.Distance(b) < BarrelExplosionRange) > 0)
                    .OrderByDescending(b => enemies.Count(e => e.UnitPosition.Distance(b) < BarrelExplosionRange))
                    .FirstOrDefault();
            if (bestPoint.IsValid() &&
                !savedBarrels.Any(b => b.barrel.Position.Distance(bestPoint) < BarrelConnectionRange) && !justE)
            {
                E.Cast(bestPoint);
            }
        }

        private void Game_ProcessSpell(Obj_AI_Base sender, GameObjectProcessSpellCastEventArgs args)
        {
            if (sender.IsMe)
            {
                if (args.SData.Name == "GangplankE")
                {
                    ePos = args.End;
                }
            }
        }

        private void CastEtarget(AIHeroClient target)
        {
            var ePred = E.GetPrediction(target);
            if (ePred.CastPosition.Distance(ePos) > 400 && !justE)
            {
                E.Cast(target.Position.Extend(ePred.CastPosition, BarrelExplosionRange).To3D());
            }
        }
    }
}