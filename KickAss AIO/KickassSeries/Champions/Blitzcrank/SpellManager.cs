using EloBuddy;
using EloBuddy.SDK;
using EloBuddy.SDK.Enumerations;

namespace KickassSeries.Champions.Blitzcrank
{
    public static class SpellManager
    {
        public static Spell.Skillshot Q { get; private set; }
        public static Spell.Active W { get; private set; }
        public static Spell.Active E { get; private set; }
        public static Spell.Active R { get; private set; }

        static SpellManager()
        {
            Q = new Spell.Skillshot(SpellSlot.Q, 900, SkillShotType.Linear, 270, 1750, 50);
            W = new Spell.Active(SpellSlot.W, 700);
            E = new Spell.Active(SpellSlot.E, (uint)Player.Instance.GetAutoAttackRange());
            R = new Spell.Active(SpellSlot.R, 590);
        }

        public static void Initialize()
        {
        }
    }
}
