using UnityEngine;

namespace PingPong.Model.Ball
{
    public class BallModel
    {
        public BallModel(BallParams parameters)
        {
            _params = parameters;

            ChangeBallParams();
        }


        public float Speed { get; private set; }
        public float Size { get; private set; }
        public Vector2 Pos { get; private set; }


        private readonly BallParams _params;
        private TrajectoryBall _trajectory;


        public void ChangeBallParams()
        {
            Speed = Random.Range(_params.RangeOfSpeeds.x, _params.RangeOfSpeeds.y);
            Size = Random.Range(_params.RangeOfSizes.x, _params.RangeOfSizes.y);
        }
        public void ToFly(TrajectoryBall trajectory)
        {

        }
        public void ContinueFly()
        {

        }
    }
}
