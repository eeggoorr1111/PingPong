using UnityEngine;
using PingPong.Model.Ball;
using PingPong.Model.Racket;


namespace PingPong.Model
{
    public sealed class ModelConfigurator : MonoBehaviour
    {
        [SerializeField] private GameConfig _config;


        public ModelConfigurator ChangeConfig(Map map)
        {
            _config.Map = map;

            return this;
        }
        public IModelPingPong NewModel(bool isMaster)
        {
            RacketParams pRacket = _config.RacketParams;
            BallParams pBall = _config.BallParams;

            RacketModel racket1 = new RacketModel(_config.Map, pRacket, false);
            RacketModel racket2 = new RacketModel(_config.Map, pRacket, true);
            BallModel ball = new BallModel(pBall);
            Player me = new Player();


            return new ModelPingPongMaster(racket1, racket2, ball, me);
        }
    }
}
