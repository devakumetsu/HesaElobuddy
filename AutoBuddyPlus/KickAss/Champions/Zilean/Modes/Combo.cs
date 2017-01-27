using System.Linq;
using EloBuddy;
using EloBuddy.SDK;
using KickassSeries.Activator;
using Settings = KickassSeries.Champions.Zilean.Config.Modes.Combo;

namespace KickassSeries.Champions.Zilean.Modes
{
    public sealed class Combo : ModeBase
    {
        public override bool ShouldBeExecuted()
        {
            return Orbwalker.ActiveModesFlags.HasFlag(Orbwalker.ActiveModes.Combo);
        }

        public override void Execute()
        {
            var target = TargetSelector.GetTarget(Q.Range, DamageType.Magical);
            if (target == null || target.IsZombie) return;

            if (Q.IsReady() && target.IsValidTarget(Q.Range) && Settings.UseQ && !E.IsReady())
            {
                Q.Cast(target);
            }

            if (E.IsReady() && target.IsValidTarget(E.Range) && Settings.UseE)
            {
                E.Cast(target);
            }

            if (W.IsReady() && target.IsValidTarget(W.Range) && Settings.UseW && !Q.IsReady())
            {
                W.Cast();
            }

            if (R.IsReady() && Settings.UseR)
            {
                var ally = EntityManager.Heroes.Allies.FirstOrDefault(a => a.HealthPercent <= 15);
                if (ally == null) return;
                R.Cast(ally);
            }
        }
    }
}
