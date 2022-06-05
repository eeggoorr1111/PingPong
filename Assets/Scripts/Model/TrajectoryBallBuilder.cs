using UnityEngine;
using PingPong.Model.Ball;
using PingPong.Model.Racket;
using System.Collections.Generic;
using Narratore.Primitives;
using Narratore.DebugTools;


namespace PingPong.Model
{
    public sealed class TrajectoryBallBuilder
    {
        private static int GetMaxCountSegments(Map map, RacketParams pRackets)
        {
            float angleRicochetFromMap = 90 - pRackets.MinAngleRicochet;
            float angleRicochetFromMapRad = angleRicochetFromMap * Mathf.Deg2Rad;
            float heightOfTriangleRicochet = map.Width;
            float hypotenuseOfTriangleRicochet = heightOfTriangleRicochet / Mathf.Sin(angleRicochetFromMapRad);
            float legOfTriangleRicochet = hypotenuseOfTriangleRicochet * Mathf.Cos(angleRicochetFromMapRad);
            float areaRectangleRicochet = legOfTriangleRicochet * hypotenuseOfTriangleRicochet;
            int countIntegerSegments = Mathf.FloorToInt(map.Area / areaRectangleRicochet);

            // Кроме целого колиества сегментов, может быть до 2 нецелых. 
            // Один в начале полета до первого рикошета от границ карты. Другой в конце.
            return countIntegerSegments + 2;
        }


        public TrajectoryBallBuilder(Map map, RacketParams pRackets, BallModel ball, float allowableError)
        {
            _map = map;
            _ball = ball;
            _allowableError = allowableError;
            _maxCountSegments = GetMaxCountSegments(map, pRackets);

            _corners = new List<Vector2>();
        }


        private readonly Map _map;
        private readonly BallModel _ball;
        private readonly int _maxCountSegments;
        private readonly List<Vector2> _corners;
        private readonly float _allowableError;


        public TrajectoryBall FlyFromCenterToRandomDir()
        {
            return Fly(_map.Center, Random.insideUnitCircle.normalized);
        }
        public TrajectoryBall Fly(Vector2 from, Vector2 direction)
        {
            _corners.Clear();
            _corners.Add(from);

            for (int i = 0; i < _maxCountSegments; i++)
            {
                Segment path = new Segment(from, from + direction * _map.Diagonal, _allowableError);
                Vector2 intersect;

                if (path.Intersect(_map.TopBorder, out intersect) ||
                    path.Intersect(_map.BottomBorder, out intersect))
                {
                    _corners.Add(intersect);
                    break;
                }
                else if (path.Intersect(_map.RightBorder, out intersect))
                {
                    _corners.Add(intersect);
                    direction = Vector2.Reflect(direction, Vector2.left);
                }
                else if (path.Intersect(_map.LeftBorder, out intersect))
                {
                    _corners.Add(intersect);
                    direction = Vector2.Reflect(direction, Vector2.right);
                }
                else
                    Debug.LogError("Почему то отрезок полета мяча ни с чем не пересекается");

                from = _corners[_corners.Count - 1];
            }

            return new TrajectoryBall(_corners.ToArray(), Time.time, Time.time + GetTimeFly(_corners));
        }


        private float GetTimeFly(IReadOnlyList<Vector2> corners)
        {
            float distanceTrajectory = 0f;
            for (int i = 0; i < corners.Count - 1; i++)
                distanceTrajectory += (corners[i + 1] - corners[i]).magnitude;
            
            return distanceTrajectory / _ball.Speed;
        }
    }
}
