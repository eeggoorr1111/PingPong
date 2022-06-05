using UnityEngine;

namespace PingPong.Model.Player
{
    /// <summary>
    /// Используется только в игре по сети, на стороне клиента
    /// </summary>
    public sealed class PlayerModelClient : IPlayer
    {
        public int RecordReflectedBalls { get; private set; }
        public int ReflectedBalls { get; private set; }


        public void ReflectedBall()
        {
            ReflectedBalls++;

            if (ReflectedBalls > RecordReflectedBalls)
                RecordReflectedBalls = ReflectedBalls;
        }
        public void LoseBall()
        {
            ReflectedBalls = 0;
        }
    }
}
