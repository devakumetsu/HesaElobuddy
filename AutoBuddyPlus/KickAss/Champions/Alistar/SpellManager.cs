using EloBuddy;
using EloBuddy.SDK;

namespace KickassSeries.Champions.Alistar
{
    public static class SpellManager
    {
        public static Spell.Active Q { get; private set; }
        public static Spell.Targeted W { get; private set; }
        public static Spell.Active E { get; private set; }
        public static Spell.Active R { get; private set; }

        static SpellManager()
        {
            Q = new Spell.Active(SpellSlot.Q, 365);
            W = new Spell.Targeted(SpellSlot.W, 650);
            E = new Spell.Active(SpellSlot.E, 575);
            R = new Spell.Active(SpellSlot.R);
        }

        public static void Initialize()
        {
        }
    }
}
