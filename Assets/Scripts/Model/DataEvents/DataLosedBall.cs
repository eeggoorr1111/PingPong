using Narratore.Helpers;
using PingPong.Model.Ball;
using System;
using System.Collections.Generic;


namespace PingPong.Model
{
    public sealed class DataLosedBall
    {
        public static object Deserialize(byte[] bytes)
        {
            int startByte = 0;

            int playerId = BitConverter.ToInt32(bytes, startByte);
            startByte += playerId.Sizeof();

            float newDiameterBall = BitConverter.ToSingle(bytes, startByte);
            startByte += newDiameterBall.Sizeof();

            float newSpeedBall = BitConverter.ToSingle(bytes, startByte);
            startByte += newSpeedBall.Sizeof();

            TrajectoryBall trajectory = TrajectoryBall.Deserialize(bytes, startByte);

            return new DataLosedBall(playerId, newDiameterBall, newSpeedBall, trajectory);
        }
        public static byte[] Serialize(object obj)
        {
            DataLosedBall data = (DataLosedBall)obj;
            TrajectoryBall trajectory = data.NewTrajectoryBall;
            List<byte> bytes = new List<byte>(data.Sizeof);

            bytes.AddRange(BitConverter.GetBytes(data.PlayerId));
            bytes.AddRange(BitConverter.GetBytes(data.NewDiameterBall));
            bytes.AddRange(BitConverter.GetBytes(data.NewSpeedBall));
            bytes.AddRange(TrajectoryBall.Serialize(trajectory));

            return bytes.ToArray();
        }


        public DataLosedBall(int playerId, float newDiameterBall, float newSpeedBall, TrajectoryBall trajectory)
        {
            PlayerId = playerId;
            NewDiameterBall = newDiameterBall;
            NewSpeedBall = newSpeedBall;
            NewTrajectoryBall = trajectory;
        }


        public int PlayerId { get; }
        public float NewDiameterBall { get; }
        public float NewSpeedBall { get; }
        public TrajectoryBall NewTrajectoryBall { get; }
        public int Sizeof => PlayerId.Sizeof() + NewDiameterBall.Sizeof() + 
                                NewSpeedBall.Sizeof() + NewTrajectoryBall.Sizeof;
    }
}
