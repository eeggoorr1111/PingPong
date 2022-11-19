using Narratore.Helpers;
using PingPong.Model.Ball;
using System;
using System.Collections.Generic;


namespace PingPong.Model
{
    public sealed class LosedBallData
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

            BallTrajectory trajectory = BallTrajectory.Deserialize(bytes, startByte);

            return new LosedBallData(playerId, newDiameterBall, newSpeedBall, trajectory);
        }
        public static byte[] Serialize(object obj)
        {
            LosedBallData data = (LosedBallData)obj;
            BallTrajectory trajectory = data.NewTrajectoryBall;
            List<byte> bytes = new List<byte>(data.Sizeof);

            bytes.AddRange(BitConverter.GetBytes(data.PlayerId));
            bytes.AddRange(BitConverter.GetBytes(data.NewDiameterBall));
            bytes.AddRange(BitConverter.GetBytes(data.NewSpeedBall));
            bytes.AddRange(BallTrajectory.Serialize(trajectory));

            return bytes.ToArray();
        }


        public LosedBallData(int playerId, float newDiameterBall, float newSpeedBall, BallTrajectory trajectory)
        {
            PlayerId = playerId;
            NewDiameterBall = newDiameterBall;
            NewSpeedBall = newSpeedBall;
            NewTrajectoryBall = trajectory;
        }


        public int PlayerId { get; }
        public float NewDiameterBall { get; }
        public float NewSpeedBall { get; }
        public BallTrajectory NewTrajectoryBall { get; }
        public int Sizeof => PlayerId.Sizeof() + NewDiameterBall.Sizeof() + 
                                NewSpeedBall.Sizeof() + NewTrajectoryBall.Sizeof;
    }
}
