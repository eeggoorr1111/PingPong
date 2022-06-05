using UnityEngine;
using PingPong.Model.Ball;
using PingPong.Model.Racket;
using PingPong.Model;



namespace PingPong
{
    [System.Serializable]
    public sealed class GameConfig
    {
        public BallParams BallParams
        {
            get { return _ballParams; }
            set { _ballParams = value; }
        }
        public RacketParams RacketParams
        {
            get { return _racketParams; }
            set { _racketParams = value; }
        }
        public Map Map
        {
            get { return _map; }
            set { _map = value; }
        }
        public float AllowableError => _allowableError;
        public DataBase DataBase => _dataBase;


        [SerializeField] private float _allowableError;
        [SerializeField] private BallParams _ballParams;
        [SerializeField] private RacketParams _racketParams;
        [SerializeField] private Map _map;
        [SerializeField] private DataBase _dataBase;
    }
}
