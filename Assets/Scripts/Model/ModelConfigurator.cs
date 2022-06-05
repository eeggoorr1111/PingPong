using UnityEngine;
using PingPong.Model.Ball;
using PingPong.Model.Racket;


namespace PingPong.Model
{
    public sealed class ModelConfigurator : MonoBehaviour
    {
        [SerializeField] private GameConfig _config;


        /// <summary>
        /// Пересоздаем Map, чтобы был вызыван конструктор у Map и были установлены все расчетные данные
        /// </summary>
        public void Init()
        {
            ChangeConfig(new Map(_config.Map.MinPoint, _config.Map.MaxPoint, _config.AllowableError));
        }
        public ModelConfigurator ChangeConfig(Map map)
        {
            _config.Map = map;

            return this;
        }
        public IModelPingPong NewModel(bool isMaster)
        {
            RacketParams pRacket = _config.RacketParams;
            BallParams pBall = _config.BallParams;

            RacketModel racket1 = new RacketModel(_config.Map, pRacket, false, _config.AllowableError);
            RacketModel racket2 = new RacketModel(_config.Map, pRacket, true, _config.AllowableError);
            BallModel ball = new BallModel(pBall);
            Player me = new Player();
            TrajectoryBallBuilder trajectoryBuilder = new TrajectoryBallBuilder(_config.Map, pRacket, ball, _config.AllowableError);


            return new ModelPingPongMaster(racket1, racket2, ball, me, trajectoryBuilder);
        }
    }
}
