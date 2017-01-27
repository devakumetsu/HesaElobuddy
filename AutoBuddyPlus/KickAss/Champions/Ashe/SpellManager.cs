using EloBuddy;
using EloBuddy.SDK;
using EloBuddy.SDK.Enumerations;

namespace KickassSeries.Champions.Ashe
{
    public static class SpellManager
    {
        public static Spell.Active Q { get; private set; }
        public static Spell.Skillshot W { get; private set; }
        public static Spell.Skillshot E { get; private set; }
        public static Spell.Skillshot R { get; private set; }

        static SpellManager()
        {
            Q = new Spell.Active(SpellSlot.Q, (uint)Player.Instance.GetAutoAttackRange());
            W = new Spell.Skillshot(SpellSlot.W, 1200, SkillShotType.Cone, 250, 2000, 56);
            E = new Spell.Skillshot(SpellSlot.E, int.MaxValue, SkillShotType.Circular, 250, 1400, 10);
            R = new Spell.Skillshot(SpellSlot.R, int.MaxValue, SkillShotType.Linear, 275, 1600, 250);
        }

        public static void Initialize()
        {
        }
    }
}
