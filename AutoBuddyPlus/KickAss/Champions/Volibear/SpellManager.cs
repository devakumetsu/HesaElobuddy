using EloBuddy;
using EloBuddy.SDK;

namespace KickassSeries.Champions.Volibear
{
    public static class SpellManager
    {
        public static Spell.Active Q { get; private set; }
        public static Spell.Targeted W { get; private set; }
        public static Spell.Active E { get; private set; }
        public static Spell.Active R { get; private set; }

        static SpellManager()
        {
            Q = new Spell.Active(SpellSlot.Q, 750);
            W = new Spell.Targeted(SpellSlot.W, 395);
            E = new Spell.Active(SpellSlot.E, 415);
            R = new Spell.Active(SpellSlot.R, (uint)Player.Instance.GetAutoAttackRange());
        }

        public static void Initialize()
        {
        }
    }
}
