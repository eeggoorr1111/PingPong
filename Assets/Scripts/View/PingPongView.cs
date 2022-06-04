using UnityEngine;
using PingPong.View.GameItems;
using Narratore.Input;
using Narratore.Helpers;
using System;
using PingPong.View.UI;

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

        [Header("SKINS")]
        [SerializeField] private SkinsDatabase _skins;

        [Header("UI")]
        [SerializeField] private PingPongUI _ui;


        private event Action<float> _joystickMoved;


        public void AwakeCustom()
        {
            _ball.AwakeCustom();
            _racket1.AwakeCustom();
            _racket2.AwakeCustom();
            _ui.AwakeCustom();

            _joystickMoved = newPos => { };
        }
        public void StartCustom()
        {
            _joystick.StartCustom();
            _ui.StartCustom();

            _ui.SelectedSkinOfBall += SetSkinOnBall;
        }
        public void NewGame(Vector2 sizeRocket1, Vector2 sizeRocket2, float sizeBall, Action<float> callbackJoystick)
        {
            _joystickMoved += callbackJoystick;

            _racket1.SetSize(sizeRocket1);
            _racket2.SetSize(sizeRocket2);
            _ball.SetSize(sizeBall);
        }
        public void EndGame(Action<float> callbackJoystick)
        {
            _joystickMoved -= callbackJoystick;
        }
        public void NextFrame(Vector2 posBall, Vector2 posRacket1, Vector2 posRacket2)
        {
            _ball.Transf.position = posBall.To3D();

            _racket1.Transf.position = posRacket1.To3D();
            _racket2.Transf.position = posRacket2.To3D();

            if (_joystick.NextFrame(_racket1.Transf.position.To2D(), out Vector2 newPos))
                _joystickMoved.Invoke(newPos.x);
        }


        private void SetSkinOnBall(int index)
        {
            _ball.SetSkin(_skins.GetSkin(index));
        }
        private void OnDestroy()
        {
            _ui.SelectedSkinOfBall -= SetSkinOnBall;
        }
    }
}
