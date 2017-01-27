using System.Linq;
using EloBuddy;
using EloBuddy.SDK;
using EloBuddy.SDK.Enumerations;
using SharpDX;

namespace KickassSeries.Champions.Ziggs.Modes
{
    public static class Functions
    {
        public static void CastQ(Obj_AI_Base target)
        {
            PredictionResult prediction = null;

            //Getting Q Pred
            if (Player.Instance.Distance(target) < SpellManager.Q1.Range)
            {
                var oldrange = SpellManager.Q1.Range;
                SpellManager.Q1.Range = SpellManager.Q2.Range;
                prediction = SpellManager.Q1.GetPrediction(target);
                SpellManager.Q1.Range = oldrange;
            }
            else if (Player.Instance.Distance(target) < SpellManager.Q2.Range)
            {
                var oldrange = SpellManager.Q2.Range;
                SpellManager.Q2.Range = SpellManager.Q3.Range;
                prediction = SpellManager.Q2.GetPrediction(target);
                SpellManager.Q2.Range = oldrange;
            }
            else if (Player.Instance.Distance(target) < SpellManager.Q3.Range)
            {
                prediction = SpellManager.Q3.GetPrediction(target);
            }
            //Getting Q Pred

            // ReSharper disable once UseNullPropagation
            if (prediction == null) return;

            if (prediction.HitChance >= HitChance.High)
            {
                if (ObjectManager.Player.ServerPosition.Distance(prediction.CastPosition) <= SpellManager.Q1.Range + SpellManager.Q1.Width)
                {
                    Vector3 p;
                    if (ObjectManager.Player.ServerPosition.Distance(prediction.CastPosition) > 300)
                    {
                        p = prediction.CastPosition -
                            100 *
                            (prediction.CastPosition.To2D() - ObjectManager.Player.ServerPosition.To2D()).Normalized()
                                .To3D();
                    }
                    else
                    {
                        p = prediction.CastPosition;
                    }

                    SpellManager.Q1.Cast(p);
                }
                else if (ObjectManager.Player.ServerPosition.Distance(prediction.CastPosition) <=
                         ((SpellManager.Q1.Range + SpellManager.Q2.Range) / 2))
                {
                    var p = ObjectManager.Player.ServerPosition.To2D()
                        .Extend(prediction.CastPosition.To2D(), SpellManager.Q1.Range - 100);

                    if (!CheckQCollision(target, prediction.UnitPosition, p.To3D()))
                    {
                        SpellManager.Q1.Cast(p.To3D());
                    }
                }
                else
                {
                    var p = ObjectManager.Player.ServerPosition.To2D() +
                            SpellManager.Q1.Range *
                            (prediction.CastPosition.To2D() - ObjectManager.Player.ServerPosition.To2D()).Normalized
                                ();

                    if (!CheckQCollision(target, prediction.UnitPosition, p.To3D()))
                    {
                        SpellManager.Q1.Cast(p.To3D());
                    }
                }
            }
        }

        private static bool CheckQCollision(Obj_AI_Base target, Vector3 targetPosition, Vector3 castPosition)
        {
            var direction = (castPosition.To2D() - ObjectManager.Player.ServerPosition.To2D()).Normalized();
            var firstBouncePosition = castPosition.To2D();
            var secondBouncePosition = firstBouncePosition +
                                       direction * 0.4f *
                                       ObjectManager.Player.ServerPosition.To2D().Distance(firstBouncePosition);
            var thirdBouncePosition = secondBouncePosition +
                                      direction * 0.6f * firstBouncePosition.Distance(secondBouncePosition);

            //TODO: WALL COLL

            if (thirdBouncePosition.Distance(targetPosition.To2D()) < SpellManager.Q1.Width + target.BoundingRadius)
            {
                var minion = EntityManager.MinionsAndMonsters.EnemyMinions.FirstOrDefault(m => m.IsValidTarget(2000));
                if (minion != null)
                {
                    var predictedPos = SpellManager.Q2.GetPrediction(minion);
                    if (predictedPos.UnitPosition.To2D().Distance(secondBouncePosition) <
                        SpellManager.Q2.Width + minion.BoundingRadius)
                    {
                        return true;
                    }
                }
            }

            if (!(secondBouncePosition.Distance(targetPosition.To2D()) < SpellManager.Q1.Width + target.BoundingRadius) &&
                !(thirdBouncePosition.Distance(targetPosition.To2D()) < SpellManager.Q1.Width + target.BoundingRadius)) return true;
            {
                //Check the first one
                var minion = EntityManager.MinionsAndMonsters.EnemyMinions.FirstOrDefault(m => m.IsValidTarget(2000));
                if (minion == null) return false;

                var predictedPos = SpellManager.Q1.GetPrediction(minion);
                if (predictedPos.UnitPosition.To2D().Distance(firstBouncePosition) <
                    SpellManager.Q1.Width + minion.BoundingRadius)
                {
                    return true;
                }
            }

            return false;
        }
    }
}
