using UnityEngine;

namespace PingPong
{
    public sealed class DataBase : MonoBehaviour
    {
        private static readonly string _reflectedBallsKey = "ReflectedBalls";
        private static readonly string _skinBallKey = "ReflectedBalls";


        public void SetMaxReflectedBalls(int record)
        {
            PlayerPrefs.SetInt(_reflectedBallsKey, record);
            PlayerPrefs.Save();
        }
        public int GetMaxReflectedBalls()
        {
            return PlayerPrefs.GetInt(_reflectedBallsKey);
        }
        public void SetIdxSkinOfBall(int index)
        {
            PlayerPrefs.SetInt(_skinBallKey, index);
            PlayerPrefs.Save();
        }
        public int GetIdxSkinOfBall()
        {
            return PlayerPrefs.GetInt(_skinBallKey);
        }
    }
}
