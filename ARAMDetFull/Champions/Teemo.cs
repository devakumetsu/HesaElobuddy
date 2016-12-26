using System.Collections.Generic;
using System.Linq;
using EloBuddy;
using EloBuddy.SDK;
using EloBuddy.SDK.Enumerations;

namespace ARAMDetFull.Champions
{
    class Teemo : Champion
    {
        public Teemo()
        {
            ARAMSimulator.champBuild = new Build
            {
                coreItems = new List<ConditionalItem>
                {
                    new ConditionalItem(ItemId.Liandrys_Torment),
                    new ConditionalItem(ItemId.Mercurys_Treads,ItemId.Ninja_Tabi,ItemCondition.ENEMY_AP),
                    new ConditionalItem(ItemId.Morellonomicon),
                    new ConditionalItem(ItemId.Ludens_Echo),
                    new ConditionalItem(ItemId.Void_Staff,ItemId.Zhonyas_Hourglass,ItemCondition.ENEMY_AP),
                    new ConditionalItem(ItemId.Rabadons_Deathcap,ItemId.Rylais_Crystal_Scepter,ItemCondition.ENEMY_LOSING),
                },
                startingItems = new List<ItemId>
                {
                    ItemId.Ruby_Crystal,ItemId.Amplifying_Tome,ItemId.Boots_of_Speed
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
            if (!W.IsReady())
                W.Cast();
        }

        public override void useE(Obj_AI_Base target)
        {
        }
        
        public override void useR(Obj_AI_Base target)
        {
            if (target == null)
                return;
            if (target.IsValidTarget(R.Range) && R.IsReady())
            {
                R.Cast(target.Position);
            }
        }

        public override void useSpells()
        {
            var tar = ARAMTargetSelector.getBestTarget(Q.Range);
            useQ(tar);
            tar = ARAMTargetSelector.getBestTarget(W.Range);
            useW(tar);
            tar = ARAMTargetSelector.getBestTarget(R.Range);
            useR(tar);

            Obj_AI_Base.OnBuffGain += (sender, args) =>
            {
                if (R.IsReady())
                {
                    BuffInstance aBuff =
                        (from fBuffs in
                             sender.Buffs.Where(
                                 s =>
                                 sender.Team != ObjectManager.Player.Team
                                 && sender.Distance(ObjectManager.Player.Position) < R.Range)
                         from b in new[]
                            {
                                "teleport", /* Teleport */ "pantheon_grandskyfall_jump", /* Pantheon */ 
                                "crowstorm", /* FiddleScitck */
                                "zhonya", "katarinar", /* Katarita */
                                "MissFortuneBulletTime", /* MissFortune */
                                "gate", /* Twisted Fate */
                                "chronorevive" /* Zilean */
                            }
                         where args.Buff.Name.ToLower().Contains(b)
                         select fBuffs).FirstOrDefault();

                    if (aBuff != null)
                    {
                        R.Cast(sender.Position);
                    }
                }
            };
        }
        
        public override void setUpSpells()
        {

            Q = new Spell.Targeted(SpellSlot.Q, 680);
            W = new Spell.Active(SpellSlot.W);
            E = new Spell.Active(SpellSlot.E, (uint)Player.Instance.AttackRange);
            R = new Spell.Skillshot(SpellSlot.R, 0, SkillShotType.Circular, 1000, 1000, 135);
        }

    }
}