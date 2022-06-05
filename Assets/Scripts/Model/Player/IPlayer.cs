using UnityEngine;

namespace PingPong.Model.Player
{
    public interface IPlayer
    {
        int RecordReflectedBalls { get; }
        int ReflectedBalls { get; }


        void ReflectedBall();
        void LoseBall();
    }
}
