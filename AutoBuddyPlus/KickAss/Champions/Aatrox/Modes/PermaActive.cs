using EloBuddy;
using EloBuddy.SDK;

using Settings = KickassSeries.Champions.Aatrox.Config.Modes.Misc;

namespace KickassSeries.Champions.Aatrox.Modes
{
    public sealed class PermaActive : ModeBase
    {
        public override bool ShouldBeExecuted()
        {
            return true;
        }

        public override void Execute()
        {
            var target = TargetSelector.GetTarget(Q.Range, DamageType.Physical);
            if (target == null || target.IsZombie) return;

            //W Manager
            if (Player.Instance.Spellbook.GetSpell(SpellSlot.W).ToggleState == 2 && Player.Instance.HealthPercent <= Settings.WHeal)
            {
                W.Cast();
            }

            if (Player.Instance.Spellbook.GetSpell(SpellSlot.W).ToggleState == 1 && Player.Instance.HealthPercent >= Settings.WDamage)
            {
                W.Cast();
            }
            //W Manager
        }
    }
}
