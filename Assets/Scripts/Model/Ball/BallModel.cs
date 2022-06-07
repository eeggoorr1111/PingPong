using UnityEngine;
using Narratore.DebugTools;
using System;
using Random = UnityEngine.Random;

namespace PingPong.Model.Ball
{
    public class BallModel
    {
        public BallModel(Func<double> timeGetter)
        {
            _timeGetter = timeGetter;
        }


        public float Speed { get; private set; }
        public float Diameter { get; private set; }
        public float Radius => Diameter / 2;
        public Vector2 Pos { get; private set; }
        public TrajectoryBall Trajectory { get; private set; }


        private readonly Func<double> _timeGetter;


        public void NewParams(float newSpeed, float newDiameter)
        {
            Speed = newSpeed; 
            Diameter = newDiameter;
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
