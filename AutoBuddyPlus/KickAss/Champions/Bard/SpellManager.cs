using EloBuddy;
using EloBuddy.SDK;
using EloBuddy.SDK.Enumerations;

namespace KickassSeries.Champions.Bard
{
    public static class SpellManager
    {
        public static Spell.Skillshot Q { get; private set; }
        public static Spell.Skillshot W { get; private set; }
        public static Spell.Skillshot E { get; private set; }
        public static Spell.Skillshot R { get; private set; }

        static SpellManager()
        {
            Q = new Spell.Skillshot(SpellSlot.Q, 950, SkillShotType.Linear, 250, 1500, 55)
            {
                AllowedCollisionCount = 2
            };
            W = new Spell.Skillshot(SpellSlot.W, 1000, SkillShotType.Circular, 280, 2500, 30);
            E = new Spell.Skillshot(SpellSlot.E, 900, SkillShotType.Linear, 250, 1530, 120);
            R = new Spell.Skillshot(SpellSlot.R, 3400, SkillShotType.Circular, 250, 1200, 350);
        }

        public static void Initialize()
        {
        }
    }
}
