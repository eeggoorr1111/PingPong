using PingPong.Model.Ball;
using Narratore.Helpers;
using System;
using System.Collections.Generic;


namespace PingPong.Model
{
    public sealed class DataReflectBall
    {
        public static object Deserialize(byte[] bytes)
        {
            int startByte = 0;

            int playerId = BitConverter.ToInt32(bytes, 0);
            startByte += playerId.Sizeof();

            return new DataReflectBall(playerId, TrajectoryBall.Deserialize(bytes, startByte));
        }
        public static byte[] Serialize(object obj)
        {
            DataReflectBall data = (DataReflectBall)obj;
            TrajectoryBall trajectory = data.NewTrajectoryBall;
            List<byte> bytes = new List<byte>(data.Sizeof);

            bytes.AddRange(BitConverter.GetBytes(data.PlayerId));
            bytes.AddRange(TrajectoryBall.Serialize(trajectory));

            return bytes.ToArray();
        }


        public DataReflectBall(int playerId, TrajectoryBall trajectory)
        {
            PlayerId = playerId;
            NewTrajectoryBall = trajectory;
        }


        public int PlayerId { get; }
        public TrajectoryBall NewTrajectoryBall { get; }
        public int Sizeof => PlayerId.Sizeof() + NewTrajectoryBall.Sizeof;
    }
}
