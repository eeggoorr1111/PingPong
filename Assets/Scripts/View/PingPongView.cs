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


        public void AwakeCustom()
        {
            _ball.AwakeCustom();
            _racket1.AwakeCustom();
            _racket2.AwakeCustom();
        }
        public void StartCustom()
        {
            _joystick.StartCustom();
        }
        public void NextFrame(Vector2 posBall, Vector2 posRacket1, Vector2 posRacket2)
        {
            //_ball.Transf.position = posBall;
            //_racket1.Transf.position = posRacket1;
            //_racket2.Transf.position = posRacket2;

            if (_joystick.NextFrame(_racket1.Transf.position.To2D(), out Vector2 newPos))
                _racket1.Transf.position = new Vector3(newPos.x, _racket1.Transf.position.y, 0);
        }
    }
}
