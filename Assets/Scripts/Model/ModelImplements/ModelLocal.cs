using UnityEngine;
using System.Collections.Generic;
using PingPong.Model.Racket;
using PingPong.Model.Ball;
using Narratore.DebugTools;
using Narratore.Primitives;
using PingPong.Model.Player;


namespace PingPong.Model
{
    public sealed class ModelLocal : IModelPingPong
    {
        public ModelLocal(  (IPlayer, RacketModel) meWithRacket,
                            (IPlayer, RacketModel) opponentWithRacket,
                            BallModel ball, 
                            Map map,
                            TrajectoryBallBuilder trajectoryBuilder)
        {
            PlayerMe = meWithRacket.Item1;
            MeRacket = meWithRacket.Item2;

            PlayerOpponent = opponentWithRacket.Item1;
            OpponentRacket = opponentWithRacket.Item2;

            Ball = ball;
            _map = map;
            
            _tranjectoryBuilder = trajectoryBuilder;
            _racketsOfPlayers = new (IPlayer, RacketModel)[2]
            {
                meWithRacket,
                opponentWithRacket
            };

            NewRound();
        }


        public RacketModel MeRacket { get; }
        public RacketModel OpponentRacket { get; }
        public IPlayer PlayerMe { get; }
        public IPlayer PlayerOpponent { get; }
        public BallModel Ball { get; }


        private readonly Map _map;
        private readonly TrajectoryBallBuilder _tranjectoryBuilder;
        private readonly (IPlayer, RacketModel)[] _racketsOfPlayers;
        private RacketModel _lastRicochet;


        public void MoveRacket(float newPos)
        {
            List<RacketModel> rackets = GetRacketsOf(PlayerMe);

            for (int i = 0; i < rackets.Count; i++)
                rackets[i].Move(newPos);
        }
        public void NextFrame()
        {
            NextFrame(out WhatHappenedToBall whatHappened);
        }
        public void NextFrame(out WhatHappenedToBall whatHappened)
        {
            Vector2 ricochetDir;

            whatHappened = WhatHappenedToBall.ContineFly;
            if (_lastRicochet == OpponentRacket && IsCollisionBallWith(MeRacket, out ricochetDir))
            {
                _lastRicochet = MeRacket;
                Ball.ToFly(_tranjectoryBuilder.Create(Ball.Pos, ricochetDir));
                PlayerMe.ReflectedBall();

                whatHappened = WhatHappenedToBall.ReflectedMe;
            }
            else if (_lastRicochet == MeRacket && IsCollisionBallWith(OpponentRacket, out ricochetDir))
            {
                _lastRicochet = OpponentRacket;
                Ball.ToFly(_tranjectoryBuilder.Create(Ball.Pos, ricochetDir));
                PlayerOpponent.ReflectedBall();

                whatHappened = WhatHappenedToBall.ReflectedOpponent;
            }
            else if (IsCollisionBallWith(_map.BottomBorder))
            {
                PlayerMe.LoseBall();
                NewRound();

                whatHappened = WhatHappenedToBall.LosedMe;
            }
            else if (IsCollisionBallWith(_map.TopBorder))
            {
                PlayerOpponent.LoseBall();
                NewRound();

                whatHappened = WhatHappenedToBall.LosedOpponent;
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
            TrajectoryBall trajectory = _tranjectoryBuilder.FlyFromCenterToRandomDir(flyToTop);

            Ball.ChangeBallParams();
            Ball.ToFly(trajectory);

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


        /// <summary>
        /// По хорошшему вместо возврата enum, лучше бы события. Но приложение небольшое,
        /// поэтому чтобы не заморачиваться с отпиской событий, лучше пока так.
        /// </summary>
        public enum WhatHappenedToBall : byte
        {
            ContineFly,
            LosedMe,
            LosedOpponent,
            ReflectedMe,
            ReflectedOpponent
        }
    }
}
