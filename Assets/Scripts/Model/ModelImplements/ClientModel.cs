using UnityEngine;
using PingPong.Model.Racket;
using PingPong.Model.Ball;
using PingPong.Model.Player;
using Photon.Pun;
using Photon.Realtime;
using ExitGames.Client.Photon;
using PingPong.Network;
using System;


namespace PingPong.Model
{
    public sealed class ClientModel : IModel, IOnEventCallback
    {
        public ClientModel( (IPlayer, RacketModel) meWithRacket,
                            (IPlayer, RacketModel) opponentWithRacket,
                            BallModel ball,
                            TrajectoryBallBuilder trajectoryBuilder,
                            NetworkTimeCounter timeCounter)
        {
            PlayerMe = meWithRacket.Item1;
            PlayerOpponent = opponentWithRacket.Item1;
            MeRacket = meWithRacket.Item2;
            OpponentRacket = opponentWithRacket.Item2;

            Ball = ball;

            _tranjectoryBuilder = trajectoryBuilder;
            _timeCounter = timeCounter;

            ReflectedBall += data => { };
            LoseBall += data => { };

            PhotonNetwork.AddCallbackTarget(this);

            RaiseEventOptions options = new RaiseEventOptions() { Receivers = ReceiverGroup.MasterClient };
            PhotonNetwork.RaiseEvent((byte)NetworkEvents.ClientReadyToGame, null, options, new SendOptions());
        }


        public event Action<ReflectBallData> ReflectedBall;
        public event Action<LosedBallData> LoseBall;


        public RacketModel MeRacket { get; }
        public RacketModel OpponentRacket { get; }
        public IPlayer PlayerMe { get; }
        public IPlayer PlayerOpponent { get; }
        public BallModel Ball { get; }


        private readonly TrajectoryBallBuilder _tranjectoryBuilder;
        private readonly NetworkTimeCounter _timeCounter;
        private bool _startedGame;


        public void OnEvent(EventData photonEvent)
        {
            NetworkEvents code = (NetworkEvents)photonEvent.Code;

            switch (code)
            {
                case NetworkEvents.StartGame:
                    StartedGame((StartedGameData)photonEvent.CustomData);
                    break;

                case NetworkEvents.MovedRacket:
                    OpponentRacket.Move((float)photonEvent.CustomData);
                    break;

                case NetworkEvents.ReflectBall:
                    ReflectedBallHandler((ReflectBallData)photonEvent.CustomData);
                    break;

                case NetworkEvents.LosedBall:
                    LosedBallHandler((LosedBallData)photonEvent.CustomData);
                    break;
            }
        }
        public void MoveRacket(float newPos)
        {
            if (_startedGame)
            {
                MeRacket.Move(newPos);

                RaiseEventOptions options = new RaiseEventOptions() { Receivers = ReceiverGroup.MasterClient };
                PhotonNetwork.RaiseEvent((byte)NetworkEvents.MovedRacket, newPos, options, new SendOptions());
            }
        }
        public void NextFrame()
        {
            if (_startedGame)
            {
                Vector2 ricochetDir;

                _timeCounter.NextFrame();

                if (IsCollisionBallWith(MeRacket, out ricochetDir))
                    Ball.ToFly(_tranjectoryBuilder.Create(Ball.Pos, ricochetDir));
                else if (IsCollisionBallWith(OpponentRacket, out ricochetDir))
                    Ball.ToFly(_tranjectoryBuilder.Create(Ball.Pos, ricochetDir));

                Ball.ContinueFly();
            }
        }
        public void Dispose()
        {
            PhotonNetwork.RemoveCallbackTarget(this);
        }


        private void StartedGame(StartedGameData data)
        {
            _startedGame = true;

            Ball.NewParams(data.SpeedBall, data.DiameterBall);
            Ball.ToFly(data.TrajectoryBall);
        }
        private void ReflectedBallHandler(ReflectBallData data)
        {
            IPlayer player = data.PlayerId == PlayerMe.Id ? PlayerMe : PlayerOpponent;
            player.ReflectedBall();

            ReflectedBall.Invoke(data);

            MergeSelfTrajectoryWithTrajectoryMaster(data.NewTrajectoryBall);
        }
        private void LosedBallHandler(LosedBallData data)
        {
            IPlayer player = data.PlayerId == PlayerMe.Id ? PlayerMe : PlayerOpponent;
            player.LoseBall();

            Ball.NewParams(data.NewSpeedBall, data.NewDiameterBall);
            LoseBall.Invoke(data);

            MergeSelfTrajectoryWithTrajectoryMaster(data.NewTrajectoryBall);
        }
        private void MergeSelfTrajectoryWithTrajectoryMaster(BallTrajectory trajectory)
        {
            // TODO: ������� ������� ������ ������ ���������, ����� ��������� ��������� 
            // ������� ���� �������� �� ����. ���, ����� ��������� ������� ������ ������� �� �������
            // ������ � ��������� �������� ������ ������� �� �������

            Ball.ToFly(trajectory);
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
    }
}
