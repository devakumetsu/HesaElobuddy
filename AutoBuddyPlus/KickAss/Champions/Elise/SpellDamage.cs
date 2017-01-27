using EloBuddy;
using EloBuddy.SDK;

namespace KickassSeries.Champions.Elise
{
    public static class SpellDamage
    {
        public static float GetTotalDamage(AIHeroClient target)
        {
            // Auto attack
            var damage = Player.Instance.GetAutoAttackDamage(target);

            //Human
            if (Player.Instance.Spellbook.GetSpell(SpellSlot.R).ToggleState == 1)
            {
                // Q
                if (SpellManager.Q1.IsReady())
                {
                    damage += SpellManager.Q1.GetRealDamage(target);
                }

                // W
                if (SpellManager.W1.IsReady())
                {
                    damage += SpellManager.W1.GetRealDamage(target);
                }

                // E
                if (SpellManager.E1.IsReady())
                {
                    damage += SpellManager.E1.GetRealDamage(target);
                }
            }
            if (Player.Instance.Spellbook.GetSpell(SpellSlot.R).ToggleState == 2)
            {
                // Q
                if (SpellManager.Q2.IsReady())
                {
                    damage += SpellManager.Q2.GetRealDamage(target);
                }

                // W
                if (SpellManager.W1.IsReady())
                {
                    damage += SpellManager.W2.GetRealDamage(target);
                }

                // E
                if (SpellManager.E1.IsReady())
                {
                    damage += SpellManager.E2.GetRealDamage(target);
                }
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
            if (Player.Instance.Spellbook.GetSpell(SpellSlot.R).ToggleState == 1)
            {
                switch (slot)
                {
                    case SpellSlot.Q:

                        damage = new float[] {40, 75, 110, 145, 180}[spellLevel] + 0.04f + 0.03f /100 * Player.Instance.FlatMagicDamageMod* target.Health;
                        break;

                    case SpellSlot.W:

                        damage = new float[] {75, 125, 175, 225, 275}[spellLevel] + 0.8f*Player.Instance.FlatMagicDamageMod;
                        break;

                    case SpellSlot.E:

                        damage = new float[] {0, 0, 0, 0, 0}[spellLevel] + 0.0f*Player.Instance.FlatMagicDamageMod;
                        break;

                    case SpellSlot.R:

                        damage = new float[] {0, 0, 0}[spellLevel] + 0.0f*Player.Instance.FlatMagicDamageMod;
                        break;
                }
            }

            if (Player.Instance.Spellbook.GetSpell(SpellSlot.R).ToggleState == 2)
            {
                switch (slot)
                {
                    case SpellSlot.Q:

                        damage = new float[] { 60, 100, 140, 180, 220 }[spellLevel] + 0.08f + 0.03f / 100 * Player.Instance.FlatMagicDamageMod * target.Health;
                        break;

                    case SpellSlot.W:

                        damage = new float[] { 0, 0, 0, 0, 0 }[spellLevel] + 0.0f * Player.Instance.FlatMagicDamageMod;
                        break;

                    case SpellSlot.E:

                        damage = new float[] { 0, 0, 0, 0, 0 }[spellLevel] + 0.0f * Player.Instance.FlatMagicDamageMod;
                        break;

                    case SpellSlot.R:

                        damage = new float[] { 0, 0, 0 }[spellLevel] + 0.0f * Player.Instance.FlatMagicDamageMod;
                        break;
                }
            }

            if (damage <= 0)
            {
                return 0;
            }

            return Player.Instance.CalculateDamageOnUnit(target, damageType, damage) - 10;
        }
    }
}