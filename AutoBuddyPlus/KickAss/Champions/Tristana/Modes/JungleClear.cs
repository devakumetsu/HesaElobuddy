using System.Linq;
using EloBuddy;
using EloBuddy.SDK;

using Settings = KickassSeries.Champions.Tristana.Config.Modes.LaneClear;

namespace KickassSeries.Champions.Tristana.Modes
{
    public sealed class JungleClear : ModeBase
    {
        public override bool ShouldBeExecuted()
        {
            return Orbwalker.ActiveModesFlags.HasFlag(Orbwalker.ActiveModes.JungleClear);
        }

        public override void Execute()
        {
            var minion =
                EntityManager.MinionsAndMonsters.GetJungleMonsters()
                    .OrderBy(m=>m.MaxHealth).FirstOrDefault(m => m.IsValidTarget(Player.Instance.AttackRange));
            if (minion == null) return;

            if (minion.IsValidTarget(E.Range) && Settings.UseE)
            {
                E.Cast(minion);
            }

            if (minion.IsValidTarget(Q.Range) && Settings.UseQ)
            {
                Q.Cast();
            }

            var minionE =
                EntityManager.MinionsAndMonsters.GetLaneMinions()
                    .FirstOrDefault(
                        m => m.IsValidTarget(Player.Instance.AttackRange) && m.GetBuffCount("tristanaecharge") > 0);
            if (minionE != null)
            {
                Orbwalker.ForcedTarget = minionE;
            }
        }
    }
}
