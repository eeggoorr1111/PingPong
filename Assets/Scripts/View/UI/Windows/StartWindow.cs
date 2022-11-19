using UnityEngine;
using System;
using UnityEngine.UI;
using TMPro;

namespace PingPong.View.UI.Windows
{
    [RequireComponent(typeof(Canvas))]
    public sealed class StartWindow : MonoBehaviour
    {
        public event Action PressedStartGameBtn;
        public event Action PressedCreateRoomBtn;
        public event Action PressedJoinRoomBtn;


        [SerializeField] private Button _startGameBtn;
        [SerializeField] private Button _createRoomBtn;
        [SerializeField] private Button _joinRoomBtn;
        [SerializeField] private TextMeshProUGUI _joinServerLbl;


        private Canvas _canvas;


        public void Init()
        {
            PressedStartGameBtn += () => { };
            PressedCreateRoomBtn += () => { };
            PressedJoinRoomBtn += () => { };

            _startGameBtn.onClick.AddListener(OnPressedStartGameBtn);
            _createRoomBtn.onClick.AddListener(OnPressedCreateRoomBtn);
            _joinRoomBtn.onClick.AddListener(OnPressedJoinRoomBtn);

            _canvas = GetComponent<Canvas>();
        }
        public void ActivateNetworkButtons()
        {
            _joinServerLbl.gameObject.SetActive(false);

            _createRoomBtn.gameObject.SetActive(true);
            _joinRoomBtn.gameObject.SetActive(true);
        }


        private void OnPressedStartGameBtn()
        {
            _canvas.enabled = false;
            PressedStartGameBtn.Invoke();
        }
        private void OnPressedCreateRoomBtn()
        {
            _canvas.enabled = false;
            PressedCreateRoomBtn.Invoke();
        }
        private void OnPressedJoinRoomBtn()
        {
            _canvas.enabled = false;
            PressedJoinRoomBtn.Invoke();
        }
        private void OnDestroy()
        {
            _startGameBtn.onClick.RemoveListener(OnPressedStartGameBtn);
            _createRoomBtn.onClick.RemoveListener(OnPressedCreateRoomBtn);
            _joinRoomBtn.onClick.RemoveListener(OnPressedJoinRoomBtn);
        }
    }
}
