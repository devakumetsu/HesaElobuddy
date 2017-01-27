using EloBuddy.SDK;

namespace KickassSeries.Champions.Garen.Modes
{
    public abstract class ModeBase
    {
        protected static Spell.Active Q
        {
            get { return SpellManager.Q; }
        }
        protected static Spell.Active W
        {
            get { return SpellManager.W; }
        }
        protected static Spell.Active E
        {
            get { return SpellManager.E; }
        }
        protected static Spell.Targeted R
        {
            get { return SpellManager.R; }
        }

        public abstract bool ShouldBeExecuted();

        public abstract void Execute();
    }
}
