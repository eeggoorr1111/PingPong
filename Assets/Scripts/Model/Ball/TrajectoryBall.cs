using UnityEngine;
using System.Collections.Generic;
using System;

namespace PingPong.Model.Ball
{
    public sealed class TrajectoryBall
    {
        public static TrajectoryBall Deserialize(byte[] bytes, int startByte)
        {
            double timeBeginFly = BitConverter.ToDouble(bytes, startByte);
            double timeEndFly = BitConverter.ToDouble(bytes, startByte + 8);
            short countCorners = BitConverter.ToInt16(bytes, startByte + 16);
            Vector2[] corners = new Vector2[countCorners];

            startByte += 18;
            for (short i = 0; i < countCorners; i++)
            {
                float x = BitConverter.ToSingle(bytes, startByte);
                float y = BitConverter.ToSingle(bytes, startByte + 4);

                corners[i] = new Vector2(x, y);
                startByte += 8;
            }

            return new TrajectoryBall(corners, timeBeginFly, timeEndFly);
        }
        


        public TrajectoryBall(Vector2[] corners, double timeBeginFly, double timeEndFly)
        {
            _corners = corners;

            TimeBeginFly = timeBeginFly;
            TimeEndFly = timeEndFly;

            float length = 0f;

            _lengthPartsOfPath = new float[_corners.Length - 1];
            for (int i = 0; i < _lengthPartsOfPath.Length; i++)
            {
                _lengthPartsOfPath[i] = (_corners[i + 1] - _corners[i]).magnitude;
                length += _lengthPartsOfPath[i];
            }

            Length = length;
        }


        public double TimeBeginFly { get; }
        public double TimeEndFly { get; }
        public float Length { get; }
        public double TimeFly => TimeEndFly - TimeBeginFly;
        public IReadOnlyList<Vector2> Corners => _corners;


        private readonly Vector2[] _corners;
        private readonly float[] _lengthPartsOfPath;


        public Vector2 GetPosForTime(double time)
        {
            double normalizedTime = (time - TimeBeginFly) / TimeFly;
            double checkedLength = 0f;

            for (int i = 0; i < _lengthPartsOfPath.Length; i++)
            {
                double normalizedPosCorner = checkedLength / Length;
                double nextLength = checkedLength + _lengthPartsOfPath[i];
                double normalizedPosNextCorner = nextLength / Length;
                double normalizedLengthOfPart = normalizedPosNextCorner - normalizedPosCorner;

                if (normalizedTime > normalizedPosCorner - float.Epsilon &&
                    normalizedTime < normalizedPosNextCorner + float.Epsilon)
                {
                    // TODO: убрать каст во float
                    double normalizedTimeBySegment = (normalizedTime - normalizedPosCorner) / normalizedLengthOfPart;
                    return Vector2.Lerp(_corners[i], _corners[i + 1], (float)normalizedTimeBySegment);
                }

                checkedLength = nextLength;
            }

            if (time > TimeEndFly - float.Epsilon)
                return _corners[_corners.Length - 1];

            if (time < TimeBeginFly + float.Epsilon)
                return _corners[0];

            Debug.LogError($"Не удалось найти точку на траектории полета от времени {time}");

            return new Vector2();
        }
        public int GetSizeInBytes()
        {
            return _corners.Length * 8 + 2 + 8 + 8;
        }
        public byte[] Serialize()
        {
            byte[] bytes = new byte[GetSizeInBytes()];

            BitConverter.GetBytes(TimeBeginFly).CopyTo(bytes, 0);
            BitConverter.GetBytes(TimeEndFly).CopyTo(bytes, 8);
            BitConverter.GetBytes((short)Corners.Count).CopyTo(bytes, 16);

            int startByte = 18;
            for (short i = 0; i < Corners.Count; i++)
            {
                BitConverter.GetBytes(_corners[i].x).CopyTo(bytes, startByte);
                BitConverter.GetBytes(_corners[i].y).CopyTo(bytes, startByte + 4);

                startByte += 8;
            }

            return bytes;
        }
    }
}
