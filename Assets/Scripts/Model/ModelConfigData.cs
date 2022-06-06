using UnityEngine;
using PingPong.Model.Ball;
using PingPong.Model.Racket;
using PingPong.Model;
using PingPong.Database;
using System;



namespace PingPong
{
    [System.Serializable]
    public sealed class ModelConfigData
    {
        public static int SizeBytes = 4 + BallParams.SizeBytes + RacketParams.SizeBytes + Map.SizeBytes;
        public static object Deserialize(byte[] bytes)
        {
            ModelConfigData config = new ModelConfigData();
            int startByte = 0;

            config._allowableError = BitConverter.ToSingle(bytes, startByte);
            startByte += 4;

            config.BallParams = (BallParams)BallParams.Deserialize(bytes, startByte);
            startByte += BallParams.SizeBytes;

            config.RacketParams = (RacketParams)RacketParams.Deserialize(bytes, startByte);
            startByte += RacketParams.SizeBytes;

            config.Map = (Map)Map.Deserialize(bytes, startByte);

            return config;
        }
        public static byte[] Serialize(object obj)
        {
            ModelConfigData data = (ModelConfigData)obj;
            byte[] bytes = new byte[SizeBytes];
            int startByte = 0;

            BitConverter.GetBytes(data.AllowableError).CopyTo(bytes, startByte);
            startByte += 4;

            BallParams.Serialize(data.BallParams).CopyTo(bytes, startByte);
            startByte += BallParams.SizeBytes;

            RacketParams.Serialize(data.RacketParams).CopyTo(bytes, startByte);
            startByte += RacketParams.SizeBytes;

            Map.Serialize(data.Map).CopyTo(bytes, startByte);

            return bytes;
        }


        public BallParams BallParams
        {
            get { return _ballParams; }
            set { _ballParams = value; }
        }
        public RacketParams RacketParams
        {
            get { return _racketParams; }
            set { _racketParams = value; }
        }
        public Map Map
        {
            get { return _map; }
            set { _map = value; }
        }
        public float AllowableError => _allowableError;
        public DatabaseProvider DataBase => _dataBase;


        [SerializeField] private float _allowableError;
        [SerializeField] private BallParams _ballParams;
        [SerializeField] private RacketParams _racketParams;
        [SerializeField] private Map _map;
        [SerializeField] private DatabaseProvider _dataBase;
    }
}
