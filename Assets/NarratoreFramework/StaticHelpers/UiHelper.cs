using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Narratore.UI
{
    public static class UiHelper
    {
        /// <summary>
        /// Когда используется <see cref= "UnityEngine.UI.CanvasScaler" /> при изменении соотношении сторон, он меняет scale канваса. Делаем на это поправку.
        /// </summary>
        public static Vector2 CanvasScaleCompensation(Transform transfCanvasArg, Vector2 posArg)
        {
            float x = posArg.x / transfCanvasArg.localScale.x;
            float y = posArg.y / transfCanvasArg.localScale.y;

            return new Vector2(x, y);
        }
    }
}

