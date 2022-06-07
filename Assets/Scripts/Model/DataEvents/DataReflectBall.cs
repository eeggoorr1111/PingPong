using PingPong.Model.Ball;
using System;
using UnityEngine;


namespace PingPong.Model
{
    public sealed class DataReflectBall
    {
        public static object Deserialize(byte[] bytes)
        {
            bool isClient = BitConverter.ToBoolean(bytes, 0);
            TrajectoryBall trajectory = TrajectoryBall.Deserialize(bytes, 1);

            return new DataReflectBall(isClient, trajectory);
        }
        public static byte[] Serialize(object obj)
        {
            DataReflectBall data = (DataReflectBall)obj;
            TrajectoryBall trajectory = data.NewTrajectoryBall;
            byte[] bytes = new byte[data.GetSizeInBytes()];

            BitConverter.GetBytes(data.IsClient).CopyTo(bytes, 0);

            trajectory.Serialize().CopyTo(bytes, 1);

            return bytes;
        }


        public DataReflectBall(bool isClient, TrajectoryBall trajectory)
        {
            IsClient = isClient;
            NewTrajectoryBall = trajectory;
        }


        public bool IsClient { get; }
        public TrajectoryBall NewTrajectoryBall { get; }


        public int GetSizeInBytes()
        {
            return 1 + NewTrajectoryBall.GetSizeInBytes();
        }
    }
}
