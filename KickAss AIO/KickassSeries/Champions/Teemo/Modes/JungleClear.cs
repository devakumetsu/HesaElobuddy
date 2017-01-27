using System.Linq;
using EloBuddy;
using EloBuddy.SDK;

//using Settings = KickassSeries.Champions.Teemo.Config.Modes.JungleClear;

namespace KickassSeries.Champions.Teemo.Modes
{
    public sealed class JungleClear : ModeBase
    {
        public override bool ShouldBeExecuted()
        {
            return Orbwalker.ActiveModesFlags.HasFlag(Orbwalker.ActiveModes.JungleClear);
        }

        public override void Execute()
        {
            /*
            var jgminion =
                EntityManager.MinionsAndMonsters.GetJungleMonsters()
                    .OrderByDescending(j => j.Health)
                    .FirstOrDefault(j => j.IsValidTarget(Q.Range));

            if (jgminion == null)return;

            var ammoR = Player.Instance.Spellbook.GetSpell(SpellSlot.R).Ammo;
            var jungleMobQ = EntityManager.MinionsAndMonsters.GetJungleMonsters(Player.Instance.ServerPosition, Q.Range).FirstOrDefault();
            var jungleMobR = EntityManager.MinionsAndMonsters.GetJungleMonsters(Player.Instance.ServerPosition, R.Range);

            if (Settings.UseQ && jungleMobQ != null)
            {
                if (Q.IsReady() && Settings.QMana <= (int)Player.Instance.ManaPercent)
                {
                    Q.Cast(jungleMobQ);
                }
            }

            var firstjunglemobR = jungleMobR.FirstOrDefault();

            if (!Settings.UseR || firstjunglemobR == null)
            {
                return;
            }

            if (R.IsReady() && ammoR >= 1)
            {
                R.Cast(firstjunglemobR.ServerPosition);
            }*/
        }
    }
}
