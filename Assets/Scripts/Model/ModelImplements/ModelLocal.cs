using UnityEngine;
using System.Collections.Generic;
using PingPong.Model.Racket;
using PingPong.Model.Ball;
using Narratore.Primitives;
using PingPong.Model.Player;
using System;
using Random = UnityEngine.Random;


namespace PingPong.Model
{
    public sealed class ModelLocal : IModel
    {
        public ModelLocal(  (IPlayer, RacketModel) meWithRacket,
                            (IPlayer, RacketModel) opponentWithRacket,
                            BallModel ball, 
                            BallParams pBall,
                            Map map,
                            TrajectoryBallBuilder trajectoryBuilder)
        {
            PlayerMe = meWithRacket.Item1;
            MeRacket = meWithRacket.Item2;

            PlayerOpponent = opponentWithRacket.Item1;
            OpponentRacket = opponentWithRacket.Item2;

            _pBall = pBall;
            Ball = ball;

            _map = map;
            
            _tranjectoryBuilder = trajectoryBuilder;
            _racketsOfPlayers = new (IPlayer, RacketModel)[2]
            {
                meWithRacket,
                opponentWithRacket
            };

            ReflectedBall += data => { };
            LoseBall += data => { };

            NewRound();
        }


        public event Action<DataReflectBall> ReflectedBall;
        public event Action<DataLosedBall> LoseBall;


        public RacketModel MeRacket { get; }
        public RacketModel OpponentRacket { get; }
        public IPlayer PlayerMe { get; }
        public IPlayer PlayerOpponent { get; }
        public BallModel Ball { get; }


        private readonly Map _map;
        private readonly TrajectoryBallBuilder _tranjectoryBuilder;
        private readonly (IPlayer, RacketModel)[] _racketsOfPlayers;
        private readonly BallParams _pBall;
        private RacketModel _lastRicochet;


        public void MoveRacket(float newPos)
        {
            List<RacketModel> rackets = GetRacketsOf(PlayerMe);

            for (int i = 0; i < rackets.Count; i++)
                rackets[i].Move(newPos);
        }
        public void NextFrame()
        {
            Vector2 ricochetDir;

            if (_lastRicochet == OpponentRacket && IsCollisionBallWith(MeRacket, out ricochetDir))
            {
                _lastRicochet = MeRacket;
                Ball.ToFly(_tranjectoryBuilder.Create(Ball.Pos, ricochetDir));
                PlayerMe.ReflectedBall();

                ReflectedBall.Invoke(new DataReflectBall(false, Ball.Trajectory));
            }
            else if (_lastRicochet == MeRacket && IsCollisionBallWith(OpponentRacket, out ricochetDir))
            {
                _lastRicochet = OpponentRacket;
                Ball.ToFly(_tranjectoryBuilder.Create(Ball.Pos, ricochetDir));
                PlayerOpponent.ReflectedBall();

                ReflectedBall.Invoke(new DataReflectBall(true, Ball.Trajectory));
            }
            else if (IsCollisionBallWith(_map.BottomBorder))
            {
                PlayerMe.LoseBall();
                NewRound();

                LoseBall.Invoke(new DataLosedBall(false, Ball.Diameter, Ball.Speed, Ball.Trajectory));
            }
            else if (IsCollisionBallWith(_map.TopBorder))
            {
                PlayerOpponent.LoseBall();
                NewRound();

                LoseBall.Invoke(new DataLosedBall(true, Ball.Diameter, Ball.Speed, Ball.Trajectory));
            }   

            Ball.ContinueFly();
        }


        private List<RacketModel> GetRacketsOf(IPlayer player)
        {
            List<RacketModel> rackets = new List<RacketModel>();

            for (int i = 0; i < _racketsOfPlayers.Length; i++)
                if (player == _racketsOfPlayers[i].Item1)
                    rackets.Add(_racketsOfPlayers[i].Item2);

            return rackets;
        }
        private void NewRound()
        {
            bool flyToTop = Random.Range(0f, 1f) > 0.5f;

            NewBall();
            Ball.ToFly(_tranjectoryBuilder.FlyFromCenterToRandomDir(flyToTop));

            _lastRicochet = flyToTop == MeRacket.IsTop ? OpponentRacket : MeRacket;
        }
        private bool IsCollisionBallWith(RacketModel racket, out Vector2 ricochetDir)
        {
            ricochetDir = new Vector2();
            
            if (racket.RicochetSurface.GetIntersectWithPerpendicularFromPoint(Ball.Pos, out Vector2 intersect))
            {
                float distanceToBall = (Ball.Pos - intersect).magnitude;
                if (distanceToBall < Ball.Radius)
                {
                    ricochetDir = racket.GetRicochetDir(intersect.x);
                    return true;
                }
            }

            return false;
        }
        private bool IsCollisionBallWith(Segment border)
        {
            if (border.GetIntersectWithPerpendicularFromPoint(Ball.Pos, out Vector2 intersect))
            {
                float distanceToBall = (Ball.Pos - intersect).magnitude;
                if (distanceToBall < Ball.Radius)
                    return true;
            }

            return false;
        }
        private void NewBall()
        {
            float speed = Random.Range(_pBall.RangeOfSpeeds.x, _pBall.RangeOfSpeeds.y);
            float diameter = Random.Range(_pBall.RangeOfSizes.x, _pBall.RangeOfSizes.y);

            Ball.NewParams(speed, diameter);
        }
    }
}
