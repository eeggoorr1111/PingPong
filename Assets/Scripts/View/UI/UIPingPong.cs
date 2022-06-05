using UnityEngine;
using PingPong.View.UI.Windows;
using UnityEngine.UI;
using TMPro;


namespace PingPong.View.UI
{
    public sealed class UIPingPong : MonoBehaviour
    {
        public WindowSkins WindowSkins => _skinWindows;
        public WindowStart WindowStart => _startWindows;


        [Header("WINDOWS")]
        [SerializeField] private WindowSkins _skinWindows;
        [SerializeField] private WindowStart _startWindows;

        [Header("GAME UI ITEMS")]
        [SerializeField] private Button _openWindowSkinsBtn;
        [SerializeField] private TextMeshProUGUI _reflectedBallLbl;
        [SerializeField] private TextMeshProUGUI _recordReflectedBallLbl;


        public void StartCustom()
        {
            _startWindows.enabled = true;
            _skinWindows.enabled = false;

            _skinWindows.Init();
            _startWindows.Init();

            _openWindowSkinsBtn.onClick.AddListener(_skinWindows.Open);
        }
        public void UpdateReflectedBallsInfo(int reflectedBalls, int recordReflectedBalls)
        {
            _reflectedBallLbl.text = reflectedBalls.ToString();
            _recordReflectedBallLbl.text = recordReflectedBalls.ToString();
        }


        private void OnDestroy()
        {
            _openWindowSkinsBtn.onClick.RemoveListener(_skinWindows.Open);
        }
    }
}
