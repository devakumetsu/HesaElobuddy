using EloBuddy;
using EloBuddy.SDK;

namespace KickassSeries.Champions.Xerath
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

                    damage = new float[] { 80, 120, 160, 200, 240 }[spellLevel] + 0.75f * Player.Instance.TotalMagicalDamage;
                    break;

                case SpellSlot.W:

                    damage = new float[] { 60, 90, 120, 150, 180 }[spellLevel] + 0.6f * Player.Instance.TotalMagicalDamage;
                    break;

                case SpellSlot.E:

                    damage = new float[] { 80, 110, 140, 170, 200 }[spellLevel] + 0.45f * Player.Instance.TotalMagicalDamage;
                    break;

                case SpellSlot.R:

                    damage = new float[] { 190, 245, 300 }[spellLevel] + 0.43f * Player.Instance.TotalMagicalDamage * 2.8f;
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