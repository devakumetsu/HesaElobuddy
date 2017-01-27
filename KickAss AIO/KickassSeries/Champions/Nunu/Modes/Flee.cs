using EloBuddy;
using EloBuddy.SDK;

namespace KickassSeries.Champions.Nunu.Modes
{
    public sealed class Flee : ModeBase
    {
        public override bool ShouldBeExecuted()
        {
            return Orbwalker.ActiveModesFlags.HasFlag(Orbwalker.ActiveModes.Flee);
        }

        public override void Execute()
        {
            if (W.IsReady())
            {
                W.Cast(Player.Instance);
            }
        }
    }
}
