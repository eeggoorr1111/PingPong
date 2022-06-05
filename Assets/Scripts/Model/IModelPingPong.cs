using PingPong.Model.Racket;
using PingPong.Model.Ball;
using PingPong.Model.Player;


namespace PingPong.Model
{
    public interface IModelPingPong
    {
        public RacketModel MeRacket { get; }
        public RacketModel OpponentRacket { get; }
        public BallModel Ball { get; }
        public IPlayer PlayerMe { get; }
        public IPlayer PlayerOpponent { get; }


        void MoveRacket(float newPos);
        void NextFrame();
    }
}
