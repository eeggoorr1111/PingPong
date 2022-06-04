using UnityEngine;
using UnityEngine.UI;
using System;

namespace PingPong.View.UI
{
    [RequireComponent(typeof(Button), typeof(Image))]
    public sealed class SkinIcon : MonoBehaviour
    {
        public event Action<int> Selected;


        private Button _button;
        private Image _img;
        private int _indexIcon;


        public void Init(Sprite img, int index)
        {
            _button = GetComponent<Button>();
            _img = GetComponent<Image>();

            _indexIcon = index;
            _img.sprite = img;

            _button.onClick.AddListener(OnSelected);
        }


        private void OnSelected()
        {
            Selected.Invoke(_indexIcon);
        }
        private void OnDestroy()
        {
            _button.onClick.RemoveListener(OnSelected);
        }
    }
}
