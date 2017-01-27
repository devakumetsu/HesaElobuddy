using EloBuddy;
using EloBuddy.SDK;
using EloBuddy.SDK.Enumerations;

namespace KickassSeries.Champions.Varus
{
    public static class SpellManager
    {
        public static Spell.Chargeable Q { get; private set; }
        public static Spell.Active W { get; private set; }
        public static Spell.Skillshot E { get; private set; }
        public static Spell.Skillshot R { get; private set; }

        static SpellManager()
        {
            Q = new Spell.Chargeable(SpellSlot.Q, 925, 1650, 1200, 250, 1850, 70)
            {
                AllowedCollisionCount = int.MaxValue
            };
            W = new Spell.Active(SpellSlot.W);
            E = new Spell.Skillshot(SpellSlot.E, 1100, SkillShotType.Circular, 350, 1500, 120)
            {
                AllowedCollisionCount = int.MaxValue
            };
            R = new Spell.Skillshot(SpellSlot.R, 700, SkillShotType.Linear, 250, 1950, 120);
        }

        public static void Initialize()
        {
        }
    }
}
