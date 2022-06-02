using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Narratore.Helpers
{
    public static class Vector2Helper
    {
        public static Vector3 To3D(this Vector2 vector, TwoAxis setToArg = TwoAxis.XY, float thirdAxisArg = 0f)
        {
            switch (setToArg)
            {
                case TwoAxis.XY: return new Vector3(vector.x, vector.y, thirdAxisArg);
                case TwoAxis.XZ: return new Vector3(vector.x, thirdAxisArg, vector.y);
                case TwoAxis.YX: return new Vector3(vector.y, vector.x, thirdAxisArg);
                case TwoAxis.YZ: return new Vector3(thirdAxisArg, vector.x, vector.y);
                case TwoAxis.ZX: return new Vector3(vector.y, thirdAxisArg, vector.x);
                case TwoAxis.ZY: return new Vector3(thirdAxisArg, vector.y, vector.x);
                default:
                    Debug.LogError("При конвертации из Vector2 в Vector3 задана не существующая пара осей");
                    return new Vector3(vector.x, vector.y, thirdAxisArg);
            }
        }
        public static Vector2 RandomIn(Vector2 startArg, Vector2 endArg)
        {
            float x = Random.Range(startArg.x, endArg.x);
            float y = Random.Range(startArg.y, endArg.y);

            return new Vector2(x, y);
        }



        public static Vector3Int To3D(this Vector2Int vector, TwoAxis setToArg = TwoAxis.XY, int thirdAxisArg = 0)
        {
            switch (setToArg)
            {
                case TwoAxis.XY: return new Vector3Int(vector.x, vector.y, thirdAxisArg);
                case TwoAxis.XZ: return new Vector3Int(vector.x, thirdAxisArg, vector.y);
                case TwoAxis.YX: return new Vector3Int(vector.y, vector.x, thirdAxisArg);
                case TwoAxis.YZ: return new Vector3Int(thirdAxisArg, vector.x, vector.y);
                case TwoAxis.ZX: return new Vector3Int(vector.y, thirdAxisArg, vector.x);
                case TwoAxis.ZY: return new Vector3Int(thirdAxisArg, vector.y, vector.x);
                default:
                    Debug.LogError("При конвертации из Vector2Int в Vector3Int задана не существующая пара осей");
                    return new Vector3Int(vector.x, vector.y, thirdAxisArg);
            }
        }
        public static Vector2Int RandomIn(Vector2Int startArg, Vector2Int endArg)
        {
            int x = Random.Range(startArg.x, endArg.x);
            int y = Random.Range(startArg.y, endArg.y);

            return new Vector2Int(x, y);
        }
    }
}

