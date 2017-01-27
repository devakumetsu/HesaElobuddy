using EloBuddy;
using EloBuddy.SDK;
using EloBuddy.SDK.Enumerations;

namespace KickassSeries.Champions.Gnar
{
    public static class SpellManager
    {
        //Gnarzinhu
        public static Spell.Skillshot Q1 { get; private set; }
        public static Spell.Skillshot E1 { get; private set; }
        //Ganarzao
        public static Spell.Skillshot Q2 { get; private set; }
        public static Spell.Skillshot W2 { get; private set; }
        public static Spell.Skillshot E2 { get; private set; }

        public static Spell.Skillshot R { get; private set; }

        static SpellManager()
        {
            //Gnarzinhu
            Q1 = new Spell.Skillshot(SpellSlot.Q, 1100, SkillShotType.Linear, 250, 1200, 55)
            {
                AllowedCollisionCount = 2
            };
            E1 = new Spell.Skillshot(SpellSlot.E, 475, SkillShotType.Circular, 500, int.MaxValue, 150)
            {
                AllowedCollisionCount = int.MaxValue
            };
            //GnarZao
            Q2 = new Spell.Skillshot(SpellSlot.Q, 1100, SkillShotType.Linear, 250, 1200, 80);
            W2 = new Spell.Skillshot(SpellSlot.W, 525, SkillShotType.Circular, 250, int.MaxValue, 80)
            {
                AllowedCollisionCount = int.MaxValue
            };
            E2 = new Spell.Skillshot(SpellSlot.E, 475, SkillShotType.Circular, 500, int.MaxValue, 150)
            {
                AllowedCollisionCount = int.MaxValue
            };
            //
            R = new Spell.Skillshot(SpellSlot.R, 580, SkillShotType.Circular, 250, 1200, 500);
        }

        public static void Initialize()
        {
        }
    }
}
