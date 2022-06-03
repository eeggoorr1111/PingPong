using PingPong.Model.Racket;
using PingPong.Model.Ball;


namespace PingPong.Model
{
    public interface IModelPingPong
    {
        public RacketModel Racket1 { get; }
        public RacketModel Racket2 { get; }
        public BallModel Ball { get; }
        public Player PlayerMe { get; }
        public Player PlayerOpponent { get; }


        void MoveRacket(float newPos);
        void NextFrame();
    }
}
