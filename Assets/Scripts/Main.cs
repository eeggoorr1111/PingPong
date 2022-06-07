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


        private IModel _model;
        private IModelNetwork _lastModelNetwork;


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
            ModelMaster model = _modelConfigurator.NewNetworkGameAsMaster();
            NewGame(model);

            _lastModelNetwork = model;
        }
        private void NewNetworkGameAsClient(ModelConfigData config)
        {
            ModelClient model = _modelConfigurator.NewNetworkGameAsClient(config);
            NewGame(model);

            _lastModelNetwork = model;
        }
        private void NewLocalGame()
        {
            NewGame(_modelConfigurator.NewLocalGame());
        }
        private void NewGame(IModel model)
        {
            if (_lastModelNetwork != null)
                _lastModelNetwork.Dispose();

            _model = model;
            _view.NewGame(GetNewGameData(model), model.MoveRacket);

            _model.LoseBall += LoseBallHandler;
        }
        private void EndGame()
        {
            if (_lastModelNetwork != null)
                _lastModelNetwork.Dispose();

            if (_model != null)
            {
                _view.EndGame(_model.MoveRacket);
                _model.LoseBall -= LoseBallHandler;
            }
        }
        private void LoseBallHandler(DataLosedBall data)
        {
            _view.LosedBall(data.NewDiameterBall);
        }
        private PingPongView.FrameData GetFrameData(IModel model)
        {
            PingPongView.FrameData data = new PingPongView.FrameData();

            data.PosBall = model.Ball.Pos;
            data.PosRacket1 = model.MeRacket.Pos;
            data.PosRacket2 = model.OpponentRacket.Pos;
            data.ReflectedBalls = model.PlayerMe.ReflectedBalls;
            data.RecordReflectedBalls = model.PlayerMe.RecordReflectedBalls;

            return data;
        }
        private PingPongView.NewGameData GetNewGameData(IModel model)
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

            EndGame();
        }
    }
}

