using EloBuddy;
using EloBuddy.SDK;

namespace KickassSeries.Champions.KogMaw.Modes
{
    public sealed class PermaActive : ModeBase
    {
        public override bool ShouldBeExecuted()
        {
            return true;
        }

        public override void Execute()
        {
            var target = TargetSelector.GetTarget(R.Range, DamageType.Magical);
            if (target == null || target.IsZombie) return;

            //Passive Follow target
            if (Player.Instance.IsDead)
            {
                Player.IssueOrder(GameObjectOrder.MoveTo, target);
            }
            //Passive Follow target

            //R KS
            if (R.IsReady() && target.IsValidTarget(R.Range) && target.Health <= SpellDamage.GetRealDamage(SpellSlot.R, target))
            {
                R.Cast(target);
            }
            //R KS
        }
    }
}
