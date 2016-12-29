using System.Collections.Generic;
using System.Linq;
using EloBuddy;
using EloBuddy.SDK;
using EloBuddy.SDK.Enumerations;

namespace ARAMDetFull.Champions
{
    class Amumu : Champion
    {
        public Amumu()
        {
            ARAMSimulator.champBuild = new Build
            {
                coreItems = new List<ConditionalItem>
                {
                    /*Abyssal Scepter
Ionian Boots of Lucidity
Liandry's Torment
Righteous Glory
Sunfire Cape
Thornmail
Negatron Cloak,Amplifying Tome*/
                    new ConditionalItem(ItemId.Abyssal_Scepter),
                    new ConditionalItem(ItemId.Ionian_Boots_of_Lucidity),
                    new ConditionalItem(ItemId.Liandrys_Torment),
                    new ConditionalItem(ItemId.Righteous_Glory),
                    new ConditionalItem(ItemId.Sunfire_Cape),
                    new ConditionalItem(ItemId.Thornmail),
                },
                startingItems = new List<ItemId>
                {
                    ItemId.Negatron_Cloak,ItemId.Amplifying_Tome
                }
            };
        }

        public override void useQ(Obj_AI_Base target)
        {
            if (safeGap(target))
            {
                CastQ(target);
            }
        }

        public override void useW(Obj_AI_Base target)
        {
            if (!W.IsReady())
                return;
        }

        public override void useE(Obj_AI_Base target)
        {
            if (E.CanCast(target) )
            {
                CastE(target);
            }
        }

        public override void useR(Obj_AI_Base target)
        {
            if (R.CanCast(target))
            {
                AutoUlt();
            }
        }

        public override void setUpSpells()
        {
            Q = new Spell.Skillshot(SpellSlot.Q, 1100, SkillShotType.Linear, 250, 2000, 50);
            W = new Spell.Active(SpellSlot.W, 300);
            E = new Spell.Active(SpellSlot.E, 350);
            R = new Spell.Skillshot(SpellSlot.R, 550, SkillShotType.Circular);
        }

        public override void useSpells()
        {
            var tar = ARAMTargetSelector.getBestTarget(E.Range);
            if (tar != null) useQ(tar);
            if (tar != null) useW(tar);
            if (tar != null) useE(tar);
            if (tar != null) useR(tar);
        }

        void AutoUlt()
        {
            var comboR = 2;

            if (comboR > 0 && R.IsReady())
            {
                int enemiesHit = 0;
                int killableHits = 0;

                foreach (AIHeroClient enemy in ObjectManager.Get<AIHeroClient>().Where(he => he.IsEnemy && he.IsValidTarget(R.Range)))
                {
                    var prediction = (R as Spell.Skillshot).GetPrediction(enemy);

                    if (prediction != null && prediction.UnitPosition.Distance(ObjectManager.Player.ServerPosition) <= R.Range)
                    {
                        enemiesHit++;

                        if (ObjectManager.Player.GetSpellDamage(enemy, SpellSlot.W) >= enemy.Health)
                            killableHits++;
                    }
                }

                if (enemiesHit >= comboR || (killableHits >= 1 && ObjectManager.Player.Health / ObjectManager.Player.MaxHealth <= 0.1))
                    CastR();
            }
        }

        void CastE(Obj_AI_Base target)
        {
            if (!E.IsReady() || target == null || !target.IsValidTarget())
                return;
            if (player.Distance(target) <= E.Range)
                E.Cast(target);
        }


        void CastQ(Obj_AI_Base target, HitChance hitChance = HitChance.High)
        {
            if (!Q.IsReady())
                return;
            if (target == null || !target.IsValidTarget())
                return;

            var prediction = (Q as Spell.Skillshot).GetPrediction(target);
            if (prediction.HitChance != hitChance) return;
            Q.Cast(target);
        }

        void CastR()
        {
            if (!R.IsReady())
                return;
            R.Cast();
        }
    }
}
