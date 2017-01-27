using EloBuddy;
using EloBuddy.SDK;
using EloBuddy.SDK.Enumerations;

namespace KickassSeries.Champions.JarvanIV
{
    public static class SpellManager
    {
        public static Spell.Skillshot Q { get; private set; }
        public static Spell.Skillshot Q2 { get; private set; }
        public static Spell.Active W { get; private set; }
        public static Spell.Skillshot E { get; private set; }
        public static Spell.Targeted R { get; private set; }

        static SpellManager()
        {
            Q = new Spell.Skillshot(SpellSlot.Q, 770, SkillShotType.Linear, 250, int.MaxValue, 85)
            {
                AllowedCollisionCount = int.MaxValue
            };
            Q2 = new Spell.Skillshot(SpellSlot.Q, 770, SkillShotType.Linear, 250, int.MaxValue, 85)
            {
                AllowedCollisionCount = int.MaxValue
            };
            W = new Spell.Active(SpellSlot.W, 520);
            E = new Spell.Skillshot(SpellSlot.E, 830, SkillShotType.Linear, 250, 1150, 75)
            {
                AllowedCollisionCount = int.MaxValue
            };
            R = new Spell.Targeted(SpellSlot.R, 650);
            //R radius 325
        }

        public static void Initialize()
        {
        }
    }
}
