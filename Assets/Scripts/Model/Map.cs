using UnityEngine;
using Narratore.Primitives;
using System;


namespace PingPong.Model
{
    [System.Serializable]
    public sealed class Map
    {
        public static int SizeBytes => 20;
        public static object Deserialize(byte[] bytes, int fromByte)
        {
            float minPointX = BitConverter.ToSingle(bytes, fromByte);
            float minPointY = BitConverter.ToSingle(bytes, fromByte + 4);

            float maxPointX = BitConverter.ToSingle(bytes, fromByte + 8);
            float maxPointY = BitConverter.ToSingle(bytes, fromByte + 12);

            float allowableError = BitConverter.ToSingle(bytes, fromByte + 16);

            return new Map( new Vector2(minPointX, minPointY), 
                            new Vector2(maxPointX, maxPointY),
                            allowableError);
        }
        public static byte[] Serialize(Map obj)
        {
            byte[] bytes = new byte[SizeBytes];

            BitConverter.GetBytes(obj.MinPoint.x).CopyTo(bytes, 0);
            BitConverter.GetBytes(obj.MinPoint.y).CopyTo(bytes, 4);

            BitConverter.GetBytes(obj.MaxPoint.x).CopyTo(bytes, 8);
            BitConverter.GetBytes(obj.MaxPoint.y).CopyTo(bytes, 12);

            BitConverter.GetBytes(obj.LeftBorder.AllowableError).CopyTo(bytes, 16);

            return bytes;
        }


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
