using EloBuddy;
using EloBuddy.SDK;

namespace KickassSeries.Champions.Evelynn
{
    public static class SpellDamage
    {
        public static float GetTotalDamage(AIHeroClient target)
        {
            // Auto attack
            var damage = Player.Instance.GetAutoAttackDamage(target);

            // Q
            if (SpellManager.Q.IsReady())
            {
                damage += SpellManager.Q.GetRealDamage(target);
            }

            // W
            if (SpellManager.W.IsReady())
            {
                damage += SpellManager.W.GetRealDamage(target);
            }

            // E
            if (SpellManager.E.IsReady())
            {
                damage += SpellManager.E.GetRealDamage(target);
            }

            // R
            if (SpellManager.R.IsReady())
            {
                damage += SpellManager.R.GetRealDamage(target);
            }

            return damage;
        }

        public static float GetRealDamage(this Spell.SpellBase spell, Obj_AI_Base target)
        {
            return spell.Slot.GetRealDamage(target);
        }

        public static float GetRealDamage(this SpellSlot slot, Obj_AI_Base target)
        {
            // Helpers
            var spellLevel = Player.Instance.Spellbook.GetSpell(slot).Level;
            const DamageType damageType = DamageType.Magical;
            float damage = 0;

            // Validate spell level
            if (spellLevel == 0)
            {
                return 0;
            }
            spellLevel--;

            switch (slot)
            {
                case SpellSlot.Q:

                    damage = new float[] {40, 50, 60, 70, 80}[spellLevel] +
                             new[] {0.35f, 0.40f, 0.45f, 0.50f, 0.55f}[spellLevel]*Player.Instance.FlatMagicDamageMod +
                             new[] {0.5f, 0.55f, 0.60f, 0.65f, 0.70f}[spellLevel]*Player.Instance.FlatPhysicalDamageMod;
                    break;

                case SpellSlot.W:

                    damage = new float[] { 0, 0, 0, 0, 0 }[spellLevel] + 0.0f * Player.Instance.FlatMagicDamageMod;
                    break;

                case SpellSlot.E:

                    damage = new float[] {35, 55, 75, 95, 115}[spellLevel] + 0.5f*Player.Instance.FlatMagicDamageMod +
                             0.5f*Player.Instance.FlatPhysicalDamageMod;
                    break;

                case SpellSlot.R:

                    damage = (new [] { 0.15f, 0.20f, 0.25f }[spellLevel] + 0.01f/100 * Player.Instance.FlatMagicDamageMod) * target.Health;
                    break;
            }

            if (damage <= 0)
            {
                return 0;
            }

            return Player.Instance.CalculateDamageOnUnit(target, damageType, damage) - 10;
        }
    }
}