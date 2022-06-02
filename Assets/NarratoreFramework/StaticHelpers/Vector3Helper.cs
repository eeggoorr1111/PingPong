using UnityEngine;

namespace Narratore.Helpers
{
    public static class Vector3Helper
    {
        #region Vector3
        public static Vector3 NullY(this Vector3 vectorArg)
        {
            return new Vector3(vectorArg.x, 0, vectorArg.z);
        }
        public static Vector3 NullX(this Vector3 vectorArg)
        {
            return new Vector3(0, vectorArg.y, vectorArg.z);
        }
        public static Vector3 NullZ(this Vector3 vectorArg)
        {
            return new Vector3(vectorArg.x, vectorArg.y, 0);
        }
        /// <summary>
        /// Оставляет значение только по первой попавшейся оси. Остальные зануляет
        /// </summary>
        public static bool By1Axis(this Vector3 vectorArg, ref Vector3 res)
        {
            if (Mathf.Abs(vectorArg.x) > float.Epsilon)
            {
                res = new Vector3(vectorArg.x, 0, 0);
                return true;
            }

            if (Mathf.Abs(vectorArg.y) > float.Epsilon)
            {
                res = new Vector3(0, vectorArg.y, 0);
                return true;
            }

            if (Mathf.Abs(vectorArg.z) > float.Epsilon)
            {
                res = new Vector3(0, 0, vectorArg.z);
                return true;
            }

            return false;
        }
        /// <summary>
        /// Простое перемножение осей вектора 1 на соответствующие оси вектора 2
        /// </summary>
        public static Vector3 ScaleAxis(this Vector3 vector1Arg, Vector3 vector2Arg)
        {
            return new Vector3( vector1Arg.x * vector2Arg.x, 
                                vector1Arg.y * vector2Arg.y, 
                                vector1Arg.z * vector2Arg.z);
        }
        public static Vector3 WithX(this Vector3 vectorArg, float xArg)
        {
            return new Vector3(xArg, vectorArg.y, vectorArg.z);
        }
        public static Vector3 WithY(this Vector3 vectorArg, float yArg)
        {
            return new Vector3(vectorArg.x, yArg, vectorArg.z);
        }
        public static Vector3 WithZ(this Vector3 vectorArg, float zArg)
        {
            return new Vector3(vectorArg.x, vectorArg.y, zArg);
        }
        public static Vector2 To2D(this Vector3 vectorArg, TwoAxis leftArg = TwoAxis.XY)
        {
            switch (leftArg)
            {
                case TwoAxis.XY: return new Vector2(vectorArg.x, vectorArg.y);
                case TwoAxis.XZ: return new Vector2(vectorArg.x, vectorArg.z);
                case TwoAxis.YX: return new Vector2(vectorArg.y, vectorArg.x);
                case TwoAxis.YZ: return new Vector2(vectorArg.y, vectorArg.z);
                case TwoAxis.ZX: return new Vector2(vectorArg.z, vectorArg.x);
                case TwoAxis.ZY: return new Vector2(vectorArg.z, vectorArg.y);
                default:
                    Debug.LogError("При конвертации из Vector3 в Vector2 задана не существующая пара осей");
                    return new Vector2(vectorArg.x, vectorArg.y);
            }
        }
        public static Vector3 RandomIn(Vector3 startArg, Vector3 endArg)
        {
            float x = Random.Range(startArg.x, endArg.x);
            float y = Random.Range(startArg.y, endArg.y);
            float z = Random.Range(startArg.z, endArg.z);

            return new Vector3(x, y, z);
        }
        #endregion


        #region Vector3Int
        public static Vector3Int NullY(this Vector3Int vectorArg)
        {
            return new Vector3Int(vectorArg.x, 0, vectorArg.z);
        }
        public static Vector3Int NullX(this Vector3Int vectorArg)
        {
            return new Vector3Int(0, vectorArg.y, vectorArg.z);
        }
        public static Vector3Int NullZ(this Vector3Int vectorArg)
        {
            return new Vector3Int(vectorArg.x, vectorArg.y, 0);
        }
        /// <summary>
        /// Оставляет значение только по первой попавшейся оси. Остальные зануляет
        /// </summary>
        public static bool By1Axis(this Vector3Int vectorArg, ref Vector3Int res)
        {
            if (Mathf.Abs(vectorArg.x) > 0)
            {
                res = new Vector3Int(vectorArg.x, 0, 0);
                return true;
            }

            if (Mathf.Abs(vectorArg.y) > 0)
            {
                res = new Vector3Int(0, vectorArg.y, 0);
                return true;
            }

            if (Mathf.Abs(vectorArg.z) > 0)
            {
                res = new Vector3Int(0, 0, vectorArg.z);
                return true;
            }

            return false;
        }
        public static Vector3Int WithX(this Vector3Int vectorArg, int xArg)
        {
            return new Vector3Int(xArg, vectorArg.y, vectorArg.z);
        }
        public static Vector3Int WithY(this Vector3Int vectorArg, int yArg)
        {
            return new Vector3Int(vectorArg.x, yArg, vectorArg.z);
        }
        public static Vector3Int WithZ(this Vector3Int vectorArg, int zArg)
        {
            return new Vector3Int(vectorArg.x, vectorArg.y, zArg);
        }
        public static Vector2Int To2D(this Vector3Int vectorArg, TwoAxis leftArg = TwoAxis.XY)
        {
            switch (leftArg)
            {
                case TwoAxis.XY: return new Vector2Int(vectorArg.x, vectorArg.y);
                case TwoAxis.XZ: return new Vector2Int(vectorArg.x, vectorArg.z);
                case TwoAxis.YX: return new Vector2Int(vectorArg.y, vectorArg.x);
                case TwoAxis.YZ: return new Vector2Int(vectorArg.y, vectorArg.z);
                case TwoAxis.ZX: return new Vector2Int(vectorArg.z, vectorArg.x);
                case TwoAxis.ZY: return new Vector2Int(vectorArg.z, vectorArg.y);
                default:
                    Debug.LogError("При конвертации из Vector3Int в Vector2Int задана не существующая пара осей");
                    return new Vector2Int(vectorArg.x, vectorArg.y);
            }
        }
        public static Vector3Int RandomIn(Vector3Int startArg, Vector3Int endArg)
        {
            int x = Random.Range(startArg.x, endArg.x);
            int y = Random.Range(startArg.y, endArg.y);
            int z = Random.Range(startArg.z, endArg.z);

            return new Vector3Int(x, y, z);
        }
        #endregion
    }
}

