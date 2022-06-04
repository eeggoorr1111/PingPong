using UnityEngine;

namespace PingPong.View.GameItems
{
    [RequireComponent(typeof(SpriteRenderer))]
    public class Ball : MonoBehaviour
    {
        public Transform Transf { get; private set; }


        private Vector2 _startScale;
        private SpriteRenderer _spriteRenderer;


        public void AwakeCustom()
        {
            Transf = GetComponent<Transform>();

            _startScale = Transf.localScale;
            _spriteRenderer = GetComponent<SpriteRenderer>();
        }
        public void SetSize(float newSize)
        {
            Transf.localScale = new Vector3(newSize * _startScale.x,
                                            newSize * _startScale.y,
                                            0);
        }
        public void SetSkin(Sprite sprite)
        {
            if (sprite == null)
                Debug.LogWarning("Try set skin is equal null");
            else
                _spriteRenderer.sprite = sprite;
        }
    }
}
