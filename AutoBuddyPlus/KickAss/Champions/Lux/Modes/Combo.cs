using EloBuddy;
using EloBuddy.SDK;

using Settings = KickassSeries.Champions.Lux.Config.Modes.Combo;

namespace KickassSeries.Champions.Lux.Modes
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

            if (Q.IsReady() && target.IsValidTarget(Q.Range) && Settings.UseQ)
            {
                Q.Cast(Q.GetPrediction(target).CastPosition);
            }

            if (E.IsReady() && target.IsValidTarget(E.Range) && Settings.UseE && Settings.UseESnared
                ? target.HasBuffOfType(BuffType.Snare)
                : target.IsValidTarget(E.Range))
            {
                E.Cast(E.GetPrediction(target).CastPosition);
            }

            if (R.IsReady() && Settings.UseR)
            {
                var targetR = TargetSelector.GetTarget(R.Range, DamageType.Magical);
                if(targetR == null)return;

                if (target.IsValidTarget(R.Range) && Settings.UseRSnared? target.HasBuffOfType(BuffType.Snare): target.IsValidTarget(R.Range))
                {
                    if (targetR.HasBuffOfType(BuffType.Snare) || targetR.HasBuffOfType(BuffType.Stun))
                    {
                        R.Cast(targetR.Position);
                    }
                    else
                    {
                        R.Cast(R.GetPrediction(targetR).CastPosition);
                    }
                }
            }
        }
    }
}
