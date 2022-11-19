using Photon.Pun;
using UnityEngine;

namespace PingPong.Network
{
    public sealed class NetworkTimeCounter : IReadOnlyTimeCounter
    {
        public NetworkTimeCounter()
        {
            _prevTimeServer = PhotonNetwork.Time;
        }


        private double _prevTimeServer;
        private double _elapsedMilliseconds; 


        public void NextFrame()
        {
            if (PhotonNetwork.Time > _prevTimeServer + double.Epsilon)
            {
                _elapsedMilliseconds = 0.0;
                _prevTimeServer = PhotonNetwork.Time;
            }
            else
                _elapsedMilliseconds += Time.unscaledDeltaTime;
        }
        public double GetTime()
        {
            return _prevTimeServer + _elapsedMilliseconds;
        }
    
    }
}
