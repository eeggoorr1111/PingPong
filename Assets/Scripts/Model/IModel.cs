using PingPong.Model.Racket;
using PingPong.Model.Ball;
using PingPong.Model.Player;
using PingPong;
using System;


namespace PingPong.Model
{
    public interface IModel : IDisposable
    {
        event Action<ReflectBallData> ReflectedBall;
        event Action<LosedBallData> LoseBall;


        RacketModel MeRacket { get; }
        RacketModel OpponentRacket { get; }
        BallModel Ball { get; }
        IPlayer PlayerMe { get; }
        IPlayer PlayerOpponent { get; }


        void MoveRacket(float newPos);
        void NextFrame();
    }
}
