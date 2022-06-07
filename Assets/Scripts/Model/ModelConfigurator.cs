using UnityEngine;
using PingPong.Model.Ball;
using PingPong.Model.Racket;
using PingPong.Model.Player;
using PingPong.Network;
using System;
using Random = UnityEngine.Random;



namespace PingPong.Model
{
    public sealed class ModelConfigurator : MonoBehaviour
    {
        public ModelConfigData Config => _localConfig;


        [SerializeField] private ModelConfigData _localConfig;


        /// <summary>
        /// Пересоздаем Map, чтобы был вызыван конструктор у Map и были установлены все расчетные данные
        /// </summary>
        public void Init()
        {
            ChangeConfig(new Map(_localConfig.Map.MinPoint, _localConfig.Map.MaxPoint, _localConfig.AllowableError));
        }
        public ModelConfigurator ChangeConfig(Map map)
        {
            _localConfig.Map = map;

            return this;
        }
        public ModelClient NewNetworkGameAsClient(ModelConfigData configFromMaster)
        {
            RacketParams pRacket = configFromMaster.RacketParams;
            BallParams pBall = configFromMaster.BallParams;
            float allowableError = configFromMaster.AllowableError;
            Map map = configFromMaster.Map;

            TimeCounterNetwork timeCounter = new TimeCounterNetwork();

            BallModel ball = new BallModel(timeCounter.GetTime);
            RacketModel topRacket = new RacketModel(map, pRacket, ball, true, allowableError);
            RacketModel bottomRacket = new RacketModel(map, pRacket, ball, false, allowableError);
            TrajectoryBallBuilder trajectoryBuilder = new TrajectoryBallBuilder(map, pRacket, ball, timeCounter.GetTime, allowableError);

            IPlayer me = new PlayerModel(_localConfig.DataBase);
            IPlayer notMe = new PlayerModelNotMe();

            ModelClient model = new ModelClient((me, topRacket), (notMe, bottomRacket), ball, trajectoryBuilder, timeCounter);

            return model;
        }
        public ModelMaster NewNetworkGameAsMaster()
        {
            RacketParams pRacket = _localConfig.RacketParams;
            BallParams pBall = _localConfig.BallParams;
            Map map = _localConfig.Map;
            float allowableError = _localConfig.AllowableError;

            TimeCounterNetwork timeCounter = new TimeCounterNetwork();

            BallModel ball = new BallModel(timeCounter.GetTime);
            RacketModel topRacket = new RacketModel(map, pRacket, ball, true, allowableError);
            RacketModel bottomRacket = new RacketModel(map, pRacket, ball, false, allowableError);
            TrajectoryBallBuilder trajectoryBuilder = new TrajectoryBallBuilder(map, pRacket, ball, timeCounter.GetTime, allowableError);

            IPlayer me = new PlayerModel(_localConfig.DataBase);
            IPlayer notMe = new PlayerModelNotMe();

            ModelLocal modelLocal = new ModelLocal((me, bottomRacket), (notMe, topRacket), ball, pBall, map, trajectoryBuilder);
            ModelMaster modelMaster = new ModelMaster(modelLocal, timeCounter);

            return modelMaster;
        }
        public ModelLocal NewLocalGame()
        {
            RacketParams pRacket = _localConfig.RacketParams;
            BallParams pBall = _localConfig.BallParams;
            Map map = _localConfig.Map;
            float allowableError = _localConfig.AllowableError;

            Func<double> timeGetter = () => Time.time;

            BallModel ball = new BallModel(timeGetter);
            RacketModel topRacket = new RacketModel(map, pRacket, ball, true, allowableError);
            RacketModel bottomRacket = new RacketModel(map, pRacket, ball, false, allowableError);
            TrajectoryBallBuilder trajectoryBuilder = new TrajectoryBallBuilder(map, pRacket, ball, timeGetter, allowableError);

            PlayerModel me = new PlayerModel(_localConfig.DataBase);

            return new ModelLocal((me, topRacket), (me, bottomRacket), ball, pBall, map, trajectoryBuilder);
        }
    }
}
