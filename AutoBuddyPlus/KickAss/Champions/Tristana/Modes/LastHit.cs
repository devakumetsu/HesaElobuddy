using System.Linq;
using EloBuddy;
using EloBuddy.SDK;

namespace KickassSeries.Champions.Tristana.Modes
{
    public sealed class LastHit : ModeBase
    {
        public override bool ShouldBeExecuted()
        {
            return Orbwalker.ActiveModesFlags.HasFlag(Orbwalker.ActiveModes.LastHit);
        }

        public override void Execute()
        {
            var minionE =
    EntityManager.MinionsAndMonsters.GetLaneMinions()
        .FirstOrDefault(
            m => m.IsValidTarget(Player.Instance.AttackRange) && m.GetBuffCount("tristanaecharge") > 0);
            if (minionE != null)
            {
                Orbwalker.ForcedTarget = minionE;
            }
        }
    }
}
