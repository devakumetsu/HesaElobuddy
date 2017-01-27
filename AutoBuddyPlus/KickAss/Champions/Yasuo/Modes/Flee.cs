using System.Linq;
using EloBuddy;
using EloBuddy.SDK;
using EloBuddy.SDK.Events;

namespace KickassSeries.Champions.Yasuo.Modes
{
    public sealed class Flee : ModeBase
    {
        public override bool ShouldBeExecuted()
        {
            return Orbwalker.ActiveModesFlags.HasFlag(Orbwalker.ActiveModes.Flee);
        }

        public override void Execute()
        {
            var minion =
                ObjectManager.Get<Obj_AI_Base>()
                    .Where(x => x.IsValidTarget(SpellManager.E.Range))
                    .OrderBy(x => x.Distance(Game.CursorPos))
                    .FirstOrDefault();
            if (minion == null) return;

            if (minion.CanE() && Player.Instance.IsFacing(minion) && !minion.GetAfterEPos().Tower())
            {
                SpellManager.E.Cast(minion);
            }

            if (!Player.Instance.HasQ3())
            {
                if (Player.Instance.IsDashing() && SpellManager.Q.IsReady())
                {
                    SpellManager.Q.Cast(minion);
                }
                if (SpellManager.Q.IsReady())
                {
                    var target = TargetSelector.GetTarget(SpellManager.Q.Range, DamageType.Physical);

                    if (target != null && target.IsValidTarget(SpellManager.Q.Range))
                    {
                        SpellManager.Q.Cast(target);

                    }
                }
            }
        }
    }
}
