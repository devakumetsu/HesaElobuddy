using System;
using System.Linq;
using SharpDX;
using EloBuddy;
using EloBuddy.SDK;
using EloBuddy.SDK.Events;
using EloBuddy.SDK.Enumerations;

namespace ARAMDetFull.Champions
{
    class Xerath : Champion
    {
        private Vector2 PingLocation;
        private int LastPingT = 0;
        private string LastCastedSpellName = "";
        private int LastCastedSpellT = 0;
        private bool AttacksEnabled
        {
            get
            {
                if (IsCastingR)
                    return false;

                if (player.Spellbook.IsCharging)
                    return false;

                return IsPassiveUp || (!Q.IsReady() && !W.IsReady() && !E.IsReady());

                return true;
            }
        }

        public bool IsPassiveUp
        {
            get { return ObjectManager.Player.HasBuff("xerathascended2onhit"); }
        }

        public bool IsCastingR
        {
            get
            {
                return ObjectManager.Player.HasBuff("XerathLocusOfPower2") ||
                       (LastCastedSpellName.Equals("XerathLocusOfPower2", StringComparison.InvariantCultureIgnoreCase) &&
                        Core.GameTickCount - LastCastedSpellT < 500);
            }
        }

        public class RCharge
        {
            public static int CastT;
            public static int Index;
            public static Vector3 Position;
            public static bool TapKeyPressed;
        }

        public Xerath()
        {
            Interrupter.OnInterruptableSpell += Interrupter_OnInterruptableSpell;
            Gapcloser.OnGapcloser += Gapcloser_OnGapcloser;
            
            Obj_AI_Base.OnProcessSpellCast += AIHeroClient_OnProcessSpellCast;
            Orbwalker.OnPreAttack += Orbwalker_OnPreAttack;
            Player.OnIssueOrder += Player_OnIssueOrder; ;
        }

        private void Gapcloser_OnGapcloser(AIHeroClient sender, Gapcloser.GapcloserEventArgs e)
        {
            if (sender.IsEnemy && E.IsReady() && player.Distance(e.End) < E.Range)
            {
                // Cast E on the gapcloser caster
                E.Cast(sender);
            }
        }

        private void Orbwalker_OnPreAttack(AttackableUnit target, Orbwalker.PreAttackArgs args)
        {
            args.Process = AttacksEnabled;
        }

        private void Interrupter_OnInterruptableSpell(Obj_AI_Base sender, Interrupter.InterruptableSpellEventArgs e)
        {
            if (sender.IsEnemy && e.DangerLevel == DangerLevel.High && E.IsReady() && player.Distance(sender) < E.Range)
            {
                // Cast E on the unit casting the interruptable spell
                E.Cast(sender);
            }
        }
        
        private void AIHeroClient_OnProcessSpellCast(Obj_AI_Base sender, GameObjectProcessSpellCastEventArgs args)
        {
            if (sender.IsMe)
            {
                LastCastedSpellT = Core.GameTickCount;
                LastCastedSpellName = args.SData.Name;

                if (args.SData.Name == "XerathLocusOfPower2")
                {
                    RCharge.CastT = 0;
                    RCharge.Index = 0;
                    RCharge.Position = new Vector3();
                    RCharge.TapKeyPressed = false;
                }
                else if (args.SData.Name == "xerathlocuspulse")
                {
                    RCharge.CastT = Core.GameTickCount;
                    RCharge.Index++;
                    RCharge.Position = args.End;
                    RCharge.TapKeyPressed = false;
                }
            }
        }
        
        private void Player_OnIssueOrder(Obj_AI_Base sender, PlayerIssueOrderEventArgs args)
        {
            if (IsCastingR)
            {
                args.Process = false;
            }
        }
        
        public override void useQ(Obj_AI_Base target)
        {
        }

        public override void useW(Obj_AI_Base target)
        {
            //  if (!W.IsReady())
            //      return;
            //  W.Cast(target);
        }

        public override void useE(Obj_AI_Base target)
        {

        }

        public override void useR(Obj_AI_Base target)
        {
        }

        public override void setUpSpells()
        {
            // Initialize spells
            Q = new Spell.Chargeable(SpellSlot.Q, 750, 1500, 1500, 500, int.MaxValue, 95) { AllowedCollisionCount = int.MaxValue };
            W = new Spell.Skillshot(SpellSlot.W, 1100, SkillShotType.Circular, 250, int.MaxValue, 500) { AllowedCollisionCount = int.MaxValue };
            E = new Spell.Skillshot(SpellSlot.E, 1050, SkillShotType.Linear, 250, 1400, 60);
            R = new Spell.Skillshot(SpellSlot.R, 3200, SkillShotType.Circular, 500, int.MaxValue, 120) { AllowedCollisionCount = int.MaxValue };
        }

        public override void useSpells()
        {
            try
            {
                if (IsCastingR)
                {
                    Orbwalker.DisableMovement = true;
                    WhileCastingR();
                    return;
                }
                else
                {
                    Orbwalker.DisableMovement = false;
                }

                if (R.IsReady())
                {
                    foreach (var enemy in ObjectManager.Get<AIHeroClient>().Where(h => h.IsValidTarget() && R.IsInRange(h) && (float)player.GetSpellDamage(h, SpellSlot.R) * 3 > h.Health))
                    {
                        R.Cast();
                    }
                }

                var tar = ARAMTargetSelector.getBestTarget(Q.Range);
                if (tar != null)
                    Combo();
                else
                    Farm(false);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
        }
        
        private void Combo()
        {

            UseSpells(true, true, true);
        }

        private void Harass()
        {
            UseSpells(true, true,
                false);
        }

        private void UseSpells(bool useQ, bool useW, bool useE)
        {
            
            var qTarget = TargetSelector.GetTarget(Q.ChargedMaxRange() -250, DamageType.Magical);
            var wTarget = TargetSelector.GetTarget(W.Range + W.Width() * 0.5f, DamageType.Magical);
            var eTarget = TargetSelector.GetTarget(E.Range, DamageType.Magical);

            if (eTarget != null && useE && E.IsReady())
            {
                if (player.Distance(eTarget) < E.Range * 0.4f)
                    E.Cast(eTarget);
                else if ((!useW || !W.IsReady()))
                    E.Cast(eTarget);
            }

            if (useQ && Q.IsReady() && qTarget != null)
            {
                if (Q.IsCharging())
                {
                    Q.Cast(qTarget);
                }
                else if (!useW || !W.IsReady() || player.Distance(qTarget) > W.Range)
                {
                    Q.StartCharging();
                }
            }

            if (wTarget != null && useW && W.IsReady())
                W.Cast(wTarget);
        }
        
        private void WhileCastingR()
        {

            var rTarget = ARAMTargetSelector.getBestTarget(R.Range);

            if (rTarget != null)
            {
                //Wait at least 0.6f if the target is going to die or if the target is to far away
                if (rTarget.Health - R.GetDamage(rTarget) < 0)
                    if (Core.GameTickCount - RCharge.CastT <= 700) return;

                if ((RCharge.Index != 0 && rTarget.Distance(RCharge.Position) > 1000))
                    if (Core.GameTickCount - RCharge.CastT <= Math.Min(2500, rTarget.Distance(RCharge.Position) - 1000)) return;

                R.Cast(rTarget);
            }
        }

        private void Farm(bool laneClear)
        {
            if (player.ManaPercent < 55) return;

            var allMinionsQ = EntityManager.MinionsAndMonsters.EnemyMinions.Where(x => x.IsInRange(Player.Instance.ServerPosition, Q.ChargedMaxRange()));

            var rangedMinionsW = EntityManager.MinionsAndMonsters.EnemyMinions.Where(x => !x.IsMelee && x.IsInRange(Player.Instance.ServerPosition, W.Range + W.Width() + 30));

            var useQi = 2;
            var useWi = 2;
            var useQ = (laneClear && (useQi == 1 || useQi == 2)) || (!laneClear && (useQi == 0 || useQi == 2));
            var useW = (laneClear && (useWi == 1 || useWi == 2)) || (!laneClear && (useWi == 0 || useWi == 2));

            if (useW && W.IsReady())
            {
                var locW = W.GetCircularFarmLocation(rangedMinionsW, (int) (W.Width() * 0.75f));
                if (locW.HitNumber >= 3 && W.IsInRange(locW.CastPosition))
                {
                    W.Cast(locW.CastPosition);
                    return;
                }
                else
                {
                    var locW2 = W.GetCircularFarmLocation(allMinionsQ, (int)(W.Width() * 0.75f));
                    if (locW2.HitNumber >= 1 && W.IsInRange(locW.CastPosition))
                    {
                        W.Cast(locW.CastPosition);
                        return;
                    }

                }
            }

            if (useQ && Q.IsReady())
            {
                if (Q.IsCharging())
                {
                    var locQ = Q.GetLineFarmLocation(allMinionsQ, Q.Width());
                    if (allMinionsQ.Count() == allMinionsQ.Count(m => player.Distance(m) < Q.Range) && locQ.HitNumber > 0 && locQ.CastPosition.IsValid())
                        Q.Cast(locQ.CastPosition);
                }
                else if (allMinionsQ.Count() > 0)
                    Q.StartCharging();
            }
        }
    }
}