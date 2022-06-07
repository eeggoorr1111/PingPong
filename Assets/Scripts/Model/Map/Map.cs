using UnityEngine;
using Narratore.Primitives;


namespace PingPong.Model
{
    public sealed class Map
    {
        public Map(MapData data, float allowableError)
        {
            _data = data;

            LeftBorder = new Segment(MinPoint, new Vector2(MinPoint.x, MaxPoint.y), allowableError);
            RightBorder = new Segment(new Vector2(MaxPoint.x, MinPoint.y), MaxPoint, allowableError);
            BottomBorder = new Segment(MinPoint, new Vector2(MaxPoint.x, MinPoint.y), allowableError);
            TopBorder = new Segment(new Vector2(MinPoint.x, MaxPoint.y), MaxPoint, allowableError);

            Diagonal = (MaxPoint - MinPoint).magnitude;
        }


        public Vector2 MinPoint => _data.MinPoint;
        public Vector2 MaxPoint => _data.MaxPoint;
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


        private readonly MapData _data;
    }
}
