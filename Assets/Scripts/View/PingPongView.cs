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
        public PingPongUI UI => _ui;


        public event Action<float> JoystickMoved;


        [Header("GAME ITEMS")]
        [SerializeField] private Ball _ball;
        [SerializeField] private Racket _racket1;
        [SerializeField] private Racket _racket2;

        [Header("INPUT")]
        [SerializeField] private Joystick _joystick;

        [Header("DATABASE")]
        [SerializeField] private DatabaseProvider _database;

        [Header("UI")]
        [SerializeField] private PingPongUI _ui;


        public void AwakeCustom()
        {
            _ball.AwakeCustom();
            _racket1.AwakeCustom();
            _racket2.AwakeCustom();

            JoystickMoved = newPos => { };
        }
        public void StartCustom()
        {
            _ui.StartCustom();
            _joystick.StartCustom();
            _ball.SetSkin(_database.GetSavedSkinOfBall());

            _ui.SkinsWindow.SelectedSkinOfBall += SetSkinOnBall;
        }
        public void NewGame(NewGameData data, Action<float> callbackJoystick)
        {
            JoystickMoved += callbackJoystick;

            _racket1.SetSize(data.SizeRacket1);
            _racket2.SetSize(data.SizeRacket2);

            NewRound(data.DiameterBall);
        }
        public void EndGame(Action<float> callbackJoystick)
        {
            JoystickMoved -= callbackJoystick;
        }
        public void NewRound(float diameterBall)
        {
            _ball.SetDiameter(diameterBall);
        }
        public void NextFrame(FrameData data)
        {
            _ball.Transf.position = data.PosBall.To3D();

            _racket1.Transf.position = data.PosRacket1.To3D();
            _racket2.Transf.position = data.PosRacket2.To3D();

            _ui.UpdateReflectedBallsInfo(data.ReflectedBalls, data.RecordReflectedBalls);

            if (_joystick.NextFrame(_racket1.Transf.position.To2D(), out Vector2 newPos))
                JoystickMoved.Invoke(newPos.x);
        }
        public void LosedBall(float newDiameterBall)
        {
            _ball.SetDiameter(newDiameterBall);
        }



        private void SetSkinOnBall(int index)
        {
            _ball.SetSkin(_database.GetSkin(index));
        }
        private void OnDestroy()
        {
            _ui.SkinsWindow.SelectedSkinOfBall -= SetSkinOnBall;
        }


        public struct FrameData
        {
            public Vector2 PosBall;
            public Vector2 PosRacket1;
            public Vector2 PosRacket2;
            public int ReflectedBalls;
            public int RecordReflectedBalls;
        }
        public struct NewGameData
        {
            public Vector2 SizeRacket1;
            public Vector2 SizeRacket2;
            public float DiameterBall;
        }
    }
}
