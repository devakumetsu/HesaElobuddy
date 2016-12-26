using EloBuddy;
using EloBuddy.SDK;
using EloBuddy.SDK.Enumerations;
using System.Collections.Generic;
using System.Linq;

namespace ARAMDetFull.Champions
{
    class Darius : Champion
    {

        public Darius()
        {
            //DeathWalker.AfterAttack += ExecuteAfterAttack;
            Orbwalker.OnPostAttack += Orbwalker_OnPostAttack;
            ARAMSimulator.champBuild = new Build
            {
                coreItems = new List<ConditionalItem>
                {
                    new ConditionalItem(ItemId.The_Black_Cleaver),
                    new ConditionalItem(ItemId.Mercurys_Treads),
                    new ConditionalItem(ItemId.Ravenous_Hydra_Melee_Only),
                    new ConditionalItem(ItemId.Spirit_Visage,ItemId.Randuins_Omen,ItemCondition.ENEMY_AP),
                    new ConditionalItem((ItemId)3742),
                    new ConditionalItem(ItemId.Guardian_Angel),
                },
                startingItems = new List<ItemId>
                {
                    ItemId.Phage
                }
            };
        }

        private void Orbwalker_OnPostAttack(AttackableUnit target, System.EventArgs args)
        {
            if (!target.IsMe || !(target is AIHeroClient))
                return;
            W.Cast();
        }

        public override void useQ(Obj_AI_Base target)
        {
           // if (Q.CanCast(target))
          //  {
                Q.Cast();
          //  }
        }

        public override void useW(Obj_AI_Base target)
        {
           
        }

        public override void useE(Obj_AI_Base target)
        {
            if ( E.CanCast(target) && (Q.IsReady() || R.IsReady()))
            {
                if ((MapControl.balanceAroundPoint(player.Position.To2D(), 700) >= -1 || (MapControl.fightIsOn() != null && MapControl.fightIsOn().NetworkId == target.NetworkId)))
     
                E.Cast(target.ServerPosition);
            }
        }

        public override void useR(Obj_AI_Base target)
        {
            if (R.CanCast(target) && (Q.GetSpellDamage(target) < target.Health || !Q.IsReady()))
            {
                CastR(target);
            }
        }

        public override void setUpSpells()
        {
            //Initialize our Spells
            Q = new Spell.Active(SpellSlot.Q, 400);
            W = new Spell.Active(SpellSlot.W, 145);
            E = new Spell.Skillshot(SpellSlot.E, 540, SkillShotType.Cone, 250, 100, 120);
            R = new Spell.Targeted(SpellSlot.R, 460);
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
                if (player.GetSpellDamage(target, SpellSlot.R) -50 > hero.Health)
                {
                    R.Cast(target);
                }
                else if (player.GetSpellDamage(target, SpellSlot.R) -50 < hero.Health)
                {
                    foreach (var buff in hero.Buffs.Where(buff => buff.Name == "dariushemo"))
                    {
                        if (player.GetSpellDamage(target, SpellSlot.R, DamageLibrary.SpellStages.SecondCast) * (1 + buff.Count / 5) -50> target.Health)
                        {
                            R.Cast(target);
                        }
                    }
                }
            }
        }

    }
}