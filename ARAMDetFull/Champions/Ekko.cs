using System;
using System.Collections.Generic;
using System.Linq;
using EloBuddy;
using EloBuddy.SDK;
using EloBuddy.SDK.Enumerations;

namespace ARAMDetFull.Champions
{
    class Ekko : Champion
    {

        private static GameObject RMissile, WMissile;
        public static Spell.Skillshot Q;
        public static Spell.Skillshot W;
        public static Spell.Active E;
        public static Spell.Active R;

        private float QMANA, WMANA, EMANA, RMANA;
        public Ekko()
        {

            GameObject.OnCreate += Obj_AI_Base_OnCreate;

            ARAMSimulator.champBuild = new Build
            {
                coreItems = new List<ConditionalItem>
                {
                    new ConditionalItem(ItemId.Zhonyas_Hourglass),
                    new ConditionalItem(ItemId.Sorcerers_Shoes),
                    new ConditionalItem(ItemId.Lich_Bane),
                    new ConditionalItem(ItemId.Rabadons_Deathcap),
                    new ConditionalItem(ItemId.Void_Staff),
                    new ConditionalItem(ItemId.Abyssal_Scepter),
                },
                startingItems = new List<ItemId>
                {
                    ItemId.Needlessly_Large_Rod
                }
            };
        }

        private void Obj_AI_Base_OnCreate(GameObject obj, EventArgs args)
        {
            if (obj.IsValid && obj.IsAlly)
            {
                if (obj.Name == "Ekko")
                    RMissile = obj;
                if (obj.Name == "Ekko_Base_W_Cas.troy")
                    WMissile = obj;
            }
        }


        public override void useQ(Obj_AI_Base t)
        {
            if (!Q.IsReady() || t == null)
                return;
            var qDmg = GetQdmg(t);
            if (qDmg > t.Health)
            {
                Q.Cast(t);
            }
            else if (ObjectManager.Player.Mana > RMANA + QMANA)
            {
                Q.Cast(t);
            }
            var icantstandthisanymore = EntityManager.Heroes.Enemies.Where(enemy => enemy.IsValidTarget(Q.Range));
            if (player.Mana > RMANA + QMANA + WMANA)
            {
                foreach (var enemy in icantstandthisanymore)
                    Q.Cast(enemy);
            }
        }

        public override void useW(Obj_AI_Base t)
        {
            if (!W.IsReady())
                return;
            var qDmg = GetQdmg(t);
            if (t.HasBuffOfType(BuffType.Slow) || t.CountEnemiesInRange(250) > 1)
            {
                W.Cast(t);

            }
            if (ObjectManager.Player.Mana > RMANA + WMANA + EMANA + QMANA)
            {
                W.Cast(t);
            }
            else if (!ObjectManager.Player.IsUnderTurret() && ObjectManager.Player.Mana > ObjectManager.Player.MaxMana * 0.8 &&
                     ObjectManager.Player.Mana > RMANA + WMANA + EMANA + QMANA + WMANA)
            {
                W.Cast(t);
            }
            else if (ObjectManager.Player.Mana > RMANA + WMANA + EMANA)
            {
                foreach (var enemy in EntityManager.Heroes.Enemies.Where(enemy => enemy.IsValidTarget(W.Range)))
                {
                    W.Cast(enemy);
                }
            }
        }

        public override void useE(Obj_AI_Base t)
        {
            if (!E.IsReady() || t == null)
                return;

            if (WMissile != null && WMissile.IsValid)
            {
                if (WMissile.Position.CountEnemiesInRange(200) > 0 && WMissile.Position.Distance(player.ServerPosition) < 100)
                {
                    E.Cast(player.Position.Extend(WMissile.Position, E.Range).To3D());
                }
            }

            if (E.IsReady() && ObjectManager.Player.Mana > RMANA + EMANA
                 && ObjectManager.Player.CountEnemiesInRange(260) > 0
                 && ObjectManager.Player.Position.Extend(Game.CursorPos, E.Range).CountEnemiesInRange(500) < 3
                 && t.Position.Distance(Game.CursorPos) > t.Position.Distance(ObjectManager.Player.Position))
            {
                E.Cast(ObjectManager.Player.Position.Extend(Game.CursorPos, E.Range).To3D());
            }
            else if (ObjectManager.Player.Health > ObjectManager.Player.MaxHealth * 0.4
                && ObjectManager.Player.Mana > RMANA + EMANA
                && !ObjectManager.Player.IsUnderEnemyturret()
                && ObjectManager.Player.Position.Extend(Game.CursorPos, E.Range).CountEnemiesInRange(700) < 3)
            {
                if (t.IsValidTarget() && player.Mana > QMANA + EMANA + WMANA && t.Position.Distance(Game.CursorPos) + 300 < t.Position.Distance(player.Position))
                {
                    E.Cast(player.Position.Extend(Game.CursorPos, E.Range).To3D());
                }
            }
            else if (t.IsValidTarget() && GetEdmg(t) + GetWdmg(t) > t.Health)
            {
                E.Cast(player.Position.Extend(t.Position, E.Range).To3D());
            }
        }
        
        public override void useR(Obj_AI_Base target)
        {
            if (!R.IsReady() || target == null)
                return;
            
            foreach (var t in EntityManager.Heroes.Enemies.Where(t => RMissile != null && RMissile.IsValid && t.IsValidTarget() && RMissile.Position.Distance(R.GetPrediction(t).CastPosition) < 350 && RMissile.Position.Distance(t.ServerPosition) < 350))
            {
                var comboDmg = GetRdmg(t) + GetWdmg(t);
                if (Q.IsReady())
                    comboDmg += GetQdmg(t);
                if (E.IsReady())
                    comboDmg += GetEdmg(t);
                if (t.Health < comboDmg)
                    R.Cast();
            }

            if (player.Health < player.CountEnemiesInRange(600) * player.Level * 15)
            {
                R.Cast();
            }
        }

        private void SetMana()
        {
            QMANA = Q.ManaCost;
            WMANA = W.ManaCost;
            EMANA = E.ManaCost;

            if (!R.IsReady())
                RMANA = QMANA - ObjectManager.Player.Level * 2;
            else
                RMANA = R.ManaCost; ;

            if (ObjectManager.Player.Health < ObjectManager.Player.MaxHealth * 0.2)
            {
                QMANA = 0;
                WMANA = 0;
                EMANA = 0;
                RMANA = 0;
            }
        }

        public override void useSpells()
        {
            SetMana();
            var tar = ARAMTargetSelector.getBestTarget(Q.Range);
            if (tar != null) useQ(tar);
            tar = ARAMTargetSelector.getBestTarget(W.Range);
            if (tar != null) useW(tar);
            tar = ARAMTargetSelector.getBestTarget(E.Range);
            if (tar != null) useE(tar);
            tar = ARAMTargetSelector.getBestTarget(R.Range);
            if (tar != null) useR(tar);
        }

        public override void setUpSpells()
        {
            //Create the spells
            Q = new Spell.Skillshot(SpellSlot.Q, 850, SkillShotType.Linear, 250, 2200, 60);
            Q.AllowedCollisionCount = int.MaxValue;
            W = new Spell.Skillshot(SpellSlot.W, 1600, SkillShotType.Circular, 1500, 500, 650);
            W.AllowedCollisionCount = int.MaxValue;
            E = new Spell.Active(SpellSlot.E, 450);
            R = new Spell.Active(SpellSlot.R, 375);
        }

        private double GetQdmg(Obj_AI_Base t)
        {
            float dmg = 90 + (30 * Q.Level) + player.FlatMagicDamageMod * 0.8f;
            return player.CalculateDamageOnUnit(t, DamageType.Magical, dmg);
        }
        private double GetEdmg(Obj_AI_Base t)
        {
            float dmg = 20 + (30 * E.Level) + (player.FlatMagicDamageMod * 0.2f);
            return player.CalculateDamageOnUnit(t, DamageType.Magical, dmg);
        }
        private double GetWdmg(Obj_AI_Base t)
        {
            if (t.Health < t.MaxHealth * 0.3)
            {
                float hp = t.MaxHealth - t.Health;
                float dmg = ((player.FlatMagicDamageMod / 45) + 5) * 0.01f;
                float dmg2 = hp * dmg;
                return player.CalculateDamageOnUnit(t, DamageType.Magical, dmg2);

            }
            else
                return 0;

        }

        private double GetRdmg(Obj_AI_Base t)
        {
            float dmg = 50 + (150 * R.Level) + player.FlatMagicDamageMod * 1.3f;
            return player.CalculateDamageOnUnit(t, DamageType.Magical, dmg);
        }

        public override void farm()
        {
            if (Q.IsReady())
            {
                var allMinionsQ = EntityManager.MinionsAndMonsters.GetLaneMinions(EntityManager.UnitTeam.Enemy, ObjectManager.Player.ServerPosition, Q.Range);
                var Qfarm = Q.GetBestLinearCastPosition(allMinionsQ, 100);
                if (Qfarm.HitNumber > 5)
                    Q.Cast(Qfarm.CastPosition);
            }
        }
    }
}