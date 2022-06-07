using UnityEngine;
using Narratore.Helpers;
using System;
using System.Collections.Generic;


namespace PingPong.Model
{
    [System.Serializable]
    public sealed class MapData
    {
        public static object Deserialize(byte[] bytes, int fromByte)
        {
            float minPointX = BitConverter.ToSingle(bytes, fromByte);
            fromByte += minPointX.Sizeof();

            float minPointY = BitConverter.ToSingle(bytes, fromByte);
            fromByte += minPointY.Sizeof();

            float maxPointX = BitConverter.ToSingle(bytes, fromByte);
            fromByte += maxPointX.Sizeof();

            float maxPointY = BitConverter.ToSingle(bytes, fromByte);

            return new MapData( new Vector2(minPointX, minPointY), 
                                new Vector2(maxPointX, maxPointY));
        }
        public static byte[] Serialize(MapData obj)
        {
            List<byte> bytes = new List<byte>(obj.Sizeof);

            bytes.AddRange(BitConverter.GetBytes(obj.MinPoint.x));
            bytes.AddRange(BitConverter.GetBytes(obj.MinPoint.y));

            bytes.AddRange(BitConverter.GetBytes(obj.MaxPoint.x));
            bytes.AddRange(BitConverter.GetBytes(obj.MaxPoint.y));

            return bytes.ToArray();
        }


        private MapData() {}
        public MapData(Vector2 minPoint, Vector2 maxPoint)
        {
            _minPoint = minPoint;
            _maxPoint = maxPoint;
        }


        public Vector2 MinPoint => _minPoint;
        public Vector2 MaxPoint => _maxPoint;
        public int Sizeof => _minPoint.Sizeof() + _maxPoint.Sizeof();



        [SerializeField] private Vector2 _minPoint;
        [SerializeField] private Vector2 _maxPoint;
    }
}
