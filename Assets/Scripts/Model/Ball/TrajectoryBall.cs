using UnityEngine;
using System.Collections.Generic;

namespace PingPong.Model.Ball
{
    public sealed class TrajectoryBall
    {
        public TrajectoryBall(Vector2[] corners, float timeBeginFly, float timeEndFly)
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


        public float TimeBeginFly { get; }
        public float TimeEndFly { get; }
        public float Length { get; }
        public float TimeFly => TimeEndFly - TimeBeginFly;
        public IReadOnlyCollection<Vector2> Corners => _corners;


        private readonly Vector2[] _corners;
        private readonly float[] _lengthPartsOfPath;


        public Vector2 GetPosForTime(float time)
        {
            float normalizedTime = (time - TimeBeginFly) / TimeFly;
            float checkedLength = 0f;

            for (int i = 0; i < _lengthPartsOfPath.Length; i++)
            {
                float normalizedPosCorner = checkedLength / Length;
                float nextLength = checkedLength + _lengthPartsOfPath[i];
                float normalizedPosNextCorner = nextLength / Length;
                float normalizedLengthOfPart = normalizedPosNextCorner - normalizedPosCorner;

                if (normalizedTime > normalizedPosCorner &&
                    normalizedTime < normalizedPosNextCorner)
                {
                    float normalizedTimeBySegment = (normalizedTime - normalizedPosCorner) / normalizedLengthOfPart;
                    return Vector2.Lerp(_corners[i], _corners[i + 1], normalizedTimeBySegment);
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
