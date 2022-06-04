using UnityEngine;
using UnityEngine.UI;
using System;

namespace PingPong.View.UI
{
    public sealed class PingPongUI : MonoBehaviour
    {
        public event Action<bool> OpenedWindowSettings;
        public event Action<int> SelectedSkinOfBall;


        [SerializeField] private Button _openSettingsBtn;
        [SerializeField] private Button _closeSettingsBtn;
        [SerializeField] private Canvas _windowSettings;
        [SerializeField] private GridLayoutGroup _gridSkins;
        [SerializeField] private SkinIcon _skinIconSample;
        [SerializeField] private SkinsDatabase _skins;

        
        public void AwakeCustom()
        {
            _windowSettings.enabled = false;

            _openSettingsBtn.onClick.AddListener(OpenWindowSettings);
            _closeSettingsBtn.onClick.AddListener(CloseWindowSettings);

            OpenedWindowSettings += status => { };
            SelectedSkinOfBall += index => { };
        }
        public void StartCustom()
        {
            Transform gridSkinsTransf = _gridSkins.transform;

            for (int i = 0; i < _skins.CountSkins; i++)
            {
                SkinIcon icon = Instantiate(_skinIconSample, gridSkinsTransf);

                icon.Init(_skins.GetSkin(i), i);
                icon.Selected += OnSelectedSkinOfBall;
            }
        }


        private void OpenWindowSettings()
        {
            _windowSettings.enabled = true;
            OpenedWindowSettings.Invoke(true);
        }
        private void CloseWindowSettings()
        {
            _windowSettings.enabled = false;
            OpenedWindowSettings.Invoke(false);
        }
        private void OnSelectedSkinOfBall(int index)
        {
            SelectedSkinOfBall.Invoke(index);
            CloseWindowSettings();
        }
    }
}
