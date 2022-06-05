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
            _view.NextFrame(_model.Ball.Pos, _model.Racket1.Pos, _model.Racket2.Pos);
            _model.NextFrame();
        }
        private void NewGame()
        {
            _model = _modelConfigurator.NewModel(false);
            _view.NewGame(_model.Racket1.Size, _model.Racket2.Size, _model.Ball.Size, _model.MoveRacket);
        }
        private void EndGame()
        {
            if (_model != null)
                _view.EndGame(_model.MoveRacket);
        }
    }
}

