using System;
using System.IO;
using NeuralNetwork.Interfaces;
using Presets;
using UnityEngine;

namespace NeuralNetwork.DataRecord
{
    public class GestureDataSaver : IDataSaver
    {
        public string FilePath { get; set; }

        private readonly ISerializer<GestureData> _serializer;

        public GestureDataSaver(ISerializer<GestureData> serializer)
        {
            _serializer = serializer;
        }

        public void SaveData(params object[] par)
        {
            var lastData = (GestureData) par[0];

            var patternJson = _serializer.Serialize(lastData);
            FilePath = Path.Combine(Application.persistentDataPath, (GestureType) par[1] + "Data" + DateTime.Now.Ticks);

            File.WriteAllText(FilePath, patternJson);
        }
    }
}