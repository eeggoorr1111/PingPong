using UnityEngine;
using PingPong.Model.Ball;
using PingPong.Model.Racket;
using PingPong.Model.Player;
using PingPong.Network;
using System;
using Photon.Realtime;
using Photon.Pun;



namespace PingPong.Model
{
    public sealed class ModelConfigurator : MonoBehaviour
    {
        public ModelConfigData Config => _localConfig;


        [SerializeField] private ModelConfigData _localConfig;
        private IOnEventCallback _lastNetworkModel;


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
        public IModelPingPong NewNetworkGameAsClient(ModelConfigData configFromMaster)
        {
            RacketParams pRacket = configFromMaster.RacketParams;
            BallParams pBall = configFromMaster.BallParams;
            float allowableError = configFromMaster.AllowableError;
            Map map = configFromMaster.Map;

            TimeCounterNetwork timeCounter = new TimeCounterNetwork();

            BallModel ball = new BallModel(pBall, timeCounter.GetTime);
            RacketModel topRacket = new RacketModel(map, pRacket, ball, true, allowableError);
            RacketModel bottomRacket = new RacketModel(map, pRacket, ball, false, allowableError);
            TrajectoryBallBuilder trajectoryBuilder = new TrajectoryBallBuilder(map, pRacket, ball, timeCounter.GetTime, allowableError);

            IPlayer me = new PlayerModel(_localConfig.DataBase);
            IPlayer notMe = new PlayerModelNotMe();

            UnsubscribeLastNetworkModel();
            ModelClient model = new ModelClient((me, topRacket), (notMe, bottomRacket), ball, trajectoryBuilder, timeCounter);
            SubscribeNetworkModel(model);

            return model;
        }
        public IModelPingPong NewNetworkGameAsMaster()
        {
            RacketParams pRacket = _localConfig.RacketParams;
            BallParams pBall = _localConfig.BallParams;
            Map map = _localConfig.Map;
            float allowableError = _localConfig.AllowableError;

            TimeCounterNetwork timeCounter = new TimeCounterNetwork();

            BallModel ball = new BallModel(pBall, timeCounter.GetTime);
            RacketModel topRacket = new RacketModel(map, pRacket, ball, true, allowableError);
            RacketModel bottomRacket = new RacketModel(map, pRacket, ball, false, allowableError);
            TrajectoryBallBuilder trajectoryBuilder = new TrajectoryBallBuilder(map, pRacket, ball, timeCounter.GetTime, allowableError);

            IPlayer me = new PlayerModel(_localConfig.DataBase);
            IPlayer notMe = new PlayerModelNotMe();

            UnsubscribeLastNetworkModel();
            ModelLocal modelLocal = new ModelLocal((me, bottomRacket), (notMe, topRacket), ball, map, trajectoryBuilder);
            ModelMaster modelMaster = new ModelMaster(modelLocal, timeCounter);
            SubscribeNetworkModel(modelMaster);

            return modelMaster;
        }
        public IModelPingPong NewLocalGame()
        {
            UnsubscribeLastNetworkModel();

            RacketParams pRacket = _localConfig.RacketParams;
            BallParams pBall = _localConfig.BallParams;
            Map map = _localConfig.Map;
            float allowableError = _localConfig.AllowableError;

            Func<double> timeGetter = () => Time.time;

            BallModel ball = new BallModel(pBall, timeGetter);
            RacketModel topRacket = new RacketModel(map, pRacket, ball, true, allowableError);
            RacketModel bottomRacket = new RacketModel(map, pRacket, ball, false, allowableError);
            TrajectoryBallBuilder trajectoryBuilder = new TrajectoryBallBuilder(map, pRacket, ball, timeGetter, allowableError);

            PlayerModel me = new PlayerModel(_localConfig.DataBase);

            return new ModelLocal((me, topRacket), (me, bottomRacket), ball, map, trajectoryBuilder);
        }


        private void SubscribeNetworkModel(IOnEventCallback model)
        {
            _lastNetworkModel = model;
            PhotonNetwork.AddCallbackTarget(model);
        }
        private void UnsubscribeLastNetworkModel()
        {
            if (_lastNetworkModel != null)
                PhotonNetwork.RemoveCallbackTarget(_lastNetworkModel);
        }
    }
}
