using UnityEngine;

namespace PingPong.Model.Player
{
    public interface IPlayer
    {
        int RecordReflectedBalls { get; }
        int ReflectedBalls { get; }
        int Id { get; }


        void ReflectedBall();
        void LoseBall();
    }
}
