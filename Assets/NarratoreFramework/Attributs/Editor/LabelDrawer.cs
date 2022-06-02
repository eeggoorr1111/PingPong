using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using Narratore.Helpers;

namespace Narratore.Attributs
{
    [CustomPropertyDrawer(typeof(LabelAttribute))]
    public class LabelDrawer : DecoratorDrawer
    {
        protected GUIStyle _style = GetStyle();


        public override void OnGUI(Rect posArg)
        {
            LabelAttribute label = attribute as LabelAttribute;
            EditorGUI.TextField(posArg, label.header, _style);
        }
        public override float GetHeight()
        {
            LabelAttribute label = attribute as LabelAttribute;
            return _style.CalcHeight(new GUIContent(label.header), Screen.width);
        }


        protected static GUIStyle GetStyle()
        {
            GUIStyle style = EditorStyles.helpBox;

            style.padding = new RectOffset(6, 6, 10, 10);

            return style;
        }
    }
}
