using System;
using UnityEngine;

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
        public BallTrajectory Trajectory { get; private set; }


        private readonly Func<double> _timeGetter;


        public void NewParams(float newSpeed, float newDiameter)
        {
            Speed = newSpeed; 
            Diameter = newDiameter;
        }
        public void ToFly(BallTrajectory trajectory)
        {
            Trajectory = trajectory;
        }
        public void ContinueFly()
        {
            Pos = Trajectory.GetPosForTime(_timeGetter.Invoke());
        }
    }
}
