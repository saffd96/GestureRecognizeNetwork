using System;
using System.Collections.Generic;
using Presets;
using UnityEngine;

namespace NeuralNetwork
{
    public class NNTrainingDataContainer
    {
        private readonly string _modelPath;
        private readonly List<(GestureData, float[])> _trainingData;
        private readonly int _trainingDataCount;
        private readonly int _inputDataCount;
        private readonly Array _allGestures;
        
        public NNTrainingDataContainer(NN6TrainingDataModel modelData)
        {
            _modelPath = Application.persistentDataPath;
            _trainingData = modelData.GetTrainingData();
            _trainingDataCount = _trainingData.Count;
            _inputDataCount = _trainingData.Count > 0 ? _trainingData[0].Item1.TransformDataInToList().Count : 192;
            _allGestures = Enum.GetValues(typeof(GestureType));
        }

        public List<(GestureData, float[])> GetTrainingData()
        {
            return _trainingData;
        }

        public int GetTrainingDataCount()
        {
            return _trainingDataCount;
        }

        public int GetInputDataCount()
        {
            return _inputDataCount;
        }

        public Array GetAllGestures()
        {
            return _allGestures;
        }

        public string GetModelPath()
        {
            return _modelPath;
        }
    }
}