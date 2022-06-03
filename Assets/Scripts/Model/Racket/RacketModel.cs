using UnityEngine;
using System.Collections.Generic;

namespace PingPong.Model.Racket
{
    public sealed class RacketModel
    {
        private static readonly int _sizeHistory = 10;


        public RacketModel(Map map, RacketParams parameters, bool isTop)
        {
            _map = map;
            _ricochetLeftSide = parameters.RicochetLeftSide;
            _ricochetRightSide = parameters.RicochetRightSide;
            _historyPos = new Queue<float>();

            Width = parameters.Size.x;
            Thickness = parameters.Size.y;

            _posX = _map.Center.x;
            _posY = isTop ? _map.MaxPoint.y - HalfThickness :
                            _map.MinPoint.y + HalfThickness;
        }



        public float Thickness { get; }
        public float HalfThickness => Thickness / 2;
        public Vector2 Pos => new Vector2(_posX, _posY);
        public float Width { get; private set; }
        public float HalfWidth => Width / 2;
        public IReadOnlyCollection<float> HistoryPos => _historyPos;


        private readonly Map _map;
        private readonly Queue<float> _historyPos;
        private readonly Vector2 _ricochetLeftSide;
        private readonly Vector2 _ricochetRightSide;
        private readonly float _posY;
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
            }
        }
        public Vector2 GetRicochetDir(float posCollisionByX)
        {
            (float, float) extremums = GetExtremumsByX(_posX);
            float posCollision = (posCollisionByX - extremums.Item1) / Width;
            Vector2 ricochet = Vector2.Lerp(_ricochetLeftSide, _ricochetRightSide, Mathf.Clamp01(posCollision));

            return ricochet.normalized;
        }


        private (float, float) GetExtremumsByX(float forPosition)
        {
            return (forPosition - HalfWidth, forPosition + HalfWidth);
        }
    }
}
