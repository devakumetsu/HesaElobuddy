using EloBuddy;
using EloBuddy.SDK;

namespace KickassSeries.Champions.Vi
{
    public static class SpellManager
    {
        public static Spell.Chargeable Q { get; private set; }
        public static Spell.Active W { get; private set; }
        public static Spell.Active E { get; private set; }
        public static Spell.Targeted R { get; private set; }

        static SpellManager()
        {
            Q = new Spell.Chargeable(SpellSlot.Q, 250, 875, 1250, 250, 1400, 55)
            {
                AllowedCollisionCount = int.MaxValue
            };
            W = new Spell.Active(SpellSlot.W);
            E = new Spell.Active(SpellSlot.E, 175);
            R = new Spell.Targeted(SpellSlot.R, 800);
        }

        public static void Initialize()
        {
        }
    }
}
