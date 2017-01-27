using EloBuddy;
using EloBuddy.SDK;
using SharpDX;
using Color = System.Drawing.Color;

namespace KickassSeries.Champions.Jinx
{
    internal class Essentials
    {
        public static readonly string[] JungleMobsList =
        {
            "SRU_Red", "SRU_Blue", "SRU_Dragon", "SRU_Baron", "SRU_Gromp",
            "SRU_Murkwolf", "SRU_Razorbeak", "SRU_Krug", "Sru_Crab"
        };

        public static readonly string[] JungleMobsListTwistedTreeline =
        {
            "TT_NWraith1.1", "TT_NWraith4.1",
            "TT_NGolem2.1", "TT_NGolem5.1", "TT_NWolf3.1", "TT_NWolf6.1", "TT_Spiderboss8.1"
        };

        public static bool FishBones()
        {
            return Player.Instance.HasBuff("JinxQ");
        }

        public static bool ShouldUseE(string spellName)
        {
            switch (spellName)
            {
                case "ThreshQ":
                    return true;
                case "KatarinaR":
                    return true;
                case "AlZaharNetherGrasp":
                    return true;
                case "GalioIdolOfDurand":
                    return true;
                case "LuxMaliceCannon":
                    return true;
                case "MissFortuneBulletTime":
                    return true;
                case "RocketGrabMissile":
                    return true;
                case "CaitlynPiltoverPeacemaker":
                    return true;
                case "EzrealTrueshotBarrage":
                    return true;
                case "InfiniteDuress":
                    return true;
                case "VelkozR":
                    return true;
            }
            return false;
        }

        public const float MinigunRange = 525f;

        public static double GrabTime = 0;

        public static float FishBonesRange()
        {
            return 670f + Player.Instance.BoundingRadius + 25 * SpellManager.Q.Level;
        }

        public static void DrawLineRectangle(Vector2 start, Vector2 end, int radius, int width, Color color)
        {
            var dir = (end - start).Normalized();
            var pDir = dir.Perpendicular();

            var rightStartPos = start + pDir * radius;
            var leftStartPos = start - pDir * radius;
            var rightEndPos = end + pDir * radius;
            var leftEndPos = end - pDir * radius;

            var rStartPos =
                Drawing.WorldToScreen(new Vector3(rightStartPos.X, rightStartPos.Y, Player.Instance.Position.Z));
            var lStartPos =
                Drawing.WorldToScreen(new Vector3(leftStartPos.X, leftStartPos.Y, Player.Instance.Position.Z));
            var rEndPos = Drawing.WorldToScreen(new Vector3(rightEndPos.X, rightEndPos.Y, Player.Instance.Position.Z));
            var lEndPos = Drawing.WorldToScreen(new Vector3(leftEndPos.X, leftEndPos.Y, Player.Instance.Position.Z));

            Drawing.DrawLine(rStartPos, rEndPos, width, color);
            Drawing.DrawLine(lStartPos, lEndPos, width, color);
            Drawing.DrawLine(rStartPos, lStartPos, width, color);
            Drawing.DrawLine(lEndPos, rEndPos, width, color);
        }

        public static class DamageLibrary
        {
            public static float CalculateDamage(Obj_AI_Base target, bool useQ, bool useW, bool useE, bool useR)
            {
                if (target == null)
                {
                    return 0;
                }

                var totaldamage = 0f;

                if (useQ && SpellManager.Q.IsReady())
                {
                    totaldamage = totaldamage + QDamage(target);
                }

                if (useW && SpellManager.W.IsReady())
                {
                    totaldamage = totaldamage + WDamage(target);
                }

                if (useE && SpellManager.E.IsReady())
                {
                    totaldamage = totaldamage + EDamage(target);
                }

                if (useR && SpellManager.R.IsReady())
                {
                    totaldamage = totaldamage + RDamage(target);
                }

                return totaldamage;
            }

            private static float QDamage(Obj_AI_Base target)
            {
                return Player.Instance.GetAutoAttackDamage(target);
            }

            private static float WDamage(Obj_AI_Base target)
            {
                return Player.Instance.CalculateDamageOnUnit(
                    target,
                    DamageType.Physical,
                    new[] { 0, 10, 60, 110, 160, 210 }[SpellManager.W.Level]
                    + (Player.Instance.TotalAttackDamage * 1.4f));
            }

            private static float EDamage(Obj_AI_Base target)
            {
                return Player.Instance.CalculateDamageOnUnit(
                    target,
                    DamageType.Magical,
                    new[] { 0, 80, 135, 190, 245, 300 }[SpellManager.E.Level] + (Player.Instance.TotalMagicalDamage));
            }

            private static float RDamage(Obj_AI_Base target)
            {
                if (!SpellManager.R.IsLearned) return 0;
                var level = SpellManager.R.Level - 1;

                #region Less than Range

                if (target.Distance(Player.Instance) < 1350 && !target.IsMinion && !target.IsMonster)
                {
                    return Player.Instance.CalculateDamageOnUnit(target, DamageType.Physical,
                        (float)
                            (new double[] { 25, 35, 45 }[level] +
                             new double[] { 25, 30, 35 }[level] / 100 * (target.MaxHealth - target.Health) +
                             0.1 * Player.Instance.FlatPhysicalDamageMod));
                }

                if ((target.IsMonster || target.IsMinion) && target.Distance(Player.Instance) < 1350)
                {
                    var damage = Player.Instance.CalculateDamageOnUnit(target, DamageType.Physical,
                        (float)
                            (new double[] { 25, 35, 45 }[level] +
                             new double[] { 25, 30, 35 }[level] / 100 * (target.MaxHealth - target.Health) +
                             0.1 * Player.Instance.FlatPhysicalDamageMod));

                    return (damage * 0.8) > 300f ? 300f : damage;
                }

                #endregion

                #region More Than Range

                var damage2 = Player.Instance.CalculateDamageOnUnit(target, DamageType.Physical,
                    (float)
                        (new double[] { 250, 350, 450 }[level] +
                         new double[] { 25, 30, 35 }[level] / 100 * (target.MaxHealth - target.Health) +
                         Player.Instance.FlatPhysicalDamageMod));

                if ((target.IsMonster || target.IsMinion) && (damage2 * 0.8) > 300f)
                {
                    damage2 = 300f;
                }

                return damage2;

                #endregion
            }
        }
    }
}
