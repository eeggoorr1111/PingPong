using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections.Generic;
using PingPong.Database;

namespace PingPong.View.UI
{
    [RequireComponent(typeof(Canvas))]
    public sealed class WindowSkins : MonoBehaviour
    {
        public event Action<bool> OpenedWindowSkins;
        public event Action<int> SelectedSkinOfBall;


        [SerializeField] private Button _closeSettingsBtn;
        [SerializeField] private GridLayoutGroup _gridSkins;
        [SerializeField] private SkinIcon _skinIconSample;
        [SerializeField] private DatabaseProvider _database;


        private Canvas _canvas;
        private List<SkinIcon> _icons;


        public void Init()
        {
            _canvas = GetComponent<Canvas>();
            _canvas.enabled = false;

            _closeSettingsBtn.onClick.AddListener(Close);

            OpenedWindowSkins += status => { };
            SelectedSkinOfBall += index => { };

            _icons = new List<SkinIcon>();

            Transform gridSkinsTransf = _gridSkins.transform;
            for (int i = 0; i < _database.CountSkins; i++)
            {
                SkinIcon icon = Instantiate(_skinIconSample, gridSkinsTransf);

                icon.Init(_database.GetSkin(i), i);
                icon.Selected += OnSelectedSkinOfBall;

                _icons.Add(icon);
            }
        }
        public void Open()
        {
            if (!_canvas.enabled)
            {
                _canvas.enabled = true;
                OpenedWindowSkins.Invoke(true);
            }
        }


        private void Close()
        {
            _canvas.enabled = false;
            OpenedWindowSkins.Invoke(false);
        }
        private void OnSelectedSkinOfBall(int index)
        {
            _database.SaveIdxSkinOfBall(index);

            SelectedSkinOfBall.Invoke(index);
            Close();
        }
        private void OnDestroy()
        {
            foreach (var icon in _icons)
                icon.Selected -= OnSelectedSkinOfBall;
        }
    }
}
