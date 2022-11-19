using UnityEngine;

namespace PingPong.Database
{
    public interface IReadOnlyDatabaseProvider
    {
        int GetMaxReflectedBalls();
        Sprite GetSavedSkinOfBall();
        Sprite GetSkin(int index);
    }
}
