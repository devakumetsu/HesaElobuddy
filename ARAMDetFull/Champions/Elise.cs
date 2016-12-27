using EloBuddy;
using EloBuddy.SDK;
using EloBuddy.SDK.Enumerations;
using EloBuddy.SDK.Events;
using System.Collections.Generic;
using System.Linq;
using static EloBuddy.SDK.Spell;

namespace ARAMDetFull.Champions
{
    class Elise : Champion
    {
        private static bool _human;

        private static bool _spider;

        private static SpellBase _humanQ, _humanW, _humanE, _r, _spiderQ, _spiderW, _spiderE;
        public static Spell.Targeted Ignite { get; private set; }

        private readonly float[] HumanQcd = { 6, 6, 6, 6, 6 };

        private readonly float[] HumanWcd = { 12, 12, 12, 12, 12 };

        private readonly float[] HumanEcd = { 14, 13, 12, 11, 10 };

        private readonly float[] SpiderQcd = { 6, 6, 6, 6, 6 };

        private readonly float[] SpiderWcd = { 12, 12, 12, 12, 12 };

        private readonly float[] SpiderEcd = { 26, 23, 20, 17, 14 };

        private float _humQcd = 0, _humWcd = 0, _humEcd = 0;

        private float _spidQcd = 0, _spidWcd = 0, _spidEcd = 0;

        private float _humaQcd = 0, _humaWcd = 0, _humaEcd = 0;

        private float _spideQcd = 0, _spideWcd = 0, _spideEcd = 0;

        public Elise()
        {
            Gapcloser.OnGapcloser += Gapcloser_OnGapcloser;
            Interrupter.OnInterruptableSpell += Interrupter_OnInterruptableSpell; ;
            Obj_AI_Base.OnProcessSpellCast += OnProcessSpellCast;

            ARAMSimulator.champBuild = new Build
            {
                coreItems = new List<ConditionalItem>
                {
                    new ConditionalItem(ItemId.Sorcerers_Shoes),
                    new ConditionalItem(ItemId.Rabadons_Deathcap),
                    new ConditionalItem(ItemId.Banshees_Veil,ItemId.Randuins_Omen,ItemCondition.ENEMY_AP),
                    new ConditionalItem(ItemId.Void_Staff,ItemId.Zhonyas_Hourglass,ItemCondition.ENEMY_AP),
                    new ConditionalItem(ItemId.Rylais_Crystal_Scepter),
                    new ConditionalItem(ItemId.Liandrys_Torment),
                },
                startingItems = new List<ItemId>
                {
                    ItemId.Needlessly_Large_Rod
                }
            };

        }

        private void Interrupter_OnInterruptableSpell(Obj_AI_Base target, Interrupter.InterruptableSpellEventArgs e)
        {
            if (target != null && player.Distance(target) < _humanE.Range && _humanE.GetPrediction(target).HitChance >= HitChance.Low)
            {
                _humanE.Cast(target);
            }
        }

        private void Gapcloser_OnGapcloser(AIHeroClient sender, Gapcloser.GapcloserEventArgs e)
        {
            if (_spiderE.IsReady() && _spider && sender.IsValidTarget(_spiderE.Range))
            {
                _spiderE.Cast(sender);
            }
            if (_humanE.IsReady() && _human && sender.IsValidTarget(_humanE.Range))
            {
                _humanE.Cast(sender);
            }
        }

        private static void CheckSpells()
        {
            if (player.Spellbook.GetSpell(SpellSlot.Q).Name == "EliseHumanQ" ||
                player.Spellbook.GetSpell(SpellSlot.W).Name == "EliseHumanW" ||
                player.Spellbook.GetSpell(SpellSlot.E).Name == "EliseHumanE")
            {
                _human = true;
                _spider = false;
            }

            if (player.Spellbook.GetSpell(SpellSlot.Q).Name == "EliseSpiderQCast" ||
                player.Spellbook.GetSpell(SpellSlot.W).Name == "EliseSpiderW" ||
                player.Spellbook.GetSpell(SpellSlot.E).Name == "EliseSpiderEInitial")
            {
                _human = false;
                _spider = true;
            }
        }

        private void OnProcessSpellCast(Obj_AI_Base sender, GameObjectProcessSpellCastEventArgs args)
        {
            if (sender.IsMe)
            {
                //Game.PrintChat("Spell name: " + args.SData.Name.ToString());
                GetCDs(args);
                CheckSpells();
            }
        }
        private float CalculateCd(float time)
        {
            return time + (time * player.PercentCooldownMod);
        }
        private void GetCDs(GameObjectProcessSpellCastEventArgs spell)
        {
            if (_human)
            {
                if (spell.SData.Name == "EliseHumanQ")
                    _humQcd = Game.Time + CalculateCd(HumanQcd[_humanQ.Level - 1]);
                if (spell.SData.Name == "EliseHumanW")
                    _humWcd = Game.Time + CalculateCd(HumanWcd[_humanW.Level - 1]);
                if (spell.SData.Name == "EliseHumanE")
                    _humEcd = Game.Time + CalculateCd(HumanEcd[_humanE.Level - 1]);
            }
            else
            {
                if (spell.SData.Name == "EliseSpiderQCast")
                    _spidQcd = Game.Time + CalculateCd(SpiderQcd[_spiderQ.Level - 1]);
                if (spell.SData.Name == "EliseSpiderW")
                    _spidWcd = Game.Time + CalculateCd(SpiderWcd[_spiderW.Level - 1]);
                if (spell.SData.Name == "EliseSpiderEInitial")
                    _spidEcd = Game.Time + CalculateCd(SpiderEcd[_spiderE.Level] - 1);
            }
        }

        private void Cooldowns()
        {
            _humaQcd = ((_humQcd - Game.Time) > 0) ? (_humQcd - Game.Time) : 0;
            _humaWcd = ((_humWcd - Game.Time) > 0) ? (_humWcd - Game.Time) : 0;
            _humaEcd = ((_humEcd - Game.Time) > 0) ? (_humEcd - Game.Time) : 0;
            _spideQcd = ((_spidQcd - Game.Time) > 0) ? (_spidQcd - Game.Time) : 0;
            _spideWcd = ((_spidWcd - Game.Time) > 0) ? (_spidWcd - Game.Time) : 0;
            _spideEcd = ((_spidEcd - Game.Time) > 0) ? (_spidEcd - Game.Time) : 0;
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

        public override void useSpells()
        {
            var target = ARAMTargetSelector.getBestTarget(_humanW.Range);
            if (target == null) return; //buffelisecocoon
            CheckSpells();
            var qdmg = player.GetSpellDamage(target, SpellSlot.Q);
            var wdmg = player.GetSpellDamage(target, SpellSlot.W);
            if (_human)
            {
                if (target.Distance(player.Position) < _humanE.Range && _humanE.IsReady())
                {
                    if (_humanE.GetPrediction(target).HitChance >= HitChance.High)
                    {
                        _humanE.Cast(target);
                    }
                }

                if (player.Distance(target) <= _humanQ.Range && _humanQ.IsReady())
                {
                    _humanQ.Cast(target);
                }
                if (player.Distance(target) <= _humanW.Range && _humanW.IsReady())
                {
                    _humanW.Cast(target);
                }
                if (!_humanQ.IsReady() && !_humanW.IsReady() && !_humanE.IsReady() && _r.IsReady())
                {
                    _r.Cast();
                }
                if (!_humanQ.IsReady() && !_humanW.IsReady() && player.Distance(target) <= _spiderQ.Range && _r.IsReady())
                {
                    _r.Cast();
                }
            }
            if (!_spider) return;
            if (player.Distance(target) <= _spiderQ.Range && _spiderQ.IsReady())
            {
                _spiderQ.Cast(target);
            }
            if (player.Distance(target) <= 200 && _spiderW.IsReady())
            {
                _spiderW.Cast();
            }
            if (player.Distance(target) <= _spiderE.Range && player.Distance(target) > _spiderQ.Range && _spiderE.IsReady() && !_spiderQ.IsReady())
            {
                if ((safeGap(target)) || target.Distance(ARAMSimulator.fromNex.Position, true) < player.Distance(ARAMSimulator.fromNex.Position, true))
                    _spiderE.Cast(target);
            }
            if (player.Distance(target) > _spiderQ.Range && !_spiderE.IsReady() && _r.IsReady() && !_spiderQ.IsReady())
            {
                _r.Cast();
            }
            if (_humanQ.IsReady() && _humanW.IsReady() && _r.IsReady())
            {
                _r.Cast();
            }
            if (_humanQ.IsReady() && _humanW.IsReady() && _r.IsReady())
            {
                _r.Cast();
            }
            if ((_humanQ.IsReady() && qdmg >= target.Health || _humanW.IsReady() && wdmg >= target.Health))
            {
                _r.Cast();
            }

            Core.DelayAction(() => Player.IssueOrder(GameObjectOrder.AttackUnit, target), 100);
        }

        public override void farm()
        {
            if (player.ManaPercent < 40)
                return;
            CheckSpells();
            Cooldowns();
            
            foreach (var minion in EntityManager.MinionsAndMonsters.GetLaneMinions(EntityManager.UnitTeam.Enemy, ObjectManager.Player.ServerPosition, _humanQ.Range).Where(x => !x.IsDead).OrderBy(x => x.Health))
                if (_human)
                {
                    if (_humanQ.IsReady() && minion.IsValidTarget() &&
                        player.Distance(minion) <= _humanQ.Range)
                    {
                        _humanQ.Cast(minion);
                    }
                    if (_humanW.IsReady() && minion.IsValidTarget() &&
                        player.Distance(minion) <= _humanW.Range)
                    {
                        _humanW.Cast(minion);
                    }
                    if (_r.IsReady())
                    {
                        _r.Cast();
                    }
                }
                else if (_spider)
                {
                    if (_spiderQ.IsReady() && minion.IsValidTarget() &&
                        player.Distance(minion) <= _spiderQ.Range)
                    {
                        _spiderQ.Cast(minion);
                    }
                    if (_spiderW.IsReady() && minion.IsValidTarget() &&
                        player.Distance(minion) <= 125)
                    {
                        _spiderW.Cast();
                    }
                }
        }

        public override void killSteal()
        {
            base.killSteal();
            Cooldowns();
            CheckSpells();
            var target = ARAMTargetSelector.getBestTarget(_humanQ.Range);
            if (target == null) return;
            var igniteDmg = player.GetSummonerSpellDamage(target, DamageLibrary.SummonerSpells.Ignite);
            var qhDmg = player.GetSpellDamage(target, SpellSlot.Q);
            var wDmg = player.GetSpellDamage(target, SpellSlot.W);

            if (_human)
            {
                if (_humanQ.IsReady() && player.Distance(target) <= _humanQ.Range && target != null)
                {
                    if (target.Health <= qhDmg)
                    {
                        _humanQ.Cast(target);
                    }
                }
                if (_humanW.IsReady() && player.Distance(target) <= _humanW.Range && target != null)
                {
                    if (target.Health <= wDmg)
                    {
                        _humanW.Cast(target);
                    }
                }
            }
            if (_spider && _spiderQ.IsReady() && player.Distance(target) <= _spiderQ.Range && target != null)
            {
                if (target.Health <= qhDmg)
                {
                    _spiderQ.Cast(target);
                }
            }
        }

        public override void setUpSpells()
        {
            //Initialize Spells.
            _humanQ = new Spell.Targeted(SpellSlot.Q, 625);
            _humanW = new Spell.Skillshot(SpellSlot.W, 950, SkillShotType.Linear, 250, 1000, 100);
            _humanE = new Spell.Skillshot(SpellSlot.E, 1075, SkillShotType.Linear, 250, 1300, 55) { AllowedCollisionCount = 0 };
            _r = new Spell.Active(SpellSlot.R);
            _spiderQ = new Spell.Targeted(SpellSlot.Q, 950);
            _spiderW = new Spell.Active(SpellSlot.W);
            _spiderE = new Spell.Targeted(SpellSlot.E, 750);
        }
    }
}