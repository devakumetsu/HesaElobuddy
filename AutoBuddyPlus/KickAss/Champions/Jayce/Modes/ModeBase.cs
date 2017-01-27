using EloBuddy.SDK;

namespace KickassSeries.Champions.Jayce.Modes
{
    public abstract class ModeBase
    {
        protected static Spell.Targeted Q
        {
            get { return SpellManager.Qh; }
        }
        protected static Spell.Active W
        {
            get { return SpellManager.Wh; }
        }
        protected static Spell.Targeted E
        {
            get { return SpellManager.Eh; }
        }
        protected static Spell.Skillshot R
        {
            get { return SpellManager.R; }
        }

        public abstract bool ShouldBeExecuted();

        public abstract void Execute();
    }
}
