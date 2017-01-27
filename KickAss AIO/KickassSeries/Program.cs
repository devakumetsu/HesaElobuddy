using System;
using System.Linq;
using EloBuddy;
using EloBuddy.SDK.Events;

namespace KickassSeries
{
    internal static class Program
    {
        // ReSharper disable once UnusedParameter.Local
        private static void Main(string[] args)
        {
            Loading.OnLoadingComplete += Loading_OnLoadingComplete;
        }

        private static void Loading_OnLoadingComplete(EventArgs args)
        {
            try
            {
                #region Champion`s Swith
                switch (ObjectManager.Player.Hero)
                {
                    case Champion.Aatrox:
                        Champions.Aatrox.Aatrox.Initialize();
                        break;
                    case Champion.Ahri:
                        Champions.Ahri.Ahri.Initialize();
                        break;
                    case Champion.Akali:
                        Champions.Akali.Akali.Initialize();
                        break;
                    case Champion.Alistar:
                        Champions.Alistar.Alistar.Initialize();
                        break;
                    case Champion.Amumu:
                        Champions.Amumu.Amumu.Initialize();
                        break;
                    case Champion.Anivia:
                        Champions.Anivia.Anivia.Initialize();
                        break;
                    case Champion.Annie:
                        Champions.Annie.Annie.Initialize();
                        break;
                    case Champion.Ashe:
                        Champions.Ashe.Ashe.Initialize();
                        break;
                    case Champion.Azir:
                        Champions.Azir.Azir.Initialize();
                        break;
                    case Champion.Bard:
                        Champions.Bard.Bard.Initialize();
                        break;
                    case Champion.Blitzcrank:
                        Champions.Blitzcrank.Blitzcrank.Initialize();
                        break;
                    case Champion.Brand:
                        Champions.Brand.Brand.Initialize();
                        break;
                    case Champion.Braum:
                        Champions.Braum.Braum.Initialize();
                        break;
                    case Champion.Caitlyn:
                        Champions.Caitlyn.Caitlyn.Initialize();
                        break;
                    case Champion.Cassiopeia:
                        Champions.Cassiopeia.Cassiopeia.Initialize();
                        break;
                    case Champion.Chogath:
                        Champions.ChoGath.ChoGath.Initialize();
                        break;
                    case Champion.Corki:
                        Champions.Corki.Corki.Initialize();
                        break;
                    case Champion.Darius:
                        Champions.Darius.Darius.Initialize();
                        break;
                    case Champion.Diana:
                        Champions.Diana.Diana.Initialize();
                        break;
                    case Champion.DrMundo:
                        Champions.DrMundo.DrMundo.Initialize();
                        break;
                    case Champion.Draven:
                        Champions.Draven.Draven.Initialize();
                        break;
                    case Champion.Ekko:
                        Champions.Ekko.Ekko.Initialize();
                        break;
                    case Champion.Elise:
                        Champions.Elise.Elise.Initialize();
                        break;
                    case Champion.Evelynn:
                        Champions.Evelynn.Evelynn.Initialize();
                        break;
                    case Champion.Ezreal:
                        Champions.Ezreal.Ezreal.Initialize();
                        break;
                    case Champion.FiddleSticks:
                        Champions.FiddleSticks.FiddleSticks.Initialize();
                        break;
                    case Champion.Fiora:
                        Champions.Fiora.Fiora.Initialize();
                        break;
                    case Champion.Fizz:
                        Champions.Fizz.Fizz.Initialize();
                        break;
                    case Champion.Galio:
                        Champions.Galio.Galio.Initialize();
                        break;
                    case Champion.Gangplank:
                        Champions.Gangplank.Gangplank.Initialize();
                        break;
                    case Champion.Garen:
                        Champions.Garen.Garen.Initialize();
                        break;
                    case Champion.Gnar:
                        Champions.Gnar.Gnar.Initialize();
                        break;
                    case Champion.Gragas:
                        Champions.Gragas.Gragas.Initialize();
                        break;
                    case Champion.Graves:
                        Champions.Graves.Graves.Initialize();
                        break;
                    case Champion.Hecarim:
                        Champions.Hecarim.Hecarim.Initialize();
                        break;
                    case Champion.Heimerdinger:
                        Champions.Heimerdinger.Heimerdinger.Initialize();
                        break;
                    case Champion.Illaoi:
                        Champions.Illaoi.Illaoi.Initialize();
                        break;
                    case Champion.Irelia:
                        Champions.Irelia.Irelia.Initialize();
                        break;
                    case Champion.Janna:
                        Champions.Janna.Janna.Initialize();
                        break;
                    case Champion.JarvanIV:
                        Champions.JarvanIV.JarvanIV.Initialize();
                        break;
                    case Champion.Jax:
                        Champions.Jax.Jax.Initialize();
                        break;
                    case Champion.Jayce:
                        Champions.Jayce.Jayce.Initialize();
                        break;
                    case Champion.Jinx:
                        Champions.Jinx.Jinx.Initialize();
                        break;
                    case Champion.Kalista:
                        Champions.Kalista.Kalista.Initialize();
                        break;
                    case Champion.Karma:
                        Champions.Karma.Karma.Initialize();
                        break;
                    case Champion.Karthus:
                        Champions.Karthus.Karthus.Initialize();
                        break;
                    case Champion.Kassadin:
                        Champions.Kassadin.Kassadin.Initialize();
                        break;
                    case Champion.Katarina:
                        Champions.Katarina.Katarina.Initialize();
                        break;
                    case Champion.Kayle:
                        Champions.Kayle.Kayle.Initialize();
                        break;
                    case Champion.Kennen:
                        Champions.Kennen.Kennen.Initialize();
                        break;
                    case Champion.Khazix:
                        Champions.Khazix.Khazix.Initialize();
                        break;
                    case Champion.Kindred:
                        Champions.Kindred.Kindred.Initialize();
                        break;
                    case Champion.KogMaw:
                        Champions.KogMaw.KogMaw.Initialize();
                        break;
                    case Champion.Leblanc:
                        Champions.Leblanc.Leblanc.Initialize();
                        break;
                    case Champion.LeeSin:
                        //Champions.LeeSin.LeeSin.Initialize();
                        break;
                    case Champion.Leona:
                        Champions.Leona.Leona.Initialize();
                        break;
                    case Champion.Lissandra:
                        Champions.Lissandra.Lissandra.Initialize();
                        break;
                    case Champion.Lucian:
                        Champions.Lucian.Lucian.Initialize();
                        break;
                    case Champion.Lulu:
                        Champions.Lulu.Lulu.Initialize();
                        break;
                    case Champion.Lux:
                        Champions.Lux.Lux.Initialize();
                        break;
                    case Champion.Malphite:
                        Champions.Malphite.Malphite.Initialize();
                        break;
                    case Champion.Malzahar:
                        Champions.Malzahar.Malzahar.Initialize();
                        break;
                    case Champion.Maokai:
                        Champions.Maokai.Maokai.Initialize();
                        break;
                    case Champion.MasterYi:
                        Champions.MasterYi.MasterYi.Initialize();
                        break;
                    case Champion.MissFortune:
                        Champions.MissFortune.MissFortune.Initialize();
                        break;
                    case Champion.Mordekaiser:
                        Champions.Mordekaiser.Mordekaiser.Initialize();
                        break;
                    case Champion.Morgana:
                        Champions.Morgana.Morgana.Initialize();
                        break;
                    case Champion.MonkeyKing:
                        Champions.MonkeyKing.MonkeyKing.Initialize();
                        break;
                    case Champion.Nami:
                        Champions.Nami.Nami.Initialize();
                        break;
                    case Champion.Nasus:
                        Champions.Nasus.Nasus.Initialize();
                        break;
                    case Champion.Nautilus:
                        Champions.Nautilus.Nautilus.Initialize();
                        break;
                    case Champion.Nidalee:
                        Champions.Nidalee.Nidalee.Initialize();
                        break;
                    case Champion.Nocturne:
                        Champions.Nocturne.Nocturne.Initialize();
                        break;
                    case Champion.Nunu:
                        Champions.Nunu.Nunu.Initialize();
                        break;
                    case Champion.Olaf:
                        Champions.Olaf.Olaf.Initialize();
                        break;
                    case Champion.Orianna:
                        Champions.Orianna.Orianna.Initialize();
                        break;
                    case Champion.Pantheon:
                        Champions.Pantheon.Pantheon.Initialize();
                        break;
                    case Champion.Poppy:
                        Champions.Poppy.Poppy.Initialize();
                        break;
                    case Champion.Quinn:
                        Champions.Quinn.Quinn.Initialize();
                        break;
                    case Champion.Rammus:
                        Champions.Rammus.Rammus.Initialize();
                        break;
                    case Champion.RekSai:
                        Champions.RekSai.RekSai.Initialize();
                        break;
                    case Champion.Renekton:
                        Champions.Renekton.Renekton.Initialize();
                        break;
                    case Champion.Rengar:
                        //Champions.Rengar.Rengar.Initialize();
                        break;
                    case Champion.Riven:
                        Champions.Riven.Riven.Initialize();
                        break;
                    case Champion.Rumble:
                        Champions.Rumble.Rumble.Initialize();
                        break;
                    case Champion.Ryze:
                        Champions.Ryze.Ryze.Initialize();
                        break;
                    case Champion.Sejuani:
                        Champions.Sejuani.Sejuani.Initialize();
                        break;
                    case Champion.Shaco:
                        Champions.Shaco.Shaco.Initialize();
                        break;
                    case Champion.Shen:
                        Champions.Shen.Shen.Initialize();
                        break;
                    case Champion.Shyvana:
                        Champions.Shyvana.Shyvana.Initialize();
                        break;
                    case Champion.Singed:
                        Champions.Singed.Singed.Initialize();
                        break;
                    case Champion.Sion:
                        Champions.Sion.Sion.Initialize();
                        break;
                    case Champion.Sivir:
                        Champions.Sivir.Sivir.Initialize();
                        break;
                    case Champion.Skarner:
                        Champions.Skarner.Skarner.Initialize();
                        break;
                    case Champion.Sona:
                        Champions.Sona.Sona.Initialize();
                        break;
                    case Champion.Soraka:
                        Champions.Soraka.Soraka.Initialize();
                        break;
                    case Champion.Swain:
                        Champions.Swain.Swain.Initialize();
                        break;
                    case Champion.Syndra:
                        Champions.Syndra.Syndra.Initialize();
                        break;
                    case Champion.TahmKench:
                        Champions.TahmKench.TahmKench.Initialize();
                        break;
                    case Champion.Talon:
                        Champions.Talon.Talon.Initialize();
                        break;
                    case Champion.Taric:
                        Champions.Taric.Taric.Initialize();
                        break;
                    case Champion.Teemo:
                        Champions.Teemo.Teemo.Initialize();
                        break;
                    case Champion.Thresh:
                        Champions.Thresh.Thresh.Initialize();
                        break;
                    case Champion.Tristana:
                        Champions.Tristana.Tristana.Initialize();
                        break;
                    case Champion.Trundle:
                        Champions.Trundle.Trundle.Initialize();
                        break;
                    case Champion.Tryndamere:
                        Champions.Tryndamere.Tryndamere.Initialize();
                        break;
                    case Champion.TwistedFate:
                        //Champions.TwistedFate.TwistedFate.Initialize();
                        break;
                    case Champion.Twitch:
                        Champions.Twitch.Twitch.Initialize();
                        break;
                    case Champion.Udyr:
                        Champions.Udyr.Udyr.Initialize();
                        break;
                    case Champion.Urgot:
                        Champions.Urgot.Urgot.Initialize();
                        break;
                    case Champion.Varus:
                        Champions.Varus.Varus.Initialize();
                        break;
                    case Champion.Vayne:
                        Champions.Vayne.Vayne.Initialize();
                        break;
                    case Champion.Veigar:
                        Champions.Veigar.Veigar.Initialize();
                        break;
                    case Champion.Velkoz:
                        Champions.Velkoz.Velkoz.Initialize();
                        break;
                    case Champion.Vi:
                        Champions.Vi.Vi.Initialize();
                        break;
                    case Champion.Viktor:
                        Champions.Viktor.Viktor.Initialize();
                        break;
                    case Champion.Vladimir:
                        Champions.Vladimir.Vladimir.Initialize();
                        break;
                    case Champion.Volibear:
                        Champions.Volibear.Volibear.Initialize();
                        break;
                    case Champion.Warwick:
                        Champions.Warwick.Warwick.Initialize();
                        break;
                    case Champion.Xerath:
                        Champions.Xerath.Xerath.Initialize();
                        break;
                    case Champion.XinZhao:
                        Champions.XinZhao.XinZhao.Initialize();
                        break;
                    case Champion.Yasuo:
                        //Champions.Yasuo.Yasuo.Initialize();
                        break;
                    case Champion.Yorick:
                        Champions.Yorick.Yorick.Initialize();
                        break;
                    case Champion.Zac:
                        Champions.Zac.Zac.Initialize();
                        break;
                    case Champion.Zed:
                        Champions.Zed.Zed.Initialize();
                        break;
                    case Champion.Ziggs:
                        Champions.Ziggs.Ziggs.Initialize();
                        break;
                    case Champion.Zilean:
                        Champions.Zilean.Zilean.Initialize();
                        break;
                    case Champion.Zyra:
                        Champions.Zyra.Zyra.Initialize();
                        break;
                }

                #endregion Champion`s Swith

                Console.WriteLine(" ");

                #region LoadingChampion
                try
                {
                    Console.WriteLine(Player.Instance.ChampionName + " Loaded Kickass Series");
                }
                catch (Exception exp)
                {
                    Console.Write(exp);
                }
                #endregion LoadingChampion

                Console.WriteLine(" ");

                #region LoadingActivator
                try
                {
                    var anotherActivator = AppDomain.CurrentDomain.GetAssemblies().FirstOrDefault(a => a.FullName.Contains("ctivator"));

                    if (anotherActivator != null)
                    {
                        Console.WriteLine("There is another activator");
                    }
                    else
                    {
                        Activator.Activator.Init();
                        Console.WriteLine("Kickass Activator Loaded");
                    }
                }
                catch (Exception exp)
                {
                    Console.Write(exp);
                }
                #endregion LoadingActivator

                Console.WriteLine(" ");
                /*
                #region LoadingEvade
                try
                {
                    //Evade.Initialize.Init();
                    Console.WriteLine("Evade Loaded");
                }
                catch (Exception exp)
                {
                    Console.Write(exp);
                }
                #endregion LoadingEvade
                
                Console.WriteLine(" ");
                */
                #region LoadingUltilities
                try
                {
                    Ultilities.Initialize.Init();
                    Console.WriteLine("Ultilities Loaded");
                }
                catch (Exception exp)
                {
                    Console.Write(exp);
                }
                #endregion LoadingSkinHack


            }
            catch (Exception exp)
            {
                Console.Write(exp);
            }
        }
    }
}
