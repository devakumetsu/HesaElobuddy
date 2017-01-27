using EloBuddy.SDK;

using Settings = KickassSeries.Champions.Zilean.Config.Modes.LastHit;

namespace KickassSeries.Champions.Zilean.Modes
{
    public sealed class LastHit : ModeBase
    {
        public override bool ShouldBeExecuted()
        {
            return Orbwalker.ActiveModesFlags.HasFlag(Orbwalker.ActiveModes.LastHit);
        }

        public override void Execute()
        {

        }
    }
}
