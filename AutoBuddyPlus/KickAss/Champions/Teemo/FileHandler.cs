using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using EloBuddy;
using EloBuddy.Sandbox;
using SharpDX;

//using Debug = KickassSeries.Champions.Teemo.Config.Modes.Debug;

namespace KickassSeries.Champions.Teemo
{
    internal class FileHandler
    {
        public static List<Vector3> Position = new List<Vector3>();
        private static readonly string ShroomLocation = SandboxConfig.DataDirectory + @"\PandaTeemo\";
        private static readonly string xFile = ShroomLocation + Game.MapId + @"\" + "xFile" + ".txt";
        private static readonly string yFile = ShroomLocation + Game.MapId + @"\" + "yFile" + ".txt";
        private static readonly string zFile = ShroomLocation + Game.MapId + @"\" + "zFile" + ".txt";
        private static string[] xString;
        private static string[] zString;
        private static string[] yString;
        private static int[] xInt;
        private static int[] zInt;
        private static int[] yInt;

        public FileHandler()
        {
            DoChecks();
        }

        private static void DoChecks()
        {
            if (!Directory.Exists(ShroomLocation))
            {
                Directory.CreateDirectory(ShroomLocation);
                Directory.CreateDirectory(ShroomLocation + GameMapId.SummonersRift);
                Directory.CreateDirectory(ShroomLocation + GameMapId.HowlingAbyss);
                Directory.CreateDirectory(ShroomLocation + GameMapId.SummonersRift);
                Directory.CreateDirectory(ShroomLocation + GameMapId.TwistedTreeline);
                CreateFile();
            }
            else if (!File.Exists(xFile) || !File.Exists(zFile) || !File.Exists(yFile))
            {
                CreateFile();
            }
            else if (File.Exists(xFile) && File.Exists(zFile) && File.Exists(yFile))
            {
                ConvertToInt();
            }
        }

        private static void CreateFile()
        {
            if (!File.Exists(xFile))
            {
                File.WriteAllText(xFile, "5020");
            }
            else if (!File.Exists(yFile))
            {
                File.WriteAllText(yFile, "8430");
            }
            else if (!File.Exists(zFile))
            {
                File.WriteAllText(zFile, "2");
            }
        }

        public static void GetShroomLocation()
        {
            for (var i = 0; i < xInt.Count() && i < yInt.Count() && i < zInt.Count(); i++)
            {
                Position.Add(new Vector3(xInt[i], zInt[i], yInt[i]));
                /*if (Debug.DebugPos)
                {
                    Chat.Print(Position[i].ToString());
                }*/
            }
        }

        private static void ConvertToInt()
        {
            xString = new string[File.ReadAllLines(xFile).Count()];
            yString = new string[File.ReadAllLines(yFile).Count()];
            zString = new string[File.ReadAllLines(zFile).Count()];

            xInt = new int[File.ReadAllLines(xFile).Count()];
            yInt = new int[File.ReadAllLines(yFile).Count()];
            zInt = new int[File.ReadAllLines(zFile).Count()];

            xString = File.ReadAllLines(xFile);
            yString = File.ReadAllLines(yFile);
            zString = File.ReadAllLines(zFile);

            for (var i = 0; i < xString.Count(); i++)
            {
                xInt[i] = Convert.ToInt32(xString[i]);
            }

            for (var i = 0; i < xString.Count(); i++)
            {
                zInt[i] = Convert.ToInt32(zString[i]);
            }

            for (var i = 0; i < xString.Count(); i++)
            {
                yInt[i] = Convert.ToInt32(yString[i]);
            }

            GetShroomLocation();
            /*
            if (Debug.DebugPos)
            {
                Chat.Print("Sucessfully Initialized FileHandler");
            }
            */
        }
    }
}