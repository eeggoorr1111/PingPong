using UnityEngine;
using PingPong.View;
using PingPong.Model;
using PingPong.Network;

namespace PingPong
{
    /// <summary>
    /// Точка входа и Controller в контексте паттерна MVC.
    /// 
    /// Использование MVC обусловлено тем, что логика View абсолютно одинаковая что для клиента, что для мастера, что 
    /// при игре оффлайн. 
    /// 
    /// Поэтому удобны было вынести отдельно Model, которая имеет 3 реализации. В зависимости от
    /// того играем оффлайн, играем как клиент или играем как мастер.
    /// </summary>
    public sealed class Main : MonoBehaviour
    {
        [SerializeField] private PingPongView _view;
        [SerializeField] private ModelConfigurator _modelConfigurator;
        [SerializeField] private NetworkLobbyPingPong _networkLobby;


        private IModel _model;


        private void Awake()
        {
            _view.AwakeCustom();
        }
        private void Start()
        {
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
            _model?.Dispose();
            NewGame(_modelConfigurator.NewNetworkGameAsMaster());
        }
        private void NewNetworkGameAsClient(ModelConfigData config)
        {
            _model?.Dispose();
            NewGame(_modelConfigurator.NewNetworkGameAsClient(config));
        }
        private void NewLocalGame()
        {
            _model?.Dispose();
            NewGame(_modelConfigurator.NewLocalGame());
        }
        private void NewGame(IModel model)
        {
            _model = model;
            _view.NewGame(GetNewGameData(model), model.MoveRacket);

            _model.LoseBall += LoseBallHandler;
        }
        private void EndGame()
        {
            if (_model != null)
            {
                _view.EndGame(_model.MoveRacket);
                _model.LoseBall -= LoseBallHandler;
                _model.Dispose();
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

