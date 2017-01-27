using System.Linq;
using EloBuddy;
using EloBuddy.SDK;

using Settings = KickassSeries.Champions.Tristana.Config.Modes.LaneClear;

namespace KickassSeries.Champions.Tristana.Modes
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
                    .FirstOrDefault(m => m.IsValidTarget(Player.Instance.AttackRange));
            if (minion != null)
            {

                if (minion.IsValidTarget(E.Range) && Settings.UseE && E.IsReady())
                {
                    E.Cast(minion);
                }

                if (minion.IsValidTarget(Q.Range) && Settings.UseQ && Q.IsReady())
                {
                    Q.IsReady();
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

            var tower = EntityManager.Turrets.Enemies.FirstOrDefault(t => !t.IsDead && t.IsInRange(Player.Instance, 800));
            if (tower != null)
            {

                if (tower.IsInRange(Player.Instance, E.Range) && Settings.UseE && E.IsReady())
                {
                    E.Cast(tower);
                }

                if (tower.IsInRange(Player.Instance, Q.Range) && Settings.UseQ && Q.IsReady())
                {
                    Q.Cast();
                }
            }
        }
    }
}
