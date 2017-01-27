using EloBuddy;
using EloBuddy.SDK;

namespace KickassSeries.Champions.Vayne
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

                    damage = Player.Instance.GetAutoAttackDamage(target) + new float[] { 30, 35, 40, 45, 50 }[spellLevel] * Player.Instance.FlatPhysicalDamageMod;
                    break;

                case SpellSlot.E:

                    damage = new float[] { 45 ,80 ,115, 150, 185 }[spellLevel] + 0.5f * Player.Instance.FlatPhysicalDamageMod;
                    break;

                case SpellSlot.R:

                    damage = new float[] { 30, 50, 70 }[spellLevel] * 4f;
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