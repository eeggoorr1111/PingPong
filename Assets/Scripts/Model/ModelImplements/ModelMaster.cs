using PingPong.Model.Racket;
using PingPong.Model.Ball;
using PingPong.Model.Player;
using PingPong.Network;
using Photon.Pun;
using Photon.Realtime;
using ExitGames.Client.Photon;

using WhatHappenedToBall = PingPong.Model.ModelLocal.WhatHappenedToBall;


namespace PingPong.Model
{
    public sealed class ModelMaster : IModelPingPong, IOnEventCallback
    {
        public ModelMaster(ModelLocal model, TimeCounterNetwork timeCounter)
        {
            _modelLocal = model;
            _timeCounter = timeCounter;
        }


        public RacketModel MeRacket => _modelLocal.MeRacket;
        public RacketModel OpponentRacket => _modelLocal.OpponentRacket;
        public IPlayer PlayerMe => _modelLocal.PlayerMe;
        public IPlayer PlayerOpponent => _modelLocal.PlayerOpponent;
        public BallModel Ball => _modelLocal.Ball;


        private readonly ModelLocal _modelLocal;
        private readonly TimeCounterNetwork _timeCounter;


        public void OnEvent(EventData photonEvent)
        {
            NetworkEvents code = (NetworkEvents)photonEvent.Code;

            switch (code)
            {
                case NetworkEvents.MovedRacket:
                    OpponentRacket.Move((float)photonEvent.CustomData);
                    break;
            }
        }
        public void MoveRacket(float newPos)
        {
            _modelLocal.MoveRacket(newPos);

            RaiseEventOptions options = new RaiseEventOptions() { Receivers = ReceiverGroup.Others };
            PhotonNetwork.RaiseEvent((byte)NetworkEvents.MovedRacket, newPos, options, new SendOptions());
        }
        public void NextFrame()
        {
            _timeCounter.NextFrame();
            _modelLocal.NextFrame(out WhatHappenedToBall whatHappened);
            SendEventIfNeed(whatHappened);
        }



        private void SendEventIfNeed(WhatHappenedToBall whatHappened)
        {
            if (whatHappened == WhatHappenedToBall.ContineFly)
                return;

            bool isSuccess = whatHappened == WhatHappenedToBall.ReflectedMe ||
                             whatHappened == WhatHappenedToBall.ReflectedOpponent;
            bool isClient =  whatHappened == WhatHappenedToBall.ReflectedOpponent ||
                             whatHappened == WhatHappenedToBall.LosedOpponent;

            RaiseEventOptions options = new RaiseEventOptions() { Receivers = ReceiverGroup.Others };
            DataTryReflectBall data = new DataTryReflectBall(isClient, isSuccess, Ball.Trajectory);

            PhotonNetwork.RaiseEvent((byte)NetworkEvents.TryReflectBall, data, options, new SendOptions());
        }
    }
}
