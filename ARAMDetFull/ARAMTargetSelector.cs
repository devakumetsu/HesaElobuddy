using System.Collections.Generic;
using System.Linq;
using SharpDX;
using EloBuddy;
using EloBuddy.SDK;
using System;

namespace ARAMDetFull
{
    class ARAMTargetSelector
    {
        public static AIHeroClient getBestTarget(float range, bool calcInRadius = false, Vector3 fromPlus = new Vector3(), List<AIHeroClient> fromEnes = null)
        {
            try
            {
                var target = TargetSelector.GetTarget(range, DamageType.Physical);
                if (target != null && target.IsValidTarget() && !target.IsDead)
                {
                    return target;
                }

                return null;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return null;
            }
        }

        public static AIHeroClient getSafeMeleeTarget(float range = 750)
        {
            return getBestTarget(range, true, new Vector3(), EntityManager.Heroes.Enemies.Where(ene => ene != null && MapControl.safeGap(ene)).ToList());
        }

        public static AIHeroClient getBestTargetAlly(float range, bool calcInRadius = false, Vector3 fromPlus = new Vector3())
        {
            try
            {
                return EntityManager.Heroes.Allies.Where(x => !x.IsDead && x.IsVisible && x.IsHPBarRendered && ObjectManager.Player.Distance(x) < range).OrderBy(x => x.Health).FirstOrDefault();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return null;
            }
        }
        
        public static bool IsInvulnerable(Obj_AI_Base target)
        {
            // Tryndamere's Undying Rage (R)
            if (target.HasBuff("Undying Rage") && target.Health <= target.Health*0.1f)
            {
                return true;
            }

            // Kayle's Intervention (R)
            if (target.HasBuff("JudicatorIntervention"))
            {
                return true;
            }
            //ChronoShift
            if (target.HasBuff("ChronoShift"))
            {
                return true;
            }

            return false;
        }

    }
}