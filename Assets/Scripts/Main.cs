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
            NewGame();
        }
        private void Update()
        {
            _view.NextFrame(_model.Ball.Pos, _model.Racket1.Pos, _model.Racket2.Pos);
        }
        private void NewGame()
        {
            _model = _modelConfigurator.NewModel(false);
            _view.SubscribeOnJoystick(_model.MoveRacket);
        }
        private void EndGame()
        {
            if (_model != null)
                _view.UnsubscribeOnJoystick(_model.MoveRacket);
        }
    }
}

