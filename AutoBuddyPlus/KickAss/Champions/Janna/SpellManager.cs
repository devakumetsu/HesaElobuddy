using EloBuddy;
using EloBuddy.SDK;
using EloBuddy.SDK.Enumerations;

namespace KickassSeries.Champions.Janna
{
    public static class SpellManager
    {
        public static Spell.Chargeable Q { get; private set; }
        public static Spell.Targeted W { get; private set; }
        public static Spell.Targeted E { get; private set; }
        public static Spell.Active R { get; private set; }

        static SpellManager()
        {
            Q = new Spell.Chargeable(SpellSlot.Q, 800, 1743, 3000,250, 900, 85)
            {
                AllowedCollisionCount = int.MaxValue
            };
            W = new Spell.Targeted(SpellSlot.W, 600);
            E = new Spell.Targeted(SpellSlot.E, 800);
            R = new Spell.Active(SpellSlot.R, 550);
        }

        public static void Initialize()
        {
        }
    }
}
