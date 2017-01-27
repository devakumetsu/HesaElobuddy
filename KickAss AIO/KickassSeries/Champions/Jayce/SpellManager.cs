using EloBuddy;
using EloBuddy.SDK;
using EloBuddy.SDK.Enumerations;

namespace KickassSeries.Champions.Jayce
{
    public static class SpellManager
    {
        //Hammer
        public static Spell.Targeted Qh { get; private set; }
        public static Spell.Active Wh { get; private set; }
        public static Spell.Targeted Eh { get; private set; }
        //Gun
        public static Spell.Skillshot Qg { get; private set; }
        public static Spell.Active Wg { get; private set; }
        public static Spell.Skillshot Eg { get; private set; }

        public static Spell.Skillshot R { get; private set; }

        static SpellManager()
        {
            //Hammer
            Qh = new Spell.Targeted(SpellSlot.Q,600);
            Wh = new Spell.Active(SpellSlot.W, 285);
            Eh = new Spell.Targeted(SpellSlot.E, 240);
            //Gun
            Qg = new Spell.Skillshot(SpellSlot.Q, 800, SkillShotType.Linear, 250, int.MaxValue, 85)
            {
                AllowedCollisionCount = int.MaxValue
            };
            Wg = new Spell.Active(SpellSlot.E, (uint)Player.Instance.GetAutoAttackRange());
            Eg = new Spell.Skillshot(SpellSlot.W, 650, SkillShotType.Circular, 250, int.MaxValue, 20)
            {
                AllowedCollisionCount = int.MaxValue
            };
            


            R = new Spell.Skillshot(SpellSlot.R, 700, SkillShotType.Circular, 250, 1200, 500)
            {
                AllowedCollisionCount = int.MaxValue
            };
        }

        public static void Initialize()
        {
        }
    }
}
