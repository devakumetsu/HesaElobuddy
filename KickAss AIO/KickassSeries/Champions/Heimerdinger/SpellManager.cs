using EloBuddy;
using EloBuddy.SDK;
using EloBuddy.SDK.Enumerations;

namespace KickassSeries.Champions.Heimerdinger
{
    public static class SpellManager
    {
        public static Spell.Skillshot Q { get; private set; }
        public static Spell.Skillshot W { get; private set; }
        public static Spell.Skillshot E { get; private set; }
        public static Spell.Active R { get; private set; }

        static SpellManager()
        {
            Q = new Spell.Skillshot(SpellSlot.Q, 425, SkillShotType.Circular, 250, int.MaxValue, 10)
            {
                AllowedCollisionCount = int.MaxValue
            };
            W = new Spell.Skillshot(SpellSlot.W, 1100, SkillShotType.Linear, 250 , 3000, 20);
            E = new Spell.Skillshot(SpellSlot.E, 925, SkillShotType.Circular, 250, 1200, 70)
            {
                AllowedCollisionCount = int.MaxValue
            };
            R = new Spell.Active(SpellSlot.R);
        }

        public static void Initialize()
        {
        }
    }
}
