using System.Linq;
using EloBuddy;
using EloBuddy.SDK;

using Settings = KickassSeries.Champions.Ezreal.Config.Modes.Misc;
using Configs = KickassSeries.Champions.Ezreal.Config.Modes.Harass;

namespace KickassSeries.Champions.Ezreal.Modes
{
    public sealed class PermaActive : ModeBase
    {
        public override bool ShouldBeExecuted()
        {
            return true;
        }

        public override void Execute()
        {
            if (Settings.UseR && R.IsReady())
            {
                var hero =
                    EntityManager.Heroes.Enemies.OrderByDescending(e => e.Distance(Player.Instance)).FirstOrDefault(
                        e => e.IsVisible && e.HasBuffOfType(BuffType.Stun) || e.HasBuffOfType(BuffType.Knockup) ||
                             e.HasBuffOfType(BuffType.Snare));

                if (hero != null)
                {
                    R.Cast(hero);
                }

                var target = TargetSelector.GetTarget(Settings.maxR, DamageType.Physical);
                if (target == null || target.IsZombie) return;

                if (!target.IsInRange(Player.Instance, Settings.minR) &&
                    target.Health <= SpellDamage.GetRealDamage(SpellSlot.R, target) &&
                    target.Health >= Settings.MinHealthR)
                {
                    R.Cast(target);
                }
            }

            if (Configs.UseQAuto && Q.IsReady() && Configs.ManaAutoHarass <= Player.Instance.ManaPercent)
            {
                var target = TargetSelector.GetTarget(Q.Range, DamageType.Physical);
                if (target == null || target.IsZombie) return;

                if (target.IsValidTarget(Q.Range))
                {
                    Q.Cast(target);
                }
            }

            if (Configs.UseWAuto && W.IsReady() && Configs.ManaAutoHarass <= Player.Instance.ManaPercent)
            {
                var target = TargetSelector.GetTarget(W.Range, DamageType.Physical);
                if (target == null || target.IsZombie) return;

                if (target.IsValidTarget(W.Range))
                {
                    W.Cast(target);
                }
            }
        }
    }
}
