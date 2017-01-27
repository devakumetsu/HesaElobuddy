using EloBuddy;
using EloBuddy.SDK;
using EloBuddy.SDK.Enumerations;

namespace KickassSeries.Champions.Elise
{
    public static class SpellManager
    {
        public static Spell.Active R { get; private set; }
        //Human
        public static Spell.Targeted Q1 { get; private set; }
        public static Spell.Skillshot W1 { get; private set; }
        public static Spell.Skillshot E1 { get; private set; }
        //Spider
        public static Spell.Targeted Q2 { get; private set; }
        public static Spell.Active W2 { get; private set; }
        public static Spell.Targeted E2 { get; private set; }

        static SpellManager()
        {
            R = new Spell.Active(SpellSlot.R);

            Q1 = new Spell.Targeted(SpellSlot.Q, 800);
            W1 = new Spell.Skillshot(SpellSlot.W, 825, SkillShotType.Circular, 250, int.MaxValue, 20);
            E1 = new Spell.Skillshot(SpellSlot.E, 1100, SkillShotType.Linear, 250, 1150, 70);
        }

        public static void Initialize()
        {
            Obj_AI_Base.OnProcessSpellCast += Player_OnProcessSpellCast;
        }

        private static void Player_OnProcessSpellCast(Obj_AI_Base sender, GameObjectProcessSpellCastEventArgs args)
        {
            if (!sender.IsMe || !args.Slot.Equals(SpellSlot.R)) return;
            if (Player.Instance.Spellbook.GetSpell(SpellSlot.R).ToggleState == 1)
            {
                Q1 = new Spell.Targeted(SpellSlot.Q, 800);
                W1 = new Spell.Skillshot(SpellSlot.W, 825, SkillShotType.Circular, 250, int.MaxValue, 20);
                E1 = new Spell.Skillshot(SpellSlot.E, 1100, SkillShotType.Linear, 250, 1150, 70);
            }
            if (Player.Instance.Spellbook.GetSpell(SpellSlot.R).ToggleState == 2)
            {
                Q2 = new Spell.Targeted(SpellSlot.Q, 800);
                W2 = new Spell.Active(SpellSlot.W, 825);
                E2 = new Spell.Targeted(SpellSlot.E, 1100);
            }
        }
    }
}
