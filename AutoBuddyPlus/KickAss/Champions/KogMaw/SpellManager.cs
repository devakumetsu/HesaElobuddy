using EloBuddy;
using EloBuddy.SDK;
using EloBuddy.SDK.Enumerations;

namespace KickassSeries.Champions.KogMaw
{
    public static class SpellManager
    {
        public static Spell.Skillshot Q { get; private set; }
        public static Spell.Active W { get; private set; }
        public static Spell.Skillshot E { get; private set; }
        public static Spell.Skillshot R { get; private set; }

        static SpellManager()
        {
            Q = new Spell.Skillshot(SpellSlot.Q, 980, SkillShotType.Linear, 250, 2000, 50)
            {
                AllowedCollisionCount = int.MaxValue
            };
            W = new Spell.Active(SpellSlot.W, 700);
            E = new Spell.Skillshot(SpellSlot.E, 1000, SkillShotType.Linear, 250, 1530, 60);
            R = new Spell.Skillshot(SpellSlot.R, 1200, SkillShotType.Circular, 250, 1200, 30);
        }

        public static void Initialize()
        {
            Obj_AI_Base.OnLevelUp += Obj_AI_Base_OnLevelUp;
        }

        //Changing Spells Ranges When Player level up his ultimate
        //TODO there must be another simpler way to do research later
        private static void Obj_AI_Base_OnLevelUp(Obj_AI_Base sender, Obj_AI_BaseLevelUpEventArgs args)
        {
            if (!sender.IsMe) return;

            if (Player.Instance.Level <= 11 || Player.Instance.Level >= 16)
            {
                R = new Spell.Skillshot(SpellSlot.R, 1500, SkillShotType.Circular, 250, 1200, 30);
            }
            if (Player.Instance.Level <= 16 || Player.Instance.Level >= 18)
            {
                R = new Spell.Skillshot(SpellSlot.R, 1800, SkillShotType.Circular, 250, 1200, 30);
            }
        }
    }
}
