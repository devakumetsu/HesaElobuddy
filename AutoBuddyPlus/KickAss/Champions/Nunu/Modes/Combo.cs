using EloBuddy;
using EloBuddy.SDK;

using Settings = KickassSeries.Champions.Nunu.Config.Modes.Combo;

namespace KickassSeries.Champions.Nunu.Modes
{
    public sealed class Combo : ModeBase
    {
        public override bool ShouldBeExecuted()
        {
            return Orbwalker.ActiveModesFlags.HasFlag(Orbwalker.ActiveModes.Combo);
        }

        public override void Execute()
        {
            var target = TargetSelector.GetTarget(E.Range, DamageType.Magical);
            if (target == null || target.IsZombie) return;

            if (E.IsReady() && target.IsValidTarget(E.Range) && Settings.UseE)
            {
                E.Cast(target);
            }

            if (W.IsReady() && target.IsValidTarget(W.Range) && Settings.UseW)
            {
                W.Cast(Player.Instance);
            }

            if (R.IsReady() && target.IsValidTarget(R.Range) && Settings.UseR)
            {
                if (!Player.Instance.Spellbook.IsChanneling)
                {
                    R.Cast();
                }

                if (Player.Instance.Spellbook.IsChanneling)
                {
                    Orbwalker.DisableAttacking = true;
                    Orbwalker.DisableMovement = true;

                    if (!target.IsInRange(Player.Instance, R.Range - 75) && target.IsInRange(Player.Instance, R.Range))
                    {
                        Orbwalker.DisableAttacking = false;
                        Orbwalker.DisableMovement = false;
                    }
                }
            }
        }
    }
}
