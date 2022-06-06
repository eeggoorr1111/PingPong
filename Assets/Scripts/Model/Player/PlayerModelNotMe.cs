using UnityEngine;

namespace PingPong.Model.Player
{
    /// <summary>
    /// Используется только в игре по сети. Для оппонента текущего игрока
    /// </summary>
    public sealed class PlayerModelNotMe : IPlayer
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
