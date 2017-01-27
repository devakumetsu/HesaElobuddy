using System.Collections.Generic;
using System.Linq;
using EloBuddy;
using EloBuddy.SDK;

using Settings = KickassSeries.Champions.Ezreal.Config.Modes.Combo;

namespace KickassSeries.Champions.Ezreal.Modes
{
    public sealed class Combo : ModeBase
    {
        public override bool ShouldBeExecuted()
        {
            return Orbwalker.ActiveModesFlags.HasFlag(Orbwalker.ActiveModes.Combo);
        }

        public override void Execute()
        {
            var target = TargetSelector.GetTarget(Q.Range, DamageType.Physical);
            if (target == null || target.IsZombie) return;

            if (Settings.UseQ && Q.IsReady() && target.IsValidTarget(Q.Range) &&
                !target.IsInRange(Player.Instance, Player.Instance.GetAutoAttackRange()))
            {
                Q.Cast(target);
            }

            if (Settings.UseQ && EventsManager.CanQCancel && target.IsValidTarget(Q.Range) &&
                target.IsInRange(Player.Instance, Player.Instance.GetAutoAttackRange()))
            {
                Q.Cast(target);
            }

            if (Settings.UseW && W.IsReady() && target.IsValidTarget(W.Range) &&
                !target.IsInRange(Player.Instance, Player.Instance.GetAutoAttackRange()))
            {
                W.Cast(target);
            }

            if (Settings.UseW && EventsManager.CanWCancel && target.IsValidTarget(W.Range) &&
                target.IsInRange(Player.Instance, Player.Instance.GetAutoAttackRange()))
            {
                W.Cast(target);
            }

            if (Settings.UseR && R.IsReady() && target.IsValidTarget(R.Range))
            {
                var enemies = EntityManager.Heroes.Enemies.Where(e => e.IsValidTarget(R.Range)).ToArray();
                if(enemies.Length == 0)return;

                
            }

        }
    }
}
