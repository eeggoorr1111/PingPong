using UnityEngine;
using PingPong.View;
using PingPong.Model;

namespace PingPong
{
    /// <summary>
    /// Точка входа и Controller в контексте паттерна MVC
    /// </summary>
    public sealed class Main : MonoBehaviour
    {
        [SerializeField] private PingPongView _view;
        [SerializeField] private ModelConfigurator _modelConfigurator;


        private IModelPingPong _model;


        private void Awake()
        {
            _view.AwakeCustom();
        }
        private void Start()
        {
            _view.StartCustom();
            _modelConfigurator.Init();

            NewGame();
        }
        private void Update()
        {
            _model.NextFrame();
            _view.NextFrame(GetFrameData());
        }
        private void NewGame()
        {
            _model = _modelConfigurator.NewLocalGame();
            _view.NewGame(_model.MeRacket.Size, _model.OpponentRacket.Size, _model.Ball.Diameter, _model.MoveRacket);
        }
        private void EndGame()
        {
            if (_model != null)
                _view.EndGame(_model.MoveRacket);
        }
        private FrameData GetFrameData()
        {
            FrameData data = new FrameData();

            data.PosBall = _model.Ball.Pos;
            data.PosRacket1 = _model.MeRacket.Pos;
            data.PosRacket2 = _model.OpponentRacket.Pos;
            data.ReflectedBalls = _model.PlayerMe.ReflectedBalls;
            data.RecordReflectedBalls = _model.PlayerMe.RecordReflectedBalls;

            return data;
        }
    }
}

