using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using EloBuddy;
using EloBuddy.SDK;
using EloBuddy.SDK.Enumerations;
using EloBuddy.SDK.Menu;
using EloBuddy.SDK.Menu.Values;
using SharpDX;

namespace AutoBuddy
{
    public static class KappaMisc
    {
        /// <summary>
        ///     Returns Spell Mana Cost.
        /// </summary>
        public static float Mana(this Spell.SpellBase spell)
        {
            return spell.Handle.SData.Mana;
        }
        /// <summary>
        ///     Returns true if target Is CC'D.
        /// </summary>
        public static bool IsCC(this Obj_AI_Base target)
        {
            return !target.CanMove || target.HasBuffOfType(BuffType.Charm) || target.HasBuffOfType(BuffType.Knockback) || target.HasBuffOfType(BuffType.Knockup) || target.HasBuffOfType(BuffType.Fear)
                   || target.HasBuffOfType(BuffType.Snare) || target.HasBuffOfType(BuffType.Stun) || target.HasBuffOfType(BuffType.Suppression) || target.HasBuffOfType(BuffType.Taunt)
                   ;//|| target.HasBuffOfType(BuffType.Sleep);
        }
        /// <summary>
        ///     Zombie heros list.
        /// </summary>
        public static List<Champion> ZombieHeros = new List<Champion>
        {
            Champion.KogMaw, Champion.Sion, Champion.Karthus
        };
        /// <summary>
        ///     Returns true if the hero Has a Zombie form.
        /// </summary>
        public static bool IsZombie(this AIHeroClient hero)
        {
            return ZombieHeros.Contains(hero.Hero) && hero.IsZombie;
        }
        /// <summary>
        ///     Returns true if Obj_AI_Base is UnderEnemyTurret.
        /// </summary>
        public static bool UnderEnemyTurret(this Obj_AI_Base target)
        {
            return EntityManager.Turrets.AllTurrets.Any(t => t.Team != target.Team && t.IsValidTarget() && t.IsInRange(target, t.GetAutoAttackRange(target)));
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
        /// <summary>
        ///     Returns a recreated name of the target.
        /// </summary>
        public static string Name(this Obj_AI_Base target)
        {
            if (ObjectManager.Get<Obj_AI_Base>().Count(o => o.BaseSkinName.Equals(target.BaseSkinName)) > 1)
            {
                return target.BaseSkinName + "(" + target.Name + ")";
            }
            return target.BaseSkinName;
        }

        public static string CleanChampionName(this AIHeroClient hero)
        {
            return hero.ChampionName.CleanChampionName();
        }

        public static string CleanChampionName(this string str)
        {
            var clean = str.Trim().Replace("\'", "").Replace(".", "").Replace(" ", "");
            if (clean.Equals("MonkeyKing"))
                return "Wukong";
            if (clean.Equals("Chogath"))
                return "ChoGath";
            if (clean.Equals("Kogmaw"))
                return "KogMaw";
            if (clean.Equals("Leesin"))
                return "LeeSin";
            if (clean.Equals("Khazix"))
                return "KhaZix";
            if (clean.Equals("Velkoz"))
                return "VelKoz";
            return clean;
        }

        /// <summary>
        ///     Returns the target KDA [BROKEN FROM CORE].
        /// </summary>
        public static float KDA(this AIHeroClient target)
        {
            return target.ChampionsKilled + target.Assists / target.Deaths;
        }

        public static float DistanceFromAllHeros(Vector3 pos)
        {
            return EntityManager.Heroes.AllHeroes.Where(a => a.IsValidTarget()).Sum(a => pos.Distance(a));
        }

        public static float DistanceFromAllHeros(this Obj_AI_Base target)
        {
            return DistanceFromAllHeros(target.ServerPosition);
        }

        public static float DistanceFromAllies(Vector3 pos)
        {
            return EntityManager.Heroes.Allies.Where(a => a.IsValidTarget()).Sum(a => pos.Distance(a));
        }

        public static float DistanceFromAllies(this Obj_AI_Base target)
        {
            return DistanceFromAllies(target.PredictPosition());
        }

        public static float DistanceFromEnemies(Vector3 pos)
        {
            return EntityManager.Heroes.Enemies.Where(a => a.IsValidTarget()).Sum(a => pos.Distance(a));
        }

        public static float DistanceFromEnemies(this Obj_AI_Base target)
        {
            return DistanceFromEnemies(target.PredictPosition());
        }

        public static bool CanKill(this AIHeroClient hero, AIHeroClient target)
        {
            return hero.GetAutoAttackDamage(target, true) >= target.TotalShieldHealth() && target.IsKillable(hero.GetAutoAttackRange(target));
        }

        /// <summary>
        ///     Returns a recreated name of the target.
        /// </summary>
        public static string Name(this AIHeroClient target)
        {
            if (EntityManager.Heroes.AllHeroes.Count(h => h.BaseSkinName.Equals(target.BaseSkinName)) > 1)
            {
                return target.BaseSkinName + "(" + target.Name + ")";
            }
            return target.BaseSkinName;
        }
        
        /// <summary>
        ///     Returns The predicted position for the target.
        /// </summary>
        public static Vector3 PredictPosition(this Obj_AI_Base target, int Time = 250)
        {
            return Prediction.Position.PredictUnitPosition(target, Time).To3D();
        }

        /// <summary>
        ///     Returns The predicted position for the target.
        /// </summary>
        public static Vector2 PredictPosition2D(this Obj_AI_Base target, int Time = 250)
        {
            return Prediction.Position.PredictUnitPosition(target, Time);
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

        /// <summary>
        ///     Returns true if you can deal damage to the target.
        /// </summary>
        public static bool IsKillable(this AIHeroClient target, float range)
        {
            return !target.HasBuff("kindredrnodeathbuff") && !target.HasUndyingBuff(true) && !target.Buffs.Any(b => b.Name.ToLower().Contains("fioraw")) && !target.HasBuff("JudicatorIntervention") && !target.IsZombie
                   && !target.HasBuff("ChronoShift") && !target.HasBuff("UndyingRage") && !target.IsInvulnerable && !target.IsZombie && !target.HasBuff("bansheesveil") && !target.IsDead
                   && !target.IsPhysicalImmune && target.PredictHealth() > 0 && !target.HasBuffOfType(BuffType.Invulnerability) && !target.HasBuffOfType(BuffType.PhysicalImmunity) && target.IsValidTarget(range);
        }

        /// <summary>
        ///     Returns true if you can deal damage to the target.
        /// </summary>
        public static bool IsKillable(this AIHeroClient target)
        {
            return !target.HasBuff("kindredrnodeathbuff") && !target.HasUndyingBuff(true) && !target.Buffs.Any(b => b.Name.ToLower().Contains("fioraw")) && !target.HasBuff("JudicatorIntervention") && !target.IsZombie
                   && !target.HasBuff("ChronoShift") && !target.HasBuff("UndyingRage") && !target.IsInvulnerable && !target.IsZombie && !target.HasBuff("bansheesveil") && !target.IsDead
                   && !target.IsPhysicalImmune && target.PredictHealth() > 0 && !target.HasBuffOfType(BuffType.Invulnerability) && !target.HasBuffOfType(BuffType.PhysicalImmunity) && target.IsValidTarget();
        }

        /// <summary>
        ///     Returns true if you can deal damage to the target.
        /// </summary>
        public static bool IsKillable(this Obj_AI_Base target, float range)
        {
            return !target.HasBuff("kindredrnodeathbuff") && !target.Buffs.Any(b => b.Name.ToLower().Contains("fioraw")) && !target.HasBuff("JudicatorIntervention") && !target.IsZombie
                   && !target.HasBuff("ChronoShift") && !target.HasBuff("UndyingRage") && !target.IsInvulnerable && !target.IsZombie && !target.HasBuff("bansheesveil") && !target.IsDead
                   && !target.IsPhysicalImmune && target.PredictHealth() > 0 && !target.HasBuffOfType(BuffType.Invulnerability) && !target.HasBuffOfType(BuffType.PhysicalImmunity) && target.IsValidTarget(range);
        }

        /// <summary>
        ///     Returns true if you can deal damage to the target.
        /// </summary>
        public static bool IsKillable(this Obj_AI_Base target)
        {
            return !target.HasBuff("kindredrnodeathbuff") && !target.Buffs.Any(b => b.Name.ToLower().Contains("fioraw")) && !target.HasBuff("JudicatorIntervention") && !target.IsZombie
                   && !target.HasBuff("ChronoShift") && !target.HasBuff("UndyingRage") && !target.IsInvulnerable && !target.IsZombie && !target.HasBuff("bansheesveil") && !target.IsDead
                   && !target.IsPhysicalImmune && target.PredictHealth() > 0 && !target.HasBuffOfType(BuffType.Invulnerability) && !target.HasBuffOfType(BuffType.PhysicalImmunity) && target.IsValidTarget();
        }

        /// <summary>
        ///     Casts spell with selected hitchance.
        /// </summary>
        public static void Cast(this Spell.Skillshot spell, Obj_AI_Base target, HitChance hitChance)
        {
            if (target != null && spell.IsReady() && target.IsKillable(spell.Range))
            {
                var pred = spell.GetPrediction(target);
                if (pred.HitChance >= hitChance || target.IsCC())
                {
                    spell.Cast(pred.CastPosition);
                }
            }
        }

        /// <summary>
        ///     Casts spell with selected hitchance.
        /// </summary>
        public static void Cast(this Spell.SpellBase spell, Obj_AI_Base target, HitChance hitChance)
        {
            if (target != null && spell.IsReady() && target.IsKillable(spell.Range))
            {
                var pred = ((Spell.Skillshot)spell).GetPrediction(target);
                if (pred.HitChance >= hitChance || target.IsCC())
                {
                    spell.Cast(pred.CastPosition);
                }
            }
        }

        /// <summary>
        ///     Casts spell with selected hitchancepercent.
        /// </summary>
        public static void Cast(this Spell.Skillshot spell, Obj_AI_Base target, float hitchancepercent)
        {
            if (target != null && spell.IsReady() && target.IsKillable(spell.Range))
            {
                var pred = spell.GetPrediction(target);
                if (pred.HitChancePercent >= hitchancepercent || target.IsCC())
                {
                    spell.Cast(pred.CastPosition);
                }
            }
        }

        /// <summary>
        ///     Casts spell with selected hitchancepercent.
        /// </summary>
        public static void Cast(this Spell.SpellBase spell, Obj_AI_Base target, float hitchancepercent)
        {
            if (target != null && spell.IsReady() && target.IsKillable(spell.Range))
            {
                var pred = ((Spell.Skillshot)spell).GetPrediction(target);
                if (pred.HitChancePercent >= hitchancepercent || target.IsCC())
                {
                    spell.Cast(pred.CastPosition);
                }
            }
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
    }
}
