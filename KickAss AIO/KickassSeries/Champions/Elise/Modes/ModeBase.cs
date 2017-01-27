using EloBuddy.SDK;

namespace KickassSeries.Champions.Elise.Modes
{
    public abstract class ModeBase
    {
        protected static Spell.Targeted Q
        {
            get { return SpellManager.Q1; }
        }
        protected static Spell.Skillshot W
        {
            get { return SpellManager.W1; }
        }
        protected static Spell.Skillshot E
        {
            get { return SpellManager.E1; }
        }
        protected static Spell.Active R
        {
            get { return SpellManager.R; }
        }

        public abstract bool ShouldBeExecuted();

        public abstract void Execute();
    }
}
