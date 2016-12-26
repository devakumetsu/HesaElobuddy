using EloBuddy;
using EloBuddy.SDK;
using System;
using System.Linq;
using static EloBuddy.SDK.Spell;

namespace ARAMDetFull
{
    abstract class SumSpell
    {
        public SpellBase spell;
        public string spellName;
        public abstract void useSpell();

        public SumSpell(SpellBase spel)
        {
            spell = spel;
        }
    }
    


    class SummonerSpells
    {
        Spell.Active sum1 = new Spell.Active(SpellSlot.Summoner1);
        Spell.Active sum2 = new Spell.Active(SpellSlot.Summoner2);
        SumSpell sSpell1 = null;
        SumSpell sSpell2 = null;

        private static AIHeroClient player = ObjectManager.Player;

        public SummonerSpells()
        {
            Chat.Print(sum1.Name);
            switch (sum1.Name.ToLower())
            {
                case "summonerflash":
                    sSpell1 = new Flash(sum1);
                    break;
                case "summonerdot"://Ignite
                    sSpell1 = new Ignite(sum1);
                    break;
                case "summonerheal"://Heal
                    sSpell1 = new Heal(sum1);
                    break;
                case "summonerhaste"://Ghost
                    sSpell1 = new Ghost(sum1);
                    break;
                case "summonerexhaust"://Exhoust
                    sSpell1 = new Exhoust(sum1);
                    break;
                case "summonerbarrier"://Barrier
                    sSpell1 = new Barrier(sum1);
                    break;
                case "summonermana"://Clarity
                    sSpell1 = new Clarity(sum1);
                    break;
                case "summonersnowball"://Clarity
                    sSpell1 = new SnowBall(sum1);
                    break;
            }
            Chat.Print(sum2.Name);
            switch (sum2.Name.ToLower())
            {
                case "summonerflash":
                    sSpell2 = new Flash(sum2);
                    break;
                case "summonerdot"://Ignite
                    sSpell2 = new Ignite(sum2);
                    break;
                case "summonerheal"://Heal
                    sSpell2 = new Heal(sum2);
                    break;
                case "summonerhaste"://Ghost
                    sSpell2 = new Ghost(sum2);
                    break;
                case "summonerexhaust"://Exhoust
                    sSpell2 = new Exhoust(sum2);
                    break;
                case "summonerbarrier"://Barrier
                    sSpell2 = new Barrier(sum2);
                    break;
                case "summonermana"://Clarity
                    sSpell2 = new Clarity(sum2);
                    break;
                case "summonersnowball"://Clarity
                    sSpell1 = new SnowBall(sum1);
                    break;
            }
        }

        public void useSumoners()
        {
            if(sSpell1 != null)
                sSpell1.useSpell();
            if (sSpell2 != null)
                sSpell2.useSpell();
        }

        class SnowBall : SumSpell
        {
            Obj_AI_Base snowed;
            private int lastCast = 0;
            public override void useSpell()
            {
                if (!spell.IsReady() || lastCast + 700 > ARAMDetFull.now)
                    return;
                lastCast = ARAMDetFull.now;

                if (spell.Name.ToLower().Equals("snowballfollowupcast"))
                {
                    if (snowed != null)
                    {
                        if (MapControl.safeGap(snowed))
                        {
                            spell.Cast();
                            Aggresivity.addAgresiveMove(new AgresiveMove(100,2500,true));
                        }
                    }
                }
                else
                {
                    var tar = ARAMTargetSelector.getBestTarget(spell.Range);
                    if (tar != null )
                    {
                        spell.Cast(tar);
                        snowed = tar;
                    }
                }

                
            }

            public SnowBall(SpellBase spel) : base(spel)
            {
                //spell.Range = 1200f;
                //spell.SetSkillshot(.33f, 50f, 1600, true, SkillshotType.SkillshotLine);
                //spell.MinHitChance = HitChance.High;
                Console.WriteLine("SummonerSpell Set");
            }
        }

        //SummonerFlash
        class Flash : SumSpell
        {
            public override void useSpell()
            {
                if (!spell.IsReady())
                    return;
                if (ARAMSimulator.balance < -250 && player.HealthPercent > 70 && player.HealthPercent > 40 && player.CountEnemiesInRange(500)>0)
                {
                        spell.Cast(player.Position.Extend(ARAMSimulator.fromNex.Position, 450).To3D());
                }
            }

            public Flash(SpellBase spel) : base(spel)
            {
                Console.WriteLine("SummonerSpell Set");
            }
        }

        class Ignite : SumSpell
        {
            public override void useSpell()
            {
                if (!spell.IsReady())
                    return;
                var tar = ARAMTargetSelector.getBestTarget(450);
                if(tar != null)
                    if (tar.HealthPercent > 20 && tar.HealthPercent < 50)
                    {
                        spell.Cast(tar);
                    }
            }

            public Ignite(SpellBase spel) : base(spel)
            {
                Console.WriteLine("SummonerSpell Set");
            }
        }

        class Heal : SumSpell
        {
            public override void useSpell()
            {
                if (!spell.IsReady())
                    return;
                if (player.CountEnemiesInRange(600) > 0 && player.HealthPercent < 30)
                {
                    spell.Cast(player);
                }
            }

            public Heal(SpellBase spel)
                : base(spel)
            {
                Console.WriteLine("SummonerSpell Set");
            }
        }

        class Ghost : SumSpell
        {
            public override void useSpell()
            {
                if (!spell.IsReady())
                    return;
                if (player.IsInShopRange())
                {
                    spell.Cast();
                }
            }

            public Ghost(SpellBase spel)
                : base(spel)
            {
                Console.WriteLine("SummonerSpell Set");
            }
        }

        class Exhoust : SumSpell
        {
            public override void useSpell()
            {
                if (!spell.IsReady())
                    return;
                var tar = ARAMTargetSelector.getBestTarget(300);
                if (tar != null)
                    if (tar.HealthPercent > 20)
                    {
                        spell.Cast(tar);
                    }
            }

            public Exhoust(SpellBase spel)
                : base(spel)
            {
                Console.WriteLine("SummonerSpell Set");
            }
        }

        class Barrier : SumSpell
        {
            public override void useSpell()
            {
                if (!spell.IsReady())
                    return;
                if (player.CountEnemiesInRange(600) > 0 && player.HealthPercent < 20)
                {
                    spell.Cast();
                }
            }

            public Barrier(SpellBase spel) : base(spel)
            {
                Console.WriteLine("SummonerSpell Set");
            }
        }

        class Clarity : SumSpell
        {
            public override void useSpell()
            {
                if (!spell.IsReady())
                    return;
                if (player.ManaPercent < 25)
                {
                    spell.Cast();
                }
            }

            public Clarity(SpellBase spel) : base(spel)
            {
                Console.WriteLine("SummonerSpell Set");
            }
        }

    }
}
