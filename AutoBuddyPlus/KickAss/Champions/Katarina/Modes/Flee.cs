using System;
using System.Linq;
using EloBuddy;
using EloBuddy.SDK;
using SharpDX;

namespace KickassSeries.Champions.Katarina.Modes
{
    public sealed class Flee : ModeBase
    {
        public override bool ShouldBeExecuted()
        {
            return Orbwalker.ActiveModesFlags.HasFlag(Orbwalker.ActiveModes.Flee);
        }

        private static int _lastWardCast;

        public static void WardJump(Vector3 pos)
        {
            var wards = new[]
           {
                ItemId.Warding_Totem_Trinket, ItemId.Sightstone, ItemId.Ruby_Sightstone,
                ItemId.Greater_Stealth_Totem_Trinket, ItemId.Greater_Vision_Totem_Trinket
            };
            var ward = Player.Instance.InventoryItems.FirstOrDefault(a => wards.Contains(a.Id) && a.CanUseItem());

            if (SpellManager.Q.IsReady() && _lastWardCast < Environment.TickCount && ward != null)
            {
                ward.Cast(Player.Instance.Position.Extend(pos, 590).To3D());
                _lastWardCast = Environment.TickCount + 1000;
            }
        }

        public override void Execute()
        {  
            WardJump(Game.CursorPos);
        }
    }
}
