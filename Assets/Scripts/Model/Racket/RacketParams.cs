using UnityEngine;

namespace PingPong.Model.Racket
{
    [System.Serializable]
    public sealed class RacketParams
    {
        public Vector2 Size => _size;
        public float MinAngleRicochet => _minAngleRicochet;


        [SerializeField] private Vector2 _size;
        [SerializeField] private float _minAngleRicochet;
    }
}
