using UnityEngine;

namespace PingPong.View.GameItems
{
    public class Racket : MonoBehaviour
    {
        public Transform Transf { get; private set; }


        private Vector2 _startScale;


        public void AwakeCustom()
        {
            Transf = GetComponent<Transform>();

            _startScale = Transf.localScale;
        }
        public void SetSize(Vector2 newSize)
        {
            Transf.localScale = new Vector3(newSize.x * _startScale.x, 
                                            newSize.y * _startScale.y, 
                                            0);
        }
    }
}
