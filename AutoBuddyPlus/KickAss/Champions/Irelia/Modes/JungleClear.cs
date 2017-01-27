using System.Linq;
using EloBuddy;
using EloBuddy.SDK;

using Settings = KickassSeries.Champions.Irelia.Config.Modes.LaneClear;

namespace KickassSeries.Champions.Irelia.Modes
{
    public sealed class JungleClear : ModeBase
    {
        public override bool ShouldBeExecuted()
        {
            return Orbwalker.ActiveModesFlags.HasFlag(Orbwalker.ActiveModes.JungleClear);
        }

        public override void Execute()
        {
            if (Q.IsReady() && Settings.UseQ && Player.Instance.ManaPercent >= Settings.LaneMana)
            {
                var minionq =
                    EntityManager.MinionsAndMonsters.GetJungleMonsters().FirstOrDefault(
                        m => m.IsValidTarget(Q.Range));

                if (minionq != null)
                {
                    Q.Cast(minionq);
                }
            }

            if (EventsManager.CanW && Settings.UseW && Player.Instance.ManaPercent <= Settings.LaneMana)
            {
                W.Cast();
            }
        }
    }
}
