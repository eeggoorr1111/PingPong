using UnityEngine;
using Narratore.DebugTools;

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
        public float Diameter { get; private set; }
        public float Radius => Diameter / 2;
        public Vector2 Pos { get; private set; }


        private readonly BallParams _params;
        private TrajectoryBall _trajectory;


        public void ChangeBallParams()
        {
            Speed = Random.Range(_params.RangeOfSpeeds.x, _params.RangeOfSpeeds.y);
            Diameter = Random.Range(_params.RangeOfSizes.x, _params.RangeOfSizes.y);
        }
        public void ToFly(TrajectoryBall trajectory)
        {
            _trajectory = trajectory;
        }
        public void ContinueFly()
        {
            Pos = _trajectory.GetPosForTime(Time.time);

            DrawerGizmos.Draw(() => {
                Gizmos.color = Color.red;
                foreach (var corner in _trajectory.Corners)
                    Gizmos.DrawSphere(new Vector3(corner.x, corner.y, 0), 0.1f);
            });
        }
    }
}
