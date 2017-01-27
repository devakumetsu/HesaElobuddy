using EloBuddy;
using EloBuddy.SDK;
using EloBuddy.SDK.Menu;
using EloBuddy.SDK.Menu.Values;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutoBuddy.Utilities.AutoShop
{
    internal class AutoShop
    {
        static bool isDelayed = false;
        static bool Enabled
        {
            get { return MainMenu.GetMenu("AB").Get<CheckBox>("autoshop").CurrentValue; }
        }

        public static void OnTick(EventArgs args)
        {
            if (!Enabled || isDelayed) return;
            //Core.DelayAction(() =>
            //{
            isDelayed = true;
            if (IsInShop())
            {
                //Build
                BuildController.BuyOrSellItems();
            }
            isDelayed = false;
            //}, 100);
        }

        public static bool IsInShop()
        {
            return Shop.CanShop || ObjectManager.Player.IsInShopRange();
        }
        
        public static bool Recalling()
        {
            return ObjectManager.Player.IsRecalling();
        }
    }
}