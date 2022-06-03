using UnityEngine;

namespace PingPong.Model.Racket
{
    [System.Serializable]
    public sealed class RacketParams
    {
        public Vector2 Size => _size;
        public Vector2 RicochetLeftSide => _ricochetLeftSide;
        public Vector2 RicochetRightSide => _ricochetRightSide;


        [SerializeField] private Vector2 _size;
        [SerializeField] private Vector2 _ricochetLeftSide;
        [SerializeField] private Vector2 _ricochetRightSide;
    }
}
