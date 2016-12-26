using System;
using System.Collections.Generic;
using System.Linq;
using EloBuddy;
using EloBuddy.SDK;
using EloBuddy.SDK.Enumerations;

namespace ARAMDetFull.Champions
{
    class Jinx : Champion
    {
        //ManaMenager
        public static int QMANA;
        public static int WMANA;
        public static int EMANA;
        public static int RMANA;
        public static bool Farm = true;
        public static double WCastTime = 0;

        public Jinx()
        {
            Orbwalker.OnPreAttack += Orbwalker_OnPreAttack;
            Orbwalker.OnPostAttack += Orbwalker_OnPostAttack;


            ARAMSimulator.champBuild = new Build
            {
                coreItems = new List<ConditionalItem>
                {
                    new ConditionalItem(ItemId.Infinity_Edge),
                    new ConditionalItem(ItemId.Berserkers_Greaves),
                    new ConditionalItem(ItemId.Phantom_Dancer),
                    new ConditionalItem(ItemId.Blade_of_the_Ruined_King),
                    new ConditionalItem(ItemId.Mercurial_Scimitar),
                    new ConditionalItem((ItemId)3036),
                },
                startingItems = new List<ItemId>
                {
                    ItemId.Pickaxe,ItemId.Boots_of_Speed
                }
            };
        }

        private void Orbwalker_OnPostAttack(AttackableUnit unit, EventArgs args)
        {
            if (!unit.IsMe || !Q.IsReady() || !FishBoneActive) return;
            var t = ARAMTargetSelector.getBestTarget(bonusRange() + 50);
            if (t.IsValidTarget())
            {
                var distance = GetRealDistance(t);
                var powPowRange = GetRealPowPowRange(t);
                if ((distance < powPowRange) && (ObjectManager.Player.Mana < RMANA + WMANA + 20 || ObjectManager.Player.GetAutoAttackDamage(t) * 2 < t.Health))
                    Q.Cast();
                else if (Farm && (distance > bonusRange() || distance < powPowRange || ObjectManager.Player.Mana < RMANA + EMANA + WMANA + WMANA))
                    Q.Cast();
            }
        }

        private void Orbwalker_OnPreAttack(AttackableUnit unit, Orbwalker.PreAttackArgs args)
        {
            var t = ARAMTargetSelector.getBestTarget(bonusRange() + 50);
            if (t.IsValidTarget() && Q.IsReady() && FishBoneActive)
            {
                var distance = GetRealDistance(t);
                var powPowRange = GetRealPowPowRange(t);
                if ((distance < powPowRange) && (ObjectManager.Player.Mana < RMANA + WMANA + 20 || ObjectManager.Player.GetAutoAttackDamage(t) * 2 < t.Health))
                    Q.Cast();
                else if (Farm && (distance > bonusRange() || distance < powPowRange || ObjectManager.Player.Mana < RMANA + EMANA + WMANA + WMANA))
                    Q.Cast();
            }
        }

        public override void useQ(Obj_AI_Base target)
        {
            if (!Q.IsReady())
                return; 
            ManaManager();
            if (Farm)
                if (ObjectManager.Player.Mana > RMANA + WMANA + EMANA + 10 && !FishBoneActive)
                    farmQ();
            var t = ARAMTargetSelector.getBestTarget(bonusRange() + 50);
            if (t.IsValidTarget())
            {
                var distance = GetRealDistance(t);
                var powPowRange = GetRealPowPowRange(t);

                if (!FishBoneActive && (distance > powPowRange))
                {
                    if ( (ObjectManager.Player.Mana > RMANA + WMANA + 20 || ObjectManager.Player.GetAutoAttackDamage(t) * 2 > t.Health))
                        Q.Cast();
                    else if (Farm && haras() && ObjectManager.Player.Mana > RMANA + WMANA + EMANA + WMANA && distance < bonusRange() + t.BoundingRadius)
                        Q.Cast();
                }
            }
            Core.DelayAction(() => Player.IssueOrder(GameObjectOrder.AttackUnit, target), 100);
        }

        public override void useW(Obj_AI_Base target)
        {
            if (!W.IsReady())
                return;
            W.Cast(target);
            Core.DelayAction(() => Player.IssueOrder(GameObjectOrder.AttackUnit, target), 250);
        }

        public override void useE(Obj_AI_Base target)
        {
            if (!E.IsReady())
                return;
            E.Cast(target);
            Core.DelayAction(() => Player.IssueOrder(GameObjectOrder.AttackUnit, target), 100);
        }

        public override void useR(Obj_AI_Base target)
        {
            if (!R.IsReady())
                return;
            if (target.Health < R.GetDamage(target))
                R.Cast(target);
            Core.DelayAction(() => Player.IssueOrder(GameObjectOrder.AttackUnit, target), 250);
        }

        public override void setUpSpells()
        {
            //Create the spells
            Q = new Spell.Active(SpellSlot.Q);
            W = new Spell.Skillshot(SpellSlot.W, 1500, SkillShotType.Linear, 500, 3300, 60)
            {
                AllowedCollisionCount = 0
            };
            E = new Spell.Skillshot(SpellSlot.E, 900, SkillShotType.Circular, 250, 1750, 315)
            {
                AllowedCollisionCount = int.MaxValue
            };
            R = new Spell.Skillshot(SpellSlot.R, 25000, SkillShotType.Linear, 500, 1700, 140)
            {
                AllowedCollisionCount = int.MaxValue
            };
        }

        public override void useSpells()
        {
            var tar = ARAMTargetSelector.getBestTarget(1010);
            useQ(tar);
            tar = ARAMTargetSelector.getBestTarget(E.Range);
            if (tar != null) useE(tar);
            tar = ARAMTargetSelector.getBestTarget(W.Range);
            if (tar != null) useW(tar);
            tar = ARAMTargetSelector.getBestTarget(R.Range);
            if (tar != null) useR(tar);
        }
        
        public void farmQ()
        {
            var allMinionsQ = EntityManager.MinionsAndMonsters.EnemyMinions.Where(x => x.IsInRange(Player.Instance.ServerPosition, bonusRange() + 30));
            foreach (var minion in allMinionsQ)
            {
                if (!Player.Instance.IsInAutoAttackRange(minion) && minion.Health < ObjectManager.Player.GetAutoAttackDamage(minion) && GetRealPowPowRange(minion) < GetRealDistance(minion) && bonusRange() < GetRealDistance(minion))
                {
                    Q.Cast();
                    Player.IssueOrder(GameObjectOrder.AttackUnit, minion);
                    return;
                }
                else if (Player.Instance.IsInAutoAttackRange(minion) && CountEnemies(minion, 150) > 0)
                {
                    Q.Cast();
                    Player.IssueOrder(GameObjectOrder.AttackUnit, minion);
                    return;
                }
            }
        }

        public bool haras()
        {
            var allMinionsQ = EntityManager.MinionsAndMonsters.EnemyMinions.Where(x => x.IsInRange(Player.Instance.ServerPosition, bonusRange()));
            var haras = true;
            foreach (var minion in allMinionsQ)
            {
                if (minion.Health < ObjectManager.Player.GetAutoAttackDamage(minion) * 1.5 && bonusRange() > GetRealDistance(minion))
                    haras = false;
            }
            if (haras)
                return true;
            else
                return false;
        }

        public override void farm()
        {
            if (FishBoneActive && Q.IsReady())
                Q.Cast();
        }

        public float bonusRange()
        {
            return 620f + ObjectManager.Player.BoundingRadius + 50 + 25 * ObjectManager.Player.Spellbook.GetSpell(SpellSlot.Q).Level;
        }

        private bool FishBoneActive
        {
            get { return Math.Abs(ObjectManager.Player.AttackRange - 525f) > float.Epsilon; }
        }

        private int CountEnemies(Obj_AI_Base target, float range)
        {
            return
                ObjectManager.Get<AIHeroClient>()
                    .Count(
                        hero =>
                            hero.IsValidTarget() && hero.Team != ObjectManager.Player.Team &&
                            hero.ServerPosition.Distance(target.ServerPosition) <= range);
        }
        private int CountAlliesNearTarget(Obj_AI_Base target, float range)
        {
            return
                ObjectManager.Get<AIHeroClient>()
                    .Count(
                        hero =>
                            hero.Team == ObjectManager.Player.Team &&
                            hero.ServerPosition.Distance(target.ServerPosition) <= range);
        }

        private float GetRealPowPowRange(GameObject target)
        {
            return 610f + ObjectManager.Player.BoundingRadius + target.BoundingRadius;
        }

        private float GetRealDistance(GameObject target)
        {
            return ObjectManager.Player.ServerPosition.Distance(target.Position) + ObjectManager.Player.BoundingRadius +
                   target.BoundingRadius;
        }

        public void ManaManager()
        {
            QMANA = 10;
            WMANA = 40 + 10 * W.Level;
            EMANA = 50;
            if (!R.IsReady())
                RMANA = WMANA - ObjectManager.Player.Level * 2;
            else
                RMANA = 100;

            if (Farm)
                RMANA = RMANA + (CountEnemies(ObjectManager.Player, 2500) * 20);

            if (ObjectManager.Player.Health < ObjectManager.Player.MaxHealth * 0.2)
            {
                QMANA = 0;
                WMANA = 0;
                EMANA = 0;
                RMANA = 0;
            }
        }

    }
}
