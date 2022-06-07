using UnityEngine;
using System.Collections.Generic;
using System;
using Narratore.Helpers;

namespace PingPong.Model.Ball
{
    public sealed class TrajectoryBall
    {
        public static TrajectoryBall Deserialize(byte[] bytes, int startByte)
        {
            double timeBeginFly = BitConverter.ToDouble(bytes, startByte);
            startByte += timeBeginFly.Sizeof();

            double timeEndFly = BitConverter.ToDouble(bytes, startByte);
            startByte += timeEndFly.Sizeof();

            int countCorners = BitConverter.ToInt32(bytes, startByte);
            startByte += countCorners.Sizeof();

            Vector2[] corners = new Vector2[countCorners];
            for (short i = 0; i < countCorners; i++)
            {
                float x = BitConverter.ToSingle(bytes, startByte);
                startByte += x.Sizeof();

                float y = BitConverter.ToSingle(bytes, startByte);
                startByte += y.Sizeof();

                corners[i] = new Vector2(x, y);
            }

            return new TrajectoryBall(corners, timeBeginFly, timeEndFly);
        }
        public static byte[] Serialize(TrajectoryBall trajectory)
        {
            List<byte> bytes = new List<byte>(trajectory.Sizeof);

            bytes.AddRange(BitConverter.GetBytes(trajectory.TimeBeginFly));
            bytes.AddRange(BitConverter.GetBytes(trajectory.TimeEndFly));
            bytes.AddRange(BitConverter.GetBytes(trajectory.Corners.Count));

            for (short i = 0; i < trajectory.Corners.Count; i++)
            {
                Vector2 corner = trajectory.Corners[i];

                bytes.AddRange(BitConverter.GetBytes(corner.x));
                bytes.AddRange(BitConverter.GetBytes(corner.y));
            }

            return bytes.ToArray();
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
        public int Sizeof => _corners.Sizeof(true) + TimeBeginFly.Sizeof() + TimeEndFly.Sizeof();


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
    }
}
