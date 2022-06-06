using UnityEngine;
using System;

namespace PingPong.Model.Ball
{
    [System.Serializable]
    public sealed class BallParams
    {
        public static int SizeBytes => 16;
        public static object Deserialize(byte[] bytes, int fromByte)
        {
            float minSize = BitConverter.ToSingle(bytes, fromByte);
            float maxSize = BitConverter.ToSingle(bytes, fromByte + 4);

            float minSpeed = BitConverter.ToSingle(bytes, fromByte + 8);
            float maxSpeed = BitConverter.ToSingle(bytes, fromByte + 12);

            return new BallParams(  new Vector2(minSize, maxSize), 
                                    new Vector2(minSpeed, maxSpeed));
        }
        public static byte[] Serialize(BallParams obj)
        {
            byte[] bytes = new byte[SizeBytes];

            BitConverter.GetBytes(obj.RangeOfSizes.x).CopyTo(bytes, 0);
            BitConverter.GetBytes(obj.RangeOfSizes.y).CopyTo(bytes, 4);

            BitConverter.GetBytes(obj.RangeOfSpeeds.x).CopyTo(bytes, 8);
            BitConverter.GetBytes(obj.RangeOfSpeeds.y).CopyTo(bytes, 12);

            return bytes;
        }


        private BallParams() { }
        public BallParams(Vector2 rangeOfSizes, Vector2 rangeOfSpeeds)
        {
            _rangeOfSizes = rangeOfSizes;
            _rangeOfSpeeds = rangeOfSpeeds;
        }


        public Vector2 RangeOfSizes => _rangeOfSizes;
        public Vector2 RangeOfSpeeds => _rangeOfSpeeds;


        [SerializeField] private Vector2 _rangeOfSizes;
        [SerializeField] private Vector2 _rangeOfSpeeds;
    }
}
