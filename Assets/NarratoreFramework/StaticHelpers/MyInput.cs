using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Narratore.Input
{
    public static class MyInput
    {
        public static bool StartedSwipe()
        {
            Vector2 pos = Vector2.zero;
            return StartedSwipe(ref pos);
        }
        public static bool StartedSwipe(ref Vector2 position)
        {
            if (UnityEngine.Input.touchSupported)
            {
                if (UnityEngine.Input.touchCount > 0)
                {
                    Touch t = UnityEngine.Input.GetTouch(0);
                    if (t.phase == TouchPhase.Began)
                    {
                        position = new Vector2(t.position.x / (float)Screen.width, t.position.y / (float)Screen.height);
                        return true;
                    }
                }

                return false;
            }
            else if (UnityEngine.Input.GetMouseButtonDown(0))
            {
                Vector2 mousePosition = UnityEngine.Input.mousePosition;
                position = new Vector2(mousePosition.x / (float)Screen.width, mousePosition.y / (float)Screen.height);

                return true;
            }

            return false;
        }
        public static bool EndedSwipe()
        {
            Vector2 pos = Vector2.zero;
            return EndedSwipe(ref pos);
        }
        public static bool EndedSwipe(ref Vector2 position)
        {
            if (UnityEngine.Input.touchSupported)
            {
                if (UnityEngine.Input.touchCount > 0)
                {
                    Touch t = UnityEngine.Input.GetTouch(0);
                    if (t.phase == TouchPhase.Ended)
                    {
                        position = new Vector2(t.position.x / (float)Screen.width, t.position.y / (float)Screen.height);
                        return true;
                    }
                }

                return false;
            }
            else if (UnityEngine.Input.GetMouseButtonUp(0))
            {
                Vector2 mousePosition = UnityEngine.Input.mousePosition;
                position = new Vector2(mousePosition.x / (float)Screen.width, mousePosition.y / (float)Screen.height);

                return true;
            }

            return false;
        }
    }
}

