using UnityEngine;
using PingPong.View.GameItems;
using Narratore.Input;
using Narratore.Helpers;
using System;

namespace PingPong.View
{
    public class PingPongView : MonoBehaviour
    {
        [Header("GAME ITEMS")]
        [SerializeField] private Ball _ball;
        [SerializeField] private Racket _racket1;
        [SerializeField] private Racket _racket2;

        [Header("INPUT")]
        [SerializeField] private Joystick _joystick;

        private event Action<float> _joystickMoved;


        public void AwakeCustom()
        {
            _ball.AwakeCustom();
            _racket1.AwakeCustom();
            _racket2.AwakeCustom();

            _joystickMoved = newPos => { };
        }
        public void StartCustom()
        {
            _joystick.StartCustom();
        }
        public void SubscribeOnJoystick(Action<float> callback)
        {
            _joystickMoved += callback;
        }
        public void UnsubscribeOnJoystick(Action<float> callback)
        {
            _joystickMoved += callback;
        }
        public void NextFrame(Vector2 posBall, Vector2 posRacket1, Vector2 posRacket2)
        {
            _ball.Transf.position = posBall.To3D();

            _racket1.Transf.position = posRacket1.To3D();
            _racket2.Transf.position = posRacket2.To3D();

            if (_joystick.NextFrame(_racket1.Transf.position.To2D(), out Vector2 newPos))
                _joystickMoved.Invoke(newPos.x);
        }
    }
}
