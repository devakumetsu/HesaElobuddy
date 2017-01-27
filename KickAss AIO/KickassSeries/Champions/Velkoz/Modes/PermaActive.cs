using EloBuddy;
using SharpDX;

namespace KickassSeries.Champions.Velkoz.Modes
{
    public sealed class PermaActive : ModeBase
    {
        public override bool ShouldBeExecuted()
        {
            return true;
        }

        public override void Execute()
        {
            var targetpos = new Vector3();
            Player.Instance.Spellbook.UpdateChargeableSpell(SpellSlot.R, targetpos, false);
        }
    }
}
