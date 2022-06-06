using UnityEngine;
using PingPong.View;
using PingPong.Model;
using PingPong.Network;

namespace PingPong
{
    /// <summary>
    /// Точка входа и Controller в контексте паттерна MVC
    /// </summary>
    public sealed class Main : MonoBehaviour
    {
        [SerializeField] private PingPongView _view;
        [SerializeField] private ModelConfigurator _modelConfigurator;
        [SerializeField] private NetworkLobbyPingPong _networkLobby;


        private IModelPingPong _model;


        private void Awake()
        {
            _view.AwakeCustom();
        }
        private void Start()
        {
            _modelConfigurator.Init();

            _view.StartCustom();
            _view.UI.WindowStart.PressedStartGameBtn += NewLocalGame;
            _view.UI.WindowStart.PressedCreateRoomBtn += _networkLobby.CreateRoom;
            _view.UI.WindowStart.PressedJoinRoomBtn += _networkLobby.JoinRoom;

            _networkLobby.Init(_modelConfigurator.Config);
            _networkLobby.ConnectedToMasterServer += _view.UI.WindowStart.ActivateNetworkButtons;
            _networkLobby.NewGameAsMaster += NewNetworkGameAsMaster;
            _networkLobby.NewGameAsClient += NewNetworkGameAsClient;
        }
        private void Update()
        {
            if (_model != null)
            {
                _model.NextFrame();
                _view.NextFrame(GetFrameData(_model));
            }
        }
        private void NewNetworkGameAsMaster()
        {
            _model = _modelConfigurator.NewNetworkGameAsMaster();
            _view.NewGame(GetNewGameData(_model), _model.MoveRacket);
        }
        private void NewNetworkGameAsClient(ModelConfigData config)
        {
            _model = _modelConfigurator.NewNetworkGameAsClient(config);
            _view.NewGame(GetNewGameData(_model), _model.MoveRacket);
        }
        private void NewLocalGame()
        {
            _model = _modelConfigurator.NewLocalGame();
            _view.NewGame(GetNewGameData(_model), _model.MoveRacket);
        }
        private void EndGame()
        {
            if (_model != null)
            {
                _view.EndGame(_model.MoveRacket);
                _model = null;
            }
        }
        private PingPongView.FrameData GetFrameData(IModelPingPong model)
        {
            PingPongView.FrameData data = new PingPongView.FrameData();

            data.PosBall = model.Ball.Pos;
            data.PosRacket1 = model.MeRacket.Pos;
            data.PosRacket2 = model.OpponentRacket.Pos;
            data.ReflectedBalls = model.PlayerMe.ReflectedBalls;
            data.RecordReflectedBalls = model.PlayerMe.RecordReflectedBalls;

            return data;
        }
        private PingPongView.NewGameData GetNewGameData(IModelPingPong model)
        {
            PingPongView.NewGameData data = new PingPongView.NewGameData();

            data.SizeRacket1 = model.MeRacket.Size;
            data.SizeRacket2 = model.OpponentRacket.Size;
            data.DiameterBall = model.Ball.Diameter;

            return data;
        }
        private void OnDestroy()
        {
            _view.UI.WindowStart.PressedStartGameBtn -= NewLocalGame;
            _view.UI.WindowStart.PressedCreateRoomBtn -= _networkLobby.CreateRoom;
            _view.UI.WindowStart.PressedJoinRoomBtn -= _networkLobby.JoinRoom;

            _networkLobby.ConnectedToMasterServer -= _view.UI.WindowStart.ActivateNetworkButtons;
            _networkLobby.NewGameAsMaster -= NewNetworkGameAsMaster;
            _networkLobby.NewGameAsClient -= NewNetworkGameAsClient;
        }
    }
}

