using UnityEngine;
using System;
using Narratore.Helpers;
using System.Collections.Generic;

namespace PingPong.Model.Racket
{
    [System.Serializable]
    public sealed class RacketParams
    {
        public static object Deserialize(byte[] bytes, int fromByte)
        {
            float sizeX = BitConverter.ToSingle(bytes, fromByte);
            fromByte += sizeX.Sizeof();

            float sizeY = BitConverter.ToSingle(bytes, fromByte);
            fromByte += sizeY.Sizeof();

            float minAngleRicochet = BitConverter.ToSingle(bytes, fromByte);

            return new RacketParams(new Vector2(sizeX, sizeY), minAngleRicochet);
        }
        public static byte[] Serialize(RacketParams obj)
        {
            List<byte> bytes = new List<byte>(obj.Sizeof);

            bytes.AddRange(BitConverter.GetBytes(obj.Size.x));
            bytes.AddRange(BitConverter.GetBytes(obj.Size.y));
            bytes.AddRange(BitConverter.GetBytes(obj.MinAngleRicochet));

            return bytes.ToArray();
        }


        private RacketParams() { }
        public RacketParams(Vector2 size, float minAngleRicochet) 
        {
            _size = size;
            _minAngleRicochet = minAngleRicochet;
        }


        public int Sizeof => _size.Sizeof() + _minAngleRicochet.Sizeof();
        public Vector2 Size => _size;
        public float MinAngleRicochet => _minAngleRicochet;
        public float MaxAngleRicochet => 180 - _minAngleRicochet;


        [SerializeField] private Vector2 _size;
        [SerializeField] private float _minAngleRicochet;
    }
}
