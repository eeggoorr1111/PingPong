using UnityEngine;
using PingPong.Model.Ball;
using PingPong.Model.Racket;
using PingPong.Model;
using PingPong.Database;
using System;
using System.Collections.Generic;
using Narratore.Helpers;



namespace PingPong
{
    [System.Serializable]
    public sealed class ModelConfigData
    {
        public static object Deserialize(byte[] bytes)
        {
            ModelConfigData config = new ModelConfigData();
            int startByte = 0;

            config._allowableError = BitConverter.ToSingle(bytes, startByte);
            startByte += config._allowableError.Sizeof();

            config.BallParams = (BallParams)BallParams.Deserialize(bytes, startByte);
            startByte += config.BallParams.Sizeof;

            config.RacketParams = (RacketParams)RacketParams.Deserialize(bytes, startByte);
            startByte += config.RacketParams.Sizeof;

            config.MapData = (MapData)MapData.Deserialize(bytes, startByte);

            return config;
        }
        public static byte[] Serialize(object obj)
        {
            ModelConfigData data = (ModelConfigData)obj;
            List<byte> bytesRes = new List<byte>();

            bytesRes.AddRange(BitConverter.GetBytes(data.AllowableError));
            bytesRes.AddRange(BallParams.Serialize(data.BallParams));
            bytesRes.AddRange(RacketParams.Serialize(data.RacketParams));
            bytesRes.AddRange(MapData.Serialize(data.MapData));

            return bytesRes.ToArray();
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
        public MapData MapData
        {
            get { return _map; }
            set { _map = value; }
        }
        public float AllowableError => _allowableError;
        public DatabaseProvider DataBase => _dataBase;


        [SerializeField] private float _allowableError;
        [SerializeField] private BallParams _ballParams;
        [SerializeField] private RacketParams _racketParams;
        [SerializeField] private MapData _map;
        [SerializeField] private DatabaseProvider _dataBase;
    }
}
