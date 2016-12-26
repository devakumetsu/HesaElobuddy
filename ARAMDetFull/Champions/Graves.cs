using System.Collections.Generic;
using EloBuddy;
using EloBuddy.SDK;
using EloBuddy.SDK.Enumerations;
using SharpDX;

namespace ARAMDetFull.Champions
{
    class Graves : Champion
    {
        public static Vector2 QCastPos = new Vector2();
        
        public Graves()
        {
            ARAMSimulator.champBuild = new Build
            {
                coreItems = new List<ConditionalItem>
                {
                    new ConditionalItem(ItemId.The_Bloodthirster),
                    new ConditionalItem(ItemId.Berserkers_Greaves),
                    new ConditionalItem(ItemId.Infinity_Edge),
                    new ConditionalItem(ItemId.Blade_of_the_Ruined_King),
                    new ConditionalItem(ItemId.Last_Whisper),
                    new ConditionalItem(ItemId.Phantom_Dancer),
                },
                startingItems = new List<ItemId>
                {
                    ItemId.Boots_of_Speed,
                    ItemId.Vampiric_Scepter,
                }
            };
        }
        
        public override void useQ(Obj_AI_Base target)
        {
            if (!Q.IsReady() || target == null)
                return;
            Q.Cast(target);
        }

        public override void useW(Obj_AI_Base target)
        {
            if (!W.IsReady() || target == null)
                return;
            //if (!Q.IsReady(4500) && player.Mana > 200)
            W.Cast(target);
        }

        public override void useE(Obj_AI_Base target)
        {
            if (!E.IsReady() || target == null || !safeGap(player.Position.Extend(target.Position,450)))
                return;
            E.Cast(target.Position);
        }

        public override void useR(Obj_AI_Base target)
        {
            if (!R.IsReady() || target == null)
                return;
            R.CastIfWillHit(target, 2);
        }

        public override void useSpells()
        {
            if (ObjectManager.Player.Spellbook.IsChanneling)
              return;

            var tar = ARAMTargetSelector.getBestTarget(Q.Range);
            if (tar != null) useQ(tar);
            tar = ARAMTargetSelector.getBestTarget(W.Range);
            if (tar != null) useW(tar);
            tar = ARAMTargetSelector.getBestTarget(E.Range);
            if (tar != null) useE(tar);
            tar = ARAMTargetSelector.getBestTarget(R.Range);
            if (tar != null) useR(tar);

        }
        
        public override void setUpSpells()
        {
            Q = new Spell.Skillshot(SpellSlot.Q, 850, SkillShotType.Linear, 250, 2000, 50);
            W = new Spell.Skillshot(SpellSlot.W, 940, SkillShotType.Circular, 350, 1650, 150);
            E = new Spell.Active(SpellSlot.E, 850);
            R = new Spell.Skillshot(SpellSlot.R, 1000, SkillShotType.Linear, 250, 2100, 120);
        }

        public override void killSteal()
        {
            return;
            //base.killSteal();
            //if(!E.IsReady())
                //return;
            //foreach (var ene in EntityManager.Heroes.Enemies)
            {
                //if(ene.Distance(player)<)
            }
        }

        public override void farm()
        {
            if (player.ManaPercent < 55)
                return;

            var lanemonsters = EntityManager.MinionsAndMonsters.GetLaneMinions(EntityManager.UnitTeam.Enemy, player.Position, Q.Range);
            foreach (var minion in lanemonsters)
            {
                if (Q.IsReady() && Q.GetDamage(minion) > minion.Health)
                {
                    Q.Cast(minion);
                }
            }
        }
    }
}