using Narratore.Helpers;
using PingPong.Model.Ball;
using System;
using System.Collections.Generic;

namespace PingPong.Model
{
    public sealed class StartedGameData
    {
        public static object Deserialize(byte[] bytes)
        {
            int startByte = 0;

            float diameterBall = BitConverter.ToSingle(bytes, startByte);
            startByte += diameterBall.Sizeof();

            float speedBall = BitConverter.ToSingle(bytes, startByte);
            startByte += speedBall.Sizeof();

            BallTrajectory trajectory = BallTrajectory.Deserialize(bytes, startByte);

            return new StartedGameData(diameterBall, speedBall, trajectory);
        }
        public static byte[] Serialize(object obj)
        {
            StartedGameData data = (StartedGameData)obj;
            BallTrajectory trajectory = data.TrajectoryBall;
            List<byte> bytes = new List<byte>(data.Sizeof);

            bytes.AddRange(BitConverter.GetBytes(data.DiameterBall));
            bytes.AddRange(BitConverter.GetBytes(data.SpeedBall));
            bytes.AddRange(BallTrajectory.Serialize(trajectory));

            return bytes.ToArray();
        }


        public StartedGameData(float diameterBall, float speedBall, BallTrajectory trajectory)
        {
            DiameterBall = diameterBall;
            SpeedBall = speedBall;
            TrajectoryBall = trajectory;
        }


        public float DiameterBall { get; }
        public float SpeedBall { get; }
        public BallTrajectory TrajectoryBall { get; }
        public int Sizeof => DiameterBall.Sizeof() + SpeedBall.Sizeof() + TrajectoryBall.Sizeof;
    }
}
