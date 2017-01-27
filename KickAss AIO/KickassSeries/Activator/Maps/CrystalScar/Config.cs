using System.Linq;
using EloBuddy.SDK;
using EloBuddy.SDK.Menu;
using EloBuddy.SDK.Menu.Values;

// ReSharper disable UnusedMember.Global
// ReSharper disable MemberHidesStaticFromOuterClass

namespace KickassSeries.Activator.Maps.CrystalScar
{
    internal static class Config
    {
        private const string MenuName = "KA Activator: CS";

        private static readonly Menu Menu;

        static Config()
        {
            // Initialize the menu
            Menu = MainMenu.AddMenu(MenuName, MenuName.ToLower());
            Menu.AddGroupLabel("KA Activator");
            Menu.AddLabel("Made By: MarioGK", 50);

            // Initialize the modes
            Types.Initialize();
        }

        public static void Initialize()
        {
        }

        public static class Types
        {
            private static readonly Menu OffensiveMenu, DefensiveMenu, ConsumablesMenu;
            public static readonly Menu SummonerMenu, SettingsMenu;

            static Types()
            {
                OffensiveMenu = Menu.AddSubMenu("::Offensive Items::");
                OffensiveItems.Initialize();

                DefensiveMenu = Menu.AddSubMenu("::Defensive Items::");
                DeffensiveItems.Initialize();

                ConsumablesMenu = Menu.AddSubMenu("::Consumables Items::");
                ConsumablesItems.Initialize();

                SummonerMenu = Menu.AddSubMenu("::Summoner Spells::");
                SummonerSpells.Initialize();

                SettingsMenu = Menu.AddSubMenu("::Settings::");
                Settings.Initialize();
            }

            public static void Initialize()
            {
            }

            public static class OffensiveItems
            {
                #region Bilgewater

                private static readonly CheckBox UseBilgewater;
                private static readonly Slider TargetHpBilgewater;

                public static bool Bilgewater
                {
                    get { return UseBilgewater.CurrentValue; }
                }

                public static int BilgewaterTargetHp
                {
                    get { return TargetHpBilgewater.CurrentValue; }
                }

                #endregion Bilgewater

                #region BladeOfTheRuinedKing

                private static readonly CheckBox UseBlade;
                private static readonly Slider MyHpBlade;
                private static readonly Slider TargetHpBlade;

                public static bool Blade
                {
                    get { return UseBlade.CurrentValue; }
                }

                public static int BladeMyHp
                {
                    get { return MyHpBlade.CurrentValue; }
                }

                public static int BladeTargetHp
                {
                    get { return TargetHpBlade.CurrentValue; }
                }

                #endregion BladeOfTheRuinedKing

                #region Tiamat

                private static readonly CheckBox UseTiamat;
                private static readonly Slider TargetHpTiamat;

                public static bool Tiamat
                {
                    get { return UseTiamat.CurrentValue; }
                }

                public static int TiamatTargetHp
                {
                    get { return TargetHpTiamat.CurrentValue; }
                }

                #endregion Tiamat

                #region Hydra

                private static readonly CheckBox UseHydra;
                private static readonly Slider TargetHpHydra;

                public static bool Hydra
                {
                    get { return UseHydra.CurrentValue; }
                }

                public static int HydraTargetHp
                {
                    get { return TargetHpHydra.CurrentValue; }
                }

                #endregion Hydra

                #region Titanic

                private static readonly CheckBox UseTitanicHydra;
                private static readonly Slider TargetHpTitanicHydra;

                public static bool Titanic
                {
                    get { return UseTitanicHydra.CurrentValue; }
                }

                public static int TitanicTargetHp
                {
                    get { return TargetHpTitanicHydra.CurrentValue; }
                }

                #endregion Titanic

                #region Youmuu

                private static readonly CheckBox UseYoumuu;
                private static readonly Slider TargetHpYoumuu;

                public static bool Youmuu
                {
                    get { return UseYoumuu.CurrentValue; }
                }

                public static int YoumuuTargetHp
                {
                    get { return TargetHpYoumuu.CurrentValue; }
                }

                #endregion Youmuu

                #region Hextech

                private static readonly CheckBox UseHextech;
                private static readonly Slider TargetHpHextech;

                public static bool Hextech
                {
                    get { return UseHextech.CurrentValue; }
                }

                public static int HextechTargetHp
                {
                    get { return TargetHpHextech.CurrentValue; }
                }

                #endregion Hextech

                #region Manamune

                private static readonly CheckBox _useManamune;
                private static readonly Slider _targetHPManamune;
                private static readonly Slider _myManaManamune;

                public static bool Manamune
                {
                    get { return _useManamune.CurrentValue; }
                }

                public static int ManamuneTargetHP
                {
                    get { return _targetHPManamune.CurrentValue; }
                }

                public static int ManamuneMana
                {
                    get { return _myManaManamune.CurrentValue; }
                }

                #endregion Manamune

                #region FrostQueen
                private static readonly CheckBox _useFrostQueen;
                private static readonly Slider _targetHPFrostQueen;

                public static bool FrostQueen
                {
                    get { return _useFrostQueen.CurrentValue; }
                }

                public static int TargetHPFrostQueen
                {
                    get { return _targetHPFrostQueen.CurrentValue; }
                }

                #endregion FrostQueen

                #region Offensive Menu

                static OffensiveItems()
                {
                    // Initialize the menu values
                    OffensiveMenu.AddGroupLabel("Bilgewater Cutlass");
                    UseBilgewater = OffensiveMenu.Add("useBilgewater", new CheckBox("Use Bilgewater Cutlass ?"));
                    TargetHpBilgewater = OffensiveMenu.Add("useBilgewaterTargetHP",
                        new Slider("Use Bilgewater Cutlass When Target`s Health is lower than ({0}%)", 80));

                    OffensiveMenu.AddGroupLabel("Blade Of The Ruined King");
                    UseBlade = OffensiveMenu.Add("useBlade", new CheckBox("Use Blade Of The Ruined King ?"));
                    MyHpBlade = OffensiveMenu.Add("useBladeMyHP",
                        new Slider("Use Blade Of The Ruined King When My Health hits X%", 80));
                    TargetHpBlade = OffensiveMenu.Add("useBladeTargetHP",
                        new Slider("Use Blade Of The Ruined King When Target`s Health is lower than ({0}%)", 80));

                    OffensiveMenu.AddGroupLabel("Tiamat");
                    UseTiamat = OffensiveMenu.Add("useTiamat", new CheckBox("Use Tiamat ?"));
                    TargetHpTiamat = OffensiveMenu.Add("useTiamatTargetHP",
                        new Slider("Use Tiamat When Target`s Health is lower than ({0}%)", 80));

                    OffensiveMenu.AddGroupLabel("Ravenous Hydra");
                    UseHydra = OffensiveMenu.Add("useHydra", new CheckBox("Use Ravenous Hydra ?"));
                    TargetHpHydra = OffensiveMenu.Add("useHydraTargetHP",
                        new Slider("Use Ravenous Hydra When Target`s Health is lower than ({0}%)", 80));

                    OffensiveMenu.AddGroupLabel("Titanic Hydra");
                    UseTitanicHydra = OffensiveMenu.Add("useTitanic", new CheckBox("Use Titanic Hydra ?"));
                    TargetHpTitanicHydra = OffensiveMenu.Add("useTitanicTargetHP",
                        new Slider("Use Titanic Hydra When Target`s Health is lower than ({0}%)", 80));

                    OffensiveMenu.AddGroupLabel("Youmuu");
                    UseYoumuu = OffensiveMenu.Add("useYoumuu ", new CheckBox("Use Youmuu ?"));
                    TargetHpYoumuu = OffensiveMenu.Add("useYoumuuTargetHP",
                        new Slider("Use Youmuu  When Target`s Health is lower than ({0}%)", 80));

                    OffensiveMenu.AddGroupLabel("Hextech");
                    UseHextech = OffensiveMenu.Add("useHextech", new CheckBox("Use Hextech ?"));
                    TargetHpHextech = OffensiveMenu.Add("useHextechTargetHP",
                        new Slider("Use Hextech When Target`s Health is lower than ({0}%)", 80));

                    OffensiveMenu.AddGroupLabel("Manamune");
                    _useManamune = OffensiveMenu.Add("useManamune", new CheckBox("Use Manamune ?"));
                    _targetHPManamune = OffensiveMenu.Add("useManamuneTargetHP",
                        new Slider("Use Manamune When Target`s Health is lower than ({0}%)", 80));
                    _myManaManamune = OffensiveMenu.Add("useManamuneManamine",
                        new Slider("Use Manamune When mana is greater than ({0}%)", 30));

                    OffensiveMenu.AddGroupLabel("FrostQueen");
                    _useFrostQueen = OffensiveMenu.Add("useFrostQueen", new CheckBox("Use FrostQueen ?"));
                    _targetHPFrostQueen = OffensiveMenu.Add("useFrostQueenTargetHP",
                        new Slider("Use Manamune When Target`s Health is lower than ({0}%)", 80));

                }

                #endregion Offensive Menu

                public static void Initialize()
                {
                }
            }

            public static class DeffensiveItems
            {
                #region Zhonyas

                private static readonly CheckBox UseZhonyas;
                private static readonly Slider HpZhonyas;

                public static bool Zhonyas
                {
                    get { return UseZhonyas.CurrentValue; }
                }

                public static int ZhonyasMyHp
                {
                    get { return HpZhonyas.CurrentValue; }
                }

                #endregion Zhonyas

                #region Seraph

                private static readonly CheckBox _useSeraph;
                private static readonly Slider _myHPSeraph;

                public static bool UseSeraph
                {
                    get { return _useSeraph.CurrentValue; }
                }

                public static int MYHPSeraph
                {
                    get { return _myHPSeraph.CurrentValue; }
                }

                #endregion Seraph

                #region FaceOfTheMountain

                private static readonly CheckBox _useFaceOfTheMountain;
                private static readonly Slider _AllyHPFaceOfTheMountain;

                public static bool UseFaceOfTheMountain
                {
                    get { return _useFaceOfTheMountain.CurrentValue; }
                }

                public static int AllyHPFaceOfTheMountain
                {
                    get { return _AllyHPFaceOfTheMountain.CurrentValue; }
                }

                #endregion FaceOfTheMountain

                #region Talisman

                private static readonly CheckBox _useTalisman;
                private static readonly Slider _myHPTalisman;

                public static bool UseTalisman
                {
                    get { return _useTalisman.CurrentValue; }
                }

                public static int MYHPTalisman
                {
                    get { return _myHPTalisman.CurrentValue; }
                }

                #endregion Talisman

                #region Mikael(to heal)

                private static readonly CheckBox _useMikaelHeal;
                private static readonly Slider _allyHPMikaelHeal;

                public static bool UseMikaelHeal
                {
                    get { return _useMikaelHeal.CurrentValue; }
                }

                public static int AllyHPMikaelHeal
                {
                    get { return _allyHPMikaelHeal.CurrentValue; }
                }

                #endregion Mikael(to heal)

                #region Solari

                private static readonly CheckBox _useSolari;
                private static readonly Slider _allyHealthSolari;

                public static bool Solari
                {
                    get { return _useSolari.CurrentValue; }
                }

                public static int AllyHealthSolari
                {
                    get { return _allyHealthSolari.CurrentValue; }
                }

                #endregion Solari

                #region Randuin

                private static readonly CheckBox _useRanduin;
                private static readonly Slider _enemyCountRanduin;

                public static bool Randuin
                {
                    get { return _useRanduin.CurrentValue; }
                }

                public static int EnemiesCountRanduin
                {
                    get { return _enemyCountRanduin.CurrentValue; }
                }

                #endregion Randuin

                #region Ohm

                private static readonly CheckBox _useOhm;
                private static readonly Slider _healthOhm;

                public static bool Ohm
                {
                    get { return _useOhm.CurrentValue; }
                }

                public static int OhmHealth
                {
                    get { return _healthOhm.CurrentValue; }
                }

                #endregion Ohm

                #region Cleanse
                //Items
                private static readonly CheckBox _useDerbishBlade;
                public static bool DerbishBlade
                {
                    get { return _useDerbishBlade.CurrentValue; }
                }

                private static readonly CheckBox _useMercurial;
                public static bool Mercurial
                {
                    get { return _useMercurial.CurrentValue; }
                }

                private static readonly CheckBox _useQuickSilver;
                public static bool QuickSilver
                {
                    get { return _useQuickSilver.CurrentValue; }
                }

                private static readonly CheckBox _useMikaelCleanse;
                public static bool MikaelCleanse
                {
                    get { return _useMikaelCleanse.CurrentValue; }
                }
                //Settings
                private static readonly Slider _useCleanseHP;
                public static int CleanseHP
                {
                    get { return _useCleanseHP.CurrentValue; }
                }

                private static readonly Slider _useCleanseDelay;
                public static int CleanseDelay
                {
                    get { return _useCleanseDelay.CurrentValue; }
                }
                //CCs
                private static readonly CheckBox _useForPolymorphs;
                public static bool Polymorphs
                {
                    get { return _useForPolymorphs.CurrentValue; }
                }

                private static readonly CheckBox _useForSnares;
                public static bool Snares
                {
                    get { return _useForSnares.CurrentValue; }
                }

                private static readonly CheckBox _useForTaunts;
                public static bool Taunts
                {
                    get { return _useForTaunts.CurrentValue; }
                }

                private static readonly CheckBox _useForSilences;
                public static bool Silences
                {
                    get { return _useForSilences.CurrentValue; }
                }

                private static readonly CheckBox _useForCharms;
                public static bool Charms
                {
                    get { return _useForCharms.CurrentValue; }
                }

                private static readonly CheckBox _useForStuns;
                public static bool Stuns
                {
                    get { return _useForStuns.CurrentValue; }
                }

                private static readonly CheckBox _useForBlinds;
                public static bool Blinds
                {
                    get { return _useForBlinds.CurrentValue; }
                }

                private static readonly CheckBox _useForFears;
                public static bool Fears
                {
                    get { return _useForFears.CurrentValue; }
                }
                //Special Cases
                private static readonly CheckBox _useCleanseZedUlt;
                public static bool ZedUlt
                {
                    get { return _useCleanseZedUlt.CurrentValue; }
                }

                private static readonly CheckBox _useCleanseVladUlt;
                public static bool VladUlt
                {
                    get { return _useCleanseVladUlt.CurrentValue; }
                }

                private static readonly CheckBox _useCleanseMordeUlt;
                public static bool MordeUlt
                {
                    get { return _useCleanseMordeUlt.CurrentValue; }
                }

                #endregion Cleanse

                static DeffensiveItems()
                {
                    DefensiveMenu.AddGroupLabel("Wooglet");
                    UseZhonyas = DefensiveMenu.Add("useZhonyas", new CheckBox("Use Wooglet ?"));
                    HpZhonyas = DefensiveMenu.Add("useZhonyasrMyHP",
                        new Slider("Use Wooglet When My Health hits {0}%", 20));

                    DefensiveMenu.AddGroupLabel("Seraph");
                    _useSeraph = DefensiveMenu.Add("useSeraph", new CheckBox("Use Seraph ?"));
                    _myHPSeraph = DefensiveMenu.Add("useSeraphMyHP",
                        new Slider("Use Seraph When My Health is lower than {0}%", 30));

                    DefensiveMenu.AddGroupLabel("Face Of The Mountain");
                    _useFaceOfTheMountain = DefensiveMenu.Add("useFaceOfTheMountain", new CheckBox("Use Face Of The Mountain ?"));
                    _AllyHPFaceOfTheMountain = DefensiveMenu.Add("useFaceOfTheMountainMyHP",
                        new Slider("Use Face Of The Mountain When Ally Health is lower than {0}%", 30));

                    DefensiveMenu.AddGroupLabel("Talisman");
                    _useTalisman = DefensiveMenu.Add("useTalisman", new CheckBox("Use Talisman?"));
                    _myHPTalisman = DefensiveMenu.Add("useTalismanMyHP",
                        new Slider("Use Talisman When My Health hits {0}%", 30));
                    
                    DefensiveMenu.AddGroupLabel("Mikael(to heal)");
                    _useMikaelHeal = DefensiveMenu.Add("useMikael", new CheckBox("Use Mikael(to heal) ?"));
                    _allyHPMikaelHeal = DefensiveMenu.Add("useMikaelAllyHP",
                        new Slider("Use Mikael(to heal) When Ally Health is lower than {0}%", 10));

                    DefensiveMenu.AddGroupLabel("Solari");
                    _useSolari = DefensiveMenu.Add("useSolari", new CheckBox("Use Solari ?"));
                    _allyHealthSolari = DefensiveMenu.Add("useSolariMyHP",
                        new Slider("Use Solari When Ally Health is lower than {0}%", 30));

                    DefensiveMenu.AddGroupLabel("Randuin");
                    _useRanduin = DefensiveMenu.Add("useRanduin", new CheckBox("Use Randuin ?"));
                    _enemyCountRanduin = DefensiveMenu.Add("useRanduinEnemiesCount",
                        new Slider("Use Randuin When there are at least ({0}) enemies in range", 2,1,5));

                    DefensiveMenu.AddGroupLabel("Ohm");
                    _useOhm = DefensiveMenu.Add("useOhm", new CheckBox("Use Ohm ?"));
                    _healthOhm = DefensiveMenu.Add("useOhmMyHP",
                        new Slider("Use Ohm When Ally Health is lower than {0}% and towers is targetting you", 30));

                    DefensiveMenu.AddGroupLabel("Cleanse Settings");
                    _useDerbishBlade = DefensiveMenu.Add("useDerbishBlade", new CheckBox("Use DerbishBlade?"));
                    _useMercurial = DefensiveMenu.Add("useMercurial", new CheckBox("Use Mercurial?"));
                    _useQuickSilver = DefensiveMenu.Add("useQuickSilver", new CheckBox("Use QuickSilver?"));
                    _useMikaelCleanse = DefensiveMenu.Add("useMikaelCleanse", new CheckBox("Use Mikael(To Cleanse)?"));
                    _useCleanseHP = DefensiveMenu.Add("useCleanseHP",
                        new Slider("Use Cleanse items When Health is lower than ({0}%)", 30));
                    _useCleanseDelay = DefensiveMenu.Add("useCleanseDelay",
                        new Slider("Delay yo use the cleanse items({0} MS)", 100,0,500));
                    DefensiveMenu.AddSeparator(1);
                    DefensiveMenu.AddLabel("What kind to CC to use the items");
                    _useForPolymorphs = DefensiveMenu.Add("usePolymorph", new CheckBox("Cleanse Polymorphs ?"));
                    _useForSnares = DefensiveMenu.Add("useSnare", new CheckBox("Cleanse Snares ?", false));
                    _useForTaunts = DefensiveMenu.Add("useTaunt", new CheckBox("Cleanse Taunts ?"));
                    _useForSilences = DefensiveMenu.Add("useSilence", new CheckBox("Cleanse Silences ?"));
                    _useForCharms = DefensiveMenu.Add("useCharm", new CheckBox("Cleanse Charms ?"));
                    _useForStuns = DefensiveMenu.Add("useStun", new CheckBox("Cleanse Stuns ?"));
                    _useForBlinds = DefensiveMenu.Add("useBlind", new CheckBox("Cleanse Blinds ?"));
                    _useForFears = DefensiveMenu.Add("useFear", new CheckBox("Cleanse Fears ?"));
                    DefensiveMenu.AddSeparator(1);
                    DefensiveMenu.AddLabel("Special cases to use the items");
                    _useCleanseZedUlt = DefensiveMenu.Add("useCCZedUlt", new CheckBox("Cleanse Zed Ultimate ?"));
                    _useCleanseVladUlt = DefensiveMenu.Add("useCCVladmirUlt", new CheckBox("Cleanse Vladmir Ultimate ?"));
                    _useCleanseMordeUlt = DefensiveMenu.Add("useCCMordekaiserUlt", new CheckBox("Cleanse Mordekaiser Ultimate ?"));

                }

                public static void Initialize()
                {
                }
                
                }

                public static class ConsumablesItems
            {
                #region HP Pot

                private static readonly CheckBox UseHealthPot;
                private static readonly Slider MinHealthPot;

                public static bool UseHPpot
                {
                    get { return UseHealthPot.CurrentValue; }
                }

                public static int MinHPpot
                {
                    get { return MinHealthPot.CurrentValue; }
                }

                #endregion HP Pot

                #region Biscuit

                private static readonly CheckBox UseBiscuit;
                private static readonly Slider MinHealthBiscuit;
                private static readonly Slider MinManaBiscuit;

                public static bool UseBiscuits
                {
                    get { return UseBiscuit.CurrentValue; }
                }

                public static int MinBiscuitHp
                {
                    get { return MinHealthBiscuit.CurrentValue; }
                }

                public static int MinBiscuitMp
                {
                    get { return MinManaBiscuit.CurrentValue; }
                }

                #endregion Biscuit

                #region Refill

                private static readonly CheckBox UseRefillPotion;
                private static readonly Slider MinHPRefillPotion;

                public static bool UseRefillPOT
                {
                    get { return UseRefillPotion.CurrentValue; }
                }

                public static int MinRefillHp
                {
                    get { return MinHPRefillPotion.CurrentValue; }
                }

                #endregion Refill

                #region Corrupt

                private static readonly CheckBox UseCorrupt;
                private static readonly Slider MinHealthCorrupt;
                private static readonly Slider MinManaCorrupt;

                public static bool UseCorrupts
                {
                    get { return UseCorrupt.CurrentValue; }
                }

                public static int MinCorruptHp
                {
                    get { return MinHealthCorrupt.CurrentValue; }
                }

                public static int MinCorruptMp
                {
                    get { return MinManaCorrupt.CurrentValue; }
                }

                #endregion Corrupt

                #region Hunter`s

                private static readonly CheckBox UseHunter;
                private static readonly Slider MinHealthHunter;
                private static readonly Slider MinManaHunter;

                public static bool UseHunters
                {
                    get { return UseHunter.CurrentValue; }
                }

                public static int MinHunterHp
                {
                    get { return MinHealthHunter.CurrentValue; }
                }

                public static int MinHunterMp
                {
                    get { return MinManaHunter.CurrentValue; }
                }

                #endregion Hunter`s

                static ConsumablesItems()
                {
                    ConsumablesMenu.AddGroupLabel("Health Pot");
                    UseHealthPot = ConsumablesMenu.Add("useHealthPot", new CheckBox("Use Health Pot ?"));
                    MinHealthPot = ConsumablesMenu.Add("minHealthPot", new Slider("Min Health to use Health Pot", 30));

                    ConsumablesMenu.AddGroupLabel("Biscuit");
                    UseBiscuit = ConsumablesMenu.Add("useBiscuit", new CheckBox("Use Biscuit ?"));
                    MinHealthBiscuit = ConsumablesMenu.Add("minhpBiscuit", new Slider("Min Health to use Biscuit", 30));
                    MinManaBiscuit = ConsumablesMenu.Add("minmpBiscuit", new Slider("Min Mana to use Biscuit", 30));

                    ConsumablesMenu.AddGroupLabel("Refill Potion");
                    UseRefillPotion = ConsumablesMenu.Add("useRefill", new CheckBox("Use Refill Potion ?"));
                    MinHPRefillPotion = ConsumablesMenu.Add("minhpRefill",
                        new Slider("Min Health to use Refill Potion", 30));

                    ConsumablesMenu.AddGroupLabel("Corrupt Potion");
                    UseCorrupt = ConsumablesMenu.Add("useCorrupt", new CheckBox("Use Corrupt Potion ?"));
                    MinHealthCorrupt = ConsumablesMenu.Add("minhpCorrupt",
                        new Slider("Min Health to use Corrupt Potion", 30));
                    MinManaCorrupt = ConsumablesMenu.Add("minmpCorrupt",
                        new Slider("Min Mana to use Corrupt Potion", 30));

                    ConsumablesMenu.AddGroupLabel("Hunter`s Potion");
                    UseHunter = ConsumablesMenu.Add("useHunter", new CheckBox("Use Hunter`s Potion ?"));
                    MinHealthHunter = ConsumablesMenu.Add("minhpHunter",
                        new Slider("Min Health to use Hunter`s Potion", 30));
                    MinManaHunter = ConsumablesMenu.Add("minmpHunter", new Slider("Min Mana to use Hunter`s Potion", 30));
                }

                public static void Initialize()
                {
                }
            }

            public static class SummonerSpells
            {
                #region Smite

                private static readonly CheckBox UseSmite;
                private static readonly CheckBox UseSmiteDuel;
                private static readonly CheckBox UseSmiteKs;


                public static bool Smite
                {
                    get { return UseSmite.CurrentValue; }
                }

                public static bool DuelSmite
                {
                    get { return UseSmiteDuel.CurrentValue; }
                }

                public static bool KsSmite
                {
                    get { return UseSmiteKs.CurrentValue; }
                }

                #endregion Smite

                #region Heal

                private static readonly CheckBox _useHeal;
                private static readonly Slider _healHealth;
                private static readonly CheckBox _useHealAlly;
                private static readonly Slider _healAllyHealth;

                public static bool UseHeal
                {
                    get { return _useHeal.CurrentValue; }
                }
                
                public static int HealHealth
                {
                    get { return _healHealth.CurrentValue; }
                }

                public static bool UseHealAlly
                {
                    get { return _useHealAlly.CurrentValue; }
                }

                public static int HealAllyHealth
                {
                    get { return _healAllyHealth.CurrentValue; }
                }

                #endregion Heal

                #region Barrier

                private static readonly CheckBox _useBarrier;
                private static readonly Slider _healthBarrier;

                public static bool UseBarrier
                {
                    get { return _useBarrier.CurrentValue; }
                }
                
                public static int BarrierHealth
                {
                    get { return _healthBarrier.CurrentValue; }
                }

                #endregion Barrier

                #region Ghost

                private static readonly CheckBox Ghost;

                public static bool UseGhost
                {
                    get { return Ghost.CurrentValue; }
                }

                #endregion Ghost


                static SummonerSpells()
                {
                    #region SmiteMenu

                    if (KickassSeries.Activator.SummonerSpells.Extensions.HasSpell("summonersmite"))
                    {
                        // Initialize the menu values
                        SummonerMenu.AddGroupLabel("Smite");
                        UseSmite = SummonerMenu.Add("useSmite", new CheckBox("Use Smite ?"));
                        SummonerMenu.AddLabel("Epic Camps");
                        SummonerMenu.Add("SRU_Baron", new CheckBox("Baron"));
                        SummonerMenu.Add("SRU_Dragon", new CheckBox("Dragon"));
                        SummonerMenu.AddLabel("Normal Camps");
                        SummonerMenu.Add("SRU_Blue", new CheckBox("Blue"));
                        SummonerMenu.Add("SRU_Red", new CheckBox("Red"));
                        SummonerMenu.Add("Sru_Crab", new CheckBox("Skuttles(Crab)"));
                        SummonerMenu.AddLabel("Small Camps");
                        SummonerMenu.Add("SRU_Gromp", new CheckBox("Gromp", false));
                        SummonerMenu.Add("SRU_Murkwolf", new CheckBox("Murkwolf", false));
                        SummonerMenu.Add("SRU_Krug", new CheckBox("Krug", false));
                        SummonerMenu.Add("SRU_Razorbeak", new CheckBox("Razerbeak", false));
                        SummonerMenu.AddSeparator();

                        UseSmiteDuel = SummonerMenu.Add("useSmiteDuel", new CheckBox("Use Red Smite(Duel) ?"));
                        UseSmiteKs = SummonerMenu.Add("useSmiteKS", new CheckBox("Use Blue Smite(KS) ?"));
                        SummonerMenu.AddSeparator();
                    }

                    #endregion SmiteMenu

                    #region HealMenu

                    if (KickassSeries.Activator.SummonerSpells.Extensions.HasSpell("summonerheal"))
                    {
                        SummonerMenu.AddGroupLabel("Summoner Heal Settings:");
                        _useHeal = SummonerMenu.Add("useHeal", new CheckBox("Use heal ?"));
                        _healHealth = SummonerMenu.Add("healthHeal", new Slider("Use Heal when health hits {0}%", 20));
                        _useHealAlly = SummonerMenu.Add("useHealAlly", new CheckBox("Use heal in allies ?"));
                        _healAllyHealth = SummonerMenu.Add("healthAllyHeal", new Slider("Use Heal when ally health hits {0}%", 30));
                    }

                    #endregion HealMenu

                    #region BarrierMenu

                    if (KickassSeries.Activator.SummonerSpells.Extensions.HasSpell("summonerbarrier"))
                    {
                        SummonerMenu.AddGroupLabel("Summoner Barrier Settings:");
                        _useBarrier = SummonerMenu.Add("useBarrier", new CheckBox("Use barrier ?"));
                        _healthBarrier = SummonerMenu.Add("healthBarrier", new Slider("Use Barrier when health hits {0}%", 30));
                    }

                    #endregion BarrierMenu

                    #region GhostMenu

                    if (KickassSeries.Activator.SummonerSpells.Extensions.HasSpell("summonerghost"))
                    {
                        SummonerMenu.AddGroupLabel("Ghost");
                        Ghost = SummonerMenu.Add("Ghostuse", new CheckBox("Use ghost when you are in danger ?"));
                        SummonerMenu.AddLabel("You can configure when you are in danger in the settings menu");
                        SummonerMenu.AddSeparator();
                    }

                    #endregion GhostMenu
                }

                public static void Initialize()
                {
                }
            }

            public static class Settings
            {
                //Offensive
                private static readonly CheckBox _AAcancel;
                private static readonly Slider _DelayOff;

                public static bool AACancel
                {
                    get { return _AAcancel.CurrentValue; }
                }

                public static int DelayBetweenOff
                {
                    get { return _DelayOff.CurrentValue; }
                }

                //Danger
                private static readonly Slider EnemyRange;
                private static readonly Slider EnemySlider;
                private static readonly CheckBox Spells;
                private static readonly CheckBox Skillshots;
                private static readonly CheckBox AAs;


                public static int RangeEnemy
                {
                    get { return EnemyRange.CurrentValue; }
                }

                public static int EnemyCount
                {
                    get { return EnemySlider.CurrentValue; }
                }

                public static bool ConsiderSpells
                {
                    get { return Spells.CurrentValue; }
                }

                public static bool ConsiderSkillshots
                {
                    get { return Skillshots.CurrentValue; }
                }

                public static bool ConsiderAttacks
                {
                    get { return AAs.CurrentValue; }
                }

                static Settings()
                {
                    SettingsMenu.AddGroupLabel("Offensive Settings");
                    _AAcancel = SettingsMenu.Add("aacanceloff", new CheckBox("Cancel AA animation with items ?"));
                    _DelayOff = SettingsMenu.Add("delayoffbetween", new Slider("Delay between each offensive item used(in MS)", 1000, 100, 5000));
                    SettingsMenu.AddGroupLabel("Danger Settings");
                    EnemySlider = SettingsMenu.Add("minenemiesinrange", new Slider("Min enemies in the range determined below", 1, 1, 5));
                    EnemyRange = SettingsMenu.Add("minrangeenemy", new Slider("Enemies must be in ({0}) range to be in danger", 1000, 600, 2500));
                    Spells = SettingsMenu.Add("considerspells", new CheckBox("Consider spells ?"));
                    Skillshots = SettingsMenu.Add("considerskilshots", new CheckBox("Consider SkillShots ?"));
                    AAs = SettingsMenu.Add("consideraas", new CheckBox("Consider Auto Attacks ?"));
                    SettingsMenu.AddSeparator();
                    SettingsMenu.AddGroupLabel("Dangerous Spells");
                    foreach (var spell in DMGHandler.DangerousSpells.Spells.Where(x => EntityManager.Heroes.Enemies.Any(b => b.Hero == x.Hero)))
                    {
                        SettingsMenu.Add(spell.Hero.ToString() + spell.Slot,new CheckBox(spell.Hero + " - " + spell.Slot + ".", spell.DefaultValue));
                    }
                }

                public static void Initialize()
                {
                }
            }
        }
    }
}


