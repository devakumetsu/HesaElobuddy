using EloBuddy;
using EloBuddy.SDK;
using EloBuddy.SDK.Enumerations;

namespace KickassSeries.Champions.Braum
{
    public static class SpellManager
    {
        public static Spell.Skillshot Q { get; private set; }
        public static Spell.Active W { get; private set; }
        public static Spell.Skillshot E { get; private set; }
        public static Spell.Skillshot R { get; private set; }

        static SpellManager()
        {
            Q = new Spell.Skillshot(SpellSlot.Q, 1000, SkillShotType.Linear, 250, 1700, 60);
            W = new Spell.Active(SpellSlot.W, 650);
            E = new Spell.Skillshot(SpellSlot.E, 1000, SkillShotType.Linear, 250, int.MaxValue, 60);
            R = new Spell.Skillshot(SpellSlot.R, 1200, SkillShotType.Circular, 500, 1400, 115);
        }

        public static void Initialize()
        {
        }
    }
}
