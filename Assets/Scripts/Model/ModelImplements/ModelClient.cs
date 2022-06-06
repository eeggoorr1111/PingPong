using UnityEngine;
using PingPong.Model.Racket;
using PingPong.Model.Ball;
using PingPong.Model.Player;
using Photon.Pun;
using Photon.Realtime;
using ExitGames.Client.Photon;
using PingPong.Network;


namespace PingPong.Model
{
    public sealed class ModelClient : IModelPingPong, IOnEventCallback
    {
        public ModelClient( (IPlayer, RacketModel) meWithRacket,
                            (IPlayer, RacketModel) opponentWithRacket,
                            BallModel ball,
                            TrajectoryBallBuilder trajectoryBuilder,
                            TimeCounterNetwork timeCounter)
        {
            PlayerMe = meWithRacket.Item1;
            PlayerOpponent = opponentWithRacket.Item1;
            MeRacket = meWithRacket.Item2;
            OpponentRacket = opponentWithRacket.Item2;

            Ball = ball;

            _tranjectoryBuilder = trajectoryBuilder;
            _timeCounter = timeCounter;
        }


        public RacketModel MeRacket { get; }
        public RacketModel OpponentRacket { get; }
        public IPlayer PlayerMe { get; }
        public IPlayer PlayerOpponent { get; }
        public BallModel Ball { get; }


        private readonly TrajectoryBallBuilder _tranjectoryBuilder;
        private readonly TimeCounterNetwork _timeCounter;


        public void OnEvent(EventData photonEvent)
        {
            NetworkEvents code = (NetworkEvents)photonEvent.Code;

            switch (code)
            {
                case NetworkEvents.MovedRacket:
                    OpponentRacket.Move((float)photonEvent.CustomData);
                    break;

                case NetworkEvents.TryReflectBall:
                    TryReflectBall((DataTryReflectBall)photonEvent.CustomData);
                    break;
            }
        }
        public void MoveRacket(float newPos)
        {
            MeRacket.Move(newPos);

            RaiseEventOptions options = new RaiseEventOptions() { Receivers = ReceiverGroup.MasterClient };
            PhotonNetwork.RaiseEvent((byte)NetworkEvents.MovedRacket, newPos, options, new SendOptions());
        }
        public void NextFrame()
        {
            Vector2 ricochetDir;

            _timeCounter.NextFrame();

            if (IsCollisionBallWith(MeRacket, out ricochetDir))
                Ball.ToFly(_tranjectoryBuilder.Create(Ball.Pos, ricochetDir));
            else if (IsCollisionBallWith(OpponentRacket, out ricochetDir))
                Ball.ToFly(_tranjectoryBuilder.Create(Ball.Pos, ricochetDir));

            Ball.ContinueFly();
        }


        private void TryReflectBall(DataTryReflectBall data)
        {
            IPlayer player = data.IsClient ? PlayerMe : PlayerOpponent;

            if (data.IsSuccess)
                player.ReflectedBall();
            else
                player.LoseBall();

            MergeSelfTrajectoryWithTrajectoryMaster(data.NewTrajectoryBall);
        }
        private void MergeSelfTrajectoryWithTrajectoryMaster(TrajectoryBall trajectory)
        {
            // TODO: —делать подмену кривой полета беспалево, путем небольшой коррекции 
            // каждого угла рикошета от стен. “ак, чтобы в конец тректории полета снар€да на клиенте
            // совпал с концом полета снар€да на мастере

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
