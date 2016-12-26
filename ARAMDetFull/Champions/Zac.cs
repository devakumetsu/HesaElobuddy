using System;
using System.Collections.Generic;
using System.Linq;
using SharpDX;
using EloBuddy;
using EloBuddy.SDK;
using EloBuddy.SDK.Enumerations;

namespace ARAMDetFull.Champions
{
    class Zac : Champion
    {
        public static bool ChannelingE;

        public static int zacETime;

        public static int[] EMaxRanges = { 1150, 1300, 1450, 1600, 1750 };
        public static int[] EMaxChannelTimes = { 900, 1000, 1100, 1200, 1300 };

        public Zac()
        {
            ARAMSimulator.champBuild = new Build
            {
                coreItems = new List<ConditionalItem>
                {
                    new ConditionalItem(ItemId.Spirit_Visage),
                    new ConditionalItem(ItemId.Mercurys_Treads, ItemId.Ninja_Tabi, ItemCondition.ENEMY_AP),
                    new ConditionalItem(ItemId.Sunfire_Cape),
                    new ConditionalItem(ItemId.Rylais_Crystal_Scepter),
                    new ConditionalItem(ItemId.Warmogs_Armor),
                    new ConditionalItem(ItemId.Abyssal_Scepter),
                },
                startingItems = new List<ItemId>
                {
                    ItemId.Giants_Belt,ItemId.Boots_of_Speed
                }
            };
            Obj_AI_Base.OnBuffLose += Player_OnBuffLose;
            Obj_AI_Base.OnBuffGain += Player_OnBuffGain;
            Obj_AI_Base.OnProcessSpellCast += Game_ProcessSpell;
        }

        private static void Player_OnBuffGain(Obj_AI_Base sender, Obj_AI_BaseBuffGainEventArgs args)
        {
            if (!sender.IsMe) return;
            if (sender.IsMe && args.Buff.Name == "ZacE")
            {
                ChannelingE = true;
                Orbwalker.DisableAttacking = true;
                Orbwalker.DisableMovement = true;
            }

            if (sender.IsMe && args.Buff.Name == "ZacR")
            {
                Orbwalker.DisableAttacking = true;
            }
        }

        private static void Player_OnBuffLose(Obj_AI_Base sender, Obj_AI_BaseBuffLoseEventArgs args)
        {
            if (!sender.IsMe) return;
            if (sender.IsMe && args.Buff.Name == "ZacE")
            {
                ChannelingE = false;
                Orbwalker.DisableAttacking = false;
                Orbwalker.DisableMovement = false;
            }

            if (sender.IsMe && args.Buff.Name == "ZacR")
            {
                Orbwalker.DisableAttacking = false;
            }
        }

        private void Game_ProcessSpell(Obj_AI_Base sender, GameObjectProcessSpellCastEventArgs args)
        {
            if (args.SData.Name == "ZacE")
            {
                if (zacETime == 0)
                {
                    zacETime = System.Environment.TickCount;
                    Core.DelayAction(() => { zacETime = 0; }, 4000);
                }
            }
        }

        public override void useQ(Obj_AI_Base target)
        {
            if (!Q.IsReady())
                return;
            Q.Cast(target);
        }

        public override void useW(Obj_AI_Base target)
        {
            if (!W.IsReady())
                return;
            W.Cast();
        }

        public double GetAngle(Obj_AI_Base source, Vector3 target)
        {
            if (source == null || !target.IsValid())
            {
                return 0;
            }
            return Geometry.AngleBetween(
                Geometry.Perpendicular(source.Direction.To2D()), (target - source.Position).To2D());
            ;
        }

        public int[] eRanges = new int[] { 1150, 1300, 1450, 1600, 1750 };
        public float[] eChannelTimes = new float[] { 0.9f, 1.05f, 1.2f, 1.35f, 1.5f };
        public override void useE(Obj_AI_Base target)
        {
            if (target.Distance(player) > eRanges[E.Level - 1] || target.IsUnderHisturret())
            {
                return;
            }
            var ePred = E.GetPrediction(target);

            if (!ChannelingE)
            {
                if (target.Distance(Player.Instance.ServerPosition) <
                    EMaxRanges[E.Level - 1] - 100 &&
                    target.Distance(Player.Instance.ServerPosition) > 250)
                {
                    Player.CastSpell(SpellSlot.E, target);
                    Console.WriteLine("Started Charging E (combo)");
                    return;
                }
            }
            if (ChannelingE)
            {
                if (ePred.CastPosition.Distance(Player.Instance.ServerPosition) <= EMaxRanges[E.Level - 1] && ePred.HitChance >= HitChance.High)
                {
                    Player.CastSpell(SpellSlot.E, ePred.CastPosition);
                    Console.WriteLine("Casted E to Target in Range (combo)");
                    return;
                }

                if (((Spell.Chargeable)E).IsFullyCharged &&
                    ePred.CastPosition.Distance(Player.Instance.ServerPosition) >
                    EMaxRanges[E.Level - 1] && target.Distance(Player.Instance.ServerPosition) <
                    EMaxRanges[E.Level - 1] + 300)
                {
                    Player.CastSpell(SpellSlot.E, ePred.CastPosition);
                    Console.WriteLine("Casted E to target out of range(combo)");
                    return;
                }
            }
        }
        private static bool eActive
        {
            get { return player.Buffs.Any(buff => buff.Name == "ZacE"); }
        }

        public override void killSteal()
        {
            if (ChannelingE || eActive)
            {
                Orbwalker.DisableAttacking = true;
                Orbwalker.DisableMovement = true;
            }
            else
            {
                Orbwalker.DisableAttacking = false;
                Orbwalker.DisableMovement = false;
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
            Q = new Spell.Skillshot(SpellSlot.Q, 550, SkillShotType.Linear, 500, int.MaxValue, 120)
            {
                AllowedCollisionCount = int.MaxValue
            };
            W = new Spell.Active(SpellSlot.W, 350);
            E = new Spell.Chargeable(SpellSlot.E, 0, 1750, 1500, 500, 1500, 250)
            {
                AllowedCollisionCount = int.MaxValue
            };
            R = new Spell.Active(SpellSlot.R, 300);

        }

        public override void farm()
        {
            if (ChannelingE || eActive)
                return;
            base.farm();
        }

        public override void useSpells()
        {
            var tar = ARAMTargetSelector.getBestTarget(E.Range);
            if (tar != null) useE(tar);
            if (ChannelingE || eActive)
                return;
            tar = ARAMTargetSelector.getBestTarget(Q.Range);
            if (tar != null) useQ(tar);
            tar = ARAMTargetSelector.getBestTarget(W.Range);
            if (tar != null) useW(tar);
            tar = ARAMTargetSelector.getBestTarget(R.Range);
            if (tar != null) useR(tar);
        }

        void AutoUlt()
        {
            return;//FOR NOW UNTIL WE FIX THE CRASH ISSUE.

            var comboR = 2;

            if (comboR > 0 && R.IsReady())
            {
                int enemiesHit = 0;
                int killableHits = 0;

                foreach (AIHeroClient enemy in EntityManager.Heroes.Enemies)
                {
                    var prediction = R.GetPrediction(enemy);

                    if (prediction != null && prediction.UnitPosition.Distance(ObjectManager.Player.ServerPosition) <= R.Range)
                    {
                        enemiesHit++;
                        if (ObjectManager.Player.GetSpellDamage(enemy, SpellSlot.W) >= enemy.Health)
                            killableHits++;
                    }
                }

                if (enemiesHit >= comboR ||
                    (killableHits >= 1 && ObjectManager.Player.Health/ObjectManager.Player.MaxHealth <= 0.1))
                {
                    Aggresivity.addAgresiveMove(new AgresiveMove(105,4000,true));
                    CastR();
                }
            }
        }
        
        void CastR()
        {
            if (!R.IsReady())
                return;
            R.Cast();
        }
    }
}
