using System;
using System.Linq;
using EloBuddy;
using EloBuddy.SDK;
using EloBuddy.SDK.Enumerations;
using KickassSeries.Ultilities;

//using KS = KickassSeries.Champions.Teemo.Config.Modes.KS;
using Misc = KickassSeries.Champions.Teemo.Config.Modes.Misc;

namespace KickassSeries.Champions.Teemo.Modes
{
    public sealed class PermaActive : ModeBase
    {
        public override bool ShouldBeExecuted()
        {
            return true;
        }

        public override void Execute()
        {
            /*
            #region KS
            if (KS.UseQ)
            {
                var targetQ =
                    EntityManager.Heroes.Enemies.Where(
                        t =>
                        t.IsValidTarget() && Q.IsInRange(t) && Player.Instance.GetSpellDamage(t, SpellSlot.Q) >= t.Health)
                        .OrderBy(t => t.Health)
                        .FirstOrDefault();

                if (targetQ != null && Q.IsReady())
                {
                    Q.Cast(targetQ);
                }
            }

            if (!KS.UseR)
            {
                return;
            }

            var rTarget =
                EntityManager.Heroes.Enemies.Where(
                    t =>
                    t.IsValidTarget() && R.IsInRange(t) && Player.Instance.GetSpellDamage(t, SpellSlot.R) >= t.Health)
                    .OrderBy(t => t.Health)
                    .FirstOrDefault();

            if (rTarget == null || !R.IsReady())
            {
                return;
            }
            var pred = R.GetPrediction(rTarget);

            if (pred.HitChance >= HitChance.High)
            {
                R.Cast(pred.CastPosition);
            }
            #endregion KS

            #region AutoShroom
            if (Misc.AutoPanicR)
            {
                Player.IssueOrder(GameObjectOrder.MoveTo, Game.CursorPos);
            }

            if (!R.IsReady() || Misc.AutoPanicR)
            {
                return;
            }

            var rCount = Player.Instance.Spellbook.GetSpell(SpellSlot.R).Ammo;

            switch (Game.MapId)
            {
                case GameMapId.SummonersRift:
                    if (!Teemo.shroomPositions.SummonersRift.Any())
                    {
                        return;
                    }
                    foreach (
                        var place in
                            Teemo.shroomPositions.SummonersRift.Where(
                                pos => pos.Distance(Player.Instance.ServerPosition) <= R.Range && !pos.IsShroomed())
                                .Where(place => Misc.RCharge <= rCount && Environment.TickCount - EventsManager.LastR > 5000))
                    {
                        R.Cast(place);
                    }
                    break;
                case GameMapId.HowlingAbyss:
                    if (!Teemo.shroomPositions.HowlingAbyss.Any())
                    {
                        return;
                    }
                    foreach (
                        var place in
                            Teemo.shroomPositions.HowlingAbyss.Where(
                                pos => pos.Distance(Player.Instance.ServerPosition) <= R.Range && !pos.IsShroomed())
                                .Where(place => Misc.RCharge <= rCount && Environment.TickCount - EventsManager.LastR > 5000))
                    {
                        R.Cast(place);
                    }
                    break;
                case GameMapId.CrystalScar:
                    if (!Teemo.shroomPositions.CrystalScar.Any())
                    {
                        return;
                    }
                    foreach (
                        var place in
                            Teemo.shroomPositions.CrystalScar.Where(
                                pos => pos.Distance(Player.Instance.ServerPosition) <= R.Range && !pos.IsShroomed())
                                .Where(place => Misc.RCharge <= rCount && Environment.TickCount - EventsManager.LastR > 5000))
                    {
                        R.Cast(place);
                    }
                    break;
                case GameMapId.TwistedTreeline:
                    if (!Teemo.shroomPositions.TwistedTreeline.Any())
                    {
                        return;
                    }
                    foreach (
                        var place in
                            Teemo.shroomPositions.TwistedTreeline.Where(
                                pos => pos.Distance(Player.Instance.ServerPosition) <= R.Range && !pos.IsShroomed())
                                .Where(place => Misc.RCharge <= rCount && Environment.TickCount - EventsManager.LastR > 5000))
                    {
                        R.Cast(place);
                    }
                    break;
                default:
                    if (Game.MapId.ToString() == "Unknown")
                    {
                        if (!Teemo.shroomPositions.ButcherBridge.Any())
                        {
                            return;
                        }
                        foreach (
                            var place in
                                Teemo.shroomPositions.ButcherBridge.Where(
                                    pos =>
                                    pos.Distance(Player.Instance.ServerPosition) <= R.Range && pos.IsShroomed())
                                    .Where(place => Misc.RCharge <= rCount && Environment.TickCount - EventsManager.LastR > 5000))
                        {
                            R.Cast(place);
                        }
                    }
                    break;
            }
            #endregion Auto Shroom

            #region AutoQ
            var target = TargetSelector.GetTarget(Q.Range, DamageType.Magical);
            var allMinionsQ =
                EntityManager.MinionsAndMonsters.EnemyMinions.Where(t => Q.IsInRange(t)).OrderBy(t => t.Health);

            if (target == null)
            {
                return;
            }

            if (Q.IsReady() && allMinionsQ.Any())
            {
                foreach (var minion in allMinionsQ.Where(minion => minion.Health < Player.Instance.GetSpellDamage(minion, SpellSlot.Q) && Q.IsInRange(minion)))
                {
                    Q.Cast(minion);
                }
            }
            else if (Q.IsReady() && target.IsValidTarget(Q.Range) && Player.Instance.ManaPercent >= 25)
            {
                Q.Cast(target);
            }
            #endregion AutoQ

            #region AutoW
            if (!W.IsReady())
            {
                return;
            }

            if (W.IsReady())
            {
                W.Cast();
            }
            #endregion AutoW
            */
        }
    }
}
