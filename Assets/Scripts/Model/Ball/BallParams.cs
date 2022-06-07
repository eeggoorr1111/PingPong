using UnityEngine;
using System;
using Narratore.Helpers;
using System.Collections.Generic;

namespace PingPong.Model.Ball
{
    [System.Serializable]
    public sealed class BallParams
    {
        public static object Deserialize(byte[] bytes, int fromByte)
        {
            float minSize = BitConverter.ToSingle(bytes, fromByte);
            fromByte += minSize.Sizeof();

            float maxSize = BitConverter.ToSingle(bytes, fromByte);
            fromByte += maxSize.Sizeof();

            float minSpeed = BitConverter.ToSingle(bytes, fromByte);
            fromByte += minSpeed.Sizeof();

            float maxSpeed = BitConverter.ToSingle(bytes, fromByte);

            return new BallParams(  new Vector2(minSize, maxSize), 
                                    new Vector2(minSpeed, maxSpeed));
        }
        public static byte[] Serialize(BallParams obj)
        {
            List<byte> bytes = new List<byte>(obj.Sizeof);

            bytes.AddRange(BitConverter.GetBytes(obj.RangeOfSizes.x));
            bytes.AddRange(BitConverter.GetBytes(obj.RangeOfSizes.y));

            bytes.AddRange(BitConverter.GetBytes(obj.RangeOfSpeeds.x));
            bytes.AddRange(BitConverter.GetBytes(obj.RangeOfSpeeds.y));

            return bytes.ToArray();
        }


        private BallParams() { }
        public BallParams(Vector2 rangeOfSizes, Vector2 rangeOfSpeeds)
        {
            _rangeOfSizes = rangeOfSizes;
            _rangeOfSpeeds = rangeOfSpeeds;
        }


        public int Sizeof => _rangeOfSizes.Sizeof() + _rangeOfSpeeds.Sizeof();
        public Vector2 RangeOfSizes => _rangeOfSizes;
        public Vector2 RangeOfSpeeds => _rangeOfSpeeds;


        [SerializeField] private Vector2 _rangeOfSizes;
        [SerializeField] private Vector2 _rangeOfSpeeds;
    }
}
