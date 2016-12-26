using System.Collections.Generic;
using EloBuddy;
using EloBuddy.SDK;
using EloBuddy.SDK.Enumerations;

namespace ARAMDetFull.Champions
{
    class Jarvan : Champion
    {
        private static bool _rCasted;

        public Jarvan()
        {
            ARAMSimulator.champBuild = new Build
            {
                coreItems = new List<ConditionalItem>
                {
                    new ConditionalItem(ItemId.The_Black_Cleaver),
                    new ConditionalItem(ItemId.Mercurys_Treads,ItemId.Ninja_Tabi,ItemCondition.ENEMY_AP),
                    new ConditionalItem(ItemId.Sunfire_Cape, ItemId.Banshees_Veil, ItemCondition.ENEMY_AP),
                    new ConditionalItem(ItemId.Frozen_Mallet),
                    new ConditionalItem(ItemId.Spirit_Visage, ItemId.Randuins_Omen, ItemCondition.ENEMY_AP),
                    new ConditionalItem(ItemId.Last_Whisper),
                },
                startingItems = new List<ItemId>
                {
                    ItemId.Phage
                }
            };
            Obj_AI_Base.OnProcessSpellCast += OnProcessSpell;
        }

        private void OnProcessSpell(Obj_AI_Base sender, GameObjectProcessSpellCastEventArgs args)
        {
            if (sender.IsMe)
            {
                if (args.SData.Name == "JarvanIVCataclysm")
                {
                    _rCasted = true;
                    Core.DelayAction(() => _rCasted = false, 3500);
                }
            }
            if (!W.IsReady() || sender == null || args.Target == null || sender.IsAlly || !(sender is AIHeroClient) || !(args.Target.IsMe))
                return;
            W.Cast();
        }
        
        public override void useQ(Obj_AI_Base target)
        {
            if (!Q.IsReady() || target == null)
                return;
            Q.Cast(target);
        }

        public override void useW(Obj_AI_Base target)
        {
            if (!W.IsReady() || target == null)
                return;
        }

        public override void useE(Obj_AI_Base target)
        {
            if (!E.IsReady() || target == null)
                return;
            E.Cast(target);
        }

        public override void useR(Obj_AI_Base target)
        {
            if (R.IsReady() && _rCasted && ARAMSimulator.balance < 100)
                R.Cast();

            if (target == null || !R.IsReady() || !safeGap(target) || _rCasted)
                return;
            R.CastOnUnit(target);
        }

        public override void useSpells()
        {
            if(ObjectManager.Player.Spellbook.IsChanneling)
                return;
            var tar = ARAMTargetSelector.getBestTarget(E.Range);
            if (tar != null && E.IsReady() && safeGap(tar))
                if (tar != null) useE(tar);

            tar = ARAMTargetSelector.getBestTarget(Q.Range);
            if (tar != null) useQ(tar);
            tar = ARAMTargetSelector.getBestTarget(E.Range);
            if (tar != null) useE(tar);
            tar = ARAMTargetSelector.getBestTarget(R.Range);
            if (tar != null) useR(tar);
        }
        
        public override void setUpSpells()
        {
            Q = new Spell.Skillshot(SpellSlot.Q, 770, SkillShotType.Linear, 250, null, 180, DamageType.Physical)
            {
                AllowedCollisionCount = int.MaxValue,
            };
            W = new Spell.Active(SpellSlot.W, 520);
            E = new Spell.Skillshot(SpellSlot.E, 830, SkillShotType.Circular, 250, null, 75, DamageType.Magical)
            {
                AllowedCollisionCount = int.MaxValue
            };
            R = new Spell.Targeted(SpellSlot.R, 650);
        }
        
        public override void farm()
        {
            if (player.ManaPercent < 65)
                return;

            var AllMinions = EntityManager.MinionsAndMonsters.GetLaneMinions(EntityManager.UnitTeam.Enemy, player.ServerPosition, Q.Range);
            foreach (var minion in AllMinions)
            {
                if (Q.IsReady() && Q.GetDamage(minion) > minion.Health)
                {
                    Q.Cast(minion);
                    return;
                }
                if (E.IsReady() && E.GetDamage(minion) > minion.Health)
                {
                    E.Cast(minion);
                }
            }
        }
    }
}