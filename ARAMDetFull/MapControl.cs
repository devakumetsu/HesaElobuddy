using System;
using System.Collections.Generic;
using System.Linq;
using SharpDX;
using ARAMDetFull.SpellsSDK;
using EloBuddy;
using EloBuddy.SDK;
using static EloBuddy.SDK.Spell;
using EloBuddy.SDK.Spells;
using EloBuddy.SDK.Enumerations;

namespace ARAMDetFull
{
    class MapControl
    {

        public static SpellSlot[] spellSlots = { SpellSlot.Q, SpellSlot.W, SpellSlot.E, SpellSlot.R };

        internal class ChampControl
        {
            public AIHeroClient hero = null;

            public float reach = 0;

            public float dangerReach = 0;

            public int activeDangers = 0;

            public int lastAttackedUnitId = -1;

            protected List<SpellBase> champSpells = new List<SpellBase>();
            
            public AttackableUnit getFocusTarget()
            {
                var target = TargetSelector.SelectedTarget;
                return target;
                //HealthDeath.DamageMaker dm = null;
                //HealthDeath.activeDamageMakers.TryGetValue(hero.NetworkId, out dm);
                //return dm?.target;
                return null;
            }

            public List<Obj_AI_Base> getAttackers()
            {
                //return HealthDeath.activeDamageMakers.Values.Where(dm => dm.source.IsValid && dm.target != null && dm.target.NetworkId == hero.NetworkId).Select(dm=> dm.source).ToList();
                return new List<Obj_AI_Base>();
            }

            public bool isTransformChampion()
            {
                List<string> transChamps = new List<string>() {"Jayce", "Nidalee", "Nidalee", "Elise", "Gnar" };

                return transChamps.Contains(hero.ChampionName);
            }

            public ChampControl(AIHeroClient champ)
            {
                hero = champ;
                var spellsInfo = SpellDatabase.GetSpellInfoList(champ.BaseSkinName);
                //spellsInfo.Where(s => s.Slot);

                //champ.Spellbook.GetSpell(SpellSlot.Q).SData;

                foreach (var spell in spellsInfo)
                {
                    //champSpells.Add(spell.);
                }
                Obj_AI_Base.OnSpellCast += (sender, args) =>
                {
                    if (sender.NetworkId != hero.NetworkId)
                        return;
                    if(args.Target != null && args.Target is Obj_AI_Base)
                        lastAttackedUnitId = args.Target.NetworkId;
                };
                Obj_AI_Base.OnBasicAttack += (sender, args) =>
                {
                    if (sender.NetworkId != hero.NetworkId)
                        return;
                    if (args != null)
                        lastAttackedUnitId = args.Target.NetworkId;
                };
                getReach();
            }

            public float getReach()
            {
                dangerReach = 0;
                reach = hero.AttackRange;
                activeDangers = 0;
                /*var takeInCOunt = new List<SpellTags> { SpellTags.Dash, SpellTags.Blink, SpellTags.Teleport,SpellTags.Damage,SpellTags.CrowdControl };
                foreach (var cSpell in champSpells)
                {
                    if (cSpell.SpellTags == null || !(cSpell.SpellTags.Any(takeInCOunt.Contains)))
                        continue;
                    var spell = hero.Spellbook.GetSpell(cSpell.Slot);
                    if ((spell.CooldownExpires - Game.Time) > 3.5f || spell.State == SpellState.NotLearned || cSpell.ManaCost > hero.Mana)
                        continue;
                    var range = (spell.SData.CastRange < 1000) ? spell.SData.CastRange : 1000;
                    if (spell.SData.CastRange > range)
                        reach = range;
                }*/
                
                var extra = (hero.IsEnemy) ? (100 - ObjectManager.Player.HealthPercent)*1.5f : 0;
                reach += 150 + extra;
                return reach;
            }

            public int getccCount()
            {
                //return champSpells.Count(sShot => sShot.Slot);
                return 0;
            }
        }

        internal class MyControl : ChampControl
        {
            private List<SpellBase> spells = new List<SpellBase>();

            public static Spellbook sBook = ObjectManager.Player.Spellbook;
            public static SpellDataInst Qdata = sBook.GetSpell(SpellSlot.Q);
            public static SpellDataInst Wdata = sBook.GetSpell(SpellSlot.W);
            public static SpellDataInst Edata = sBook.GetSpell(SpellSlot.E);
            public static SpellDataInst Rdata = sBook.GetSpell(SpellSlot.R);

            public MyControl(AIHeroClient champ) : base(champ)
            {
                try
                {
                    hero = champ;
                    foreach (var spell in champSpells)
                    {
                        var spl= new Active(spell.Slot, spell.Range, spell.DamageType);
                        if (spell.IsSkillShot())
                        {
                            var spl2 = new Skillshot(spell.Slot, spell.Range, EloBuddy.SDK.Enumerations.SkillShotType.Linear, spell.CastDelay);
                            spells.Add(spl2);
                        }else spells.Add(spl);
                    }
                    getReach();
                    ARAMSimulator.farmRange = reach;
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex);
                }
            }

            public int bonusSpellBalance()
            {
                float manaUsed = 0;
                int bal = 0;
                foreach (var spell in spells)
                {
                    if (!spell.IsReady())
                        continue;
                    manaUsed += spell.ManaCost;
                    if (hero.MaxMana < 300 || hero.Mana- manaUsed>=0)
                    {
                        bal += (spell.Slot == SpellSlot.R) ? 15 : 7;
                    }
                }
                return bal;
            }

            private int lastMinionSpellUse = ARAMDetFull.now;
            public void useSpellsOnMinions()
            {
                var bTarg = EntityManager.MinionsAndMonsters.GetLaneMinions(EntityManager.UnitTeam.Enemy, Player.Instance.ServerPosition, Player.Instance.GetAutoAttackRange()).OrderBy(x => x.Distance(Player.Instance)).FirstOrDefault();
                if (bTarg != null && !bTarg.IsUnderHisturret())
                {
                    if (!Orbwalker.IsAutoAttacking || Orbwalker.GetTarget() != bTarg)
                    {
                        //Orbwalker.ForcedTarget = bTarg;
                        Player.IssueOrder(GameObjectOrder.AttackUnit, bTarg);

                    }
                }
                return; //code below is unreachable
                
                /*if (lastMinionSpellUse + 277 > ARAMDetFull.now)
                    return;
                lastMinionSpellUse = ARAMDetFull.now;
                if (hero.MaxMana > 300 && hero.ManaPercent < 78)
                    return;
                if (hero.MaxMana >199 && hero.MaxMana < 201 && hero.ManaPercent < 95)
                    return;

                foreach(var spell in spells)
                {
                    if (spell.Slot == SpellSlot.R || spell.IsOnCooldown || !spell.IsReady() || spell.ManaCost > hero.Mana || (isTransformChampion() && sBook.GetSpell(spell.Slot).Name.ToLower() != spell.Name.ToLower()))
                        continue;
                    //Farm spell
                    if (spell.IsSkillShot() && !((Skillshot) spell).HasCollision && spell.GetDamage(hero) > 5)
                    {
                        var spellSkillshot = (Skillshot)spell;
                        var farmMinions = EntityManager.MinionsAndMonsters.GetLaneMinions(EntityManager.UnitTeam.Enemy, Player.Instance.ServerPosition, (spellSkillshot.Range != 0) ? spellSkillshot.Range : 300);
                        var farmLocation = (spellSkillshot.Type == SkillShotType.Circular) ? spellSkillshot.GetCircularFarmLocation(farmMinions, spellSkillshot.Width()) : spellSkillshot.GetLineFarmLocation(farmMinions, spellSkillshot.Width());
                        if (farmLocation.HitNumber > 2)
                        {
                            spellSkillshot.Cast(farmLocation.CastPosition);
                            return;
                        }
                        //dont waste for single kills
                        return;
                    }
                    //if line
                    var minions = EntityManager.MinionsAndMonsters.GetLaneMinions(EntityManager.UnitTeam.Enemy, Player.Instance.Position, (spell.Range != 0) ? spell.Range : 500);
                    foreach (var minion in minions)
                    {

                        if (minion.Health > spell.GetDamage(minion))
                            continue;
                        var movementSpells = new List<string> { "Dash", "Blink", "Teleport" };
                        if (spell.IsSkillShot())
                        {

                            if (!movementSpells.Contains(Player.Instance.Spellbook.GetSpell(spell.Slot).SData.SpellTags) || safeGap(minion))
                            {
                                Console.WriteLine("Cast farm location: " + spell.Slot);
                                spell.Cast(minion.Position);
                                return;
                            }
                        }
                        else
                        {

                            float range = (spell.Range != 0) ? spell.Range : 500;
                            if (Player.Instance.Spellbook.GetSpell(spell.Slot).SData.CastType == (int) CastType.Self)
                            {
                                var bTarg2 = ARAMTargetSelector.getBestTarget(range, true);
                                if (bTarg2 != null)
                                {
                                    Console.WriteLine("Cast farm self: " + spell.Slot);
                                    spell.Cast(Player.Instance);
                                    return;
                                }
                            }
                            else if (Player.Instance.Spellbook.GetSpell(spell.Slot).SData.CastType == (int)CastType.EnemyMinions)
                            {
                                if (minion != null)
                                {
                                    if (!(Player.Instance.Spellbook.GetSpell(spell.Slot).SData.SpellTags != null && movementSpells.Contains(Player.Instance.Spellbook.GetSpell(spell.Slot).SData.SpellTags)) || safeGap(minion))
                                    {
                                        Console.WriteLine("Cast farm target: " + spell.Slot);
                                        spell.Cast(minion);
                                        return;
                                    }
                                }
                            }
                        }

                    }
                }*/
            }

            private float lastSpellUse = ARAMDetFull.now;
            public void useSpells() //TODO
            {
                var bTarg = ARAMTargetSelector.getBestTarget(Player.Instance.GetAutoAttackRange(), true);
                //var bTarg = ARAMTargetSelector.getBestTarget(Player.Instance.GetAutoAttackRange());
                if (bTarg != null && !bTarg.IsUnderHisturret())
                {
                    if (!Orbwalker.IsAutoAttacking || Orbwalker.GetTarget() != bTarg)
                    {
                        //Player.IssueOrder(GameObjectOrder.AttackUnit, bTarg);
                    }
                }
                return;

                /*if (lastSpellUse + 277 > ARAMDetFull.now)
                    return;
                lastSpellUse = ARAMDetFull.now;
                foreach (var spell in spells)
                {
                    if(!spell.IsReady() || spell.ManaCost > hero.Mana || (isTransformChampion() && sBook.GetSpell(spell.Slot).Name != spell.Name))
                        continue;

                    var movementSpells = new List<string> { "Dash", "Blink", "Teleport" };
                    var supportSpells = new List<string> { "Shield", "Heal", "DamageAmplifier", "SpellShield", "RemoveCrowdControl", };

                    if (Player.Instance.Spellbook.GetSpell(spell.Slot).SData.SpellTags.Contains("Transformation"))
                    {
                        var transformPoints = spells.Count(s => !s.IsReady() || spell.ManaCost > hero.Mana);
                        if (transformPoints >= 3)
                        {
                            Console.WriteLine("Cast transfrom self: " + spell.Slot);
                            spell.Cast();
                        }
                        return;
                    }
                    if (spell.IsSkillShot())
                    {
                        if (Player.Instance.Spellbook.GetSpell(spell.Slot).SData.SpellTags != null && movementSpells.Contains(Player.Instance.Spellbook.GetSpell(spell.Slot).SData.SpellTags))
                        {
                            if (hero.HealthPercent < 25 && hero.CountEnemiesInRange(600)>0)
                            {
                                Console.WriteLine("Cast esacpe location: " + spell.Slot);
                                spell.Cast(hero.Position.Extend(ARAMSimulator.fromNex.Position, 1235).To3D());
                                return;
                            }
                            else
                            {
                                var bTarg2 = ARAMTargetSelector.getBestTarget(spell.Range, true);
                                if (bTarg2 != null && safeGap(hero.Position.Extend(bTarg2.Position, (float) spell.Range)))
                                {
                                    if (spell.CastIfWillHitReturn(bTarg2, (int) HitChance.High))
                                    {
                                        Console.WriteLine("Cast attack location gap: " + spell.Slot);
                                        spell.Cast(bTarg2.Position); //BUG: Experimental
                                        return;
                                    }
                                }
                            }
                        }
                        else
                        {
                            var bTarg2 = ARAMTargetSelector.getBestTarget(spell.Range, true);
                            if (bTarg2 != null)
                            {
                                if (spell.CastIfWillHitReturn(bTarg2, (int) HitChance.High))//TODO
                                {
                                    Console.WriteLine("Cast attack location: " + spell.Slot);
                                    spell.Cast(bTarg2); //BUG: Experimental
                                    return;
                                }
                            }
                        }
                    }
                    else
                    {
                        float range = (spell.Range != 0) ? spell.Range : 500;
                        if (Player.Instance.Spellbook.GetSpell(spell.Slot).SData.CastType == (int) CastType.Self || Player.Instance.Spellbook.GetSpell(spell.Slot).SData.CastType == (int)CastType.Activate)
                        {
                            var bTarg2 = ARAMTargetSelector.getBestTarget(range, true);
                            if (bTarg2 != null)
                            {
                                Console.WriteLine("Cast self: " + spell.Slot);
                                spell.Cast(Player.Instance);
                                return;
                            }
                        }
                        else if(Player.Instance.Spellbook.GetSpell(spell.Slot).SData.CastType == (int) CastType.AllyChampions && Player.Instance.Spellbook.GetSpell(spell.Slot).SData.SpellTags != null && supportSpells.Contains(Player.Instance.Spellbook.GetSpell(spell.Slot).SData.SpellTags))
                        {
                            var bTarg2 = ARAMTargetSelector.getBestTargetAlly(range, false);
                            if (bTarg2 != null)
                            {
                                Console.WriteLine("Cast ally: " + spell.Slot);
                                spell.Cast(bTarg2);
                                return;
                            }
                        }
                        else if (Player.Instance.Spellbook.GetSpell(spell.Slot).SData.CastType == (int) CastType.EnemyChampions)
                        {
                            var bTarg2 = ARAMTargetSelector.getBestTarget(range, true);
                            if (bTarg2 != null)
                            {
                                if (!(Player.Instance.Spellbook.GetSpell(spell.Slot).SData.SpellTags != null && movementSpells.Contains(Player.Instance.Spellbook.GetSpell(spell.Slot).SData.SpellTags)) || safeGap(bTarg2))
                                {
                                    Console.WriteLine("Cast enemy: " + spell.Slot);
                                    spell.Cast(bTarg2);
                                    return;
                                }
                            }
                        }
                    }
                }*/
            }
            
            public float canDoDmgTo(Obj_AI_Base target, bool ignoreRange = false)
            {
                float dmgreal = 0;
                float mana = 0;
                foreach (var spell in spells)
                {
                    try
                    {
                        if(spell == null || !spell.IsReady())
                            continue;

                        float dmg = 0;
                        var checkRange = spell.Range + 250;
                        if (ignoreRange || hero.Distance(target, true) < checkRange * checkRange)
                            dmg = Player.Instance.GetSpellDamage(target, spell.Slot);
                        if (dmg != 0)
                            mana += spell.ManaCost;
                        if (hero.Mana > mana)
                            dmgreal += dmg;
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine("Error Map control canDoDmgTo: " + ex);
                    }
                }

                return dmgreal;
            }
        }

        public static int fearDistance
        {
            get
            {
                try
                {
                    int assistValue = (ARAMSimulator.getType() == ARAMSimulator.ChampType.Support || ARAMSimulator.getType() == ARAMSimulator.ChampType.Tank || ARAMSimulator.getType() == ARAMSimulator.ChampType.TankAS) ? 30 : 20;
                    
                    int kdaScore = myControler.hero.ChampionsKilled * 50 + myControler.hero.Assists * assistValue - myControler.hero.Deaths * 50;

                    int timeFear = 0;
                    int healthFear = (int)(-(60 - myControler.hero.HealthPercent) * 2);
                    int score = kdaScore + timeFear + 100;
                    return (score < -550) ? -550 + healthFear : ((score > 500) ? 500 : score) + healthFear;
                }
                catch(Exception ex)
                {
                    Console.WriteLine(ex);
                }
                return 0;
            }
        }

        public static List<ChampControl> enemy_champions = new List<ChampControl>();

        public static List<ChampControl> ally_champions = new List<ChampControl>();

        public static MyControl myControler;
        public static void updateReaches()
        {
            foreach (var champ in enemy_champions)
            {
                champ.getReach();
            }
            foreach (var champ in ally_champions)
            {
                champ.getReach();
            }
        }

        public static void setupMapControl()
        {
            try
            {
                ally_champions.Clear();
                enemy_champions.Clear();
                foreach (var hero in ObjectManager.Get<AIHeroClient>())
                {
                    if (hero.IsMe)
                        continue;

                    if (hero.IsAlly)
                        ally_champions.Add(new ChampControl(hero));

                    if (hero.IsEnemy)
                        enemy_champions.Add(new ChampControl(hero));
                }
                myControler = new MyControl(ObjectManager.Player);
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex);
            }
        }
        
        public static int fightLevel()
        {
            int count = 0;
            foreach (var enem in enemy_champions.Where(ene => !ene.hero.IsDead && ene.hero.IsVisible && ene.hero.IsHPBarRendered).OrderBy(ene => ene.hero.Distance(ObjectManager.Player, true)))
            {
                if (myControler.canDoDmgTo(enem.hero) * 0.7f > enem.hero.Health)
                    count++;

                if (ally_champions.Where(ene => !ene.hero.IsDead && !ene.hero.IsMe && ene.hero.IsHPBarRendered).Any(ally => enem.hero.Distance(ally.hero, true) < 600 * 600))
                {
                    count++;
                }
            }
            return count;
        }

        public static AIHeroClient fightIsOn()
        {
            foreach (var enem in enemy_champions.Where(ene => !ene.hero.IsDead && ene.hero.IsVisible && ene.hero.IsHPBarRendered && !ene.hero.IsZombie).OrderBy(ene => ene.hero.Distance(ObjectManager.Player, true)))
            {
                if (myControler.canDoDmgTo(enem.hero) * 0.7f > enem.hero.Health)
                    return enem.hero;

                if (ally_champions.Where(ene => !ene.hero.IsDead && !ene.hero.IsMe && ene.hero.IsHPBarRendered).Any(ally => enem.hero.Distance(ally.hero, true) < 600 * 600))
                {
                    return enem.hero;
                }
            }
            return null;
        }

        public static bool fightIsClose()
        {
            foreach (var enem in enemy_champions.Where(ene => !ene.hero.IsDead && ene.hero.IsVisible && ene.hero.IsHPBarRendered).OrderBy(ene => ene.hero.Distance(ObjectManager.Player, true)))
            {

                if (ally_champions.Where(ene => !ene.hero.IsDead && !ene.hero.IsMe && ene.hero.IsHPBarRendered).Any(ally => enem.hero.Distance(ally.hero, true) < 550 * 550))
                {
                    return true;
                }
            }
            return false;
        }

        public static bool fightIsOn(Obj_AI_Base target)
        {
            if (target == null)
            {
                return false;
            }

            if (myControler.canDoDmgTo(target)*0.75 > target.Health)
                    return true;

            if (ally_champions.Where(ene => !ene.hero.IsDead && !ene.hero.IsMe && ene.hero.IsHPBarRendered).Any(ally => target.Distance(ally.hero, true) < 300 * 300))
            {
                return true;
            }

            return false;
        }

        public static int getMinionImpactOnHero(ChampControl champ)
        {
            var minionAttackerCount = champ.getAttackers().Count(att => att is Obj_AI_Minion);
            return (int)(minionAttackerCount * ((18 - myControler.hero.Level) * 2f));
        }
        
        public static int balanceAroundPoint(Vector2 point, float range)
        {
            int balance = 0;
            balance -= enemy_champions.Where(ene => !ene.hero.IsDead && ene.hero.IsHPBarRendered).Count(ene => ene.hero.Distance(point, true) < range * range);
            balance += ally_champions.Where(aly => !aly.hero.IsDead && aly.hero.IsHPBarRendered).Count(aly => aly.hero.Distance(point, true) < (range - 150) * (range - 150));
            return balance;
        }
        
        public static int balanceAroundPointAdvanced(Vector2 point, float rangePlus, int fearCompansate = 0)
        {
            if (point == null) return 0;
            try
            {
                int balance = (point.To3D().IsUnderTurret()) ? -80 : (point.To3D().IsUnderTurret()) ? 110 : 0;
                foreach (var ene in enemy_champions)
                {
                    var eneBalance = 0;
                    var reach = ene.reach + rangePlus;
                    if (!ene.hero.IsDead && ene.hero.IsHPBarRendered && ene.hero.Distance(point, true) < reach * reach && !unitIsUseless(ene.hero) && !notVisibleAndMostLieklyNotThere(ene.hero))
                    {
                        eneBalance -= (int)((ene.hero.HealthPercent + 20 - ene.hero.Deaths * 4 + ene.hero.ChampionsKilled * 4));
                        if (!ene.hero.IsFacing(ObjectManager.Player))
                            eneBalance = (int)(eneBalance * 0.64f);
                        var focus = ene.getFocusTarget();
                        if (focus != null && focus.IsValid && focus.IsAlly && focus is AIHeroClient)
                        {
                            eneBalance = (int)(eneBalance * (focus.IsMe ? 1.20 : 0.80));
                        }
                        eneBalance += getMinionImpactOnHero(ene);
                    }
                    balance += eneBalance;
                    balance -= getMinionImpactOnHero(myControler);
                }
                foreach (var aly in ally_champions)
                {
                    var reach = (aly.reach - 200 < 500) ? 500 : (aly.reach - 200);
                    if (!aly.hero.IsDead && aly.hero.Distance(point, true) < reach * reach &&
                        (Vector2.Distance(aly.hero.Position.To2D(), ARAMSimulator.toNex.Position.To2D()) + reach < (Vector2.Distance(point, ARAMSimulator.toNex.Position.To2D()) + fearDistance + fearCompansate + (ARAMSimulator.tankBal * -5) + (ARAMSimulator.agrobalance * 3))))
                        balance += ((int)aly.hero.HealthPercent + 20 + 20 - aly.hero.Deaths * 4 + aly.hero.ChampionsKilled * 4);
                }
                if (myControler == null)
                {
                    Console.WriteLine("My controller is null!?");
                }
                else
                {
                    var myBal = ((int)myControler.hero.HealthPercent + 20 + 20 - myControler.hero.Deaths * 4 +
                         myControler.hero.ChampionsKilled * 4) + myControler.hero.Assists / 2 + myControler.bonusSpellBalance();
                    balance += (myBal < 0) ? 10 : myBal;
                }
                return balance;
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.LineNumber() + " : " + ex.Message + "\n" + ex.StackTrace);
            }
            return 0;
        }

        public static double unitIsUselessFor(Obj_AI_Base unit)
        {
            if (unit == null)
            {
                return 0;
            }

            var result =
                unit.Buffs.Where(
                    buff =>
                        buff.IsActive && Game.Time <= buff.EndTime &&
                        (buff.Type == BuffType.Charm || buff.Type == BuffType.Knockup || buff.Type == BuffType.Stun ||
                         buff.Type == BuffType.Suppression || buff.Type == BuffType.Snare || buff.Type == BuffType.Fear))
                    .Aggregate(0d, (current, buff) => Math.Max(current, buff.EndTime));
            return (result - Game.Time);
        }

        public static bool unitIsUseless(Obj_AI_Base unit)
        {
            return unitIsUselessFor(unit) > 0.3;
        }

        public static bool notVisibleAndMostLieklyNotThere(Obj_AI_Base unit)
        {
            if(unit == null)
            {
                return false;
            }
            if(ARAMSimulator.deepestAlly == null)
            {
                return false;
            }
            var distEneNex = Vector2.Distance(ARAMSimulator.toNex.Position.To2D(), unit.Position.To2D());
            var distEneNexDeepest = Vector2.Distance(ARAMSimulator.toNex.Position.To2D(), ARAMSimulator.deepestAlly.Position.To2D());

            return !ARAMSimulator.deepestAlly.IsDead && distEneNexDeepest + 1500 < distEneNex;
        }

        public static ChampControl getByObj(Obj_AI_Base champ)
        {
            return enemy_champions.FirstOrDefault(ene => ene.hero.NetworkId == champ.NetworkId);
        }

        public static bool safeGap(Obj_AI_Base target)
        {
            return safeGap(target.Position.To2D()) || MapControl.fightIsOn(target) || (!ARAMTargetSelector.IsInvulnerable(target) && target.Health < myControler.canDoDmgTo(target,true)/2);
        }

        public static bool safeGap(Vector2 position)
        {
            return myControler.hero.HealthPercent < 13 || (!Sector.inTowerRange(position) &&
                   (MapControl.balanceAroundPointAdvanced(position, 500) > 0)) || position.Distance(ARAMSimulator.fromNex.Position, true) < myControler.hero.Position.Distance(ARAMSimulator.fromNex.Position, true);
        }

        public static List<int> usedRelics = new List<int>();

        public static Obj_AI_Base ClosestRelic()
        {
            try
            {
                var closesEnem = ClosestEnemyTobase();
                var hprelics = ObjectManager.Get<Obj_AI_Base>().Where(
                    r => r.IsValid && !r.IsDead && (r.Name.Contains("HealthRelic") || (r.Name.ToLower().Contains("bard") && ObjectManager.Player.ChampionName == "Bard") || (r.Name.ToLower().Contains("blobdrop") && ObjectManager.Player.ChampionName == "Zac"))
                        && !usedRelics.Contains(r.NetworkId) && (closesEnem == null || (r.Name.ToLower().Contains("blobdrop") && ObjectManager.Player.ChampionName == "Zac") || r.Distance(ARAMSimulator.fromNex.Position, true) - 500 < closesEnem.Distance(ARAMSimulator.fromNex.Position, true))).ToList().OrderBy(r => ARAMSimulator.player.Distance(r, true));
                return hprelics.FirstOrDefault();
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex);
                return null;
            }
        }

        public static Obj_AI_Base ClosestEnemyTobase()
        {
            return
                EntityManager.Heroes.Enemies
                    .Where(h => h.IsValid && !h.IsDead && h.IsVisible && h.IsEnemy)
                    .OrderBy(h => h.Distance(ARAMSimulator.fromNex.Position, true))
                    .FirstOrDefault();
        }
        
        /* LOGIC!!
         * 
         * Go to Kill minions
         * If no minions go for enemy tower
         * Cut path on enemies range
         * 
         * Orbwalk all the way
         * 
         * 
         * 
         */
    }
}
