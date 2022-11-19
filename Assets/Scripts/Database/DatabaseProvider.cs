using UnityEngine;
using PingPong.View;


namespace PingPong.Database
{
    [CreateAssetMenu(fileName = "DatabaseProvider", menuName = "PingPong/CreateDatabaseProvider")]
    public sealed class DatabaseProvider : ScriptableObject, IReadOnlyDatabaseProvider
    {
        private static readonly string _reflectedBallsKey = "ReflectedBalls";
        private static readonly string _skinBallKey = "SkinBall";


        public int SkinsCount => _skinsDatabase.SkinsCount;


        [SerializeField] private SkinsDatabase _skinsDatabase;


        public void SetMaxReflectedBalls(int record)
        {
            PlayerPrefs.SetInt(_reflectedBallsKey, record);
            PlayerPrefs.Save();
        }
        public void SetIdxSkinOfBall(int index)
        {
            PlayerPrefs.SetInt(_skinBallKey, index);
            PlayerPrefs.Save();
        }
        public int GetMaxReflectedBalls()
        {
            return PlayerPrefs.GetInt(_reflectedBallsKey);
        }
        public Sprite GetSavedSkinOfBall()
        {
            return GetSkin(PlayerPrefs.GetInt(_skinBallKey));
        }
        public Sprite GetSkin(int index)
        {
            return _skinsDatabase.GetSkin(index);
        }
    }
}
