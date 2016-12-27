using EloBuddy;
using EloBuddy.SDK;
using EloBuddy.SDK.Enumerations;
using SharpDX;
using System.Collections.Generic;
using System.Linq;
using static EloBuddy.SDK.EntityManager.MinionsAndMonsters;
using static EloBuddy.SDK.Spell;

namespace ARAMDetFull
{
    public static class SpellExtension
    {
        public static bool IsKillable(this SpellBase spell, Obj_AI_Base target)
        {
            return target.Health <= spell.GetSpellDamage(target);
        }
        public static void CastOnUnit(this SpellBase spell, Obj_AI_Base target)
        {
            spell.Cast(target);
        }

        public static bool IsCharging(this SpellBase spell)
        {
            return ((Chargeable)spell).IsCharging;
        }

        public static void StartCharging(this SpellBase spell)
        {
            ((Chargeable)spell).StartCharging();
        }

        public static int ChargedMaxRange(this SpellBase spell)
        {
            return (int) ((Chargeable)spell).MaximumRange;
        }

        public static void CastIfWillHit(this SpellBase spell, Obj_AI_Base target, int hitNumber)
        {
            var prediction = (spell as Skillshot).GetPrediction(target);
            var enemiesInSpellRange = EntityManager.Heroes.Enemies.Count(x => !x.IsDead && x.Distance(prediction.CastPosition) <= spell.Range);
            if(hitNumber <= enemiesInSpellRange)
            {
                spell.Cast(prediction.CastPosition);
            }
        }

        public static bool CastIfWillHitReturn(this SpellBase spell, Obj_AI_Base target, int hitChance)
        {
            var prediction = (spell as Skillshot).GetPrediction(target);
            if((int)prediction.HitChance >= hitChance)
            {
                spell.Cast(prediction.CastPosition);
                return true;
            }
            return false;
        }

        public static bool CastIfHitchanceEquals(this SpellBase spell, Obj_AI_Base target, HitChance chance, bool value)
        {
            if (target == null) return false;
            var pred = (spell as Skillshot).GetPrediction(target);
            var willHit = false;
            if((int) pred.HitChance >= (int)chance)
            {
                willHit = true;
            }
            if(value && willHit)
            {
                return spell.Cast(pred.CastPosition);
            }
            return false;
        }

        public static PredictionResult GetPrediction(this SpellBase spell, Obj_AI_Base target, bool value = true)
        {
            if (target == null) return null;
            return (spell as Skillshot).GetPrediction(target);
        }
        
        public static float GetDamage(this SpellBase spell, Obj_AI_Base target)
        {
           return spell.GetSpellDamage(target);
        }

        public static int Width(this SpellBase spell)
        {
            return (spell as Skillshot).Width;
        }

        public static float CooldownExpires(this SpellBase spell)
        {
           return Player.Instance.Spellbook.GetSpell(spell.Slot).CooldownExpires;
        }

        public static float Speed(this SpellBase spell)
        {
            return Player.Instance.Spellbook.GetSpell(spell.Slot).SData.SpellCastTime;
        }

        public static FarmLocation GetCircularFarmLocation(this SpellBase spell, IEnumerable<Obj_AI_Minion> minions, int width)
        {
            return EntityManager.MinionsAndMonsters.GetCircularFarmLocation(minions, width, (int)spell.Range);
        }

        public static FarmLocation GetLineFarmLocation(this SpellBase spell, IEnumerable<Obj_AI_Minion> minions, int width)
        {
            return EntityManager.MinionsAndMonsters.GetLineFarmLocation(minions, width, (int)spell.Range);
        }


        public static List<Vector2> GetWaypoints(this Obj_AI_Base unit)
        {
            var result = new List<Vector2>();

            if (unit.IsVisible)
            {
                result.Add(unit.ServerPosition.To2D());
                var path = unit.Path;
                if (path.Length > 0)
                {
                    var first = path[0].To2D();
                    if (first.Distance(result[0], true) > 40)
                    {
                        result.Add(first);
                    }

                    for (int i = 1; i < path.Length; i++)
                    {
                        result.Add(path[i].To2D());
                    }
                }
            }
            else if (WaypointTracker.StoredPaths.ContainsKey(unit.NetworkId))
            {
                var path = WaypointTracker.StoredPaths[unit.NetworkId];
                var timePassed = (Core.GameTickCount - WaypointTracker.StoredTick[unit.NetworkId]) / 1000f;
                if (path.PathLength() >= unit.MoveSpeed * timePassed)
                {
                    result = CutPath(path, (int)(unit.MoveSpeed * timePassed));
                }
            }

            return result;
        }

        public static List<Vector2> CutPath(this List<Vector2> path, float distance)
        {
            var result = new List<Vector2>();
            var Distance = distance;
            if (distance < 0)
            {
                path[0] = path[0] + distance * (path[1] - path[0]).Normalized();
                return path;
            }

            for (var i = 0; i < path.Count - 1; i++)
            {
                var dist = path[i].Distance(path[i + 1]);
                if (dist > Distance)
                {
                    result.Add(path[i] + Distance * (path[i + 1] - path[i]).Normalized());
                    for (var j = i + 1; j < path.Count; j++)
                    {
                        result.Add(path[j]);
                    }

                    break;
                }
                Distance -= dist;
            }
            return result.Count > 0 ? result : new List<Vector2> { path.Last() };
        }

        /// <summary>
        ///     Internal class used to get the waypoints even when the enemy enters the fow of war.
        /// </summary>
        internal static class WaypointTracker
        {
            public static readonly Dictionary<int, List<Vector2>> StoredPaths = new Dictionary<int, List<Vector2>>();
            public static readonly Dictionary<int, int> StoredTick = new Dictionary<int, int>();
        }

        /// <summary>
        ///     Returns The predicted position for the target.
        /// </summary>
        public static Vector3 PredictPosition(this Obj_AI_Base target, int Time = 250)
        {
            return Prediction.Position.PredictUnitPosition(target, Time).To3D();
        }
        
        /// <summary>
        ///     Returns The predicted Health for the target.
        /// </summary>
        public static float PredictHealth(this Obj_AI_Base target, int Time = 250)
        {
            return Prediction.Health.GetPrediction(target, Time);
        }

        /// <summary>
        ///     Returns The predicted HealthPercent for the target.
        /// </summary>
        public static float PredictHealthPercent(this Obj_AI_Base target, int Time = 250)
        {
            return Prediction.Health.GetPrediction(target, Time) / target.MaxHealth * 100;
        }

        public static int CountEnemyHeros(this Vector3 Pos, float range = 1200, int time = 250)
        {
            return Pos.CountEnemyHeroesInRangeWithPrediction((int)range, time);
        }

        public static int CountEnemyHeros(this Vector2 Pos, float range = 1200, int time = 250)
        {
            return Pos.To3DWorld().CountEnemyHeros(range, time);
        }

        public static int CountEnemyHeros(this Obj_AI_Base target, float range = 1200, int time = 250)
        {
            return target.PredictPosition(time).CountEnemyHeros(range, time);
        }

        public static int CountEnemyHeros(this GameObject target, float range = 1200, int time = 250)
        {
            return target.Position.CountEnemyHeros(range, time);
        }

        public static int CountAllyHeros(this Vector3 Pos, float range = 1200, int time = 250)
        {
            return EntityManager.Heroes.Allies.Count(e => e.IsValidTarget() && e.PredictPosition(time).IsInRange(Pos, range));
        }

        public static int CountAllyHeros(this Obj_AI_Base target, float range = 1200, int time = 250)
        {
            return target.PredictPosition(time).CountAllyHeros(range, time);
        }

        public static int CountAllyHeros(this GameObject target, float range = 1200, int time = 250)
        {
            return target.Position.CountAllyHeros(range, time);
        }

        /// <summary>
        ///     Returns true if target Is CC'D.
        /// </summary>
        public static bool IsCC(this Obj_AI_Base target)
        {
            return !target.CanMove || target.HasBuffOfType(BuffType.Charm) || target.HasBuffOfType(BuffType.Knockback) || target.HasBuffOfType(BuffType.Knockup) || target.HasBuffOfType(BuffType.Fear)
                   || target.HasBuffOfType(BuffType.Snare) || target.HasBuffOfType(BuffType.Stun) || target.HasBuffOfType(BuffType.Suppression) || target.HasBuffOfType(BuffType.Taunt)
                   || target.HasBuffOfType(BuffType.Sleep);
        }
        
        /// <summary>
        ///     Returns true if Obj_AI_Base is UnderEnemyTurret.
        /// </summary>
        public static bool UnderEnemyTurret(this Obj_AI_Base target)
        {
            return EntityManager.Turrets.AllTurrets.Any(t => !t.IsDead && t.Team != target.Team && t.IsValidTarget() && t.IsInRange(target, t.GetAutoAttackRange(target)));
        }

        /// <summary>
        ///     Returns true if Vector3 is UnderEnemyTurret.
        /// </summary>
        public static bool UnderEnemyTurret(this Vector3 pos)
        {
            return EntityManager.Turrets.Enemies.Any(t => !t.IsDead && t.IsValid && t.PredictHealth() > 0 && t.IsInRange(pos, t.GetAutoAttackRange(Player.Instance)));
        }

        /// <summary>
        ///     Returns true if Vector2 is UnderEnemyTurret.
        /// </summary>
        public static bool UnderEnemyTurret(this Vector2 pos)
        {
            return EntityManager.Turrets.Enemies.Any(t => !t.IsDead && t.IsValid && t.PredictHealth() > 0 && t.IsInRange(pos, t.GetAutoAttackRange(Player.Instance)));
        }

        /// <summary>
        ///     Returns true if Vector3 is UnderAlliedTurret.
        /// </summary>
        public static bool UnderAlliedTurret(this Vector3 pos)
        {
            return EntityManager.Turrets.Allies.Any(t => !t.IsDead && t.IsValid && t.PredictHealth() > 0 && t.IsInRange(pos, t.GetAutoAttackRange(Player.Instance)));
        }

        /// <summary>
        ///     Returns true if Vector2 is UnderAlliedTurret.
        /// </summary>
        public static bool UnderAlliedTurret(this Vector2 pos)
        {
            return EntityManager.Turrets.Allies.Any(t => !t.IsDead && t.IsValid && t.PredictHealth() > 0 && t.IsInRange(pos, t.GetAutoAttackRange(Player.Instance)));
        }
    }
}