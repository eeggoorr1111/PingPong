using UnityEngine;

namespace PingPong.Model.Ball
{
    public class BallModel
    {
        public BallModel(BallParams parameters)
        {
            _params = parameters;
        }


        public float Speed { get; private set; }
        public float Size { get; private set; }
        public Vector2 Pos { get; private set; }


        private readonly BallParams _params;
        private TrajectoryBall _trajectory;


        public void ChangeBallParams()
        {

        }
        public void ToFly(TrajectoryBall trajectory)
        {

        }
        public void ContinueFly()
        {

        }
    }
}
