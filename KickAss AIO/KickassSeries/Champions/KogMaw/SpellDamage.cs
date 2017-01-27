using EloBuddy;
using EloBuddy.SDK;

namespace KickassSeries.Champions.KogMaw
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

                    damage = new float[] {80, 130, 180, 230, 280}[spellLevel] + 0.5f*Player.Instance.FlatMagicDamageMod;
                    break;

                case SpellSlot.W:

                    //TODO NO IDEA IF THIS MATH WILL WORK TEST LATER
                    damage = 0.02f*target.MaxHealth + 0.075f*Player.Instance.FlatMagicDamageMod%100 + Player.Instance.GetAutoAttackDamage(target);

                    break;

                case SpellSlot.E:

                    damage = new float[] {60, 110, 160, 210, 260}[spellLevel] + 0.7f*Player.Instance.FlatMagicDamageMod;
                    break;

                case SpellSlot.R:

                    //TODO TEST DAMAGE

                    if (target.HealthPercent <= 50)
                    {
                        damage = new float[] {70, 110, 150}[spellLevel] + 0.25f*Player.Instance.FlatMagicDamageMod +
                                 0.65f*Player.Instance.FlatPhysicalDamageMod * 3f;
                    }
                    else if (target.HealthPercent <= 25)
                    {
                        damage = new float[] { 70, 110, 150 }[spellLevel] + 0.25f * Player.Instance.FlatMagicDamageMod +
                                 0.65f * Player.Instance.FlatPhysicalDamageMod * 2f;
                    }
                    else
                    {
                        damage = new float[] { 70, 110, 150 }[spellLevel] + 0.25f * Player.Instance.FlatMagicDamageMod +
                                 0.65f * Player.Instance.FlatPhysicalDamageMod;
                    }
                    
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