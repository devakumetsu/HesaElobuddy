using EloBuddy;
using EloBuddy.SDK;

namespace KickassSeries.Activator.Maps.HowlingAbyss.Items
{
    public abstract class Ids
    {
        #region Consumables
        protected static readonly Item HealthPotion = new Item(ItemId.Health_Potion);
        protected static readonly Item ElixirOfIron = new Item(ItemId.Elixir_of_Iron);
        protected static readonly Item ElixirOfSorcery = new Item(ItemId.Elixir_of_Sorcery);
        protected static readonly Item ElixirOfWrath = new Item(ItemId.Elixir_of_Wrath);
        protected static readonly Item Biscuit = new Item(ItemId.Total_Biscuit_of_Rejuvenation);
        protected static readonly Item HuntersPotion = new Item(ItemId.Hunters_Potion);
        protected static readonly Item CorruptingPotion = new Item(ItemId.Corrupting_Potion); 
        protected static readonly Item RefillablePotion = new Item(ItemId.Refillable_Potion);
        #endregion Consumables

        #region Offensive
        protected static readonly Item BilgewaterCutlass = new Item(ItemId.Bilgewater_Cutlass, 550f);
        protected static readonly Item BladeOfTheRuinedKing = new Item(ItemId.Blade_of_the_Ruined_King, 550f);
        protected static readonly Item Hydra = new Item(ItemId.Ravenous_Hydra_Melee_Only, 250f);
        protected static readonly Item Tiamat = new Item(ItemId.Tiamat_Melee_Only, 250f);
        protected static readonly Item TitanicHydra = new Item(ItemId.Titanic_Hydra, Player.Instance.GetAutoAttackRange());
        protected static readonly Item Youmuu = new Item(ItemId.Youmuus_Ghostblade, 600f);
        protected static readonly Item Hextech = new Item(ItemId.Hextech_Gunblade, 700f);
        protected static readonly Item Manamune = new Item(ItemId.Manamune);
        protected static readonly Item FrostQueen = new Item(ItemId.Frost_Queens_Claim, 1000f);
        #endregion Offensive

        #region Defensive
        protected static readonly Item Zhonyas = new Item(ItemId.Zhonyas_Hourglass);
        protected static readonly Item Seraph = new Item(ItemId.Seraphs_Embrace);
        protected static readonly Item FaceOfTheMountain = new Item(ItemId.Face_of_the_Mountain);
        protected static readonly Item Talisman = new Item(ItemId.Talisman_of_Ascension);
        protected static readonly Item Mikael = new Item(ItemId.Mikaels_Crucible, 750f);
        protected static readonly Item Solari = new Item(ItemId.Locket_of_the_Iron_Solari);
        protected static readonly Item Randuin = new Item(ItemId.Randuins_Omen, 500f);
        protected static readonly Item Ohm = new Item(ItemId.Ohmwrecker);
        protected static readonly Item DerbishBlade = new Item(ItemId.Dervish_Blade);
        protected static readonly Item Mercurial = new Item(ItemId.Mercurial_Scimitar);
        protected static readonly Item QuickSilver = new Item(ItemId.Quicksilver_Sash);
        #endregion Defensive
    }
}
