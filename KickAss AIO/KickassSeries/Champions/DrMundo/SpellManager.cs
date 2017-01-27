using EloBuddy;
using EloBuddy.SDK;
using EloBuddy.SDK.Enumerations;

namespace KickassSeries.Champions.DrMundo
{
    public static class SpellManager
    {
        public static Spell.Skillshot Q { get; private set; }
        public static Spell.Active W { get; private set; }
        public static Spell.Active E { get; private set; }
        public static Spell.Active R { get; private set; }

        static SpellManager()
        {
            Q = new Spell.Skillshot(SpellSlot.Q, 1000, SkillShotType.Linear, 250, 1850, 60);
            W = new Spell.Active(SpellSlot.W, 160);
            E = new Spell.Active(SpellSlot.E, (uint)Player.Instance.GetAutoAttackRange());
            R = new Spell.Active(SpellSlot.R);
        }

        public static void Initialize()
        {
        }
    }
}
