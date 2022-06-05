using UnityEngine;
using Narratore.Primitives;


namespace PingPong.Model
{
    [System.Serializable]
    public sealed class Map
    {
        private Map() {}
        public Map(Vector2 minPoint, Vector2 maxPoint, float allowableError)
        {
            _minPoint = minPoint;
            _maxPoint = maxPoint;

            LeftBorder = new Segment(_minPoint, new Vector2(_minPoint.x, _maxPoint.y), allowableError);
            RightBorder = new Segment(new Vector2(_maxPoint.x, _minPoint.y), _maxPoint, allowableError);
            BottomBorder = new Segment(_minPoint, new Vector2(_maxPoint.x, _minPoint.y), allowableError);
            TopBorder = new Segment(new Vector2(_minPoint.x, _maxPoint.y), _maxPoint, allowableError);

            Diagonal = (_maxPoint - _minPoint).magnitude;
        }


        public Vector2 MinPoint => _minPoint;
        public Vector2 MaxPoint => _maxPoint;
        public Vector2 Center => (MinPoint + MaxPoint) / 2;
        public float Width => MaxPoint.x - MinPoint.x;
        public float HalfWidth => Width / 2;
        public float Height => MaxPoint.y - MinPoint.y;
        public float HalfHeight => Height / 2;
        public float Area => Width * Height;
        public float Diagonal { get; }
        public Segment LeftBorder { get; }
        public Segment RightBorder { get; }
        public Segment BottomBorder { get; }
        public Segment TopBorder { get; }
       


        [SerializeField] private Vector2 _minPoint;
        [SerializeField] private Vector2 _maxPoint;
    }
}
