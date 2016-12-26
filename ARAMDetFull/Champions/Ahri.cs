using System.Collections.Generic;
using System.Linq;
using SharpDX;
using EloBuddy;
using EloBuddy.SDK;
using EloBuddy.SDK.Enumerations;
using static EloBuddy.SDK.EntityManager.MinionsAndMonsters;
using ARAMDetFull;

namespace ARAMDetFull.Champions
{
    class Ahri : Champion
    {
        //settings
        private static bool _rOn;
        private static int _rTimer;
        private static int _rTimeLeft;

        public Ahri()
        {
            ARAMSimulator.champBuild = new Build
            {
                coreItems = new List<ConditionalItem>
                        {
                            new ConditionalItem(ItemId.Sorcerers_Shoes),
                            new ConditionalItem(ItemId.Athenes_Unholy_Grail),
                            new ConditionalItem(ItemId.Rabadons_Deathcap),
                            new ConditionalItem(ItemId.Zhonyas_Hourglass),
                            new ConditionalItem(ItemId.Ludens_Echo),
                            new ConditionalItem(ItemId.Void_Staff),
                        },
                startingItems = new List<ItemId>
                        {
                            ItemId.Boots_of_Speed,ItemId.Chalice_of_Harmony
                        }
            };
        }

        public override void useQ(Obj_AI_Base target)
        {
        }

        public override void useW(Obj_AI_Base target)
        {
        }

        public override void useE(Obj_AI_Base target)
        {
        }

        public override void useR(Obj_AI_Base target)
        {
        }

        public override void setUpSpells()
        {
            Q = new Spell.Skillshot(SpellSlot.Q, 900, SkillShotType.Linear, 250, 1500, 125)
            {
                AllowedCollisionCount = int.MaxValue
            };
            W = new Spell.Active(SpellSlot.W, 580);
            E = new Spell.Skillshot(SpellSlot.E, 970, SkillShotType.Linear, 250, 1500, 100)
            {
                AllowedCollisionCount = 0
            };
            R = new Spell.Skillshot(SpellSlot.R, 400, SkillShotType.Linear);
        }

        public override void useSpells()
        {
            var tar = ARAMTargetSelector.getBestTarget(Q.Range+800);
            if (tar != null)
                Combo();
            else
                Farm();
        }
        
        private float GetComboDamage(Obj_AI_Base enemy)
        {
            if (enemy == null)
                return 0;

            double damage = 0d;

            if (Q.IsReady())
            {
                damage += player.GetSpellDamage(enemy, SpellSlot.Q);
                damage += player.GetSpellDamage(enemy, SpellSlot.Q, DamageLibrary.SpellStages.SecondCast);
            }


            if (W.IsReady())
                damage += player.GetSpellDamage(enemy, SpellSlot.W);

            if (_rOn)
                damage += player.GetSpellDamage(enemy, SpellSlot.R) * RCount();
            else if (R.IsReady())
                damage += player.GetSpellDamage(enemy, SpellSlot.R) * 3;


            if (E.IsReady())
                damage += player.GetSpellDamage(enemy, SpellSlot.E);

            return (float)damage;
        }

        private void Combo()
        {
            UseSpells(true,true,true,true, "Combo");
        }

        private void Harass()
        {
            UseSpells(true,true,true, false, "Harass");
        }

        private void UseSpells(bool useQ, bool useW, bool useE, bool useR, string source)
        {
            var range = Q.Range;
            Obj_AI_Base eTarget = ARAMTargetSelector.getBestTarget(range);



            Obj_AI_Base rETarget = ARAMTargetSelector.getBestTarget(E.Range);

            var dmg = GetComboDamage(eTarget);

            if (eTarget == null)
                return;

            //end items-------

            //E
            if (useE && E.IsReady() && player.Distance(eTarget) < E.Range)
            {
                    E.Cast(eTarget);
                    if (Q.IsReady())
                    {
                        Q.Cast(eTarget);
                    }
                    return;
            }

            //W
            if (useW && W.IsReady() && player.Distance(eTarget) <= W.Range - 100 &&
                ShouldW(eTarget, source))
            {
                W.Cast();
            }

            if (source == "Harass")
            {
                if (useQ && Q.IsReady() && player.Distance(eTarget) <= Q.Range &&
                    ShouldQ(eTarget, source) && player.Distance(eTarget) > 600)
                {
                    Q.Cast(eTarget);
                    return;
                }
            }
            else if (useQ && Q.IsReady() && player.Distance(eTarget) <= Q.Range && ShouldQ(eTarget, source))
            {
                Q.Cast(eTarget);
                return;
            }

            //R
            if (useR && R.IsReady() && player.Distance(eTarget) < R.Range)
            {
                if (E.IsReady())
                {
                    if (CheckReq(rETarget))
                        E.Cast(rETarget);
                }
                if (ShouldR(eTarget) && R.IsReady())
                {
                    R.Cast(player.Position.Extend(ARAMSimulator.fromNex.Position, 250).To3D());
                    _rTimer = Core.GameTickCount - 250;
                }
                if (_rTimeLeft > 9500 && _rOn && R.IsReady())
                {
                    R.Cast(player.Position.Extend(ARAMSimulator.fromNex.Position,250).To3D());
                    _rTimer = Core.GameTickCount - 250;
                }
            }
        }

        private void CheckKs()
        {
            foreach (AIHeroClient target in ObjectManager.Get<AIHeroClient>().Where(x => x.IsValidTarget(1300) && x.IsEnemy && !x.IsDead).OrderByDescending(GetComboDamage))
            {
                if (target != null)
                {
                    if (player.Distance(target.ServerPosition) <= W.Range &&
                        (player.GetSpellDamage(target, SpellSlot.Q) + player.GetSpellDamage(target, SpellSlot.Q, DamageLibrary.SpellStages.SecondCast) +
                         player.GetSpellDamage(target, SpellSlot.W)) > target.Health && Q.IsReady() && Q.IsReady())
                    {
                        Q.Cast(target);
                        return;
                    }

                    if (player.Distance(target.ServerPosition) <= Q.Range &&
                        (player.GetSpellDamage(target, SpellSlot.Q) + player.GetSpellDamage(target, SpellSlot.Q, DamageLibrary.SpellStages.SecondCast)) >
                        target.Health && Q.IsReady())
                    {
                        Q.Cast(target);
                        return;
                    }

                    if (player.Distance(target.ServerPosition) <= E.Range &&
                        (player.GetSpellDamage(target, SpellSlot.E)) > target.Health & E.IsReady())
                    {
                        E.Cast(target);
                        return;
                    }

                    if (player.Distance(target.ServerPosition) <= W.Range &&
                        (player.GetSpellDamage(target, SpellSlot.W)) > target.Health && W.IsReady())
                    {
                        W.Cast();
                        return;
                    }

                    Vector3 dashVector = player.Position +
                                         Vector3.Normalize(target.ServerPosition - player.Position) * 425;
                    if (player.Distance(target.ServerPosition) <= R.Range &&
                        (player.GetSpellDamage(target, SpellSlot.R)) > target.Health && R.IsReady() && _rOn &&
                        target.Distance(dashVector) < 425 && R.IsReady())
                    {
                        R.Cast(dashVector);
                    }
                }
            }
        }

        private bool ShouldQ(Obj_AI_Base target, string source)
        {
            if (source == "Combo")
            {
                if ((player.GetSpellDamage(target, SpellSlot.Q) + player.GetSpellDamage(target, SpellSlot.Q, DamageLibrary.SpellStages.SecondCast)) >
                    target.Health)
                    return true;

                if (_rOn)
                    return true;


                if (target.HasBuffOfType(BuffType.Charm) || target.HasBuffOfType(BuffType.Taunt))
                    return true;
            }

            if (source == "Harass")
            {
                if ((player.GetSpellDamage(target, SpellSlot.Q) + player.GetSpellDamage(target, SpellSlot.Q, DamageLibrary.SpellStages.SecondCast)) >
                    target.Health)
                    return true;

                if (_rOn)
                    return true;


                if (target.HasBuffOfType(BuffType.Charm) || target.HasBuffOfType(BuffType.Taunt))
                    return true;
            }

            return false;
        }

        private bool ShouldW(Obj_AI_Base target, string source)
        {
            if (source == "Combo")
            {
                if (player.GetSpellDamage(target, SpellSlot.W) > target.Health)
                    return true;

                if (_rOn)
                    return true;


                if (target.HasBuffOfType(BuffType.Charm) || target.HasBuffOfType(BuffType.Taunt))
                    return true;
            }
            if (source == "Harass")
            {
                if (player.GetSpellDamage(target, SpellSlot.W) > target.Health)
                    return true;

                if (_rOn)
                    return true;


                if (target.HasBuffOfType(BuffType.Charm) || target.HasBuffOfType(BuffType.Taunt))
                    return true;
            }

            return false;
        }

        private bool ShouldR(Obj_AI_Base target)
        {


            if (GetComboDamage(target) > target.Health && !_rOn)
            {
                if (target.HasBuffOfType(BuffType.Charm) || target.HasBuffOfType(BuffType.Taunt))
                    return true;
            }

            if (target.HasBuffOfType(BuffType.Charm) && _rOn || target.HasBuffOfType(BuffType.Taunt))
                return true;


            if (player.GetSpellDamage(target, SpellSlot.R) * 2 > target.Health)
                return true;

            if (_rOn && _rTimeLeft > 9500)
                return true;

            return false;
        }

        private bool CheckReq(Obj_AI_Base target)
        {

            if (GetComboDamage(target) > target.Health && !_rOn && target.CountEnemiesInRange(500)<3)
            {
                if (E.IsReady())
                {
                    //Game.PrintChat("added delay: " + addedDelay);

                        return true;
                }
            }

            return false;
        }

        private bool IsRActive()
        {
            return player.HasBuff("AhriTumble");
        }

        private int RCount()
        {
            var buff = player.Buffs.FirstOrDefault(x => x.Name == "AhriTumble");
            if (buff != null)
                return buff.Count;
            return 0;
        }

        private void Farm()
        {
            if (player.ManaPercent < 60) return;
            var allMinionsQ = EnemyMinions.Where(x => x.IsInRange(Player.Instance.ServerPosition, Q.Range));
            var allMinionsW = EnemyMinions.Where(x => x.IsInRange(Player.Instance.ServerPosition, W.Range));
            
            if (Q.IsReady())
            {
                FarmLocation qPos = Q.GetLineFarmLocation(allMinionsQ, Q.Width());
                if (qPos.HitNumber >= 3)
                {
                    Q.Cast(qPos.CastPosition);
                }
            }

            if (allMinionsW.Count() > 0 && W.IsReady())
                W.Cast();
        }
    }
}
