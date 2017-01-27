using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EloBuddy;
using EloBuddy.SDK;
using SharpDX;

namespace KickassSeries.Champions.Syndra
{
    internal class Functions
    {
        public static Vector3 GrabWPost(bool onlyQ)
        {
            var sphere =
                EntityManager.MinionsAndMonsters.Minions.FirstOrDefault(
                    s => s.IsValid && s.Team == Player.Instance.Team && !s.IsDead && s.Name == "Seed");
            if (sphere != null)
            {
                return sphere.Position;
            }
            if (!onlyQ)
            {
                var minion = EntityManager.MinionsAndMonsters.GetLaneMinions()
                    .OrderByDescending(m => m.Health)
                    .FirstOrDefault(m => m.IsValidTarget(SpellManager.W.Range) && m.IsEnemy);
                if (minion != null)
                {
                    return minion.Position;
                }
            }
            return new Vector3();
        }
    }
}
