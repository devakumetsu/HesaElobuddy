using System.Linq;
using EloBuddy;
using EloBuddy.SDK;
using SharpDX;

namespace KickassSeries.Champions.Yasuo
{
    static class Extensions
    {
        public static bool HasQ3(this AIHeroClient target)
        {
            return target.IsMe && target.HasBuff("YasuoQ3W");
        }

        public static bool CanE(this Obj_AI_Base target)
        {
            return !target.HasBuff("YasuoDashWrapper");
        }

        public static Vector3 GetAfterEPos(this Obj_AI_Base unit)
        {
            return Player.Instance.Position.Extend(Prediction.Position.PredictUnitPosition(unit, 250), 475).To3D();
        }

        public static bool IsKnockedUp(this Obj_AI_Base unit)
        {
            return unit.HasBuffOfType(BuffType.Knockback) || unit.HasBuffOfType(BuffType.Knockup);
        }

        public static bool Tower(this Vector3 pos)
        {
            return EntityManager.Turrets.Enemies.Where(t => !t.IsDead).Any(d => d.Distance(pos) < 950);
        }
    }
}
