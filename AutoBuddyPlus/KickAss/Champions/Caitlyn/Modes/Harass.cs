using EloBuddy;
using EloBuddy.SDK;

using Settings = KickassSeries.Champions.Caitlyn.Config.Modes.Harass;

namespace KickassSeries.Champions.Caitlyn.Modes
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

            if (Settings.UseE && E.IsReady() && target.IsInRange(Player.Instance, 450) &&
                target.IsValidTarget(E.Range))
            {
                E.Cast(target);
            }

            if (Player.Instance.CanAttack)
            {
                var targetAutoAttack = TargetSelector.GetTarget(Player.Instance.AttackRange * 2, DamageType.Physical);
                if (targetAutoAttack == null) return;

                if (targetAutoAttack.HasBuff("caitlynyordletrapinternal"))
                {
                    Player.IssueOrder(GameObjectOrder.AttackUnit, targetAutoAttack);
                }
            }
        }
    }
}
