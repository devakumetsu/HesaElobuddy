using EloBuddy;
using EloBuddy.SDK;

using Settings = KickassSeries.Champions.Katarina.Config.Modes.Combo;

namespace KickassSeries.Champions.Katarina.Modes
{
    public sealed class PermaActive : ModeBase
    {
        public override bool ShouldBeExecuted()
        {
            return true;
        }

        public static bool _ulting;

        private static float dmgQstack(Obj_AI_Base target)
        {
            var damage = new float[] { 15, 30, 45, 60, 75 }[Player.Instance.Spellbook.GetSpell(SpellSlot.Q).Level] +
                              0.15f * Player.Instance.FlatMagicDamageMod;
            return Player.Instance.CalculateDamageOnUnit(target, DamageType.Magical, damage) - 10;
        }

        private static void CheckUlt()
        {
            if (!Player.Instance.IsRecalling())
            {
                _ulting = Player.Instance.Spellbook.IsChanneling;
            }

            if (_ulting)
            {

                Orbwalker.DisableAttacking = true;
                Orbwalker.DisableMovement = true;
            }
            else
            {
                Orbwalker.DisableAttacking = false;
                Orbwalker.DisableMovement = false;
            }
        }

        public override void Execute()
        {
            CheckUlt();
            var target = TargetSelector.GetTarget(SpellManager.Q.Range, DamageType.Magical);
            if (target == null) return;

            var qDamage = SpellDamage.GetRealDamage(SpellSlot.Q, target);
            var wDamage = SpellDamage.GetRealDamage(SpellSlot.W, target);
            var eDamage = SpellDamage.GetRealDamage(SpellSlot.E, target);

            //Simple KS
            if (SpellManager.Q.IsReady() && target.IsValidTarget(SpellManager.Q.Range) && target.Health <= qDamage &&
                Settings.UseQ)
            {
                SpellManager.Q.Cast(target);
            }

            if (SpellManager.E.IsReady() && target.IsValidTarget(SpellManager.E.Range) && target.Health <= eDamage &&
                Settings.UseE)
            {
                SpellManager.E.Cast(target);
            }

            if (SpellManager.W.IsReady() && target.IsValidTarget(SpellManager.W.Range) && target.Health <= wDamage &&
                Settings.UseW)
            {
                SpellManager.W.Cast();
            }

            //Smart KS

            //Q+W+Qmark
            if (SpellManager.Q.IsReady() && SpellManager.W.IsReady() && target.IsValidTarget(SpellManager.W.Range) &&
                target.Health <= (qDamage + wDamage + dmgQstack(target)) &&
                Settings.UseQ && Settings.UseW)
            {
                SpellManager.Q.Cast(target);
            }

            if (SpellManager.W.IsReady() && target.IsValidTarget(SpellManager.W.Range) &&
                target.HasBuff("KatarinaQMark") &&
                target.Health <= (wDamage + dmgQstack(target)) && Settings.UseW)
            {
                SpellManager.W.Cast();
            }
            //Q+W+Qmark

            //E+W
            if (SpellManager.E.IsReady() && SpellManager.W.IsReady() && target.IsValidTarget(SpellManager.E.Range) &&
                target.Health <= (eDamage + wDamage) &&
                target.CountEnemiesInRange(SpellManager.R.Range) <= 2 && Settings.UseE && Settings.UseW)
            {
                SpellManager.E.Cast(target);
            }
            //E+W

            //E+W+Q
            if (SpellManager.E.IsReady() && SpellManager.W.IsReady() && SpellManager.Q.IsReady() &&
                target.IsValidTarget(SpellManager.E.Range) &&
                target.Health <=
                (eDamage + wDamage + qDamage) &&
                target.CountEnemiesInRange(SpellManager.R.Range) <= 2 && Settings.UseQ && Settings.UseW && Settings.UseE)
            {
                SpellManager.E.Cast(target);
                SpellManager.W.Cast(target);
                SpellManager.Q.Cast(target);
            }
            //E+W+Q
        }
    }
}
