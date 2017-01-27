using System.Linq;
using EloBuddy;
using EloBuddy.SDK;

using Settings = KickassSeries.Champions.Caitlyn.Config.Modes.LaneClear;

namespace KickassSeries.Champions.Caitlyn.Modes
{
    public sealed class LaneClear : ModeBase
    {
        public override bool ShouldBeExecuted()
        {
            return Orbwalker.ActiveModesFlags.HasFlag(Orbwalker.ActiveModes.LaneClear);
        }

        public override void Execute()
        {
            var minions =
                EntityManager.MinionsAndMonsters.GetLaneMinions(EntityManager.UnitTeam.Enemy,
                    Player.Instance.ServerPosition, Q.Range, false).ToArray();
            if (minions.Length == 0) return;

            var farmLocation = EntityManager.MinionsAndMonsters.GetLineFarmLocation(minions, Q.Width, (int)Q.Range);
            if (farmLocation.HitNumber >= Settings.XCount && Settings.UseQ && Settings.LaneMana <= Player.Instance.ManaPercent)
            {
                Q.Cast(farmLocation.CastPosition);
            }
        }
    }
}
