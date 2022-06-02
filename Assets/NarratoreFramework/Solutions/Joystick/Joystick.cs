using Narratore.Helpers;
using Narratore.UI;
using UnityEngine;
using UnityEngine.UI;
using Narratore.Attributs; 

namespace Narratore.Input
{
    public sealed class Joystick : MonoBehaviour
    {
        public ViewOfJoystick ViewJoystick => _viewJoystick;

        
        [Header("UI ЭЛЕМЕНТЫ")]
        [Label("Принцип работы джойстика. Когда внутренний круг смещается от центра, это задает направление и интенсивность смещения целевой точки от персонажа, к которой тот должен бежать")]
        [SerializeField] private Canvas _canvas;
        [SerializeField] private Image _joystick;
        [SerializeField] private Image _stick;
        [SerializeField] private bool _moveStickOnlyByX;


        [Header("ПАРАМЕТРЫ")]
        [Tooltip("Задает размер изображение джойстика")]
        [SerializeField] private float _radiusJoystick;
        [Tooltip("Минимальный сдвиг от центра джойстика, когда мы начинаем считать, что есть движение")]
        [Range(0f, 1f)]
        [SerializeField] private float _minOffsetStick;
        [SerializeField] private float _offsetTargetPoint = 5;
        [SerializeField] private ViewOfJoystick _viewJoystick;


        private Vector2 _directionMove;
        private float _intensityMove;
        private Vector2 _centerJoystick;
        private Vector2 _defaultPosJoystick;
        private Vector2 _defaultPosStick;
        private bool _moveByTouch;
        private bool _moveByAxis;
        private Transform _canvasTransf;
        private RectTransform _joystickTransf;
        private RectTransform _stickTransf;


        public void StartCustom()
        {
            if (_joystickTransf == null)
            {
                _joystickTransf = _joystick.GetComponent<RectTransform>();
                _stickTransf = _stick.GetComponent<RectTransform>();
                _canvasTransf = _canvas.GetComponent<Transform>();
                _joystickTransf.sizeDelta = new Vector2(_radiusJoystick * 2, _radiusJoystick * 2);
                _defaultPosJoystick = _joystickTransf.anchoredPosition;
                _defaultPosStick = _stickTransf.anchoredPosition;

                if (_viewJoystick == ViewOfJoystick.AlwaysHide || _viewJoystick == ViewOfJoystick.ViewWhenMove)
                    _canvas.enabled = false;
                else
                    _canvas.enabled = true;
            }
        }
        /// <param name="isDifferentIntensity"> 
        /// true - the greater the joystick offset, the more intense the movement
        /// false - intensity the movement always the same
        /// </param>
        /// <returns> was there a joystick movement or not</returns>
        public bool NextFrame(Vector2 currentPosObj, out Vector2 newPosObj, bool isDifferentIntensity = true)
        {
            bool wasMovement = false;

            if (!MoveByAxis())
                MoveByTouch();

            newPosObj = currentPosObj;

            if (JoystickOffet(out Vector2 offset, isDifferentIntensity))
            {
                newPosObj = currentPosObj + offset * _offsetTargetPoint * Time.deltaTime;
                wasMovement = true;
            }

            if (_viewJoystick == ViewOfJoystick.ViewWhenMove)
                _canvas.enabled = _moveByTouch;

            return wasMovement;
        }


        private bool JoystickOffet(out Vector2 offset, bool isDifferentIntensity = true)
        {
            offset = Vector2.zero;
            if ((_moveByTouch && _intensityMove > _minOffsetStick) || _moveByAxis)
            {
                if (isDifferentIntensity)
                    offset = _directionMove * _intensityMove;
                else
                    offset = _directionMove;

                return true;
            }

            return false;
        }
        private void MoveByTouch()
        {
            if (MyInput.StartedSwipe())
            {
                _centerJoystick = UnityEngine.Input.mousePosition.To2D();
                _joystickTransf.anchoredPosition = TranslateToJoysticSpace(_centerJoystick);
                _moveByTouch = true;
            }
            else if (MyInput.EndedSwipe())
            {
                _joystickTransf.anchoredPosition = _defaultPosJoystick;
                _stickTransf.anchoredPosition = _defaultPosStick;
                _directionMove = Vector2.zero;
                _intensityMove = 0f;
                _moveByTouch = false;
            }
            else if (_moveByTouch)
            {
                Vector2 mousePos = UnityEngine.Input.mousePosition.To2D();
                Vector2 delta = mousePos - _centerJoystick;

                _directionMove = delta.normalized;
                _intensityMove = Mathf.Clamp01(delta.magnitude / _radiusJoystick);

                _stickTransf.anchoredPosition = _directionMove * Mathf.Clamp(delta.magnitude, 0, _radiusJoystick);
                if (_moveStickOnlyByX)
                    _stickTransf.anchoredPosition = new Vector2(_stickTransf.anchoredPosition.x, _defaultPosStick.y);
            }
        }
        private bool MoveByAxis()
        {
            float x = UnityEngine.Input.GetAxisRaw("Horizontal");
            float y = UnityEngine.Input.GetAxisRaw("Vertical");
            Vector2 moveThroughtAxis = Vector2.ClampMagnitude(new Vector2(x, y), 1f);

            _moveByAxis = moveThroughtAxis.magnitude > float.Epsilon;
            if (_moveByAxis)
            {
                _directionMove = moveThroughtAxis.normalized;
                _intensityMove = moveThroughtAxis.magnitude;

                // для фикса проблемы, когда одновременно запускается движение и по клавиатуре и по джойстику
                if (_moveByTouch)
                {
                    _joystickTransf.anchoredPosition = _defaultPosJoystick;
                    _stickTransf.anchoredPosition = _defaultPosStick;
                    _moveByTouch = false;
                }
            }
            else
            {
                _directionMove = Vector2.zero;
                _intensityMove = 0f;
            }

            return _moveByAxis;
        }
        /// <summary>
        /// Изображение джойстика привязано к низу центру экрана. Тоесть, если мы джойстику поставим координаты 0;0 он встанет внизу в центре. 0;0 для координаты мыши это нижний левый угол.
        /// Поэтому заданные в пространстве экрана координаты, надо переводить в пространство джойстика для передвижения изображения джойстика. 
        /// </summary>
        private Vector2 TranslateToJoysticSpace(Vector2 position)
        {
            float x = (position.x - Screen.width / 2);
            float y = position.y;

            return UiHelper.CanvasScaleCompensation(_canvasTransf, new Vector2(x, y));
        }


        public enum ViewOfJoystick
        {
            AlwaysView,
            AlwaysHide,
            ViewWhenMove
        }
    }
}

