using Narratore.Helpers;
using PingPong.Model.Ball;
using System;
using System.Collections.Generic;

namespace PingPong.Model
{
    public sealed class DataStartedGame
    {
        public static object Deserialize(byte[] bytes)
        {
            int startByte = 0;

            float diameterBall = BitConverter.ToSingle(bytes, startByte);
            startByte += diameterBall.Sizeof();

            float speedBall = BitConverter.ToSingle(bytes, startByte);
            startByte += speedBall.Sizeof();

            TrajectoryBall trajectory = TrajectoryBall.Deserialize(bytes, startByte);

            return new DataStartedGame(diameterBall, speedBall, trajectory);
        }
        public static byte[] Serialize(object obj)
        {
            DataStartedGame data = (DataStartedGame)obj;
            TrajectoryBall trajectory = data.TrajectoryBall;
            List<byte> bytes = new List<byte>(data.Sizeof);

            bytes.AddRange(BitConverter.GetBytes(data.DiameterBall));
            bytes.AddRange(BitConverter.GetBytes(data.SpeedBall));
            bytes.AddRange(TrajectoryBall.Serialize(trajectory));

            return bytes.ToArray();
        }


        public DataStartedGame(float diameterBall, float speedBall, TrajectoryBall trajectory)
        {
            DiameterBall = diameterBall;
            SpeedBall = speedBall;
            TrajectoryBall = trajectory;
        }


        public float DiameterBall { get; }
        public float SpeedBall { get; }
        public TrajectoryBall TrajectoryBall { get; }
        public int Sizeof => DiameterBall.Sizeof() + SpeedBall.Sizeof() + TrajectoryBall.Sizeof;
    }
}
