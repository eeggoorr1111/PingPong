using UnityEngine;
using PingPong.View;


namespace PingPong.Database
{
    [CreateAssetMenu(fileName = "DatabaseProvider", menuName = "PingPong/CreateDatabaseProvider")]
    public sealed class DatabaseProvider : ScriptableObject
    {
        private static readonly string _reflectedBallsKey = "ReflectedBalls";
        private static readonly string _skinBallKey = "SkinBall";


        public int CountSkins => _skinsDatabase.CountSkins;


        [SerializeField] private SkinsDatabase _skinsDatabase;


        public void SetMaxReflectedBalls(int record)
        {
            PlayerPrefs.SetInt(_reflectedBallsKey, record);
            PlayerPrefs.Save();
        }
        public int GetMaxReflectedBalls()
        {
            return PlayerPrefs.GetInt(_reflectedBallsKey);
        }
        public void SaveIdxSkinOfBall(int index)
        {
            PlayerPrefs.SetInt(_skinBallKey, index);
            PlayerPrefs.Save();
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
