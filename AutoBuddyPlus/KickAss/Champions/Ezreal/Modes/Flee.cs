using EloBuddy;
using EloBuddy.SDK;

using Settings = KickassSeries.Champions.Ezreal.Config.Modes.Misc;

namespace KickassSeries.Champions.Ezreal.Modes
{
    public sealed class Flee : ModeBase
    {
        public override bool ShouldBeExecuted()
        {
            return Orbwalker.ActiveModesFlags.HasFlag(Orbwalker.ActiveModes.Flee);
        }

        public override void Execute()
        {
            
            if (Settings.UseE && E.IsReady() && Player.Instance.CountEnemiesInRange(800) <= 1)
            {
                E.Cast(Player.Instance.Position.Extend(Game.CursorPos, E.Range).To3D());
            }
            
        }
    }
}
