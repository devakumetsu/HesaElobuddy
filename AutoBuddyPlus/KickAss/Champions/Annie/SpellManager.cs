using EloBuddy;
using EloBuddy.SDK;
using EloBuddy.SDK.Enumerations;

namespace KickassSeries.Champions.Annie
{
    public static class SpellManager
    {
        public static Spell.Targeted Q { get; private set; }
        public static Spell.Skillshot W { get; private set; }
        public static Spell.Active E { get; private set; }
        public static Spell.Skillshot R { get; private set; }

        static SpellManager()
        {
            Q = new Spell.Targeted(SpellSlot.Q, 625);
            W = new Spell.Skillshot(SpellSlot.W, 625, SkillShotType.Cone, 250, int.MaxValue, 50);
            E = new Spell.Active(SpellSlot.E, 0);
            R = new Spell.Skillshot(SpellSlot.R, 600, SkillShotType.Circular, 250, int.MaxValue, 290);
        }

        public static void Initialize()
        {
        }
    }
}
