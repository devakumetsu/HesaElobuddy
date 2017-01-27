using System.Linq;
using EloBuddy.SDK;

using Settings = KickassSeries.Champions.XinZhao.Config.Modes.LastHit;

namespace KickassSeries.Champions.XinZhao.Modes
{
    public sealed class LastHit : ModeBase
    {
        public override bool ShouldBeExecuted()
        {
            return Orbwalker.ActiveModesFlags.HasFlag(Orbwalker.ActiveModes.LastHit);
        }

        public override void Execute()
        {
            var minion =
                EntityManager.MinionsAndMonsters.GetLaneMinions()
                    .OrderByDescending(m => m.Health)
                    .FirstOrDefault(m => m.IsValidTarget(Q.Range));

            if (minion == null) return;

            if (E.IsReady() && minion.IsValidTarget(E.Range) && Settings.UseE)
            {
                E.Cast(minion);
            }

            if (EventsManager.CanW && minion.IsValidTarget(W.Range) && Settings.UseW)
            {
                W.Cast();
            }

            if (EventsManager.CanQ && minion.IsValidTarget(Q.Range) && Settings.UseQ)
            {
                Q.Cast();
            }
        }
    }
}
