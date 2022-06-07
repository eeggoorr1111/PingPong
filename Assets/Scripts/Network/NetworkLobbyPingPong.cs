using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using System;
using ExitGames.Client.Photon;
using PingPong.Model;


namespace PingPong.Network
{
    public sealed class NetworkLobbyPingPong : MonoBehaviourPunCallbacks, IOnEventCallback
    {
        public ModelConfigData ConfigGame { get; set; }


        public event Action ConnectedToMasterServer;
        public event Action ThisPlayerLeavedGame;
        public event Action OtherPlayerLeavedGame;
        public event Action NewGameAsMaster;
        public event Action<ModelConfigData> NewGameAsClient;


       
        public void Init(ModelConfigData config)
        {
            PhotonPeer.RegisterType(typeof(DataReflectBall), 0, DataReflectBall.Serialize, DataReflectBall.Deserialize);
            PhotonPeer.RegisterType(typeof(DataLosedBall), 1, DataLosedBall.Serialize, DataLosedBall.Deserialize);
            PhotonPeer.RegisterType(typeof(DataStartedGame), 2, DataStartedGame.Serialize, DataStartedGame.Deserialize);
            PhotonPeer.RegisterType(typeof(ModelConfigData), 3, ModelConfigData.Serialize, ModelConfigData.Deserialize);
            

            PhotonNetwork.AddCallbackTarget(this);

            ConnectedToMasterServer += () => { };
            ThisPlayerLeavedGame += () => { };
            OtherPlayerLeavedGame += () => { };
            NewGameAsMaster += () => { };
            NewGameAsClient += config => { };

            ConfigGame = config;

            TryConnectToMasterServer();
        }
        public void CreateRoom()
        {
            PhotonNetwork.CreateRoom("TEST", new RoomOptions() { MaxPlayers = 2 });
            Debug.Log("CreateRoom");
        }
        public void JoinRoom()
        {
            PhotonNetwork.JoinRoom("TEST");
            Debug.Log("JoinRoom");
        }


#region PHOTON CALLBACKS
        public override void OnConnectedToMaster()
        {
            Debug.Log("OnConnectedToMaster");
            ConnectedToMasterServer.Invoke();
        }
        public override void OnJoinedRoom()
        {
            Debug.Log("OnJoinedRoom");
        }
        public override void OnJoinRandomFailed(short returnCode, string message)
        {
            Debug.LogError("OnJoinRoomFailed \n" + message);
        }
        public override void OnLeftRoom()
        {
            Debug.Log("ThisPlayerLeaveGame");
            ThisPlayerLeavedGame.Invoke();
        }
        public override void OnPlayerLeftRoom(Player otherPlayer)
        {
            Debug.Log("OtherPlayerLeaveGame");
            OtherPlayerLeavedGame.Invoke();
        }
        public override void OnPlayerEnteredRoom(Player newPlayer)
        {
            if (PhotonNetwork.IsMasterClient && PhotonNetwork.CurrentRoom.PlayerCount == 2)
            {
                Debug.Log("StartedGameAsMaster");
                RaiseEventOptions options = new RaiseEventOptions() { Receivers = ReceiverGroup.Others };
                PhotonNetwork.RaiseEvent((byte)NetworkEvents.PrepareForGame, ConfigGame, options, new SendOptions());
                NewGameAsMaster.Invoke();
            }
        }
        public void OnEvent(EventData photonEvent)
        {
            NetworkEvents code = (NetworkEvents)photonEvent.Code;
            if (code == NetworkEvents.PrepareForGame)
            {
                Debug.Log("StartedGameAsClient");
                NewGameAsClient.Invoke((ModelConfigData)photonEvent.CustomData);
            }
                
        }
        #endregion


        private void OnDestroy()
        {
            PhotonNetwork.RemoveCallbackTarget(this);
        }
        private void TryConnectToMasterServer()
        {
            PhotonNetwork.NickName = "Игрок в красных трусах";
            PhotonNetwork.ConnectUsingSettings();
        }
    }
}
