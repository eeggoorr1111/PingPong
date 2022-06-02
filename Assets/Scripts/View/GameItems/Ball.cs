using UnityEngine;

namespace PingPong.View.GameItems
{
    public class Ball : MonoBehaviour
    {
        public Transform Transf { get; private set; }


        public void AwakeCustom()
        {
            Transf = GetComponent<Transform>();
        }
    }
}
