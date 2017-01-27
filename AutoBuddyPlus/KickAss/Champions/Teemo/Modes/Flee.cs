using EloBuddy;
using EloBuddy.SDK;

//using Settings = KickassSeries.Champions.Teemo.Config.Modes.Flee;

namespace KickassSeries.Champions.Teemo.Modes
{
    public sealed class Flee : ModeBase
    {
        public override bool ShouldBeExecuted()
        {
            return Orbwalker.ActiveModesFlags.HasFlag(Orbwalker.ActiveModes.Flee);
        }

        public override void Execute()
        {
            /*
            // Force move to player's mouse cursor
            Player.IssueOrder(GameObjectOrder.MoveTo, Game.CursorPos);

            // Uses W if avaliable and if toggle is on
            if (Settings.UseW && W.IsReady())
            {
                W.Cast();
            }

            // Uses R if avaliable and if toggle is on
            if (Settings.UseR && R.IsReady() && Settings.RCharge <= Player.Instance.Spellbook.GetSpell(SpellSlot.R).Ammo)
            {
                R.Cast(Player.Instance.ServerPosition);
            }
            */
        }
    }
}
