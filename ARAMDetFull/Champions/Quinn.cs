using EloBuddy;
using EloBuddy.SDK;
using EloBuddy.SDK.Enumerations;
using System.Collections.Generic;

namespace ARAMDetFull.Champions
{
    class Quinn : Champion
    {
        public Quinn()
        {
            ARAMSimulator.champBuild = new Build
            {
                coreItems = new List<ConditionalItem>
                {
                    new ConditionalItem(ItemId.The_Bloodthirster),
                    new ConditionalItem(ItemId.Berserkers_Greaves),
                    new ConditionalItem(ItemId.Statikk_Shiv),
                    new ConditionalItem(ItemId.Frozen_Mallet),
                    new ConditionalItem(ItemId.Blade_of_the_Ruined_King),
                    new ConditionalItem(ItemId.Banshees_Veil),
                },
                startingItems = new List<ItemId>
                {
                    ItemId.Vampiric_Scepter,
                    ItemId.Boots_of_Speed
                }
            };
        }
        public override void useQ(Obj_AI_Base target)
        {
            if (!Q.IsReady())
                return;
            if (IsValorMode() && player.Distance(target,true) <= 375*375)
            {
                Q.Cast();
            }
            if (!IsValorMode())
            {
                Q.Cast(target);
            }
            Core.DelayAction(() => Player.IssueOrder(GameObjectOrder.AttackUnit, target), 100);
        }

        public override void useW(Obj_AI_Base target)
        {
            //if (MapControl.balanceAroundPoint(target.Position.To2D(), 400) <= 1)
             //   W.CastOnUnit(target);
        }

        public override void useE(Obj_AI_Base target)
        {
            if (!E.IsReady())
                return;
            if (!Sector.inTowerRange(target.Position.To2D()) &&
                (MapControl.balanceAroundPoint(target.Position.To2D(), 700) >= -1 ||
                 (MapControl.fightIsOn() != null && MapControl.fightIsOn().NetworkId == target.NetworkId)))
            {

                var passive = player.CalculateDamageOnUnit(target, DamageType.Physical, (float) (15 + (player.Level*10) + (player.FlatPhysicalDamageMod*0.5)));

                if (IsValorMode())
                {
                    if (R.IsReady() && target.Distance(player) < R.Range)
                    {
                        var ultdamage = player.CalculateDamageOnUnit(target, DamageType.Physical, (float)(75 + (R.Level*55) + (player.FlatPhysicalDamageMod*0.5))* (2 - (target.Health/target.MaxHealth)));

                        if ((ultdamage + passive) > target.Health)
                        {
                            R.Cast();
                            E.CastOnUnit(target);
                            Core.DelayAction(() => Player.IssueOrder(GameObjectOrder.AttackUnit, target), 100);
                        }
                    }
                    else
                    {
                        E.CastOnUnit(target);
                        Core.DelayAction(() => Player.IssueOrder(GameObjectOrder.AttackUnit, target), 100);
                    }
                }
                else // human form
                {
                    if (MapControl.balanceAroundPoint(target.Position.To2D(), 600) >= -1)
                    {
                        E.CastOnUnit(target);
                        Core.DelayAction(() => Player.IssueOrder(GameObjectOrder.AttackUnit, target), 100);
                    }
                }
            }
        }

        public override void useR(Obj_AI_Base target)
        {
            return;
            if (!R.IsReady())
                return;
            if (player.Path.Length > 0 && player.Path[player.Path.Length - 1].Distance(player.Position) > 2500)
            {
                R.Cast(player.Path[player.Path.Length - 1]);
            }
        }

        public override void setUpSpells()
        {
            // Initialize spells
            Q = new Spell.Skillshot(SpellSlot.Q, 1025, SkillShotType.Linear, 0, 750, 210);
            W = new Spell.Active(SpellSlot.W, 2100);
            E = new Spell.Targeted(SpellSlot.E, 675);
            R = new Spell.Active(SpellSlot.R);
        }

        public override void useSpells()
        {
            var tar = ARAMTargetSelector.getBestTarget(1010);
            if (tar == null || tar.IsUnderHisturret())
            {
                Player.IssueOrder(GameObjectOrder.Stop, null);
                return;
            }
            Player.IssueOrder(GameObjectOrder.AttackUnit, tar);
            useQ(tar);
            useE(tar);
            useR(tar);
        }

        public bool IsValorMode()
        {
            return ObjectManager.Player.Spellbook.GetSpell(SpellSlot.R).Name == "QuinnRFinale";
        }
    }
}
