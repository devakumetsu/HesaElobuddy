using System;
using System.Collections.Generic;
using System.Linq;
using EloBuddy;
using EloBuddy.SDK.Enumerations;
using EloBuddy.SDK.Events;
using SharpDX;
using SharpDX.Direct3D9;

using Color = System.Drawing.Color;
using Line = EloBuddy.SDK.Rendering.Line;
using Settings = KickassSeries.Ultilities.Config.Types.RecallTracker;

namespace KickassSeries.Ultilities.Trackers
{
    internal class RecallTracker
    {
        public static List<Recall> Recalls = new List<Recall>();

        private static Font _Font;
        private static Font _FontNumber;

        public static void Initialize()
        {
            _Font = new Font(
                Drawing.Direct3DDevice,
                new FontDescription
                {
                    FaceName = "Tahoma",
                    Height = 18,
                    OutputPrecision = FontPrecision.Default,
                    Quality = FontQuality.ClearType,
                    Weight = FontWeight.Bold

                });

            _FontNumber = new Font(
                Drawing.Direct3DDevice,
                new FontDescription
                {
                    FaceName = "Tahoma",
                    Height = 17,
                    OutputPrecision = FontPrecision.Default,
                    Quality = FontQuality.ClearType,
                    Weight = FontWeight.Bold

                });
        }

        public static void OnTeleport(AIHeroClient sender , Teleport.TeleportEventArgs args)
        {
            if (Settings.TurnOff) return;

            if (args.Type != TeleportType.Recall || sender == null) return;

            if (!Settings.RecallAllies && sender.IsAlly)return;
            if (!Settings.RecallEnemies && sender.IsEnemy) return;


            switch (args.Status)
            {
                case TeleportStatus.Abort:
                    foreach (var source in Recalls.Where(a => a.Unit == sender))
                    {
                        source.Abort();
                    }
                    break;
                case TeleportStatus.Start:
                    var recall = Recalls.FirstOrDefault(a => a.Unit == sender);
                    if (recall != null)
                    {
                        Recalls.Remove(recall);
                    }
                    Recalls.Add(new Recall((AIHeroClient) sender, Environment.TickCount,
                        Environment.TickCount + args.Duration, args.Duration));
                    break;
            }
        }

        public static void DrawOnEnd(EventArgs args)
        {
            if(Settings.TurnOff) return;

            var x = (int)(Drawing.Width * 0.8475 - Settings.XPos);
            var y = (int)(Drawing.Height * 0.1 + Settings.YPos);

            var bonus = 0;
            foreach (var recall in Recalls.ToList())
            {
                Line.DrawLine(Color.GhostWhite, 18, new Vector2(x + 20 , y + bonus + 33), new Vector2(x + 250, y + bonus + 33));

                Line.DrawLine(recall.IsAborted ? Color.DarkRed : BarColor(recall.PercentComplete()), 18,
                    new Vector2(x + 20, y + bonus + 33),
                    new Vector2(x + 20 + 230*(recall.PercentComplete()/100), y + bonus + 33));

                //Line.DrawLine(Color.Red, 18, new Vector2(x + 180, y + bonus + 33), new Vector2(x + 182, y + bonus + 33));

                _Font.DrawText(null, recall.Unit.ChampionName, x + 25, y + bonus + 23, SharpDX.Color.Black);
                _FontNumber.DrawText(null, recall.PercentComplete() + "%", x + 203, y + bonus + 24, SharpDX.Color.Black);


                if (recall.ExpireTime < Environment.TickCount && Recalls.Contains(recall))
                {
                    Recalls.Remove(recall);
                }
                bonus += 35;
            }
        }

        private static Color BarColor(float percent)
        {
            if (percent > 80)
            {
                return Color.LimeGreen;
            }
            if (percent > 60)
            {
                return Color.YellowGreen;
            }

            if (percent > 40)
            {
                return Color.Orange;
            }
            if (percent > 20)
            {
                return Color.DarkOrange;
            }
            if (percent > 1)
            {
                return Color.OrangeRed;
            }
            return Color.White;
        }

        public class Recall
        {
            public Recall(AIHeroClient unit, int recallStart, int recallEnd, int duration)
            {
                Unit = unit;
                RecallStart = recallStart;
                Duration = duration;
                RecallEnd = recallEnd;
                ExpireTime = RecallEnd + 2000;
            }

            public int RecallEnd;
            public int Duration;
            public int RecallStart;
            public int ExpireTime;
            public int CancelT;

            public AIHeroClient Unit;

            public bool IsAborted;

            public void Abort()
            {
                CancelT = Environment.TickCount;
                ExpireTime = Environment.TickCount + 2000;
                IsAborted = true;
            }

            private float Elapsed
            {
                get { return (CancelT > 0 ? CancelT : Environment.TickCount) - RecallStart; }
            }

            public float PercentComplete()
            {
                return (float) Math.Round(Elapsed/Duration*100) > 100 ? 100 : (float) Math.Round(Elapsed/Duration*100);
            }
        }
    }
}
