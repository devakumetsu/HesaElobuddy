using System.Linq;
using EloBuddy;
using EloBuddy.SDK;
using EloBuddy.SDK.Menu.Values;
using KickassSeries.Activator.NoMenuDMGHandler;

using Settings = KickassSeries.Champions.Kayle.Config.Modes.Misc;

namespace KickassSeries.Champions.Kayle.Modes
{
    public sealed class PermaActive : ModeBase
    {
        public override bool ShouldBeExecuted()
        {
            return true;
        }

        public override void Execute()
        {
            var allyR = EntityManager.Heroes.Allies.FirstOrDefault(a => a.InDanger(Settings.UseRHealth) && a.IsValidTarget(R.Range));

            if (allyR != null)
            {
                if (Config.Modes.MiscMenu["allyUseR" + allyR.ChampionName].Cast<CheckBox>().CurrentValue &&
                    Settings.UseR && Player.Instance.ManaPercent >= Settings.UseRMana && allyR.IsValidTarget(R.Range))
                {
                    R.Cast(allyR);
                }
            }

            var allyW = EntityManager.Heroes.Allies.FirstOrDefault(a => a.InDanger(Settings.UseWHealth) && a.IsValidTarget(W.Range));

            if (allyW != null)
            {
                if (Config.Modes.MiscMenu["allyUseW" + allyW.ChampionName].Cast<CheckBox>().CurrentValue &&
                    Settings.UseW && Player.Instance.ManaPercent >= Settings.UseWMana && allyW.IsValidTarget(W.Range))
                {
                    W.Cast(allyW);
                }
            }
        }
    }
}
