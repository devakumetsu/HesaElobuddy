using System.Linq;
using EloBuddy;
using EloBuddy.SDK;

using Settings = KickassSeries.Champions.Ahri.Config.Modes.LaneClear;

namespace KickassSeries.Champions.Ahri.Modes
{
    public sealed class LaneClear : ModeBase
    {
        public override bool ShouldBeExecuted()
        {
            return Orbwalker.ActiveModesFlags.HasFlag(Orbwalker.ActiveModes.LaneClear);
        }

        public override void Execute()
        {
            var minion =
                EntityManager.MinionsAndMonsters.GetLaneMinions()
                    .OrderByDescending(m => m.Health)
                    .FirstOrDefault(m => m.IsValidTarget(Q.Range));

            if (minion == null) return;


            if (E.IsReady() && minion.IsValidTarget(E.Range) && Settings.UseE && Settings.LaneMana < Player.Instance.ManaPercent)
            {
                E.Cast(minion);
            }

            if (W.IsReady() && minion.IsValidTarget(W.Range) && Settings.UseW && Settings.LaneMana < Player.Instance.ManaPercent)
            {
                W.Cast();
            }

            if (Q.IsReady() && minion.IsValidTarget(Q.Range) && Settings.UseQ && Settings.LaneMana < Player.Instance.ManaPercent)
            {
                var minions =
                   EntityManager.MinionsAndMonsters.GetLaneMinions(EntityManager.UnitTeam.Enemy,
                       Player.Instance.ServerPosition, Q.Range, false).ToArray();
                if (minions.Length == 0) return;

                var farmLocation = EntityManager.MinionsAndMonsters.GetLineFarmLocation(minions, Q.Width, (int)Q.Range);
                if (farmLocation.HitNumber == Settings.QCount)
                {
                    Q.Cast(farmLocation.CastPosition);
                }
            }
        }
    }
}
