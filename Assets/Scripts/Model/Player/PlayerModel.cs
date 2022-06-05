using UnityEngine;
using PingPong.Database;


namespace PingPong.Model.Player
{
    public class PlayerModel : IPlayer
    {
        public PlayerModel(DatabaseProvider dataBase)
        {
            _dataBase = dataBase;
            RecordReflectedBalls = _dataBase.GetMaxReflectedBalls();
        }


        public int RecordReflectedBalls { get; private set; }
        public int ReflectedBalls { get; private set; }


        private readonly DatabaseProvider _dataBase;


        public void ReflectedBall()
        {
            ReflectedBalls++;

            if (ReflectedBalls > RecordReflectedBalls)
            {
                _dataBase.SetMaxReflectedBalls(ReflectedBalls);
                RecordReflectedBalls = ReflectedBalls;
            }
        }
        public void LoseBall()
        {
            ReflectedBalls = 0;
        }
    }
}
