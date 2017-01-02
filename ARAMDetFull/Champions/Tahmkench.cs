using System;
using System.Collections.Generic;
using System.Linq;
using EloBuddy;
using EloBuddy.SDK;
using EloBuddy.SDK.Enumerations;

namespace ARAMDetFull.Champions
{
    class Tahmkench : Champion
    {
        private Spell.Skillshot W2;
        private SwallowedTarget current = SwallowedTarget.None;

        enum SwallowedTarget
        {
            Enemy,
            Ally,
            Minion,
            None
        }
        
        public Tahmkench()
        {
            Obj_AI_Base.OnProcessSpellCast += OnProcessSpell;

            ARAMSimulator.champBuild = new Build
            {
                coreItems = new List<ConditionalItem>
                {
                    new ConditionalItem(ItemId.Sunfire_Cape),
                    new ConditionalItem(ItemId.Mercurys_Treads,ItemId.Ninja_Tabi,ItemCondition.ENEMY_AP),
                    new ConditionalItem((ItemId)3748),
                    new ConditionalItem(ItemId.Frozen_Heart),
                    new ConditionalItem((ItemId)3742),
                    new ConditionalItem(ItemId.Banshees_Veil),
                },
                startingItems = new List<ItemId>
                {
                    ItemId.Giants_Belt
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
            if (!W.IsReady() || current != SwallowedTarget.None)
                return;

            if (target.Distance(player) <= 250 && target.GetBuffCount("TahmKenchPDebuffCounter") == 3 && current == SwallowedTarget.None)
                W.CastOnUnit(target);
            else if (current == SwallowedTarget.None)
            {
                W.CastOnUnit(ObjectManager.Get<Obj_AI_Minion>().Where(x => x.IsEnemy).OrderBy(x => x.Distance(player)).First());
            }
            else if (current == SwallowedTarget.Minion)
            {
                W2.Cast(target.Position);
            }
        }

        public override void useE(Obj_AI_Base target)
        {
            if (!E.IsReady() || target == null)
                return;
            //E.Cast();
        }


        public override void useR(Obj_AI_Base target)
        {
            if (!R.IsReady() || target == null)
                return;
        }

        public override void useSpells()
        {
            var tar = ARAMTargetSelector.getBestTarget(Q.Range);
            if (tar != null) useQ(tar);
            tar = ARAMTargetSelector.getBestTarget(W2.Range);
            if (tar != null) useW(tar);
            tar = ARAMTargetSelector.getBestTarget(E.Range);
            if (tar != null) useE(tar);
            tar = ARAMTargetSelector.getBestTarget(R.Range);
            //if (tar != null) useR(tar);
        }

        public override void setUpSpells()
        {
            Q = new Spell.Skillshot(SpellSlot.Q, 800, SkillShotType.Linear, 100, 2000, 70);
            //WGRAB = new Spell.Active(SpellSlot.W);
            W = new Spell.Targeted(SpellSlot.W, 330);
            W2 = new Spell.Skillshot(SpellSlot.W, 650, SkillShotType.Linear, 100, 900, 75);
            E = new Spell.Active(SpellSlot.E);
            R = new Spell.Active(SpellSlot.R, 4000);
            //Create the spells
            /*Q = new Spell(SpellSlot.Q, 800);
            Q.SetSkillshot(.1f, 75, 2000, true, SkillshotType.SkillshotLine);
            W = new Spell(SpellSlot.W, 250);
            W2 = new Spell(SpellSlot.W, 900); //Not too sure on this value
            W2.SetSkillshot(.1f, 75, 1500, false, SkillshotType.SkillshotLine); //Not sure on these values either.
            E = new Spell(SpellSlot.E);
            R = new Spell(SpellSlot.R, 4000);*/
        }
        
        public override void farm()
        {
            if (player.ManaPercent < 55)
                return;

            foreach (var minion in EntityManager.MinionsAndMonsters.GetLaneMinions(EntityManager.UnitTeam.Enemy, player.Position, Q.Range))
            {
                if (minion.Health > ObjectManager.Player.GetAutoAttackDamage(minion) && minion.Health < Q.GetDamage(minion))
                {
                    Q.Cast(minion);
                    return;
                }
            }
        }
        
        void OnProcessSpell(Obj_AI_Base sender, GameObjectProcessSpellCastEventArgs args)
        {
            //Modified Kalista Soulbound code from Corey
            //Need to check in fountain otherwise recalls could make you swallow
            if (player.IsInFountainRange())
                return;

            try
            {
                var hero = sender as AIHeroClient;
                if (hero != null && hero.IsEnemy && !player.IsInFountainRange())
                {
                    var swallowAlly =
                        EntityManager.Heroes.Allies.FirstOrDefault(
                            x => x.HealthPercent < 25
                                && ARAMSimulator.inDanger
                                && x.IsAlly && player.Distance(x) <= 500
                                && !x.IsDead);
                    if (swallowAlly != null && current == SwallowedTarget.None && W.IsReady())
                    {
                        W.CastOnUnit(swallowAlly);
                        current = SwallowedTarget.Ally;
                    }

                    AIHeroClient enemy = hero;
                    if (E.IsReady())
                    {
                        SpellDataInst s =
                            enemy.Spellbook.Spells.FirstOrDefault(x => x.SData.Name.Equals(args.SData.Name));
                        if (s == null)
                            return;
                        if (enemy.GetSpellDamage(player, s.Slot) > player.Health)
                            E.Cast();
                        else if (player.HealthPercent <= 45)
                            E.Cast();
                    }
                }
                else if (sender.IsMe)
                {
                    var spell = player.Spellbook.Spells.FirstOrDefault(x => x.SData.Name == args.SData.Name);
                    if (spell == null)
                        return;
                    SpellSlot s = spell.Slot;
                    if (s == SpellSlot.W)
                    {
                        if (args.Target is AIHeroClient)
                            current = (args.Target.IsAlly) ? SwallowedTarget.Ally : SwallowedTarget.Enemy;
                        else
                            current = SwallowedTarget.Minion;
                    }
                    else if (s.ToString().Equals("46"))
                        current = SwallowedTarget.None;
                }
            }
            catch (Exception)
            {
            }
        }
    }
}