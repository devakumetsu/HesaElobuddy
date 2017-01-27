using EloBuddy;
using EloBuddy.SDK;

namespace KickassSeries.Champions.Garen
{
    public static class SpellManager
    {
        public static Spell.Active Q { get; private set; }
        public static Spell.Active W { get; private set; }
        public static Spell.Active E { get; private set; }
        public static Spell.Targeted R { get; private set; }

        static SpellManager()
        {
            Q = new Spell.Active(SpellSlot.Q, 800);
            W = new Spell.Active(SpellSlot.W, 825);
            E = new Spell.Active(SpellSlot.E, 1100);
            R = new Spell.Targeted(SpellSlot.R, 700);
        }

        public static void Initialize()
        {
        }
    }
}
