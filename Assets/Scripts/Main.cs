using UnityEngine;
using Zenject;
using PingPong.View;
using PingPong.Model;

namespace PingPong
{
    /// <summary>
    /// Controller в контексте паттерна MVC
    /// </summary>
    public class Main : MonoBehaviour
    {
        [Inject]
        private void Constructor(PingPongView view)
        {
            _view = view;
        }


        private PingPongView _view;


        private void Awake()
        {
            _view.AwakeCustom();
        }
        private void Start()
        {
            _view.StartCustom();
        }
        private void Update()
        {
            _view.NextFrame(new Vector2(), new Vector2(), new Vector2());
        }
    }
}

