using EloBuddy.SDK;

namespace KickassSeries.Champions.Ziggs.Modes
{
    public abstract class ModeBase
    {
        protected static Spell.Skillshot Q1
        {
            get { return SpellManager.Q1; }
        }
        protected static Spell.Skillshot Q2
        {
            get { return SpellManager.Q2; }
        }
        protected static Spell.Skillshot Q3
        {
            get { return SpellManager.Q3; }
        }
        protected static Spell.Active W
        {
            get { return SpellManager.W; }
        }
        protected static Spell.Skillshot E
        {
            get { return SpellManager.E; }
        }
        protected static Spell.Skillshot R
        {
            get { return SpellManager.R; }
        }

        public abstract bool ShouldBeExecuted();

        public abstract void Execute();
    }
}
