using System;
using System.Collections.Generic;
using System.Linq;
using EloBuddy;
using EloBuddy.SDK;
using EloBuddy.SDK.Enumerations;
namespace ARAMDetFull.Champions
{
    class Fiora : Champion
    {
        public Fiora()
        {
            Orbwalker.OnPostAttack += Orbwalker_OnPostAttack;

            ARAMSimulator.champBuild = new Build
            {
                coreItems = new List<ConditionalItem>
                {
                    new ConditionalItem(ItemId.Ravenous_Hydra_Melee_Only),
                    new ConditionalItem(ItemId.The_Black_Cleaver),
                    new ConditionalItem(ItemId.Mercurys_Treads, ItemId.Ninja_Tabi, ItemCondition.ENEMY_AP),
                    new ConditionalItem(ItemId.The_Bloodthirster),
                    new ConditionalItem(ItemId.Guardian_Angel),
                    new ConditionalItem((ItemId)3812),
                },
                startingItems = new List<ItemId>
                {
                    ItemId.Pickaxe,ItemId.Boots_of_Speed
                }
            };

        }

        private void Orbwalker_OnPostAttack(AttackableUnit target, EventArgs args)
        {
            if (!target.IsMe || !(target is AIHeroClient))
                return;
            W.Cast();
        }

        public override void useQ(Obj_AI_Base target)
        {
            if (Q.CanCast(target))
            {
                Q.Cast();
            }
        }

        public override void useW(Obj_AI_Base target)
        {

        }

        public override void useE(Obj_AI_Base target)
        {
            if (E.CanCast(target) && (Q.IsReady() || R.IsReady()))
            {
                if ((MapControl.balanceAroundPoint(player.Position.To2D(), 700) >= -1 || (MapControl.fightIsOn() != null && MapControl.fightIsOn().NetworkId == target.NetworkId)))

                    E.Cast(target.ServerPosition);
            }
        }

        public override void useR(Obj_AI_Base target)
        {
            if (R.CanCast(target) && !Q.IsKillable(target))
            {
                CastR(target);
            }
        }

        public override void setUpSpells()
        {
            //Initialize our Spells
            Q = new Spell.Skillshot(SpellSlot.Q, 425, SkillShotType.Linear, 250, 500, 0);
            W = new Spell.Skillshot(SpellSlot.W, 750, SkillShotType.Linear, 500, 3200, 70);
            E = new Spell.Active(SpellSlot.E, 200);
            E.CastDelay = 0;

            R = new Spell.Targeted(SpellSlot.R, 500);
            R.CastDelay = (int).066f;
        }

        public override void useSpells()
        {
            var tar = ARAMTargetSelector.getBestTarget(E.Range);
            if (tar != null) useQ(tar);
            if (tar != null) useE(tar);
            if (tar != null) useR(tar);
        }
        
        private void CastR(Obj_AI_Base target)
        {
            if (!R.IsReady())
                return;

            foreach (var hero in ObjectManager.Get<AIHeroClient>().Where(hero => hero.IsValidTarget(R.Range)))
            {
                if (player.GetSpellDamage(target, SpellSlot.R) - 50 > hero.Health)
                {
                    R.Cast(target);
                }

                else if (player.GetSpellDamage(target, SpellSlot.R) - 50 < hero.Health)
                {
                    foreach (var buff in hero.Buffs.Where(buff => buff.Name == "dariushemo"))
                    {
                        if (player.GetSpellDamage(target, SpellSlot.R, (DamageLibrary.SpellStages) 1) * (1 + buff.Count / 5) - 50 > target.Health)
                        {
                            R.CastOnUnit(target);
                        }
                    }
                }
            }
        }
    }
}