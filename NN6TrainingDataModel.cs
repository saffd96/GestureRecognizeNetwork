using System;
using System.Collections.Generic;
using System.Linq;
using NeuralNetwork.Interfaces;
using Presets;
using UnityEngine;

namespace NeuralNetwork
{
    public class NN6TrainingDataModel
    {
        private readonly List<(GestureData, float[])> _trainingData = new List<(GestureData, float[])>();
        private readonly IDeserializer<GestureData> _deserializer;
        private readonly IFileLoader _fileLoader;
        
        public NN6TrainingDataModel(IDeserializer<GestureData> deserializer, IFileLoader fileLoader)
        {
            _deserializer = deserializer;
            _fileLoader = fileLoader;
            CreateTrainingData();
        }

        public List<(GestureData, float[])> GetTrainingData()
        {
            return _trainingData;
        }
        
        private void CreateTrainingData()
        {
            Debug.Log($"NN LoadingTrainingData from: {Application.persistentDataPath}");

            _fileLoader.LoadFiles();

            if (!_fileLoader.LoadedFiles.Any())
            {
                Debug.LogError("NN GestureData files is not found!");
                return;
            }
            
            foreach (var file in _fileLoader.LoadedFiles.AsParallel())
            {
                var gestureData = _deserializer.Deserialize(file);

                if (gestureData is null) continue;
                
                foreach (GestureType gesture in Enum.GetValues(typeof(GestureType)))
                {
                    if (!file.Contains(gesture.ToString())) continue;
                    
                    _trainingData.Add((gestureData, GetOutputArray(gesture)));

                    break;
                }
            }
            
            Debug.Log($"NN TrainingData is loaded. Found {_fileLoader.LoadedFiles.Count} files.");
        }
        
        private float[] GetOutputArray(GestureType gesture)
        {
            switch (gesture)
            {
                case GestureType.Forward:
                    return new float[] {1, 0, 0, 0, 0, 0, 0, 0};
                case GestureType.Backward:
                    return new float[] {0, 1, 0, 0, 0, 0, 0, 0};
                case GestureType.Left:
                    return new float[] {0, 0, 1, 0, 0, 0, 0, 0};
                case GestureType.Right:
                    return new float[] {0, 0, 0, 1, 0, 0, 0, 0};
                case GestureType.Jump:
                    return new float[] {0, 0, 0, 0, 1, 0, 0, 0};
                case GestureType.Squat:
                    return new float[] {0, 0, 0, 0, 0, 1, 0, 0};
                case GestureType.TurnLeft:
                    return new float[] {0, 0, 0, 0, 0, 0, 1, 0};
                case GestureType.TurnRight:
                    return new float[] {0, 0, 0, 0, 0, 0, 0, 1};
                default:
                    return null;
            }
        }
    }
}