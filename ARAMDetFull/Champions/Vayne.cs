using System;
using System.Collections.Generic;
using System.Linq;
using EloBuddy;
using EloBuddy.SDK;
using EloBuddy.SDK.Events;
using SharpDX;

namespace ARAMDetFull.Champions
{
    class Vayne : Champion
    {
        public static Vector3 AfterCond = Vector3.Zero;
        public Vayne()
        {
            Orbwalker.OnPostAttack += AfterAttack;
            Interrupter.OnInterruptableSpell += Interrupter_OnPossibleToInterrupt;
            Gapcloser.OnGapcloser += AntiGapcloser_OnEnemyGapcloser;

            ARAMSimulator.champBuild = new Build
            {
                coreItems = new List<ConditionalItem>
                {
                    new ConditionalItem(ItemId.Blade_of_the_Ruined_King),
                    new ConditionalItem(ItemId.Berserkers_Greaves),
                    new ConditionalItem(ItemId.Phantom_Dancer),
                    new ConditionalItem(ItemId.Infinity_Edge),
                    new ConditionalItem(ItemId.Mercurial_Scimitar),
                    new ConditionalItem((ItemId)3036),
                },
                startingItems = new List<ItemId>
                {
                    ItemId.Vampiric_Scepter,
                    ItemId.Boots_of_Speed
                }
            };
        }

        private void AfterAttack(AttackableUnit target, EventArgs args)
        {
            if (!(target is AIHeroClient) || !target.IsMe) return;
            var tar = (AIHeroClient)target;
            SmartQCheck(tar);
            ENextAuto(tar);
            UseItems(tar);
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

        public override void kiteBack(Vector2 pos)
        {
            base.kiteBack(pos);
            if (Q.IsReady() && player.CountEnemiesInRange(380) != 0)//.Count(ene => !ene.IsDead) != 0)
                Q.Cast(pos.To3D());
        }

        public override void setUpSpells()
        {
            Q = new Spell.Active(SpellSlot.Q);
            E = new Spell.Targeted(SpellSlot.E, 550);
            R = new Spell.Active(SpellSlot.R);
            //E.SetTargetted(0.25f, 1600f);
        }

        public override void useSpells()
        {
            var tar = ARAMTargetSelector.getBestTarget(player.AttackRange + 50);
            FocusTarget();
            NoAAStealth();
            AIHeroClient tar2;
            if (CondemnCheck(player.ServerPosition, out tar2)) { CastE(tar2); }
        }

        void NoAAStealth()
        {
            var mb = (player.HasBuff("vaynetumblefade"));
            if (mb)
            {
                Orbwalker.DisableAttacking = true;
            }
            else
            {
                Orbwalker.DisableAttacking = false;
            }
        }

        public static bool EnemyInRange(int numOfEnemy, float range)
        {
            return Player.Instance.CountEnemiesInRange((int)range) >= numOfEnemy;
        }

        void AntiGapcloser_OnEnemyGapcloser(AIHeroClient target, Gapcloser.GapcloserEventArgs gapcloser)
        {
            var GPSender = (AIHeroClient)gapcloser.Sender;
            if (!E.IsReady() || !GPSender.IsValidTarget()) return;
            CastE(GPSender, true);

        }

        void Interrupter_OnPossibleToInterrupt(Obj_AI_Base unit, Interrupter.InterruptableSpellEventArgs spell)
        {
            var Sender = (AIHeroClient)unit;
            if (!E.IsReady() || !Sender.IsValidTarget()) return;
            CastE(Sender, true);
        }

        bool CondemnCheck(Vector3 Position, out AIHeroClient target)
        {
            if (Sector.inTowerRange(player.Position.To2D()))
            {
                target = null;
                return false;
            }
            foreach (var En in EntityManager.Heroes.Enemies.Where(hero => hero.IsEnemy && hero.IsValidTarget() && hero.Distance(player.Position) <= E.Range))
            {
                var EPred = E.GetPrediction(En);
                int pushDist = 300;
                var FinalPosition = EPred.UnitPosition.To2D().Extend(Position.To2D(), -pushDist).To3D();
                for (int i = 1; i < pushDist; i += (int)En.BoundingRadius)
                {
                    Vector3 loc3 = EPred.UnitPosition.To2D().Extend(Position.To2D(), -i).To3D();
                    var orTurret = Sector.inTowerRange(FinalPosition.To2D());
                    AfterCond = loc3;
                    if (loc3.IsWall() || orTurret)
                    {
                        target = En;
                        return true;
                    }
                }
            }
            target = null;
            return false;

        }
        
        private void ENextAuto(AIHeroClient tar)
        {
            if (!E.IsReady() || !tar.IsValid) return;
            CastE(tar, true);
        }

        void FocusTarget()
        {
            foreach (var hero in EntityManager.Heroes.Enemies.Where(hero => hero.IsValidTarget(player.GetAutoAttackRange(player))))
            {
                foreach (var b in hero.Buffs)
                {
                    if (b.Name == "vaynesilvereddebuff" && b.Count == 2)
                    {
                        Orbwalker.ForcedTarget = hero;
                        //DeathWalker.ForcedTarget=hero;
                        //Hud.SelectedTarget = hero;
                        return;
                    }
                }
            }
        }

        void SmartQCheck(AIHeroClient target)
        {
            if (!Q.IsReady() || !target.IsValidTarget()) return;
            var goodPos = player.Position.To2D().Extend((player.Position - target.Position).To2D().Perpendicular(), 300);

            if (!E.IsReady())
            {
                CastQ(goodPos.To3D(), target);
            }
            else
            {
                for (int I = 0; I <= 360; I += 65)
                {
                    var F1 = new Vector2(player.Position.X + (float)(300 * Math.Cos(I * (Math.PI / 180))), player.Position.Y + (float)(300 * Math.Sin(I * (Math.PI / 180)))).To3D();
                    // var FinalPos = player.Position.To2D().Extend(F1, 300).To3D();
                    AIHeroClient targ;
                    if (CondemnCheck(F1, out targ))
                    {
                        CastTumble(F1, target);
                        CastE(target);
                        return;
                    }
                }
                CastQ(goodPos.To3D(), target);
            }
        }

        void CastQ(Vector3 Pos, Obj_AI_Base target, bool customPos = false)
        {
            if (!Q.IsReady() || !target.IsValidTarget()) return;

            //var ManaC = 0;
            var EnMin = 1;
            var EnemiesList =
                ObjectManager.Get<AIHeroClient>()
                    .Where(h => h.IsValid && !h.IsDead && h.Distance(player.Position) <= 900 && h.IsEnemy).ToList();
            if (R.IsReady() && EnemiesList.Count >= EnMin)
            {
                Aggresivity.addAgresiveMove(new AgresiveMove(160, 10000, true));
                R.Cast();
            }
            if (!customPos) { CastTumble(target); } else { CastTumble(Pos, target); }

        }

        void CastTumble(Obj_AI_Base target)
        {
            var goodPos = player.Position.Extend((player.Position - target.Position).To2D().Perpendicular(), 300).To3D();
            var posAfterTumble = ObjectManager.Player.ServerPosition.Extend(goodPos, 300).To3D();
            var distanceAfterTumble = Vector3.DistanceSquared(posAfterTumble, target.ServerPosition);
            Q.Cast(goodPos);
        }

        void CastTumble(Vector3 Pos, Obj_AI_Base target)
        {
            var posAfterTumble =
                ObjectManager.Player.ServerPosition.To2D().Extend(Pos.To2D(), 300).To3D();
            var distanceAfterTumble = Vector3.DistanceSquared(posAfterTumble, target.ServerPosition);
            if (distanceAfterTumble < 550 * 550 && distanceAfterTumble > 100 * 100)
                Q.Cast(Pos);
        }
        
        void CastE(AIHeroClient target, bool isForGapcloser = false)
        {
            if (!E.IsReady() || !target.IsValidTarget()) return;
            if (isForGapcloser)
            {
                E.Cast(target);
                AfterCond = Vector3.Zero;
                return;
            }
            var ManaC = 15;
            if (getPerValue(true) >= ManaC)
            {
                E.Cast(target);
                AfterCond = Vector3.Zero;
            }

        }
        
        void UseItems(AIHeroClient tar)
        {
            UseItem(3153, tar);
            UseItem(3153, tar);
            UseItem(3142);
            UseItem(3142);
            UseItem(3144, tar);
            UseItem(3144, tar);
        }

        #region utility methods
        int getEnemiesInRange(Vector3 point, float range)
        {
            return
                ObjectManager.Get<AIHeroClient>()
                    .Where(h => h.IsEnemy && !h.IsDead && h.IsValid && h.Distance(point) <= range).ToList().Count;
        }
        int getAlliesInRange(Vector3 point, float range)
        {
            return
                ObjectManager.Get<AIHeroClient>()
                    .Where(h => h.IsAlly && !h.IsDead && h.IsValid && h.Distance(point) <= range).ToList().Count;
        }

        private static SpellDataInst GetItemSpell(InventorySlot invSlot)
        {
            return ObjectManager.Player.Spellbook.Spells.FirstOrDefault(spell => (int)spell.Slot == invSlot.Slot + 4);
        }

        public static void UseItem(int id, AIHeroClient target = null)
        {
            if (Item.HasItem(id) && Item.CanUseItem(id))
            {
                Item.UseItem(id, target);
            }
        }
        bool isWall(Vector3 Pos)
        {
            CollisionFlags cFlags = NavMesh.GetCollisionFlags(Pos);
            return (cFlags == CollisionFlags.Wall);
        }
        bool isUnderTurret(Vector3 Position)
        {
            foreach (var tur in ObjectManager.Get<Obj_AI_Turret>().Where(turr => turr.IsAlly && (turr.Health != 0)))
            {
                if (tur.Distance(Position) <= 975f) return true;
            }
            return false;
        }
        bool isUnderEnTurret(Vector3 Position)
        {
            foreach (var tur in ObjectManager.Get<Obj_AI_Turret>().Where(turr => turr.IsEnemy && (turr.Health != 0)))
            {
                if (tur.Distance(Position) <= 975f) return true;
            }
            return false;
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
        bool isGrass(Vector3 Pos)
        {
            return NavMesh.IsWallOfGrass(Pos, 65);
            //return false; 
        }

        bool isJ4FlagThere(Vector3 Position, AIHeroClient target)
        {
            foreach (
                var obj in ObjectManager.Get<Obj_AI_Base>().Where(m => m.Distance(Position) <= target.BoundingRadius))
            {
                if (obj.Name == "Beacon")
                {
                    return true;
                }
            }
            return false;
        }
        #endregion
    }
}