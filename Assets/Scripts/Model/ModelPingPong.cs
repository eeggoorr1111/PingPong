using UnityEngine;
using System.Collections.Generic;
using PingPong.Model.Racket;
using PingPong.Model.Ball;

namespace PingPong.Model
{
    public sealed class ModelPingPongMaster : IModelPingPong
    {
        public ModelPingPongMaster( RacketModel racket1, 
                                    RacketModel racket2, 
                                    BallModel ball, 
                                    Player playerMe, 
                                    Player playerOpponent,
                                    TrajectoryBallBuilder trajectoryBuilder)
        {
            Racket1 = racket1;
            Racket2 = racket2;
            Ball = ball;
            PlayerMe = playerMe;
            PlayerOpponent = playerOpponent;

            _tranjectoryBuilder = trajectoryBuilder;
            _racketsOfPlayers = new (Player, RacketModel)[2]
            {
                (PlayerMe, Racket1),
                (PlayerOpponent, Racket2)
            };

            NewGame();
        }
        public ModelPingPongMaster( RacketModel racket1,
                                    RacketModel racket2,
                                    BallModel ball,
                                    Player playerMe,
                                    TrajectoryBallBuilder trajectoryBuilder)
        {
            Racket1 = racket1;
            Racket2 = racket2;
            Ball = ball;
            PlayerMe = playerMe;

            _tranjectoryBuilder = trajectoryBuilder;
            _racketsOfPlayers = new (Player, RacketModel)[2]
            {
                (PlayerMe, Racket1),
                (PlayerMe, Racket2)
            };

            NewGame();
        }


        public RacketModel Racket1 { get; }
        public RacketModel Racket2 { get; }
        public BallModel Ball { get; }
        public Player PlayerMe { get; }
        public Player PlayerOpponent { get; }

        private readonly TrajectoryBallBuilder _tranjectoryBuilder;
        private readonly (Player, RacketModel)[] _racketsOfPlayers;


        public void MoveRacket(float newPos)
        {
            List<RacketModel> rackets = GetRacketsOf(PlayerMe);

            for (int i = 0; i < rackets.Count; i++)
                rackets[i].Move(newPos);
        }
        public void NextFrame()
        {
            Ball.ContinueFly();
        }


        private List<RacketModel> GetRacketsOf(Player player)
        {
            List<RacketModel> rackets = new List<RacketModel>();

            for (int i = 0; i < _racketsOfPlayers.Length; i++)
                if (player == _racketsOfPlayers[i].Item1)
                    rackets.Add(_racketsOfPlayers[i].Item2);

            return rackets;
        }
        private void NewGame()
        {
            TrajectoryBall trajectory = _tranjectoryBuilder.FlyFromCenterToRandomDir();

            Ball.ToFly(trajectory);
        }
    }
}
