using UnityEngine;
using System;

namespace PingPong.Model.Racket
{
    [System.Serializable]
    public sealed class RacketParams
    {
        public static int SizeBytes => 12;
        public static object Deserialize(byte[] bytes, int fromByte)
        {
            float sizeX = BitConverter.ToSingle(bytes, fromByte);
            float sizeY = BitConverter.ToSingle(bytes, fromByte + 4);

            float minAngleRicochet = BitConverter.ToSingle(bytes, fromByte + 8);

            return new RacketParams(new Vector2(sizeX, sizeY), minAngleRicochet);
        }
        public static byte[] Serialize(RacketParams obj)
        {
            byte[] bytes = new byte[SizeBytes];

            BitConverter.GetBytes(obj.Size.x).CopyTo(bytes, 0);
            BitConverter.GetBytes(obj.Size.y).CopyTo(bytes, 4);

            BitConverter.GetBytes(obj.MinAngleRicochet).CopyTo(bytes, 8);

            return bytes;
        }


        private RacketParams() { }
        public RacketParams(Vector2 size, float minAngleRicochet) 
        {
            _size = size;
            _minAngleRicochet = minAngleRicochet;
        }


        public Vector2 Size => _size;
        public float MinAngleRicochet => _minAngleRicochet;
        public float MaxAngleRicochet => 180 - _minAngleRicochet;


        [SerializeField] private Vector2 _size;
        [SerializeField] private float _minAngleRicochet;
    }
}
