using System.Linq;
using EloBuddy;
using EloBuddy.SDK;

namespace KickassSeries.Champions.Xerath.Modes
{
    public sealed class PermaActive : ModeBase
    {
        public override bool ShouldBeExecuted()
        {
            return true;
        }

        public static bool IsCastingUlt
        {
            get { return Player.Instance.Buffs.Any(b => b.Caster.IsMe && b.DisplayName == "XerathR"); }
        }

        public override void Execute()
        {
            if (IsCastingUlt)
            {
                Orbwalker.DisableAttacking = true;
                Orbwalker.DisableMovement = true;
            }
            else
            {
                Orbwalker.DisableAttacking = false;
                Orbwalker.DisableMovement = false;
            }
        }
    }
}
