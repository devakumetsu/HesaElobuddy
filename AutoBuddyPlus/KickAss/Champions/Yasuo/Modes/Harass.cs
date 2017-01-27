using EloBuddy;
using EloBuddy.SDK;

using Settings = KickassSeries.Champions.Yasuo.Config.Modes.Harass;

namespace KickassSeries.Champions.Yasuo.Modes
{
    public sealed class Harass : ModeBase
    {
        public override bool ShouldBeExecuted()
        {
            return Orbwalker.ActiveModesFlags.HasFlag(Orbwalker.ActiveModes.Harass);
        }

        public override void Execute()
        {
        }
    }
}
