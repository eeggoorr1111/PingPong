using UnityEngine;
using PingPong.View.GameItems;
using Narratore.Input;
using Narratore.Helpers;
using System;
using PingPong.View.UI;
using UnityEngine.UI;

namespace PingPong.View
{
    /// <summary>
    /// View - в контексте паттерна MVC. Было бы хорошо создать отдельную сущность для UI и положить туда WindowSkins и кнопку открытия этого окна. 
    /// Но на данный момент это слишком маленький функционал для выделения под это целого слоя абстракции. В любом случае, это не сложно сделать в будущем.
    /// </summary>
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
        [SerializeField] private WindowSkins _windowSkins;
        [SerializeField] private Button _openWindowSkinsBtn;


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
            _windowSkins.Init();

            _openWindowSkinsBtn.onClick.AddListener(_windowSkins.Open);
            _windowSkins.SelectedSkinOfBall += SetSkinOnBall;
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
            if (_windowSkins != null)
            {
                _openWindowSkinsBtn.onClick.RemoveListener(_windowSkins.Open);
                _windowSkins.SelectedSkinOfBall -= SetSkinOnBall;
            }
        }
    }
}
