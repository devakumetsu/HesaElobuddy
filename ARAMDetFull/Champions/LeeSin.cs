using System.Collections.Generic;
using System.Linq;
using EloBuddy;
using EloBuddy.SDK;
using EloBuddy.SDK.Enumerations;

namespace ARAMDetFull.Champions
{
    class LeeSin : Champion
    {
        private Spell.Active Q2;
        private static bool _rCasted;

        public Spell.Active E2 { get; private set; }
        public Spell.Active W2 { get; private set; }

        public LeeSin()
        {
            ARAMSimulator.champBuild = new Build
            {
                coreItems = new List<ConditionalItem>
                {
                    new ConditionalItem(ItemId.Ravenous_Hydra_Melee_Only),
                    new ConditionalItem(ItemId.Mercurys_Treads, ItemId.Ninja_Tabi, ItemCondition.ENEMY_AP),
                    new ConditionalItem(ItemId.Last_Whisper),
                    new ConditionalItem(ItemId.Spirit_Visage, ItemId.Randuins_Omen, ItemCondition.ENEMY_AP),
                    new ConditionalItem(ItemId.Maw_of_Malmortius),
                    new ConditionalItem(ItemId.Banshees_Veil),
                },
                startingItems = new List<ItemId>
                {
                    ItemId.Vampiric_Scepter,ItemId.Boots_of_Speed
                }
            };
            Obj_AI_Base.OnProcessSpellCast += OnProcessSpell;
        }

        private void OnProcessSpell(Obj_AI_Base sender, GameObjectProcessSpellCastEventArgs args)
        {
            if (!W.IsReady() || sender == null || args.Target == null || sender.IsAlly || !(sender is AIHeroClient) || !(args.Target is AIHeroClient) || !args.Target.IsAlly || safeGap((AIHeroClient)args.Target))
                return;
            if (W.IsInRange(sender))
                W.Cast((AIHeroClient)args.Target);
        }
        

        public override void useQ(Obj_AI_Base target)
        {
            if (!Q.IsReady() || target == null)
                return;
            //if (Q.Name == "BlindMonkQOne" && Q.IsReady())
            if (Q.IsReady())
                Q.Cast(target);
                Q2.Cast();
            //else if (/*safeGap(target) || */target.Distance(player, true) < 300 * 300)
            //  Q.Cast();
        }

        public override void useW(Obj_AI_Base target)
        {
            if (!W.IsReady() || target == null)
                return;
            if (W.Name == "blindmonkwtwo")
            //if (W.IsReady())
            {
                if (player.HealthPercent < 65) ;
                W.Cast();
                W2.Cast();
            }
        }

        public override void useE(Obj_AI_Base target)
        {
            if (!E.IsReady() || target == null)
                return;
            if (E.Name == "BlindMonkEOne")
            //if (E.IsReady())
            E.Cast();
            E2.Cast();
        }

        public override void useR(Obj_AI_Base target)
        {
            if (!R.IsReady() || target == null)
                return;
            if (R.IsKillable(target) || player.HealthPercent < 25)
            R.Cast(target);
            else
            {
            R.CastIfWillHit(target,2);
            }
        }

        public override void useSpells()
        {

            /*var tar = ARAMTargetSelector.getBestTarget(Q.Range);
            if (tar != null) useQ(tar);
            tar = ARAMTargetSelector.getBestTarget(W.Range);
            if (tar != null) useW(tar);
            tar = ARAMTargetSelector.getBestTarget(E.Range);
            if (tar != null) useE(tar);
            tar = ARAMTargetSelector.getBestTarget(R.Range);
            if (tar != null) useR(tar);*/
        }


        public override void setUpSpells()
        {
            Q = new Spell.Skillshot(SpellSlot.Q, 1100, SkillShotType.Linear, 250, 1800, 60)
            {
                AllowedCollisionCount = 0
            };
            Q2 = new Spell.Active(SpellSlot.Q, 1300);

            W = new Spell.Targeted(SpellSlot.W, 1200);
            W2 = new Spell.Active(SpellSlot.W, 700);

            E = new Spell.Active(SpellSlot.E, 350);
            E2 = new Spell.Active(SpellSlot.E, 675);

            R = new Spell.Targeted(SpellSlot.R, 375);

        }


        public override void farm()
        {

            var AllMinions = EntityManager.MinionsAndMonsters.GetLaneMinions(EntityManager.UnitTeam.Enemy, Player.Instance.Position, Q.Range);
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