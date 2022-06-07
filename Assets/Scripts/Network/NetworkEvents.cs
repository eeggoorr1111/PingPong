using UnityEngine;

namespace PingPong
{
    public enum NetworkEvents : byte
    {
        PrepareForGame,
        ClientReadyToGame,
        StartGame,

        MovedRacket,
        ReflectBall,
        LosedBall
    }
}
