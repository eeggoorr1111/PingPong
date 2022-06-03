using UnityEngine;

namespace PingPong.Model
{
    [System.Serializable]
    public sealed class Map
    {
        private Map() { }
        public Map(Vector2 minPoint, Vector2 maxPoint)
        {
            _minPoint = minPoint;
            _maxPoint = maxPoint;
        }


        public Vector2 MinPoint => _minPoint;
        public Vector2 MaxPoint => _maxPoint;
        public Vector2 Center => (MinPoint + MaxPoint) / 2;
        public float Width => MaxPoint.x - MinPoint.x;
        public float HalfWidth => Width / 2;
        public float Height => MaxPoint.y - MinPoint.y;
        public float HalfHeight => Height / 2;


        [SerializeField] private Vector2 _minPoint;
        [SerializeField] private Vector2 _maxPoint;

    }
}
