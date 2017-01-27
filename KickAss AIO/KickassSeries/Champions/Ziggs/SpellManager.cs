using EloBuddy;
using EloBuddy.SDK;
using EloBuddy.SDK.Enumerations;

namespace KickassSeries.Champions.Ziggs
{
    public static class SpellManager
    {
        public static Spell.Skillshot Q1 { get; private set; }
        public static Spell.Skillshot Q2 { get; private set; }
        public static Spell.Skillshot Q3 { get; private set; }
        public static Spell.Active W { get; private set; }
        public static Spell.Skillshot E { get; private set; }
        public static Spell.Skillshot R { get; private set; }

        static SpellManager()
        {
            Q1 = new Spell.Skillshot(SpellSlot.Q, 850, SkillShotType.Linear, 300, 1700, 130);
            Q2 = new Spell.Skillshot(SpellSlot.Q, 1125, SkillShotType.Linear, 250 +Q1.CastDelay, 1700, 130);
            Q3 = new Spell.Skillshot(SpellSlot.Q, 1400, SkillShotType.Linear, 300 + Q2.CastDelay, 1700, 130);

            W = new Spell.Active(SpellSlot.W, 1000);
            E = new Spell.Skillshot(SpellSlot.E, 900, SkillShotType.Linear, 250, 1530, 60);
            R = new Spell.Skillshot(SpellSlot.R, 5300, SkillShotType.Circular, 1000, 2800, 500);
        }

        public static void Initialize()
        {
        }
    }
}
