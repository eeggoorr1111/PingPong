using UnityEngine;
using System.Collections.Generic;
using Narratore.Primitives;

namespace PingPong.Model.Racket
{
    public sealed class RacketModel
    {
        private static readonly int _sizeHistory = 10;


        public RacketModel(Map map, RacketParams parameters, bool isTop, float allowableError)
        {
            _map = map;
            _minAngleRicochet = parameters.MinAngleRicochet;
            _historyPos = new Queue<float>();

            Width = parameters.Size.x;
            Thickness = parameters.Size.y;

            _isTop = isTop;
            _posX = _map.Center.x;
            _posY = _isTop ? _map.MaxPoint.y - HalfThickness : 
                             _map.MinPoint.y + HalfThickness;

            _ricochetSurface = new Segment(allowableError);
            UpdateRicochetSurface();
        }



        public float Thickness { get; }
        public float HalfThickness => Thickness / 2;
        public Vector2 Pos => new Vector2(_posX, _posY);
        public float Width { get; private set; }
        public Segment RicochetSurface => _ricochetSurface;
        public Vector2 Size => new Vector2(Width, Thickness);
        public float HalfWidth => Width / 2;
        public IReadOnlyCollection<float> HistoryPos => _historyPos;
        public float MaxAngleRicochet => 180 - _minAngleRicochet;


        private readonly Map _map;
        private readonly Queue<float> _historyPos;
        private readonly float _minAngleRicochet;
        private readonly bool _isTop;

        private Segment _ricochetSurface;
        private float _posY;
        private float _posX;
       


        public void Move(float newPos)
        {
            (float, float) extremums = GetExtremumsByX(newPos);

            if (extremums.Item1 > _map.MinPoint.x && extremums.Item2 < _map.MaxPoint.x)
            {
                if (_historyPos.Count < _sizeHistory)
                    _historyPos.Enqueue(_posX);
                else
                {
                    _historyPos.Enqueue(_posX);
                    _historyPos.Dequeue();
                }

                _posX = newPos;
                UpdateRicochetSurface();
            }
        }
        public Vector2 GetRicochetDir(float posCollisionByX)
        {
            float posOnRacket = (posCollisionByX - _ricochetSurface.Point1.x) / Width;
            float angleRicochet = Mathf.Lerp(MaxAngleRicochet, _minAngleRicochet, posOnRacket);
            float angleRicochetRad = angleRicochet * Mathf.Deg2Rad;
            Vector2 ricochet = new Vector2(Mathf.Cos(angleRicochetRad), Mathf.Sin(angleRicochetRad));

            if (_isTop)
                return new Vector2(ricochet.x, -ricochet.y);

            return ricochet;
        }


        private (float, float) GetExtremumsByX(float forPosition)
        {
            return (forPosition - HalfWidth, forPosition + HalfWidth);
        }
        private void UpdateRicochetSurface()
        {
            (float, float) extremums = GetExtremumsByX(_posX);
            float posY = _isTop ? _posY - HalfThickness : _posY + HalfThickness;
            Vector2 point1 = new Vector2(extremums.Item1, posY);
            Vector2 point2 = new Vector2(extremums.Item2, posY);

            _ricochetSurface.Change(point1, point2);
        }
    }
}
