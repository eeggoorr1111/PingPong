using PingPong.Database;
using PingPong.Model.Ball;
using PingPong.Model.Player;
using PingPong.Model.Racket;
using PingPong.Network;
using System;
using UnityEngine;



namespace PingPong.Model
{
    public sealed class ModelConfigurator : MonoBehaviour
    {
        public ModelConfigData Config => _localConfig;


        [SerializeField] private ModelConfigData _localConfig;
        [SerializeField] private DatabaseProvider _dataBase;


        public ModelConfigurator ChangeConfig(MapData map)
        {
            _localConfig.MapData = map;

            return this;
        }
        public ModelClient NewNetworkGameAsClient(ModelConfigData configFromMaster)
        {
            RacketParams pRacket = configFromMaster.RacketParams;
            BallParams pBall = configFromMaster.BallParams;
            float allowableError = configFromMaster.AllowableError;
            Map map = new Map(configFromMaster.MapData, allowableError);

            TimeCounterNetwork timeCounter = new TimeCounterNetwork();

            BallModel ball = new BallModel(timeCounter.GetTime);
            RacketModel topRacket = new RacketModel(map, pRacket, ball, true, allowableError);
            RacketModel bottomRacket = new RacketModel(map, pRacket, ball, false, allowableError);
            TrajectoryBallBuilder trajectoryBuilder = new TrajectoryBallBuilder(map, pRacket, ball, timeCounter.GetTime, allowableError);

            IPlayer me = new PlayerModel(2, _dataBase);
            IPlayer notMe = new PlayerModelNotMe(1);

            ModelClient model = new ModelClient((me, topRacket), (notMe, bottomRacket), ball, trajectoryBuilder, timeCounter);

            return model;
        }
        public ModelMaster NewNetworkGameAsMaster()
        {
            RacketParams pRacket = _localConfig.RacketParams;
            BallParams pBall = _localConfig.BallParams;
            float allowableError = _localConfig.AllowableError;
            Map map = new Map(_localConfig.MapData, allowableError);

            TimeCounterNetwork timeCounter = new TimeCounterNetwork();

            BallModel ball = new BallModel(timeCounter.GetTime);
            RacketModel topRacket = new RacketModel(map, pRacket, ball, true, allowableError);
            RacketModel bottomRacket = new RacketModel(map, pRacket, ball, false, allowableError);
            TrajectoryBallBuilder trajectoryBuilder = new TrajectoryBallBuilder(map, pRacket, ball, timeCounter.GetTime, allowableError);
            
            IPlayer me = new PlayerModel(1, _dataBase);
            IPlayer notMe = new PlayerModelNotMe(2);

            ModelLocal modelLocal = new ModelLocal((me, bottomRacket), (notMe, topRacket), ball, pBall, map, trajectoryBuilder);
            ModelMaster modelMaster = new ModelMaster(modelLocal, timeCounter);

            return modelMaster;
        }
        public ModelLocal NewLocalGame()
        {
            RacketParams pRacket = _localConfig.RacketParams;
            BallParams pBall = _localConfig.BallParams;
            float allowableError = _localConfig.AllowableError;
            Map map = new Map(_localConfig.MapData, allowableError);

            Func<double> timeGetter = () => Time.time;

            BallModel ball = new BallModel(timeGetter);
            RacketModel topRacket = new RacketModel(map, pRacket, ball, true, allowableError);
            RacketModel bottomRacket = new RacketModel(map, pRacket, ball, false, allowableError);
            TrajectoryBallBuilder trajectoryBuilder = new TrajectoryBallBuilder(map, pRacket, ball, timeGetter, allowableError);

            PlayerModel me = new PlayerModel(0, _dataBase);

            return new ModelLocal((me, topRacket), (me, bottomRacket), ball, pBall, map, trajectoryBuilder);
        }
    }
}
