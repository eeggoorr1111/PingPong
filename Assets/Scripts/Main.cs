using UnityEngine;
using PingPong.View;
using PingPong.Model;

namespace PingPong
{
    /// <summary>
    /// Controller в контексте паттерна MVC
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
            _view.NextFrame(_model.Ball.Pos, _model.MeRacket.Pos, _model.OpponentRacket.Pos, _model.PlayerMe.ReflectedBalls, _model.PlayerMe.RecordReflectedBalls);
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
    }
}

