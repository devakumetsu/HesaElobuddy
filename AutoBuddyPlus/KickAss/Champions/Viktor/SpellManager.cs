using EloBuddy;
using EloBuddy.SDK;
using EloBuddy.SDK.Enumerations;

namespace KickassSeries.Champions.Viktor
{
    public static class SpellManager
    {
        public static Spell.Active Q { get; private set; }
        public static Spell.Skillshot W { get; private set; }
        public static Spell.Targeted E { get; private set; }
        public static Spell.Active R { get; private set; }

        static SpellManager()
        {
            Q = new Spell.Active(SpellSlot.Q, (uint) Player.Instance.GetAutoAttackRange());
            W = new Spell.Skillshot(SpellSlot.W, 650, SkillShotType.Cone, 250, 2300, 75);
            E = new Spell.Targeted(SpellSlot.E, 1000);
            R = new Spell.Active(SpellSlot.R, 410);
        }

        public static void Initialize()
        {
        }
    }
}
