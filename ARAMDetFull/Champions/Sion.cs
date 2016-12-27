using System;
using System.Collections.Generic;
using System.Linq;
using EloBuddy;
using EloBuddy.SDK;
using EloBuddy.SDK.Enumerations;
using SharpDX;

namespace ARAMDetFull.Champions
{
    class Sion : Champion
    {
        public static Spell.Chargeable Q;
        public Vector2 QCastPos = new Vector2();
        private Spell.Skillshot E1;

        public Sion()
        {
            ARAMSimulator.champBuild = new Build
            {
                coreItems = new List<ConditionalItem>
                {
                    new ConditionalItem(ItemId.Spirit_Visage,ItemId.Sunfire_Cape,ItemCondition.ENEMY_AP),
                    new ConditionalItem(ItemId.Ravenous_Hydra_Melee_Only),
                    new ConditionalItem(ItemId.Banshees_Veil,ItemId.Randuins_Omen,ItemCondition.ENEMY_AP),
                    new ConditionalItem(ItemId.Frozen_Mallet),
                    new ConditionalItem(ItemId.Warmogs_Armor),
                },
                startingItems = new List<ItemId>
                {
                    ItemId.Mercurys_Treads
                }
            };
            Obj_AI_Base.OnProcessSpellCast += OnProcessSpell;
            Player.OnIssueOrder += onIssueOrder;
        }

        private void onIssueOrder(Obj_AI_Base sender, PlayerIssueOrderEventArgs args)
        {
            if (sender.IsMe && RTarg != null && args.Order == GameObjectOrder.MoveTo && player.HasBuff("SionR"))
            {
                if (args.TargetPosition.Distance(Game.CursorPos, true) < 10000)
                {
                    args.Process = false;
                    if (RTarg != null)
                        Player.IssueOrder(GameObjectOrder.MoveTo, RTarg.Position, false);
                }
            }
        }

        private void OnProcessSpell(Obj_AI_Base sender, GameObjectProcessSpellCastEventArgs args)
        {
            if (sender.IsMe && args.SData.Name == "SionQ")
            {
                QCastPos = args.End.To2D();
            }
        }
        
        public override void useQ(Obj_AI_Base target)
        {
            if (Q.IsReady() && !Player.Instance.Spellbook.IsChanneling && safeGap(player))
            {
                Q.Cast(target.ServerPosition);
            }
        }

        public override void useW(Obj_AI_Base target)
        {
            if (!W.IsReady() || target == null)
                return;
            //if (!Q.IsReady(4500) && player.Mana > 200)
            W.Cast();
        }

        public override void useE(Obj_AI_Base target)
        {
            if (!E.IsReady() || target == null)
                return;
            E.Cast(target);
        }

        public void useE1(Obj_AI_Base target)
        {
            if (!E1.IsReady() || target == null)
                return;
            var pred = E1.GetPrediction(target);
            if (pred.HitChance >= HitChance.High &&
                pred.CollisionObjects.Any(
                    obj => !obj.IsDead && obj is Obj_AI_Minion && obj.Distance(player, true) < E.Range * E.Range))
                E1.Cast(target);
        }

        private Obj_AI_Base RTarg;

        public override void useR(Obj_AI_Base target)
        {
            if (player.HasBuff("SionR"))
            {
                if (target.Distance(player) < 150)
                    R.Cast();
            }

            if (target == null || !R.IsReady() || !safeGap(target))
                return;
            RTarg = target;
            R.Cast(target.Position);
        }

        public override void useSpells()
        {
            if (Q.IsCharging)
            {
                var qwidhtx = 140;
                var start = ObjectManager.Player.ServerPosition.To2D();
                var end = start.Extend(QCastPos, Q.Range);
                var direction = (end - start).Normalized();
                var normal = direction.Perpendicular();
                var points = new List<Vector2>();
                points.Add(start + normal * (qwidhtx));
                points.Add(start - normal * (qwidhtx));
                points.Add(end - normal * (qwidhtx));
                points.Add(end + normal * (qwidhtx));

                Polygon pol = new Polygon(points);
                var enesInside = EntityManager.Heroes.Enemies.Where(ene => !ene.IsDead && pol.pointInside(ene.Position.To2D())).ToList();
                if (enesInside.Count == 0)
                    Q.Cast();
                /*
                for (var i = 0; i <= points.Count - 1; i++)
                {
                    var A = points[i];
                    var B = points[i == points.Count - 1 ? 0 : i + 1];

                    if (enesInside.Any(targ => targ.ServerPosition.To2D().Distance(A, B, true, true) < 55 * 55))
                    {
                        Q.Cast();
                    }
                }
                return;*/
            }
            var tar = ARAMTargetSelector.getBestTarget(R.Range);
            if (tar != null) useR(tar);
            tar = ARAMTargetSelector.getBestTarget(580);
            if (tar != null) useQ(tar);
            if (player.Spellbook.IsChanneling)
                return;


            tar = ARAMTargetSelector.getBestTarget(W.Range);
            if (tar != null) useW(tar);
            tar = ARAMTargetSelector.getBestTarget(E1.Range);
            if (tar != null) useE1(tar);
            tar = ARAMTargetSelector.getBestTarget(E.Range);
            if (tar != null) useE(tar);

        }
        
        private bool sionZombie()
        {
            return player.Spellbook.GetSpell(SpellSlot.Q).Name == player.Spellbook.GetSpell(SpellSlot.E).Name ||
                   player.Spellbook.GetSpell(SpellSlot.W).Name == player.Spellbook.GetSpell(SpellSlot.R).Name;
        }

        public override void setUpSpells()
        {
            Q = new Spell.Chargeable(SpellSlot.Q, 1, 600, 2000, 250, 1600, 140);
            W = new Spell.Active(SpellSlot.W, 550);
            E = new Spell.Skillshot(SpellSlot.E, 725, SkillShotType.Linear, 250, 1600, 60);
            R = new Spell.Skillshot(SpellSlot.R, 550, SkillShotType.Cone, 300, 900, 140);
            E1 = new Spell.Skillshot(SpellSlot.E, 1500, SkillShotType.Linear, 250, 1800, 80);
        }

        public override void alwaysCheck()
        {
            if (sionZombie() && Q.IsReady() && player.CountEnemiesInRange(350) != 0)
            {
                Console.WriteLine("Zombie Q cast");
                Q.Cast();
            }
        }

        public override void farm()
        {
            if (player.ManaPercent < 55)
                return;

            var AllMinions = EntityManager.MinionsAndMonsters.GetLaneMinions(EntityManager.UnitTeam.Enemy, player.ServerPosition, Q.Range);
            foreach (var minion in AllMinions)
            {
                if (E.IsReady() && E.GetDamage(minion) > minion.Health)
                {
                    E.Cast(minion);
                }
            }
        }
    }
}