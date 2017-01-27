using System.Linq;
using EloBuddy;
using EloBuddy.SDK;
using EloBuddy.SDK.Menu.Values;

using Settings = KickassSeries.Champions.Jinx.Config.Modes.LaneClear;

namespace KickassSeries.Champions.Jinx.Modes
{
    public sealed class LaneClear : ModeBase
    {
        public override bool ShouldBeExecuted()
        {
            return Orbwalker.ActiveModesFlags.HasFlag(Orbwalker.ActiveModes.LaneClear);
        }

        public override void Execute()
        {/*
            if (Settings.UseQ && Q.IsReady())
            {
                var minion = Orbwalker.LasthittableMinions.Where(t => t.IsValidTarget() && t.Distance(Player.Instance) <= Essentials.FishBonesRange());

                if (Essentials.FishBones())
                {
                    if (!minion.Any())
                    {
                        Q.Cast();
                        return;
                    }
                }

                if (!Essentials.FishBones() && Player.Instance.ManaPercent >= Settings.ManaLane)
                {
                    var m = Orbwalker.LaneclearMinion;

                    if (m == null)
                    {
                        return;
                    }

                    var minionsAoe =
                        EntityManager.MinionsAndMonsters.EnemyMinions.Count(
                            t =>
                                t.IsValidTarget() && t.Distance(m) <= 100 &&
                                t.Health <= (Player.Instance.GetAutoAttackDamage(m) * 1.1f));

                    if (m.Distance(Player.Instance) <= Essentials.FishBonesRange() && m.IsValidTarget() &&
                        minionsAoe >= Settings.MinMinionLane)
                    {
                        Q.Cast();
                        Orbwalker.ForcedTarget = m;
                    }
                    else if (m.Distance(Player.Instance) >= Player.Instance.GetAutoAttackRange() &&
                             Orbwalker.LasthittableMinions.Contains(m) && Settings.UseQOutOfRange)
                    {
                        Q.Cast();
                        Orbwalker.ForcedTarget = m;
                    }
                }
            }*/
        }
    }
}
