using EloBuddy.SDK;

namespace KickassSeries.Champions.Gnar.Modes
{
    public abstract class ModeBase
    {
        protected static Spell.Skillshot Q
        {
            get { return SpellManager.Q1; }
        }
        protected static Spell.Skillshot W
        {
            get { return SpellManager.W2; }
        }
        protected static Spell.Skillshot E
        {
            get { return SpellManager.E1; }
        }
        protected static Spell.Skillshot R
        {
            get { return SpellManager.R; }
        }

        public abstract bool ShouldBeExecuted();

        public abstract void Execute();
    }
}
