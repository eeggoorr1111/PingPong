using PingPong.Model.Ball;
using System;
using UnityEngine;


namespace PingPong.Network
{
    public sealed class DataTryReflectBall
    {
        public static object Deserialize(byte[] bytes)
        {
            bool isClient = BitConverter.ToBoolean(bytes, 0);
            bool isSuccess = BitConverter.ToBoolean(bytes, 1);
            TrajectoryBall trajectory = TrajectoryBall.Deserialize(bytes, 2);

            return new DataTryReflectBall(isClient, isSuccess, trajectory);
        }
        public static byte[] Serialize(object obj)
        {
            DataTryReflectBall data = (DataTryReflectBall)obj;
            TrajectoryBall trajectory = data.NewTrajectoryBall;
            byte[] bytes = new byte[data.GetSizeInBytes()];

            BitConverter.GetBytes(data.IsClient).CopyTo(bytes, 0);
            BitConverter.GetBytes(data.IsSuccess).CopyTo(bytes, 1);

            trajectory.Serialize().CopyTo(bytes, 2);

            return bytes;
        }


        public DataTryReflectBall(bool isClient, bool isSuccess, TrajectoryBall trajectory)
        {
            IsClient = isClient;
            IsSuccess = isSuccess;
            NewTrajectoryBall = trajectory;
        }


        public bool IsClient { get; }
        public bool IsSuccess { get; }
        public TrajectoryBall NewTrajectoryBall { get; }


        public int GetSizeInBytes()
        {
            return 1 + 1 + NewTrajectoryBall.GetSizeInBytes();
        }
    }
}
