using System;
using System.Collections.Generic;
using System.Linq;
using EloBuddy;
using EloBuddy.SDK;
using EloBuddy.SDK.Enumerations;
using EloBuddy.SDK.Events;
using SharpDX;

namespace ARAMDetFull.Champions
{
    class Riven : Champion
    {

        public static bool RushDown = false;

        public static bool RushDownQ = false;

        public static bool forceQ = false;

        public Riven()
        {
            ARAMSimulator.champBuild = new Build
            {
                coreItems = new List<ConditionalItem>
                {
                    new ConditionalItem(ItemId.Mercurys_Treads,ItemId.Ninja_Tabi,ItemCondition.ENEMY_AP),
                    new ConditionalItem(ItemId.Ravenous_Hydra_Melee_Only),
                    new ConditionalItem(ItemId.The_Black_Cleaver),
                    new ConditionalItem(ItemId.Youmuus_Ghostblade),
                    new ConditionalItem(ItemId.Maw_of_Malmortius),
                    new ConditionalItem(ItemId.Banshees_Veil,ItemId.Randuins_Omen,ItemCondition.ENEMY_AP)
                },
                startingItems = new List<ItemId>
                {
                    ItemId.Tiamat_Melee_Only
                }
            };
            Orbwalker.OnPostAttack += DeathWalkerOnAfterAttack;
            Obj_AI_Base.OnPlayAnimation += OnPlayAnimation;
            Obj_AI_Base.OnNewPath += OnNewPath;
        }

        private void OnPlayAnimation(GameObject sender, GameObjectPlayAnimationEventArgs args)
        {
            if (sender.IsMe && args.Animation != "Run")
            {

                if (args.Animation.Contains("pell"))
                    Core.DelayAction(() =>
                    {
                        cancelAnim(args.Animation.Contains("Spell1"));
                    }, Game.Ping);

                var targ = (Obj_AI_Base)Orbwalker.GetTarget();
                if (targ != null && targ is Obj_AI_Base)
                {
                    if (args.Animation == "Spell3" && R.IsReady())
                    {
                        UseRSmart(targ);
                        Core.DelayAction(() =>
                        {
                            UseRSmart(targ);
                        }, 10);
                    }

                    if (sender.IsMe && args.Animation == "Spell3" &&
                        Q.IsReady())
                    {
                        Console.WriteLine("force W");
                        Core.DelayAction(() =>
                        {
                            useWSmart(targ);
                        }, 30);
                        //Q.Cast(targ.Position);
                        //forceQ = true;
                        //timer = new System.Threading.Timer(obj => { Player.IssueOrder(GameObjectOrder.MoveTo, difPos()); }, null, (long)100, System.Threading.Timeout.Infinite);
                    }

                    if (sender.IsMe && args.Animation == "Spell2" &&
                        Q.IsReady())
                    {
                        Core.DelayAction(() =>
                        {
                            Q.Cast(targ.Position);
                        }, 30);
                        Aggresivity.addAgresiveMove(new AgresiveMove(30, 3000));
                        //Console.WriteLine("force q");

                        // Riven.forceQ = true;
                        // Riven.timer = new System.Threading.Timer(obj => { Riven.Player.IssueOrder(GameObjectOrder.MoveTo, Riven.difPos()); }, null, (long)100, System.Threading.Timeout.Infinite);
                    }


                    // useHydra(Obj_AI_Base target)

                }
            }
        }
        public bool resetAaonNewPath = false;

        public Spell.Skillshot R2 { get; private set; }

        private void OnNewPath(Obj_AI_Base sender, GameObjectNewPathEventArgs args)
        {
            if (sender.IsMe && !args.IsDash)
            {

                if (resetAaonNewPath)
                {
                    resetAaonNewPath = false;
                    Orbwalker.ResetAutoAttack();//.resetAutoAttackTimer();
                }
            }
        }

        public void cancelAnim(bool aaToo = false)
        {
            if (aaToo)
            {
                resetAaonNewPath = true;
            }
            //todo not sure if Orbwalker.GetTarget is right
            Chat.Say("/d");
            Player.IssueOrder(GameObjectOrder.MoveTo, Game.CursorPos);
            if (Orbwalker.GetTarget() != null)
            {
                if (W.IsReady())
                    useWSmart((Obj_AI_Base)Orbwalker.GetTarget());

            }


            //  Packet.C2S.Cast.Encoded(new Packet.C2S.Cast.Struct(fill iterator up)).Send();
        }

        private void DeathWalkerOnAfterAttack(AttackableUnit target, EventArgs args)
        {
            if (target is AIHeroClient)
            {
                Q.Cast(target.Position);
                Aggresivity.addAgresiveMove(new AgresiveMove(30, 3000));
            }
        }

        public override void useQ(Obj_AI_Base target)
        {
            if (Q.IsReady()) return;
            //Q.Cast();
        }

        public override void useW(Obj_AI_Base target)
        {
            if (W.IsReady()) return;
            //W.Cast();
        }

        public override void useE(Obj_AI_Base target)
        {
            if (!E.IsReady())
                return;



            float trueAARange = player.AttackRange + target.BoundingRadius;
            float trueERange = target.BoundingRadius + E.Range;



            float dist = player.Distance(target);

            var path = player.GetPath(target.Position);
            if (!target.IsMoving && dist < trueERange)
            {
                Player.IssueOrder(GameObjectOrder.MoveTo, target.Position);
                E.Cast(path.Count() > 1 ? path[1] : target.ServerPosition);
            }
            if ((dist > trueAARange && dist < trueERange) || RushDown)
            {

                E.Cast(path.Count() > 1 ? path[1] : target.ServerPosition);
            }
        }


        public override void useR(Obj_AI_Base target)
        {
            if (R.IsReady())
            {
                if (player.CountEnemiesInRange(R2.Range) <= 2)
                {
                    R.Cast();
                    R2.Cast(target);
                }
            }
        }

        public override void farm()
        {
            Obj_AI_Base target = (Obj_AI_Base)Orbwalker.GetTarget();
            if (target is Obj_AI_Minion && W.IsKillable(target))
            {
                useWSmart(target);
            }
        }

        public override void useSpells()
        {
            //var tar = ARAMTargetSelector.getBestTarget(getRivenReach() + 430);
            //doCombo(tar);
            var tar = ARAMTargetSelector.getBestTarget(Q.Range);
            useQ(tar);
            tar = ARAMTargetSelector.getBestTarget(W.Range);
            //useW(tar);
            tar = ARAMTargetSelector.getBestTarget(E.Range);
            useE(tar);
            tar = ARAMTargetSelector.getBestTarget(R2.Range);
            useR(tar);
        }

        public override void setUpSpells()
        {
            //Create the spells
            Q = new Spell.Skillshot(SpellSlot.Q, 275, SkillShotType.Circular, 250, 2200, 100);
            W = new Spell.Active(SpellSlot.W, 250);
            E = new Spell.Skillshot(SpellSlot.E, 310, SkillShotType.Linear);
            R = new Spell.Active(SpellSlot.R);
            R2 = new Spell.Skillshot(SpellSlot.R, 900, SkillShotType.Cone, 250, 1600, 125);
        }

        public float getRivenReach()
        {
            int Qtimes = getQJumpCount();
            return player.AttackRange + Qtimes * 200 + (E.IsReady() ? 390 : 0);
        }

        public void doCombo(Obj_AI_Base target)
        {
            if (target == null)
                return;

            RushDownQ = RushDmgBasedOnDist(target) * 0.7f > target.Health;
            RushDown = RushDmgBasedOnDist(target) * 1.1f > target.Health;
            if (RushDown || player.CountEnemiesInRange(600) > 2)
                UseRSmart(target);
            if (RushDown || safeGap(target))
                UseESmart(target);
            useWSmart(target);

            if (Orbwalker.CanMove && (target.Distance(player) < 700 || RushDown)) ;
            gapWithQ(target);
        }

        public void gapWithQ(Obj_AI_Base target)
        {
            if ((E.IsReady() || !Q.IsReady()) && !RushDownQ || player.IsDashing())
                return;
            reachWithQ(target);
        }

        public void reachWithQ(Obj_AI_Base target)
        {
            if (!Q.IsReady() || player.IsDashing())
                return;

            float trueAARange = player.AttackRange + target.BoundingRadius + 20;
            float trueQRange = target.BoundingRadius + Q.Range + 30;

            float dist = player.Distance(target);
            Vector2 walkPos = new Vector2();
            if (target.IsMoving && target.Path.Count() != 0)
            {
                Vector2 tpos = target.Position.To2D();
                Vector2 path = target.Path[0].To2D() - tpos;
                path.Normalize();
                walkPos = tpos + (path * 100);
            }
            float targ_ms = (target.IsMoving && player.Distance(walkPos) > dist) ? target.MoveSpeed : 0;
            float msDif = (player.MoveSpeed - targ_ms) == 0 ? 0.0001f : (player.MoveSpeed - targ_ms);
            float timeToReach = (dist - trueAARange) / msDif;
            if ((dist > trueAARange && dist < trueQRange) || RushDown)
            {
                if (timeToReach > 2.5 || timeToReach < 0.0f || RushDown)
                {
                    Vector2 to = player.Position.To2D().Extend(target.Position.To2D(), 50);
                    // Player.IssueOrder(GameObjectOrder.MoveTo,to.To3D());
                    Q.Cast(target.ServerPosition);
                    Aggresivity.addAgresiveMove(new AgresiveMove(30, 3000));
                }
            }
        }

        public void useWSmart(Obj_AI_Base target, bool aaRange = false, bool rrAa = false)
        {
            if (!W.IsReady())
                return;
            float range = 0;
            if (aaRange)
                range = player.AttackRange + target.BoundingRadius;
            else
                range = W.Range + target.BoundingRadius - 40;
            if (W.IsReady() && target.Distance(player.ServerPosition) < range)
            {
                W.Cast();
                //Utility.DelayAction.Add(50, delegate { DeathWalker.resetAutoAttackTimer(true); });
            }
        }

        public void UseESmart(Obj_AI_Base target)
        {
            if (!E.IsReady())
                return;



            float trueAARange = player.AttackRange + target.BoundingRadius;
            float trueERange = target.BoundingRadius + E.Range;



            float dist = player.Distance(target);

            var path = player.GetPath(target.Position);
            if (!target.IsMoving && dist < trueERange)
            {
                Player.IssueOrder(GameObjectOrder.MoveTo, target.Position);
                E.Cast(path.Count() > 1 ? path[1] : target.ServerPosition);
            }
            if ((dist > trueAARange && dist < trueERange) || RushDown)
            {

                E.Cast(path.Count() > 1 ? path[1] : target.ServerPosition);
            }
        }

        public void UseRSmart(Obj_AI_Base target, bool rrAA = false)
        {
            if (!R.IsReady())
                return;
            if (!UltIsOn() && !E.IsReady() && target.Distance(player.ServerPosition) < (Q.Range + target.BoundingRadius))
            {
                R.Cast();
                Aggresivity.addAgresiveMove(new AgresiveMove(150, 8000));
                if (rrAA)
                {
                    Player.IssueOrder(GameObjectOrder.MoveTo, target.Position);
                    //Utility.DelayAction.Add(50, delegate { DeathWalker.resetAutoAttackTimer(true); } );
                }
            }
            else if (canUseWindSlash() && target is AIHeroClient && (!(E.IsReady() && player.IsDashing()) || player.Distance(target) > 150))
            {
                var targ = target as AIHeroClient;
                var po = R.GetPrediction(targ);
                if (GetTrueQDmOn(targ) > ((targ.Health)) || RushDown)
                {
                    if (po.HitChance > HitChance.Medium && player.Distance(po.CastPosition) > 30)
                    {
                        R.Cast(player.Distance(po.CastPosition) < 150 ? target.Position : po.CastPosition);
                    }
                }
            }
        }

        public float GetTrueQDmOn(Obj_AI_Base target)
        {
            return player.CalculateDamageOnUnit(target, DamageType.Physical, -10f + Q.Level * 20f + (0.35f + (Q.Level * 0.05f)) * (player.FlatPhysicalDamageMod + player.BaseAttackDamage));
        }

        public float RushDmgBasedOnDist(Obj_AI_Base target)
        {
            float multi = 1.0f;
            if (!UltIsOn() && R.IsReady())
                multi = 1.2f;
            float Qdmg = GetTrueQDmOn(target);
            float Wdmg = (E.IsReady()) ? (float)player.GetSpellDamage(target, SpellSlot.W) : 0;
            float ADdmg = (float)player.GetAutoAttackDamage(target);
            float Rdmg = (R.IsReady() && (canUseWindSlash() || !UltIsOn())) ? GetTrueQDmOn(target) : 0;

            float trueAARange = player.AttackRange + target.BoundingRadius - 15;
            float dist = player.Distance(target.ServerPosition);
            float Ecan = (E.IsReady()) ? E.Range : 0;
            int Qtimes = getQJumpCount();
            int ADtimes = 0;

            if (E.IsReady())
                ADtimes++;
            
            dist -= Ecan;
            dist -= trueAARange;
            while (dist > 0 && Qtimes > 0)
            {
                dist -= player.AttackRange + 50;
                Qtimes--;
            }
            if (dist < 0)
                ADtimes++;
            return (Qdmg * Qtimes + Wdmg + ADdmg * ADtimes + Rdmg) * multi;
        }

        public float GetTrueQDmOn(Obj_AI_Base target, float minus = 0)
        {
            float baseDmg = 40 + 40 * R.Level + 0.6f * player.FlatPhysicalDamageMod;
            float eneMissHpProc = ((((target.MaxHealth - target.Health - minus) / target.MaxHealth) * 100f) > 75f) ? 75f : (((target.MaxHealth - target.Health) / target.MaxHealth) * 100f);

            float multiplier = 1 + (eneMissHpProc * 2.66f) / 100;

            return (float)player.CalculateDamageOnUnit(target, DamageType.Physical, baseDmg * multiplier);
        }

        public bool UltIsOn()
        {
            foreach (var buf in player.Buffs)
            {
                if (buf.Name == "RivenFengShuiEngine")
                {
                    return true;
                }
            }
            return false;
        }

        public bool canUseWindSlash()
        {
            foreach (var buf in player.Buffs)
            {
                if (buf.Name == "rivenwindslashready")
                {
                    return true;
                }
            }
            return false;
        }

        public int getQJumpCount()
        {
            try
            {
                var buff = player.Buffs.First(buf => buf.Name == "RivenTriCleave");

                return 3 - buff.Count;
            }
            catch (Exception ex)
            {
                if (!Q.IsReady())
                    return 0;
                return 3;
            }
        }
    }
}