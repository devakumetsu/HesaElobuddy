using System.Linq;
using EloBuddy;
using EloBuddy.SDK;
using EloBuddy.SDK.Enumerations;
using EloBuddy.SDK.Menu.Values;
using KickassSeries.Ultilities;
using Settings = KickassSeries.Champions.Teemo.Config.Modes.Combo;
using Misc = KickassSeries.Champions.Teemo.Config.Modes.Misc;

namespace KickassSeries.Champions.Teemo.Modes
{
    public sealed class Combo : ModeBase
    {
        public override bool ShouldBeExecuted()
        {
            return Orbwalker.ActiveModesFlags.HasFlag(Orbwalker.ActiveModes.Combo);
        }

        public override void Execute()
        {
            var target = TargetSelector.GetTarget(Q.Range, DamageType.Magical);
            if (target == null || target.IsZombie) return;
            /*
            if (Settings.PrevenUnstealth && Player.Instance.HasBuff("CamouflageStealth"))
            {
                return;
            }

            var enemies = EntityManager.Heroes.Enemies.FirstOrDefault(t => t.IsValidTarget() && Player.Instance.IsInAutoAttackRange(t));
            var rtarget = TargetSelector.GetTarget(R.Range, DamageType.Magical);
            var rCount = Player.Instance.Spellbook.GetSpell(SpellSlot.R).Ammo;
            /*
            if (W.IsReady() && Settings.UseW && !Settings.OnlyWInRange)
            {
                W.Cast();
            }
            /*
            if (Settings.UseW && Settings.OnlyWInRange)
            {
                if (W.IsReady() && enemies != null)
                {
                    W.Cast();
                }
            }
            
            if (rtarget == null)
            {
                return;
            }
            var predictionR = R.GetPrediction(rtarget);
            /*
            if (!R.IsReady() || !Settings.UseR || !R.IsInRange(rtarget) || Settings.RCharges > rCount || !rtarget.IsValidTarget()
                || predictionR.CastPosition.IsShroomed())
            {
                return;
            }
            
            if (predictionR.HitChance >= HitChance.High)
            {
                R.Cast(predictionR.CastPosition);
            }
            */
        }
    }
}
