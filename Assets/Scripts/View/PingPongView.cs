using UnityEngine;
using PingPong.View.GameItems;
using Narratore.Input;
using Narratore.Helpers;
using System;
using PingPong.View.UI;
using PingPong.Database;

namespace PingPong.View
{
    public class PingPongView : MonoBehaviour
    {
        public UIPingPong UI => _ui;


        [Header("GAME ITEMS")]
        [SerializeField] private Ball _ball;
        [SerializeField] private Racket _racket1;
        [SerializeField] private Racket _racket2;

        [Header("INPUT")]
        [SerializeField] private Joystick _joystick;

        [Header("DATABASE")]
        [SerializeField] private DatabaseProvider _database;

        [Header("UI")]
        [SerializeField] private UIPingPong _ui;


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
            _ui.StartCustom();
            _joystick.StartCustom();
            _ball.SetSkin(_database.GetSavedSkinOfBall());

            _ui.WindowSkins.SelectedSkinOfBall += SetSkinOnBall;
        }
        public void NewGame(Vector2 sizeRocket1, Vector2 sizeRocket2, float diameterBall, Action<float> callbackJoystick)
        {
            _joystickMoved += callbackJoystick;

            _racket1.SetSize(sizeRocket1);
            _racket2.SetSize(sizeRocket2);
            _ball.SetSize(diameterBall);
        }
        public void EndGame(Action<float> callbackJoystick)
        {
            _joystickMoved -= callbackJoystick;
        }
        public void NextFrame(FrameData data)
        {
            _ball.Transf.position = data.PosBall.To3D();

            _racket1.Transf.position = data.PosRacket1.To3D();
            _racket2.Transf.position = data.PosRacket2.To3D();

            _ui.UpdateReflectedBallsInfo(data.ReflectedBalls, data.RecordReflectedBalls);

            if (_joystick.NextFrame(_racket1.Transf.position.To2D(), out Vector2 newPos))
                _joystickMoved.Invoke(newPos.x);
        }



        private void SetSkinOnBall(int index)
        {
            _ball.SetSkin(_database.GetSkin(index));
        }
        private void OnDestroy()
        {
            _ui.WindowSkins.SelectedSkinOfBall -= SetSkinOnBall;
        }
    }
}
