using System;
using SharpDX;
using EloBuddy.SDK.Enumerations;
using EloBuddy.SDK.Spells;
using EloBuddy;
using EloBuddy.SDK;
using static EloBuddy.SDK.Spell;

namespace ARAMDetFull.SpellsSDK
{
    //Big tnx to Trees ^^
    internal static class SpellUtility
    {
        public static bool IsSkillShot(this SpellBase spell)
        {
            return spell is Skillshot;
        }

        public static bool IsSkillShot(this SpellType type)
        {
            return type.Equals(SpellType.Circle) || type.Equals(SpellType.Circle) || type.Equals(SpellType.MissileAoe) || type.Equals(SpellType.Line) || type.Equals(SpellType.MissileLine);
        }

        public static SkillShotType GetSkillshotType(this SpellType type)
        {
            switch (type)
            {
                case SpellType.Circle:
                case SpellType.MissileAoe:
                    return SkillShotType.Circular;
                case SpellType.Cone:
                    return SkillShotType.Cone;
                default:
                    return SkillShotType.Linear;
            }
        }

        public static bool IsCurrentSpell(this SpellBase entry)
        {
            var spell = ObjectManager.Player.Spellbook.GetSpell(entry.Slot);
            return string.Equals(spell.Name, entry.Name, StringComparison.CurrentCultureIgnoreCase);
        }
        
        public static void RenderCircle(Vector3 position, float radius, Color color)
        {
            Drawing.DrawCircle(position, radius, System.Drawing.Color.FromArgb(color.ToRgba()));
        }
    }
}