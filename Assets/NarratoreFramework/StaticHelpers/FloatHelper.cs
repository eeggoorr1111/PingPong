using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Narratore.Helpers
{
    public static class FloatHelper
    {
        public static bool Is0(this float number, float allowableError = float.Epsilon)
        {
            return Mathf.Abs(number) < allowableError;
        }
        public static bool Is1(this float number, float allowableError = float.Epsilon)
        {
            return Mathf.Abs(number - 1f) < allowableError;
        }
        public static bool IsEqual(this float number, float other, float allowableError = float.Epsilon)
        {
            return Mathf.Abs(number - other) < allowableError;
        }
    }
}
    
