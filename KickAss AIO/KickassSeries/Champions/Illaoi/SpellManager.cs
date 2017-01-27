using EloBuddy;
using EloBuddy.SDK;
using EloBuddy.SDK.Enumerations;

namespace KickassSeries.Champions.Illaoi
{
    public static class SpellManager
    {
        public static Spell.Skillshot Q { get; private set; }
        public static Spell.Active W { get; private set; }
        public static Spell.Skillshot E { get; private set; }
        public static Spell.Active R { get; private set; }

        static SpellManager()
        {
            Q = new Spell.Skillshot(SpellSlot.Q, 800, SkillShotType.Linear, 480, 500, 75)
            {
                AllowedCollisionCount = int.MaxValue
            };
            W = new Spell.Active(SpellSlot.W, 350);
            E = new Spell.Skillshot(SpellSlot.E, 900, SkillShotType.Linear, 250, 1900, 25);
            R = new Spell.Active(SpellSlot.R, 450);
        }

        public static void Initialize()
        {
        }
    }
}
