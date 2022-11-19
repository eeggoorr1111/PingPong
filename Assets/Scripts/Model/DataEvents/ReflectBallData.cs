using PingPong.Model.Ball;
using Narratore.Helpers;
using System;
using System.Collections.Generic;


namespace PingPong.Model
{
    public sealed class ReflectBallData
    {
        public static object Deserialize(byte[] bytes)
        {
            int startByte = 0;

            int playerId = BitConverter.ToInt32(bytes, 0);
            startByte += playerId.Sizeof();

            return new ReflectBallData(playerId, BallTrajectory.Deserialize(bytes, startByte));
        }
        public static byte[] Serialize(object obj)
        {
            ReflectBallData data = (ReflectBallData)obj;
            BallTrajectory trajectory = data.NewTrajectoryBall;
            List<byte> bytes = new List<byte>(data.Sizeof);

            bytes.AddRange(BitConverter.GetBytes(data.PlayerId));
            bytes.AddRange(BallTrajectory.Serialize(trajectory));

            return bytes.ToArray();
        }


        public ReflectBallData(int playerId, BallTrajectory trajectory)
        {
            PlayerId = playerId;
            NewTrajectoryBall = trajectory;
        }


        public int PlayerId { get; }
        public BallTrajectory NewTrajectoryBall { get; }
        public int Sizeof => PlayerId.Sizeof() + NewTrajectoryBall.Sizeof;
    }
}
