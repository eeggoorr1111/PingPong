using UnityEngine;
using PingPong.Model.Ball;
using PingPong.Model.Racket;
using PingPong.Model.Player;


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
        public IModelPingPong NewLocalGame()
        {
            RacketParams pRacket = _config.RacketParams;
            BallParams pBall = _config.BallParams;

            Map map = _config.Map;
            BallModel ball = new BallModel(pBall);
            RacketModel racket1 = new RacketModel(map, pRacket, ball, false, _config.AllowableError);
            RacketModel racket2 = new RacketModel(map, pRacket, ball, true, _config.AllowableError);
            
            PlayerModel me = new PlayerModel(_config.DataBase);
            TrajectoryBallBuilder trajectoryBuilder = new TrajectoryBallBuilder(map, pRacket, ball, _config.AllowableError);

            return new ModelPingPongLocal((me, racket1), (me, racket2), ball, map, trajectoryBuilder);
        }
    }
}
