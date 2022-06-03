using UnityEngine;

namespace PingPong.View.GameItems
{
    public class Ball : MonoBehaviour
    {
        public Transform Transf { get; private set; }


        private Vector2 _startScale;


        public void AwakeCustom()
        {
            Transf = GetComponent<Transform>();

            _startScale = Transf.localScale;
        }
        public void SetSize(float newSize)
        {
            Transf.localScale = new Vector3(newSize * _startScale.x,
                                            newSize * _startScale.y,
                                            0);
        }
    }
}
