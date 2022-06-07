using PingPong.Model.Racket;
using PingPong.Model.Ball;
using PingPong.Model.Player;
using PingPong.Network;
using Photon.Pun;
using Photon.Realtime;
using ExitGames.Client.Photon;
using System;


namespace PingPong.Model
{
    public sealed class ModelMaster : IModelNetwork
    {
        public ModelMaster(ModelLocal model, TimeCounterNetwork timeCounter)
        {
            _modelLocal = model;
            _timeCounter = timeCounter;

            ReflectedBall += data => { };
            LoseBall += data => { };

            _modelLocal.ReflectedBall += ReflectedBallHandler;
            _modelLocal.LoseBall += LoseBallHandler;

            PhotonNetwork.AddCallbackTarget(this);
        }


        public event Action<DataReflectBall> ReflectedBall;
        public event Action<DataLosedBall> LoseBall;


        public RacketModel MeRacket => _modelLocal.MeRacket;
        public RacketModel OpponentRacket => _modelLocal.OpponentRacket;
        public IPlayer PlayerMe => _modelLocal.PlayerMe;
        public IPlayer PlayerOpponent => _modelLocal.PlayerOpponent;
        public BallModel Ball => _modelLocal.Ball;


        private readonly ModelLocal _modelLocal;
        private readonly TimeCounterNetwork _timeCounter;
        private bool _gameStarted;


        public void OnEvent(EventData photonEvent)
        {
            NetworkEvents code = (NetworkEvents)photonEvent.Code;

            switch (code)
            {
                case NetworkEvents.ClientReadyToGame:
                    StartGame();
                    break;

                case NetworkEvents.MovedRacket:
                    OpponentRacket.Move((float)photonEvent.CustomData);
                    break;
            }
        }
        public void MoveRacket(float newPos)
        {
            if (_gameStarted)
            {
                _modelLocal.MoveRacket(newPos);

                RaiseEventOptions options = new RaiseEventOptions() { Receivers = ReceiverGroup.Others };
                PhotonNetwork.RaiseEvent((byte)NetworkEvents.MovedRacket, newPos, options, new SendOptions());
            }
        }
        public void NextFrame()
        {
            if (_gameStarted)
            {
                _timeCounter.NextFrame();
                _modelLocal.NextFrame();
            }
        }
        public void Dispose()
        {
            _modelLocal.ReflectedBall -= ReflectedBallHandler;
            _modelLocal.LoseBall -= LoseBallHandler;

            PhotonNetwork.RemoveCallbackTarget(this);
        }


        private void StartGame()
        {
            _gameStarted = true;
            _modelLocal.NewRound();

            BallModel ball = _modelLocal.Ball;
            DataStartedGame data = new DataStartedGame(ball.Diameter, ball.Speed, ball.Trajectory);
            RaiseEventOptions options = new RaiseEventOptions() { Receivers = ReceiverGroup.Others };

            PhotonNetwork.RaiseEvent((byte)NetworkEvents.StartGame, data, options, new SendOptions());
        }
        private void ReflectedBallHandler(DataReflectBall data)
        {
            RaiseEventOptions options = new RaiseEventOptions() { Receivers = ReceiverGroup.Others };
            PhotonNetwork.RaiseEvent((byte)NetworkEvents.ReflectBall, data, options, new SendOptions());

            ReflectedBall.Invoke(data);
        }
        private void LoseBallHandler(DataLosedBall data)
        {
            RaiseEventOptions options = new RaiseEventOptions() { Receivers = ReceiverGroup.Others };
            PhotonNetwork.RaiseEvent((byte)NetworkEvents.LosedBall, data, options, new SendOptions());

            LoseBall.Invoke(data);
        }
    }
}
