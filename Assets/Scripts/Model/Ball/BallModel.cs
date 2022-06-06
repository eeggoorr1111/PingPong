using UnityEngine;
using Narratore.DebugTools;
using System;
using Random = UnityEngine.Random;

namespace PingPong.Model.Ball
{
    public class BallModel
    {
        public BallModel(BallParams parameters, Func<double> timeGetter)
        {
            _params = parameters;
            _timeGetter = timeGetter;

            ChangeBallParams();
        }


        public double Speed { get; private set; }
        public float Diameter { get; private set; }
        public float Radius => Diameter / 2;
        public Vector2 Pos { get; private set; }
        public TrajectoryBall Trajectory { get; private set; }


        private readonly BallParams _params;
        private readonly Func<double> _timeGetter;


        public void ChangeBallParams()
        {
            Speed = Random.Range(_params.RangeOfSpeeds.x, _params.RangeOfSpeeds.y);
            Diameter = Random.Range(_params.RangeOfSizes.x, _params.RangeOfSizes.y);
        }
        public void ToFly(TrajectoryBall trajectory)
        {
            Trajectory = trajectory;
        }
        public void ContinueFly()
        {
            Pos = Trajectory.GetPosForTime(_timeGetter.Invoke());

            /*DrawerGizmos.Draw(() => {
                Gizmos.color = Color.red;
                foreach (var corner in Trajectory.Corners)
                    Gizmos.DrawSphere(new Vector3(corner.x, corner.y, 0), 0.1f);
            });*/
        }
    }
}
