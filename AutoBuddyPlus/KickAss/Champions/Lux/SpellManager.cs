using EloBuddy;
using EloBuddy.SDK;
using EloBuddy.SDK.Enumerations;

namespace KickassSeries.Champions.Lux
{
    public static class SpellManager
    {
        public static Spell.Skillshot Q { get; private set; }
        public static Spell.Skillshot W { get; private set; }
        public static Spell.Skillshot E { get; private set; }
        public static Spell.Skillshot R { get; private set; }

        static SpellManager()
        {
            Q = new Spell.Skillshot(SpellSlot.Q, 1175, SkillShotType.Linear, 250, 1200, 80)
            {
               MinimumHitChance = HitChance.High, AllowedCollisionCount = 1
            };
            W = new Spell.Skillshot(SpellSlot.W, 1175, SkillShotType.Linear, 250 , 1200, 150)
            {
                AllowedCollisionCount = int.MaxValue
            };
            E = new Spell.Skillshot(SpellSlot.E, 1250, SkillShotType.Circular, 250, 1530, 60)
            {
                AllowedCollisionCount = int.MaxValue
            };
            R = new Spell.Skillshot(SpellSlot.R, 3300, SkillShotType.Linear, 1600, 3000, 180)
            {
                AllowedCollisionCount = int.MaxValue, MinimumHitChance = HitChance.High
            };
        }

        public static void Initialize()
        {
        }
    }
}
