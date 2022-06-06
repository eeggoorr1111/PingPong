using UnityEngine;
using PingPong.View.UI.Windows;
using UnityEngine.UI;
using TMPro;


namespace PingPong.View.UI
{
    /// <summary>
    /// Не оч хорошо из <see cref="Main"/> напрямую получать окна UI. Для меньшей связанности лучше было бы сделать
    /// публичный API у UI для подписки на нужные события. Но для тестового пока так.
    /// </summary>
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
