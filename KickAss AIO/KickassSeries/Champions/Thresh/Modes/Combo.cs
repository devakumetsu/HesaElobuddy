using System.Linq;
using EloBuddy;
using EloBuddy.SDK;

using Settings = KickassSeries.Champions.Thresh.Config.Modes.Combo;

namespace KickassSeries.Champions.Thresh.Modes
{
    public sealed class Combo : ModeBase
    {
        public override bool ShouldBeExecuted()
        {
            return Orbwalker.ActiveModesFlags.HasFlag(Orbwalker.ActiveModes.Combo);
        }

        public override void Execute()
        {
            var target = TargetSelector.GetTarget(Q.Range, DamageType.Mixed);
            if (target == null) return;

            if (Settings.UseQ && Q.IsReady() && target.IsValidTarget(Q.Range))
            {
                Q.Cast(target);
            }
            /*
            if (Settings.UseE && E.IsReady() && target.IsValidTarget(E.Range))
            {
                if (Settings.ModeE == 0)
                {
                    E.Cast(target);
                }
                if (Settings.ModeE == 1)
                {
                    E.Cast(Player.Instance.Position.Shorten(target.Position, E.Range));
                }
            }
            */
            if (Settings.UseW && W.IsReady())
            {
                var ally =
                    EntityManager.Heroes.Allies.OrderByDescending(h => h.Health)
                        .FirstOrDefault(a => a.IsValidTarget(W.Range));

                if (ally != null)
                {
                    W.Cast(ally);
                }
            }
            /*
            if (Settings.UseR && R.IsReady() && Player.Instance.CountEnemiesInRange(R.Range) >= Settings.MinR)
            {
                R.Cast();
            }
            */
        }
    }
}
