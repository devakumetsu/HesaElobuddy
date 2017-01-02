using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EloBuddy;
using EloBuddy.SDK;
using EloBuddy.SDK.Enumerations;
using EloBuddy.SDK.Events;
using SharpDX;

namespace ARAMDetFull.Champions
{
    class Syndra : Champion
    {

        public static Spell.Skillshot Q;
        public static Spell.Skillshot W;
        public static Spell.Skillshot E;
        public static Spell.Targeted R;
        public static Spell.Skillshot EQ;

        private static double QEComboT;
        private static double WEComboT;

        public Syndra()
        {
            Obj_AI_Base.OnProcessSpellCast += AIHeroClient_OnProcessSpellCast;
            Interrupter.OnInterruptableSpell += Interrupter2_OnInterruptableTarget;

            ARAMSimulator.champBuild = new Build
            {
                coreItems = new List<ConditionalItem>
                        {
                            new ConditionalItem(ItemId.Rod_of_Ages),
                            new ConditionalItem(ItemId.Sorcerers_Shoes),
                            new ConditionalItem(ItemId.Liandrys_Torment),
                            new ConditionalItem(ItemId.Rabadons_Deathcap),
                            new ConditionalItem(ItemId.Void_Staff),
                            new ConditionalItem(ItemId.Banshees_Veil,ItemId.Zhonyas_Hourglass,ItemCondition.ENEMY_AP),
                        },
                startingItems = new List<ItemId>
                        {
                            ItemId.Catalyst_of_Aeons
                        }
            };
        }

        void Interrupter2_OnInterruptableTarget(Obj_AI_Base sender, Interrupter.InterruptableSpellEventArgs args)
        {
            if (player.Distance(sender) < E.Range && E.IsReady())
            {
                Q.Cast(sender.ServerPosition);
                E.Cast(sender.ServerPosition);
            }
            else if (player.Distance(sender) < EQ.Range && E.IsReady() && Q.IsReady())
            {
                UseQE(sender);
            }
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
                return;
            if (player.HealthPercent < 30)
                W.Cast();
        }

        public override void useE(Obj_AI_Base target)
        {
            if (!E.IsReady() || target == null)
                return;
            if (safeGap(target))
                E.Cast(target);
        }


        public override void useR(Obj_AI_Base target)
        {
            if (!R.IsReady() || target == null)
                return;
            if (player.CountEnemiesInRange(250) > 1)
            {
                R.Cast();
            }
        }

        public override void useSpells()
        {
            var tar = ARAMTargetSelector.getBestTarget(W.Range+250);
            if(tar != null)
                useSyndraSpells(true, true, true, true, true, true, false);
            else if(player.ManaPercent>40)
                Farm(false);
            //other
            /*var tar = ARAMTargetSelector.getBestTarget(Q.Range);
            if (tar != null) useQ(tar);
            tar = ARAMTargetSelector.getBestTarget(W.Range);
            if (tar != null) useW(tar);
            tar = ARAMTargetSelector.getBestTarget(E.Range);
            if (tar != null) UseE(tar);
            tar = ARAMTargetSelector.getBestTarget(R.Range);
            if (tar != null) useR(tar);*/

        }

        public override void setUpSpells()
        {
            //Create the spells
            Q = new Spell.Skillshot(SpellSlot.Q, 820, SkillShotType.Circular, 550, int.MaxValue, 125);
            W = new Spell.Skillshot(SpellSlot.W, 950, SkillShotType.Circular, 350, 1500, 130);
            E = new Spell.Skillshot(SpellSlot.E, 670, SkillShotType.Cone, 250, 2500, 50);
            R = new Spell.Targeted(SpellSlot.R, 675);

            EQ = new Spell.Skillshot(SpellSlot.E, 1150, SkillShotType.Linear, 600, 2400, 18);
        }






        private void UseE(Obj_AI_Base enemy)
        {
            foreach (var orb in OrbManager.GetOrbs(true))
                if (player.Distance(orb) < E.Range + 100)
                {
                    var startPoint = orb.To2D().Extend(player.ServerPosition.To2D(), 100);
                    var endPoint = player.ServerPosition.To2D()
                        .Extend(orb.To2D(), player.Distance(orb) > 200 ? 1300 : 1000);
                    //EQ.CastDelay = 670 + Player.Instance.Distance(orb) / E.Speed;
                    //EQ.From = orb;
                    var enemyPred = EQ.GetPrediction(enemy);
                    if (enemyPred.HitChance >= HitChance.High &&
                        enemyPred.UnitPosition.To2D().Distance(startPoint, endPoint, false) <
                        EQ.Width + enemy.BoundingRadius)
                    {
                        E.Cast(orb);
                        //W.LastCastFailure = ;
                        return;
                    }
                }
        }

        private void UseQE(Obj_AI_Base enemy)
        {
            EQ.CastDelay = 250 + 820 / E.Speed;
            EQ.SourcePosition = player.ServerPosition.To2D().Extend(enemy.ServerPosition.To2D(), Q.Range).To3D();

            var prediction = EQ.GetPrediction(enemy);
            if (prediction.HitChance >= HitChance.High)
            {
                Q.Cast(enemy.Position);
                //Q.Cast(player.ServerPosition.To2D().Extend(prediction.CastPosition.To2D(), Q.Range - 100));
                //QEComboT = DeathWalker.now;
                // W.LastCastAttemptT = DeathWalker.now;
            }
        }

        private Vector3 GetGrabableObjectPos(bool onlyOrbs)
        {
            if (!onlyOrbs)
                foreach (var minion in ObjectManager.Get<Obj_AI_Minion>().Where(minion => minion.IsValidTarget(W.Range))
                    )
                    return minion.ServerPosition;

            return OrbManager.GetOrbToGrab((int)W.Range);
        }

        private float GetComboDamage(Obj_AI_Base enemy)
        {
            var damage = 0d;

            if (Q.IsReady())
                damage += player.GetSpellDamage(enemy, SpellSlot.Q);

            if (W.IsReady())
                damage += player.GetSpellDamage(enemy, SpellSlot.W);

            if (E.IsReady())
                damage += player.GetSpellDamage(enemy, SpellSlot.E);


            if (R.IsReady())
                damage += Math.Min(7, player.Spellbook.GetSpell(SpellSlot.R).Ammo) * player.GetSpellDamage(enemy, SpellSlot.R);

            return (float)damage;
        }

        private void useSyndraSpells(bool useQ, bool useW, bool useE, bool useR, bool useQE, bool useIgnite, bool isHarass)
        {
            var qTarget = ARAMTargetSelector.getBestTarget(Q.Range + (isHarass ? Q.Width / 3 : Q.Width));
            var wTarget = ARAMTargetSelector.getBestTarget(W.Range + W.Width);
            var rTarget = ARAMTargetSelector.getBestTarget(R.Range);
            var qeTarget = ARAMTargetSelector.getBestTarget(EQ.Range);
            var comboDamage = rTarget != null ? GetComboDamage(rTarget) : 0;

            //Q
            if (qTarget != null && useQ)
                Q.Cast(qTarget);

            //E
            if (E.IsReady() && useE)
                foreach (var enemy in ObjectManager.Get<AIHeroClient>())
                {
                    if (enemy.IsValidTarget(EQ.Range))
                        E.Cast(enemy);
                }

            //W
            if (useW)
            {
                if (player.Spellbook.GetSpell(SpellSlot.W).ToggleState == 1 && W.IsReady() && qeTarget != null)
                {
                    var gObjectPos = GetGrabableObjectPos(wTarget == null);

                    if (gObjectPos.To2D().IsValid())
                    {
                        W.Cast(wTarget);
                        //W.LastCastFailure = Utils.TickCount;
                    }
                }
                else if (wTarget != null && player.Spellbook.GetSpell(SpellSlot.W).ToggleState != 1 && W.IsReady())
                {
                    if (OrbManager.GetOrbs(false) != null)
                    {
                        //W.From = OrbManager.WObject(false).ServerPosition;
                        W.Cast(wTarget);
                    }
                }
            }

            if (rTarget != null)
                useR = true;//Config.Item("DontUlt" + rTarget.BaseSkinName) != null &&


            //R
            if (rTarget != null && useR && R.IsReady() && !Q.IsReady())
            {
                if (comboDamage > rTarget.Health)
                {
                    R.Cast(rTarget);
                }
            }


            //QE
            if ( qeTarget != null && Q.IsReady() && E.IsReady() && useQE && player.Mana>150)
                UseQE(qeTarget);

            //WE
            if (qeTarget != null && E.IsReady() && useE)
            {
                EQ.CastDelay = 250 + 820 / W.Speed;
                //EQ.CastDelay = E.Delay + Q.Range / W.Speed;
                //EQ.From = player.ServerPosition.To2D().Extend(qeTarget.ServerPosition.To2D(), Q.Range).To3D();
                var prediction = EQ.GetPrediction(qeTarget);
                if (prediction.HitChance >= HitChance.High)
                {
                    W.Cast(qeTarget.Position);
                    //WEComboT = DeathWalker.now;
                }
            }
        }

        private void AIHeroClient_OnProcessSpellCast(Obj_AI_Base sender, GameObjectProcessSpellCastEventArgs args)
        {
            if (sender.IsMe &&
                (args.SData.Name == "SyndraQ"))
            {
                //W.LastCastAttemptT = DeathWalker.now + 400;
                E.Cast(args.End);
            }

            if (sender.IsMe &&
                (args.SData.Name == "SyndraW" || args.SData.Name == "syndrawcast"))
            {
                //W.LastCastAttemptT = DeathWalker.now + 400;
                E.Cast(args.End);
            }
        }

        private void Farm(bool laneClear)
        {
            if (Orbwalker.CanMove) return;

            var rangedMinionsQ = EntityManager.MinionsAndMonsters.GetLaneMinions(EntityManager.UnitTeam.Enemy,player.ServerPosition, Q.Range + Q.Width + 30);
            var allMinionsQ = EntityManager.MinionsAndMonsters.GetLaneMinions(EntityManager.UnitTeam.Enemy, player.ServerPosition, Q.Range + Q.Width + 30);
            var rangedMinionsW = EntityManager.MinionsAndMonsters.GetLaneMinions(EntityManager.UnitTeam.Enemy,player.ServerPosition, W.Range + W.Width + 30);
            var allMinionsW = EntityManager.MinionsAndMonsters.GetLaneMinions(EntityManager.UnitTeam.Enemy,player.ServerPosition, W.Range + W.Width + 30);


            if (Q.IsReady())
                if (laneClear)
                {
                    var fl1 = Q.GetCircularFarmLocation(rangedMinionsQ, Q.Width);
                    var fl2 = Q.GetCircularFarmLocation(allMinionsQ, Q.Width);

                    if (fl1.HitNumber >= 3)
                    {
                        Q.Cast(fl1.CastPosition);
                    }

                    else if (fl2.HitNumber >= 2)
                    {
                        Q.Cast(fl2.CastPosition);
                    }
                }
                else
                    foreach (var minion in allMinionsQ)
                        if (!Player.Instance.IsInAutoAttackRange(minion) &&
                            minion.Health < 0.75 * player.GetSpellDamage(minion, SpellSlot.Q))
                            Q.Cast(minion);

            if (W.IsReady() && W.CastIfItWillHit(3))
            {
                if (laneClear)
                {
                    if (player.Spellbook.GetSpell(SpellSlot.W).ToggleState == 1)
                    {
                        //WObject
                        var gObjectPos = GetGrabableObjectPos(false);

                        if (gObjectPos.To2D().IsValid())
                        {
                            W.Cast(gObjectPos);
                        }
                    }
                    else if (player.Spellbook.GetSpell(SpellSlot.W).ToggleState != 1)
                    {
                        var fl1 = Q.GetCircularFarmLocation(rangedMinionsW, W.Width);
                        var fl2 = Q.GetCircularFarmLocation(allMinionsW, W.Width);

                        if (fl1.HitNumber >= 3 && W.IsInRange(fl1.CastPosition))
                        {
                            W.Cast(fl1.CastPosition);
                        }

                        else if (fl2.HitNumber >= 1 && W.IsInRange(fl2.CastPosition) && fl1.HitNumber <= 2)
                        {
                            W.Cast(fl2.CastPosition);
                        }
                    }
                }
            }
        }

    }
    public static class OrbManager
    {
        private static int _wobjectnetworkid = -1;

        public static int WObjectNetworkId
        {
            get
            {
                if (ObjectManager.Player.Spellbook.GetSpell(SpellSlot.W).ToggleState == 1)
                    return -1;

                return _wobjectnetworkid;
            }
            set
            {
                _wobjectnetworkid = value;
            }
        }

        public static int tmpQOrbT;
        public static Vector3 tmpQOrbPos = new Vector3();

        public static int tmpWOrbT;
        public static Vector3 tmpWOrbPos = new Vector3();

        static OrbManager()
        {
            //Obj_AI_Base.OnPauseAnimation += Obj_AI_Base_OnPauseAnimation;
            //Obj_AI_Base.OnProcessSpellCast += AIHeroClient_OnProcessSpellCast;
        }

        /*static void Obj_AI_Base_OnPauseAnimation(Obj_AI_Base sender, Obj_AI_BasePauseAnimationEventArgs args)
        {
            if (sender is Obj_AI_Minion && sender.IsAlly)
            {
                WObjectNetworkId = sender.NetworkId;
            }
        }*/

        /*private static void AIHeroClient_OnProcessSpellCast(Obj_AI_Base sender, GameObjectProcessSpellCastEventArgs args)
        {
            if (sender.IsMe && args.SData.Name == "SyndraQ")
            {
                //tmpQOrbT = Utils.TickCount;
                tmpQOrbPos = args.End;
            }

            if (sender.IsMe && WObject(true) != null && (args.SData.Name == "SyndraW" || args.SData.Name == "syndraw2"))
            {
                //tmpWOrbT = GameTick + 250;
                tmpWOrbPos = args.End;
            }
        }*/

        //public static Obj_AI_Minion WObject(bool onlyOrb)
        
            //if (WObjectNetworkId == -1) return null;
            //var obj = ObjectManager.GetUnitByNetworkId<Obj_AI_Base>(WObjectNetworkId);
            //if (obj != null && obj.IsValid<Obj_AI_Minion>() && (obj.Name == "Seed" && onlyOrb || !onlyOrb)) return (Obj_AI_Minion)obj;
            //return null;
        

        public static List<Vector3> GetOrbs(bool toGrab = false)
        {
            var result = new List<Vector3>();
            foreach (
                var obj in
                    ObjectManager.Get<Obj_AI_Minion>()
                        .Where(obj => obj.IsValid && obj.Team == ObjectManager.Player.Team && obj.Name == "Seed"))
            {

                var valid = false;
                if (obj.NetworkId != WObjectNetworkId)
                    if (
                        ObjectManager.Get<GameObject>()
                            .Any(
                                b =>
                                    b.IsValid && b.Name.Contains("_Q_") && b.Name.Contains("Syndra_") &&
                                    b.Name.Contains("idle") && obj.Position.Distance(b.Position) < 50))
                        valid = true;

                if (valid && (!toGrab || !obj.IsMoving))
                    result.Add(obj.ServerPosition);
            }

            /*if (Utils.TickCount - tmpQOrbT < 400)
            {
                result.Add(tmpQOrbPos);
            }

            if (Utils.TickCount - tmpWOrbT < 400 && Utils.TickCount - tmpWOrbT > 0)
            {
                result.Add(tmpWOrbPos);
            }*/

            return result;
        }

        public static Vector3 GetOrbToGrab(int range)
        {
            var list = GetOrbs(true).Where(orb => ObjectManager.Player.Distance(orb) < range).ToList();
            return list.Count > 0 ? list[0] : new Vector3();
        }
    }

}
