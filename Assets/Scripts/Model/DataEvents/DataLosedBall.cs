using PingPong.Model.Ball;
using System;


namespace PingPong.Model
{
    public sealed class DataLosedBall
    {
        public static object Deserialize(byte[] bytes)
        {
            bool isClient = BitConverter.ToBoolean(bytes, 0);
            float newDiameterBall = BitConverter.ToSingle(bytes, 1);
            float newSpeedBall = BitConverter.ToSingle(bytes, 5);
            TrajectoryBall trajectory = TrajectoryBall.Deserialize(bytes, 9);

            return new DataLosedBall(isClient, newDiameterBall, newSpeedBall, trajectory);
        }
        public static byte[] Serialize(object obj)
        {
            DataLosedBall data = (DataLosedBall)obj;
            TrajectoryBall trajectory = data.NewTrajectoryBall;
            byte[] bytes = new byte[data.GetSizeInBytes()];

            BitConverter.GetBytes(data.IsClient).CopyTo(bytes, 0);
            BitConverter.GetBytes(data.NewDiameterBall).CopyTo(bytes, 1);
            BitConverter.GetBytes(data.NewSpeedBall).CopyTo(bytes, 5);

            trajectory.Serialize().CopyTo(bytes, 9);

            return bytes;
        }


        public DataLosedBall(bool isClient, float newDiameterBall, float newSpeedBall, TrajectoryBall trajectory)
        {
            IsClient = isClient;
            NewDiameterBall = newDiameterBall;
            NewSpeedBall = newSpeedBall;
            NewTrajectoryBall = trajectory;
        }


        public bool IsClient { get; }
        public float NewDiameterBall { get; }
        public float NewSpeedBall { get; }
        public TrajectoryBall NewTrajectoryBall { get; }


        public int GetSizeInBytes()
        {
            return 1 + 4 + 4 + NewTrajectoryBall.GetSizeInBytes();
        }
    }
}
