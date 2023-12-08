using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using NeuralNetwork.Interfaces;
using Presets;
using UnityEngine;

namespace NeuralNetwork
{
    public class NNController
    {
        private Noedify.Net _network;
        private Noedify_Solver _solver;
        private NNTrainingDataContainer _nnTrainingDataContainer;

        public NNTrainingDataContainer NnTrainingDataContainer => _nnTrainingDataContainer;

        private const string CModelName = "grm";
        
        private string Path => _nnTrainingDataContainer.GetModelPath();
        private string FullPath => Path + "/" + CModelName + ".dat";

        public NNController(IDeserializer<GestureData> deserializer, IFileLoader fileLoader)
        {
            Start(deserializer, fileLoader);
        }

        private void Start(IDeserializer<GestureData> deserializer, IFileLoader fileLoader)
        {
            Debug.Log($"NN <color=yellow>Starting TrainingDataContainer creating</color>");
            _nnTrainingDataContainer = new NNTrainingDataContainer(new NN6TrainingDataModel(deserializer, fileLoader));
            Debug.Log($"NN <color=green>TrainingDataContainer is created.</color>");

            Debug.Log($"NN <color=yellow>Starting model creating.</color> ");
            LoadOrCreateModel();

            if (_network != null)
            {
                Debug.Log($"NN <color=green>Model is ready</color>.");
            }
        }

        public int GetPrediction(GestureData gestureData)
        {
            if (gestureData == null)
            {
                Debug.Log($"NN <color=red>Can't find any data for prediction</color>.");
                return -1;
            }

            Debug.Log($"NN <color=yellow>Starting prediction creating.</color>");

            _solver.Evaluate(_network, TransformGestureData(gestureData));

            var result = _solver.prediction;
            
            for (var i = 0; i < result.Length; i++)
            {
                Debug.Log("NN gesture predict" + (GestureType) i + " " + result[i]);
            }

            var maxElemGestureIndex = Array.IndexOf(result, result.Max());
            Debug.Log("NN gesture predict" + "Max: " + (GestureType) maxElemGestureIndex);
            return maxElemGestureIndex;
        }

        public void TrainModel(int epochs)
        {
            if (_nnTrainingDataContainer.GetTrainingDataCount() > 0)
            {
                Debug.Log($"NN <color=yellow>Starting model training</color>.");
                
                var outputs = PrepareTrainingData();

                _solver.TrainNetwork(
                    _network,
                    outputs.Item1,
                    outputs.Item2,
                    epochs,
                    epochs / 10,
                    0.5f,
                    Noedify_Solver.CostFunction.MeanSquare,
                    Noedify_Solver.SolverMethod.MainThread
                );

                SaveModel();
                Debug.Log($"NN <color=green>Training is over.</color>");
            }
            else
            {
                Debug.Log($"NN <color=red>Can't find any data for training</color>!");
            }
        }

        public void SaveModel()
        {
            Debug.Log($"NN <color=yellow>Saving model</color>");
            _network.SaveModel(CModelName, Path);
            Debug.Log($"NN <color=green>Model saved in {Path}</color>");
        }

        public void LoadOrCreateModel()
        {
            _network = new Noedify.Net();
            
            Debug.Log($"NN Create new solver");
            _solver = Noedify.CreateSolver();
            Debug.Log($"NN Solver created");

            if (File.Exists(FullPath))
            {
                Debug.Log($"NN <color=green>Found model file in: {Path}</color>");
                Debug.Log("NN <color=yellow>Loading model</color>");
            
                _network.LoadModel(CModelName, Path);
            
                return;
            }

            Debug.Log($"NN <color=red>Don't find any model file in: {Path}</color>");
            Debug.Log($"NN <color=green>Create new model</color>");

            _network = CreateModel();

            SaveModel();
        }

        private Noedify.Net CreateModel()
        {
            var baseModel = new Noedify.Net();

            var inputCount = _nnTrainingDataContainer.GetInputDataCount();

            Noedify.Layer inputLayer = new Noedify.Layer(
                Noedify.LayerType.Input,
                inputCount,
                "input layer"
            );
            baseModel.AddLayer(inputLayer);

            Noedify.Layer hiddenLayer0 = new Noedify.Layer(
                Noedify.LayerType.FullyConnected,
                inputCount * 2,
                Noedify.ActivationFunction.Tanh,
                "fully connected 1"
            );
            baseModel.AddLayer(hiddenLayer0);

            Noedify.Layer hiddenLayer1 = new Noedify.Layer(
                Noedify.LayerType.FullyConnected,
                inputCount / 8,
                Noedify.ActivationFunction.Tanh,
                "fully connected 2"
            );
            baseModel.AddLayer(hiddenLayer1);

            Noedify.Layer outputLayer = new Noedify.Layer(
                Noedify.LayerType.Output,
                _nnTrainingDataContainer.GetAllGestures().Length,
                Noedify.ActivationFunction.Sigmoid,
                "output layer"
            );
            baseModel.AddLayer(outputLayer);

            baseModel.BuildNetwork();

            Debug.Log($"NN New model created. Model params: " +
                      //$"\n Activate function: {typeof(SigmoidFunction)}," +
                      $"\n Input layer neurons count: {inputCount}," +
                      $"\n Hidden layer 1 neurons count: {inputCount * 2}," +
                      $"\n Hidden layer 2 neurons count: {inputCount / 8}," +
                      $"\n Output layer neurons count: {_nnTrainingDataContainer.GetAllGestures().Length}");

            return baseModel;
        }

        private (List<float[,,]>, List<float[]> ) PrepareTrainingData()
        {
            var trainingData = _nnTrainingDataContainer.GetTrainingData();
            var inputNeuronsCount = trainingData[0].Item1.TransformDataInToList().Count;
            var allGesturesCount = _nnTrainingDataContainer.GetAllGestures().Length;

            var returnInputsList = new List<float[,,]>();
            var returnOutputsList = new List<float[]>();
            
            foreach (var (gestureData, floats) in trainingData.AsParallel())
            {
                var returnInputs = new float[inputNeuronsCount];
                var returnOutputs = new float[inputNeuronsCount];

                for (int j = 0; j < inputNeuronsCount; j++)
                {
                    returnInputs[j] = gestureData.TransformDataInToList()[j];
                }

                for (int i = 0; i < allGesturesCount; i++)
                {
                    returnOutputs = floats;
                }
                
                returnInputsList.Add(Noedify_Utils.AddTwoSingularDims(returnInputs));
                returnOutputsList.Add(returnOutputs);
            }
            
            return (returnInputsList, returnOutputsList);
        }

        private float[,,] TransformGestureData(GestureData gestureData)
        {
            return Noedify_Utils.AddTwoSingularDims(gestureData.TransformDataInToList().ToArray());
        }
    }
}