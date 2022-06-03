using UnityEngine;

namespace PingPong.Model.Ball
{
    [System.Serializable]
    public sealed class BallParams
    {
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
