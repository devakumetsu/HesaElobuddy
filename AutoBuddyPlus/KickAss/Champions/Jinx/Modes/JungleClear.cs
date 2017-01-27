using System.Linq;
using EloBuddy;
using EloBuddy.SDK;

using Settings = KickassSeries.Champions.Jinx.Config.Modes.LaneClear;
using Misc = KickassSeries.Champions.Jinx.Config.Modes.Misc;

namespace KickassSeries.Champions.Jinx.Modes
{
    public sealed class JungleClear : ModeBase
    {
        public override bool ShouldBeExecuted()
        {
            return Orbwalker.ActiveModesFlags.HasFlag(Orbwalker.ActiveModes.JungleClear);
        }

        public override void Execute()
        {/*
            if (Settings.UseQ && Player.Instance.ManaPercent >= Settings.ManaQ && Q.IsReady())
            {
                if (Essentials.FishBones())
                {
                    var mobs = EntityManager.MinionsAndMonsters.GetJungleMonsters(
                        Player.Instance.ServerPosition,
                        Essentials.FishBonesRange());

                    foreach (
                        var mob in
                            mobs.Where(mob => mob != null && Player.Instance.Distance(mob) <= Essentials.MinigunRange))
                    {
                        Q.Cast();
                        Orbwalker.ForcedTarget = mob;
                    }
                }
                else if (!Essentials.FishBones())
                {
                    var mobs = EntityManager.MinionsAndMonsters.GetJungleMonsters(
                        Player.Instance.ServerPosition,
                        Essentials.FishBonesRange());

                    foreach (
                        var mob in
                            mobs.Where(mob => mob != null)
                                .Where(
                                    mob =>
                                        !Player.Instance.IsInAutoAttackRange(mob) &&
                                        Player.Instance.Distance(mob) <= Essentials.FishBonesRange()))
                    {
                        Q.Cast();
                        Orbwalker.ForcedTarget = mob;
                    }
                }
            }

            if (Settings.UseW && Player.Instance.ManaPercent >= Settings.ManaW && W.IsReady())
            {
                var target =
                    EntityManager.MinionsAndMonsters.GetJungleMonsters(Player.Instance.ServerPosition, W.Range)
                        .OrderByDescending(t => t.Health)
                        .FirstOrDefault();
                if (target != null)
                {
                    var wPrediction = W.GetPrediction(target);

                    if (!(wPrediction.HitChancePercent >= Settings.WPredPercentage))
                    {
                        return;
                    }
                    if (Misc.WAARange && Player.Instance.IsInAutoAttackRange(target))
                    {
                        W.Cast(wPrediction.CastPosition);
                    }
                    else if (!Misc.WAARange)
                    {
                        W.Cast(wPrediction.CastPosition);
                    }
                }
            }*/
        }
    }
}
