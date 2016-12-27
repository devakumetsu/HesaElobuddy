using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SharpDX;
using EloBuddy;
using EloBuddy.SDK;
using EloBuddy.SDK.Enumerations;
using EloBuddy.SDK.Events;
using static EloBuddy.SDK.Spell;

namespace ARAMDetFull.Champions
{
    class Draven : Champion
    {

        private List<PossibleReticle> Axes = new List<PossibleReticle>();
        public Draven()
        {//have to fix done
            GameObject.OnCreate += GameObject_OnCreate;
            GameObject.OnDelete += GameObject_OnDelete;
            Interrupter.OnInterruptableSpell += Interrupter_OnPossibleToInterrupt;
            Gapcloser.OnGapcloser += AntiGapcloser_OnEnemyGapcloser;
            //AIHeroClientDeath.

            ARAMSimulator.champBuild = new Build
            {
                coreItems = new List<ConditionalItem>
                        {
                            new ConditionalItem(ItemId.Youmuus_Ghostblade),
                            new ConditionalItem(ItemId.Berserkers_Greaves),
                            new ConditionalItem(ItemId.Infinity_Edge),
                            new ConditionalItem(ItemId.Phantom_Dancer),
                            new ConditionalItem(ItemId.Mercurial_Scimitar, ItemId.The_Bloodthirster, ItemCondition.ENEMY_AP),
                            new ConditionalItem(ItemId.Last_Whisper),
                        },
                startingItems = new List<ItemId>
                        {
                            ItemId.Vampiric_Scepter,ItemId.Boots_of_Speed
                        }
            };
        }

        void DeathWalker_AfterAttack(AttackableUnit unit, AttackableUnit target)
        {
            if (!(target is AIHeroClient)) return;
            if (unit.IsMe && target.IsValidTarget())
            {
                bool useW;
                var axe = getClosestAxe(out useW);

                if (axe != null)
                    Player.IssueOrder(GameObjectOrder.MoveTo, axe.Position);
                else if (safeGap(target.Position.To2D()))
                    Player.IssueOrder(GameObjectOrder.MoveTo, target.Position);
                else
                    Player.IssueOrder(GameObjectOrder.MoveTo, player.Position.Extend(ARAMSimulator.fromNex.Position, 450).To3D());

                CastW(target);
                //castItems((AIHeroClient)target);
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
            W.Cast(target);
        }

        public override void useE(Obj_AI_Base target)
        {
            if (!E.IsReady())
                return;
            if (EnemyInRange(1, 300))
                E.Cast(player.Position.Extend(ARAMSimulator.fromNex.Position, 400).To3D());

        }

        public override void useR(Obj_AI_Base target)
        {
            if (!R.IsReady())
                return;
            R.CastIfWillHit(target, 2);
        }

        public override void setUpSpells()
        {
            Q = new Spell.Active(SpellSlot.Q);
            W = new Spell.Active(SpellSlot.W);
            E = new Spell.Skillshot(SpellSlot.E, 1100, SkillShotType.Linear, 250, 1400, 130);
            R = new Spell.Skillshot(SpellSlot.R, 4000, SkillShotType.Linear, 400, 2000, 160);
        }

        public override void alwaysCheck()
        {
            if (Axes.Count == 0)
            {
                Orbwalker.DisableMovement = false;
            }
        }

        public override void useSpells()
        {
            var target = ARAMTargetSelector.getBestTarget(Player.Instance.GetAutoAttackRange());
            var Etarget = ARAMTargetSelector.getBestTarget(E.Range);
            var RTarget = ARAMTargetSelector.getBestTarget(2000f);
            CatchAxes();

            if (target.IsValidTarget()) CastQ();
            if (Etarget.IsValidTarget()) CastE(Etarget);
            if (RTarget.IsValidTarget()) CastRExecute(RTarget);
            if (RTarget.IsValidTarget()) { RExecute(RTarget); }
            if (RTarget.IsValidTarget()) { RMostDamange(RTarget); }

        }

        public bool EnemyInRange(int numOfEnemy, float range)
        {
            return player.CountEnemiesInRange(range) >= numOfEnemy;
        }

        void AntiGapcloser_OnEnemyGapcloser(Obj_AI_Base unit, EventArgs gapcloser)
        {
            var GPSender = (AIHeroClient)unit;
            if (GPSender == null) return;
            if (!E.IsReady() || !GPSender.IsValidTarget()) return;
            CastEHitchance(GPSender);

        }

        void Interrupter_OnPossibleToInterrupt(Obj_AI_Base unit, EventArgs spell)
        {
            var Sender = (AIHeroClient)unit;
            if (!E.IsReady() || !Sender.IsValidTarget()) return;
            CastEHitchance(Sender);
        }

        void GameObject_OnDelete(GameObject sender, EventArgs args)
        {
            if (!sender.Name.Contains("Draven_Base_Q_reticle_self.troy"))
            {
                return;
            }
            Axes.RemoveAll(ax => ax.networkID == sender.NetworkId);

        }

        void GameObject_OnCreate(GameObject sender, EventArgs args)
        {
            if (!sender.Name.Contains("Draven_Base_Q_reticle_self.troy"))
            {
                return;
            }
            Axes.Add(new PossibleReticle(sender));
        }

        public PossibleReticle getClosestAxe(out bool useW)
        {

            if (Axes.Count <= 0)
            {
                useW = false;
                return null;
            }
            var CatchRange = 560;
            var UseW = false;

            bool ShouldUseW;

            var Axe = Axes.Where(
                    axe =>
                        axe.AxeGameObject.IsValid && axe.Position.Distance(player.Position) <= CatchRange).OrderBy(axe => axe.Distance())
                        .FirstOrDefault();
            if (Axe == null)
            {
                useW = false;
                return null;
            }
            if (Axe.canCatch(UseW, out ShouldUseW))
            {
                useW = ShouldUseW;
                return Axe;
            }
            useW = false;
            return null;
        }

        public void CatchAxes()
        {
            bool shouldUseWForIt;
            if (Axes.Count == 0)
            {
                Orbwalker.DisableAttacking = false;
                return;
            }
            if (ARAMSimulator.inDanger)
                return;
            var Axe = getClosestAxe(out shouldUseWForIt);

            if (Axe == null)
            {
                Orbwalker.DisableAttacking = false;
                return;
            }
            //  if (shouldUseWForIt) { DeathWalker.setAttack(false); } else { DeathWalker.setAttack(true);}
            Catch(shouldUseWForIt, Axe);

        }

        public void CastEHitchance(AIHeroClient target)
        {
            var Pred = E.GetPrediction(target);
            if (Pred.HitChance >= HitChance.Medium)
            {
                E.Cast(target);
            }
        }
        public void Catch(bool shouldUseWForIt, PossibleReticle Axe)
        {
            if (shouldUseWForIt && W.IsReady() && !Axe.isCatchingNow()) W.Cast();
            Orbwalker.OrbwalkTo(Axe.Position);
        }
        public void CastQ()
        {
            var ManaQCombo = 0;
            var QMax = 2;
            if (getPerValue(true) >= ManaQCombo && GetQStacks() + 1 <= QMax) Q.Cast();

        }
        private void CastW(AttackableUnit target)
        {
            if (hasWBuff() || !W.IsReady()) return;

            var MWC = 0;
            if (getPerValue(true) >= MWC)
            {
                Aggresivity.addAgresiveMove(new AgresiveMove(50, 3000, true));
                W.Cast();
            }

        }

        private void CastE(AIHeroClient target)
        {
            if (!E.IsReady() || !target.IsValidTarget()) return;
            CastEHitchance(target);
        }

        private void CastRExecute(AIHeroClient RTarget)
        {
            var Pred = R.GetPrediction(RTarget);
            if (!RTarget.IsValidTarget() || Pred.HitChance < HitChance.Medium || !R.IsReady()) return;
            var ManaR = 0;
            if (getUnitsInPath(player, RTarget, R) && getPerValue(true) >= ManaR && !player.HasBuff("dravenrdoublecast"))
            {
                R.Cast(RTarget);
            }

        }

        private void RExecute(AIHeroClient RTarget)
        {
            var Pred = R.GetPrediction(RTarget);
            if (!RTarget.IsValidTarget() || Pred.HitChance < HitChance.Medium || !R.IsReady()) return;
            if (getUnitsInPath(player, RTarget, R) && !player.HasBuff("dravenrdoublecast"))
            {
                R.Cast(RTarget);
            }
        }

        private void RMostDamange(AIHeroClient RTarget)
        {
            var Pred = R.GetPrediction(RTarget);
            if (!RTarget.IsValidTarget() || Pred.HitChance < HitChance.Medium || !R.IsReady()) return;

            if (!player.HasBuff("dravenrdoublecast"))
            {
                R.CastIfWillHit(RTarget, 3);
            }
        }

        void castItems(AIHeroClient tar)
        {
            if (tar == null)
                return;
            UseItem(3153, tar);
            UseItem(3153, tar);
            UseItem(3142);
            UseItem(3142);
            UseItem(3144, tar);
            UseItem(3144, tar);
        }
        private bool hasWBuff()
        {
            //dravenfurybuff
            //DravenFury
            return player.HasBuff("DravenFury") || player.HasBuff("dravenfurybuff");
        }
        public bool minionThere()
        {//TODO hi hesa
            var List = EntityManager.MinionsAndMonsters.GetLaneMinions(EntityManager.UnitTeam.Enemy, player.Position/*, DeathWalker.getTargetSearchDist()*/)
                .Where(m => Prediction.Health.GetPrediction(m,
                    (int)(player.Distance(m) / Orbwalker.GetMyProjectileSpeed()) * 1000) <=
                            Q.GetDamage(m) + player.GetAutoAttackDamage(m)
                        ).ToList();
            // Game.PrintChat("QDmg "+Q.GetDamage(List.FirstOrDefault()));
            return List.Count() >= 0;
        }
        public Vector3 PosAfterRange(Vector3 p1, Vector3 finalp2, float range)
        {
            var Pos2 = Vector3.Normalize(finalp2 - p1);
            return p1 + (Pos2 * range);
        }
        public int GetQStacks()
        {
            var buff = player.Buffs.FirstOrDefault(buff1 => buff1.Name.Equals("dravenspinningattack"));
            return buff != null ? buff.Count : 0;
        }

        float getPerValue(bool mana)
        {
            if (mana) return (player.Mana / player.MaxMana) * 100;
            return (player.Health / player.MaxHealth) * 100;
        }
        float getPerValueTarget(AIHeroClient target, bool mana)
        {
            if (mana) return (target.Mana / target.MaxMana) * 100;
            return (target.Health / target.MaxHealth) * 100;
        }
        public void UseItem(int id, AIHeroClient target = null)
        {
            if (Item.HasItem(id) && Item.CanUseItem(id))
            {
                Item.UseItem(id, target);
            }
        }
        public static bool isUnderEnTurret(Vector3 Position)
        {
            foreach (var tur in ObjectManager.Get<Obj_AI_Turret>().Where(turr => turr.IsEnemy && (turr.Health != 0)))
            {
                if (tur.Distance(Position) <= 975f) return true;
            }
            return false;
        }
        private bool getUnitsInPath(AIHeroClient player, AIHeroClient target, SpellBase spell)
        {
            float distance = player.Distance(target);
            var minionList = EntityManager.MinionsAndMonsters.GetLaneMinions(EntityManager.UnitTeam.Enemy, ObjectManager.Player.ServerPosition, spell.Range);
            int numberOfMinions = (from Obj_AI_Minion minion in minionList
                                   let skillshotPosition =
                                       V2E(player.Position,
                                           V2E(player.Position, target.Position,
                                               Vector3.Distance(player.Position, target.Position) - spell.Width() + 1).To3D(),
                                           Vector3.Distance(player.Position, minion.Position))
                                   where skillshotPosition.Distance(minion) <= spell.Width()
                                   select minion).Count();
            int numberOfChamps = (from minion in ObjectManager.Get<AIHeroClient>()
                                  let skillshotPosition =
                                      V2E(player.Position,
                                          V2E(player.Position, target.Position,
                                              Vector3.Distance(player.Position, target.Position) - spell.Width() + 1).To3D(),
                                          Vector3.Distance(player.Position, minion.Position))
                                  where skillshotPosition.Distance(minion) < spell.Width() && minion.IsEnemy
                                  select minion).Count();
            int totalUnits = numberOfChamps + numberOfMinions - 1;
            // total number of champions and minions the projectile will pass through.
            if (totalUnits == -1) return false;
            double damageReduction = 0;
            damageReduction = ((totalUnits > 7)) ? 0.4 : (totalUnits == 0) ? 1.0 : (1 - ((totalUnits) / 12.5));
            // the damage reduction calculations minus percentage for each unit it passes through!
            return spell.GetDamage(target) * damageReduction >= (target.Health + (distance / 2000) * target.HPRegenRate);
            // - 15 is a safeguard for certain kill.
        }
        private Vector2 V2E(Vector3 from, Vector3 direction, float distance)
        {
            return from.To2D() + distance * Vector3.Normalize(direction - from).To2D();
        }

        internal class PossibleReticle
        {
            public GameObject AxeGameObject;
            public int networkID;
            public Vector3 Position;
            public int CreationTime;
            public int EndTime;

            public PossibleReticle(GameObject Axe)
            {
                AxeGameObject = Axe;
                networkID = Axe.NetworkId;
                Position = Axe.Position;
                CreationTime = ARAMDetFull.now;
                EndTime = ARAMDetFull.now + 2000;
            }

            public bool canCatch(bool UseW, out bool ShouldUseW)
            {
                var EnemyHeroesCount =
                    ObjectManager.Get<AIHeroClient>()
                        .Where(h => h.IsEnemy && h.IsValidTarget() && h.Distance(Position) <= 350).ToList();
                if ((isUnderEnTurret(Position) && !isUnderEnTurret(player.Position)) || EnemyHeroesCount.Count > 1)
                {
                    ShouldUseW = false;
                    return false;
                }//TODO
                W = new Spell(SpellSlot.W);
                var distance = player.GetPath(Position).ToList().To2D().PathLength() - 50;
                var catchNormal = (distance * 1000) / player.MoveSpeed + ARAMDetFull.now < EndTime; // Not buffed with W, Normal
                var AdditionalSpeed = (5 * W.Level + 35) * 0.01 * player.MoveSpeed;
                var catchBuff = distance / (player.MoveSpeed + AdditionalSpeed) + ARAMDetFull.now < EndTime; //Buffed with W
                if (catchNormal)
                {
                    ShouldUseW = false;
                    return catchNormal;
                }
                if (UseW && !catchNormal && catchBuff)
                {
                    ShouldUseW = true;
                    return catchBuff;
                }
                ShouldUseW = false;
                return false;
            }
            public float Distance()
            {
                return Vector3.Distance(Position, player.Position);
            }

            public bool isCatchingNow()
            {
                return Distance() < player.BoundingRadius; //Taken from PUC Draven
            }
        }

    }
}