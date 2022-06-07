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

            bool isClient = BitConverter.ToBoolean(bytes, 0);
            startByte += isClient.Sizeof();

            return new DataReflectBall(isClient, TrajectoryBall.Deserialize(bytes, startByte));
        }
        public static byte[] Serialize(object obj)
        {
            DataReflectBall data = (DataReflectBall)obj;
            TrajectoryBall trajectory = data.NewTrajectoryBall;
            List<byte> bytes = new List<byte>(data.Sizeof);

            bytes.AddRange(BitConverter.GetBytes(data.IsClient));
            bytes.AddRange(TrajectoryBall.Serialize(trajectory));

            return bytes.ToArray();
        }


        public DataReflectBall(bool isClient, TrajectoryBall trajectory)
        {
            IsClient = isClient;
            NewTrajectoryBall = trajectory;
        }


        public bool IsClient { get; }
        public TrajectoryBall NewTrajectoryBall { get; }
        public int Sizeof => IsClient.Sizeof() + NewTrajectoryBall.Sizeof;
    }
}
