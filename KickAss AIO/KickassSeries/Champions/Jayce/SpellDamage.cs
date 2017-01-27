using EloBuddy;
using EloBuddy.SDK;

namespace KickassSeries.Champions.Jayce
{
    public static class SpellDamage
    {
        public static float GetTotalDamage(AIHeroClient target)
        {
            // Auto attack
            var damage = Player.Instance.GetAutoAttackDamage(target);
            //Hammer
            if (Player.Instance.Spellbook.GetSpell(SpellSlot.R).ToggleState == 1)
            {
                // Qh
                if (SpellManager.Qh.IsReady())
                {
                    damage += SpellManager.Qh.GetRealDamage(target);
                }

                // Wh
                if (SpellManager.Wh.IsReady())
                {
                    damage += SpellManager.Wh.GetRealDamage(target);
                }

                // Eh
                if (SpellManager.Eh.IsReady())
                {
                    damage += SpellManager.Eh.GetRealDamage(target);
                }
            }
            //Gun
            else
            {
                // Qg
                if (SpellManager.Qg.IsReady())
                {
                    damage += SpellManager.Qg.GetRealDamage(target);
                }

                // Wg
                if (SpellManager.Wg.IsReady())
                {
                    damage += SpellManager.Wg.GetRealDamage(target);
                }

                // Eg
                if (SpellManager.Eg.IsReady())
                {
                    damage += SpellManager.Eh.GetRealDamage(target);
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

            //Hammer
            if (Player.Instance.Spellbook.GetSpell(SpellSlot.R).ToggleState == 1)
            {
                switch (slot)
                {
                    case SpellSlot.Q:

                        damage = new float[] { 30, 70, 110, 150, 190, 230 }[spellLevel] + 1f * Player.Instance.FlatPhysicalDamageMod;
                        break;

                    case SpellSlot.W:

                        damage = new float[] { 25, 40, 55, 70, 85, 100 }[spellLevel] + 0.25f * Player.Instance.FlatMagicDamageMod;
                        break;

                    case SpellSlot.E:

                        damage = new [] { 0.08f, 0.104f, 0.128f, 0.152f, 0.176f,0.2f }[spellLevel] * target.MaxHealth + 1f * Player.Instance.FlatPhysicalDamageMod;
                        break;
                }
            }
            //Gun
            else
            {
                switch (slot)
                {
                    case SpellSlot.Q:

                        damage = new float[] { 70, 120, 170, 220, 270, 320 }[spellLevel] + 1.2f * Player.Instance.FlatPhysicalDamageMod;
                        break;

                    case SpellSlot.W:

                        damage = new [] { 0.70f, 0.78f, 0.86f, 0.94f, 1.02f, 1.10f }[spellLevel] * Player.Instance.GetAutoAttackDamage(target);
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