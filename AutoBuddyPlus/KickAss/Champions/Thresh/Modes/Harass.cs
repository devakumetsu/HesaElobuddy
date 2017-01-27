using EloBuddy;
using EloBuddy.SDK;

using Settings = KickassSeries.Champions.Thresh.Config.Modes.Harass;

namespace KickassSeries.Champions.Thresh.Modes
{
    public sealed class Harass : ModeBase
    {
        public override bool ShouldBeExecuted()
        {
            return Orbwalker.ActiveModesFlags.HasFlag(Orbwalker.ActiveModes.Harass);
        }

        public override void Execute()
        {
            var target = TargetSelector.GetTarget(Q.Range, DamageType.Physical);
            if (target == null) return;
            if (Settings.UseQ && Q.IsReady() && target.IsValidTarget(Q.Range))
            {
                Q.Cast(target);
            }

            if (Settings.UseE && E.IsReady() && target.IsValidTarget(E.Range))
            {
                /*if (Settings.ModeE == 0)
                {
                    E.Cast(target);
                    Chat.Print("Use E Push");
                }
                if (Settings.ModeE == 1)
                {
                    E.Cast(Player.Instance.Position.Shorten(target.Position, E.Range));
                    Chat.Print("Use E Pull");
                }
                */
            }
        }
    }
}

