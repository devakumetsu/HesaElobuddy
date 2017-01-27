using EloBuddy;
using EloBuddy.SDK;
using EloBuddy.SDK.Enumerations;

namespace KickassSeries.Champions.Caitlyn
{
    public static class SpellManager
    {
        public static Spell.Skillshot Q { get; private set; }
        public static Spell.Skillshot W { get; private set; }
        public static Spell.Skillshot E { get; private set; }
        public static Spell.Targeted R { get; private set; }

        static SpellManager()
        {
            Q = new Spell.Skillshot(SpellSlot.Q, 1200, SkillShotType.Linear, 650, 2000, 58);
            W = new Spell.Skillshot(SpellSlot.W, 790, SkillShotType.Circular, 450, 1400, 35)
            {
                MinimumHitChance = HitChance.High
            };
            E = new Spell.Skillshot(SpellSlot.E, 950, SkillShotType.Linear, 250, 1600, 80);
            R = new Spell.Targeted(SpellSlot.R, 2000);
        }

        public static void Initialize()
        {
            Obj_AI_Base.OnLevelUp += Obj_AI_Base_OnLevelUp;
        }

        static void Obj_AI_Base_OnLevelUp(Obj_AI_Base sender, Obj_AI_BaseLevelUpEventArgs args)
        {
            if (sender.IsMe)
            {
                if (Player.Instance.Level >= 11 && Player.Instance.Level < 16)
                {
                    R = new Spell.Targeted(SpellSlot.R, 2500);
                }

                if (Player.Instance.Level >= 16)
                {
                    R = new Spell.Targeted(SpellSlot.R, 3000);
                }
            }
        }
    }
}
