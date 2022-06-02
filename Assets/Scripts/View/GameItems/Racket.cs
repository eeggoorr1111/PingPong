using UnityEngine;

namespace PingPong.View.GameItems
{
    public class Racket : MonoBehaviour
    {
        public Transform Transf { get; private set; }


        public void AwakeCustom()
        {
            Transf = GetComponent<Transform>();
        }
    }
}
