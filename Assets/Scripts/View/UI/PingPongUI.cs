using UnityEngine;
using PingPong.View.UI.Windows;
using UnityEngine.UI;
using TMPro;


namespace PingPong.View.UI
{
    /// <summary>
    /// �� �� ������ �� <see cref="Main"/> �������� �������� ���� UI. ��� ������� ����������� ����� ���� �� �������
    /// ��������� API � UI ��� �������� �� ������ �������. �� ��� ��������� ���� ���.
    /// </summary>
    public sealed class PingPongUI : MonoBehaviour
    {
        public SkinsWindow SkinsWindow => _skinWindow;
        public StartWindow StartWindow => _startWindow;


        [Header("WINDOWS")]
        [SerializeField] private SkinsWindow _skinWindow;
        [SerializeField] private StartWindow _startWindow;

        [Header("GAME UI ITEMS")]
        [SerializeField] private Button _openWindowSkinsBtn;
        [SerializeField] private TextMeshProUGUI _reflectedBallLbl;
        [SerializeField] private TextMeshProUGUI _recordReflectedBallLbl;


        public void StartCustom()
        {
            _startWindow.enabled = true;
            _skinWindow.enabled = false;

            _skinWindow.Init();
            _startWindow.Init();

            _openWindowSkinsBtn.onClick.AddListener(_skinWindow.Open);
        }

        public void UpdateReflectedBallsInfo(int reflectedBalls, int recordReflectedBalls)
        {
            _reflectedBallLbl.text = reflectedBalls.ToString();
            _recordReflectedBallLbl.text = recordReflectedBalls.ToString();
        }


        private void OnDestroy()
        {
            _openWindowSkinsBtn.onClick.RemoveListener(_skinWindow.Open);
        }
    }
}
