using EloBuddy;
using EloBuddy.SDK;
using EloBuddy.SDK.Enumerations;
using EloBuddy.SDK.Events;
using System.Collections.Generic;
using System.Linq;

namespace ARAMDetFull.Champions
{
    class Ashe : Champion
    {

        public Ashe()
        {
            Interrupter.OnInterruptableSpell += Interrupter_OnInterruptableSpell; ;
            Obj_AI_Base.OnProcessSpellCast += Game_OnProcessSpell;

            ARAMSimulator.champBuild = new Build
            {
                coreItems = new List<ConditionalItem>
                {
                    new ConditionalItem(ItemId.Infinity_Edge),
                    new ConditionalItem(ItemId.Berserkers_Greaves),
                    new ConditionalItem(ItemId.Phantom_Dancer),
                    new ConditionalItem(ItemId.The_Bloodthirster),
                    new ConditionalItem(ItemId.Last_Whisper),
                    new ConditionalItem(ItemId.Banshees_Veil),
                },
                startingItems = new List<ItemId>
                {
                    ItemId.Boots_of_Speed,
                    ItemId.Pickaxe
                }
            };
        }
        
        private void AfterAttack(AttackableUnit unit, AttackableUnit target)
        {
            if (target is AIHeroClient && Q.IsReady())
            {
                Q.Cast();
                if (!Orbwalker.IsAutoAttacking || Orbwalker.GetTarget() != target)
                    Player.IssueOrder(GameObjectOrder.AttackUnit, target);
            }
        }

        public override void useQ(Obj_AI_Base target)
        {

        }

        public override void useW(Obj_AI_Base target)
        {

        }

        public override void useE(Obj_AI_Base target)
        {
            if (!E.IsReady() || target == null)
                return;
            E.Cast(target);
        }


        public override void useR(Obj_AI_Base target)
        {
            if (!R.IsReady() || target == null)
                return;
            if (target.HealthPercent < 35)
                R.Cast(target);
        }

        public override void useSpells()
        {
            var tar = ARAMTargetSelector.getBestTarget(E.Range);
            if (tar != null) useE(tar);
            tar = ARAMTargetSelector.getBestTarget(W.Range);
            if (tar != null) useW(tar);

            if (!Orbwalker.IsAutoAttacking || Orbwalker.GetTarget() != tar)
                Player.IssueOrder(GameObjectOrder.AttackUnit, tar);

            if (R.IsReady())
            {
                foreach (var enem in ObjectManager.Get<AIHeroClient>()
                    .Where(ene => ene.IsEnemy && !ene.IsDead && ene.Distance(player, true) < R.Range * R.Range).Where(enem => enem.HealthPercent < 35))
                {
                    var prediction = (R as Spell.Skillshot).CastMinimumHitchance(enem, HitChance.High);
                    return;
                }
            }


            if (W.IsReady())
            {
                foreach (var enem in ObjectManager.Get<AIHeroClient>().Where(ene => ene.Distance(player) < W.Range && !ene.IsDead))
                {
                    if (enem.Type == player.Type && enem.CountEnemiesInRange(330) > 1)
                    {
                        W.Cast(enem.ServerPosition);
                        return;
                    }
                }

            }

            if (tar is AIHeroClient && Q.IsReady() && player.IsInRange(tar, player.GetAutoAttackRange()) && !tar.IsDead)
            {
                Q.Cast();
                if(!Orbwalker.IsAutoAttacking || Orbwalker.GetTarget() != tar)
                    Player.IssueOrder(GameObjectOrder.AttackUnit, tar);
            }

        }

        public override void setUpSpells()
        {
            Q = new Spell.Active(SpellSlot.Q);
            W = new Spell.Skillshot(SpellSlot.W, 1200, SkillShotType.Linear, 0, int.MaxValue, 60)
            {
                AllowedCollisionCount = 0
            };
            E = new Spell.Skillshot(SpellSlot.E, 15000, SkillShotType.Linear, 0, int.MaxValue, 0);
            R = new Spell.Skillshot(SpellSlot.R, 15000, SkillShotType.Linear, 500, 1000, 250)
            {
                AllowedCollisionCount = int.MaxValue
            };
        }
        
        private void Interrupter_OnInterruptableSpell(Obj_AI_Base unit, Interrupter.InterruptableSpellEventArgs e)
        {
            /*if (spell.DangerLevel == .High && R.IsReady() && unit.IsValidTarget(1500))
            {
                R.Cast(unit);
            }*/
        }

        private float GetComboDamage(AIHeroClient t)
        {
            var fComboDamage = 0f;

            if (W.IsReady())
                fComboDamage += ObjectManager.Player.GetSpellDamage(t, SpellSlot.W);

            if (R.IsReady())
                fComboDamage += ObjectManager.Player.GetSpellDamage(t, SpellSlot.R);

            if (ObjectManager.Player.GetSpellSlotFromName("summonerdot") != SpellSlot.Unknown && ObjectManager.Player.Spellbook.CanUseSpell(ObjectManager.Player.GetSpellSlotFromName("summonerdot")) == SpellState.Ready && ObjectManager.Player.Distance(t) < 550)
                fComboDamage += ObjectManager.Player.GetSummonerSpellDamage(t, DamageLibrary.SummonerSpells.Ignite);


            return fComboDamage;
        }

        public void Game_OnProcessSpell(Obj_AI_Base unit, GameObjectProcessSpellCastEventArgs spell)
        {
            if ( unit.Team == ObjectManager.Player.Team)
                return;

            if (spell.SData.Name.ToLower() == "summonerflash")
                E.Cast(spell.End);
        }

        private bool AsheQCastReady
        {
            get { return ObjectManager.Player.HasBuff("AsheQCastReady"); }
        }

        public bool IsQActive
        {
            get { return ObjectManager.Player.HasBuff("FrostShot"); }
        }
    }
}
