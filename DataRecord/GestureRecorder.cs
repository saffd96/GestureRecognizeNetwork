using System.Collections.Generic;
using NeuralNetwork.Interfaces;
using Presets;
using UnityEngine;

namespace NeuralNetwork.DataRecord
{
    public class GestureRecorder : IDataRecorder<GestureType>
    {
        public bool IsRecording { get; set; }
        public GestureData CurrentData { get; private set; }
        public List<SimpleVector> AccelerationData => _accelerationData;
        public List<SimpleVector> GyroData => _gyroData;

        private GestureType _gestureType;

        private readonly List<SimpleVector> _accelerationData = new List<SimpleVector>();
        private readonly List<SimpleVector> _gyroData = new List<SimpleVector>();
        
        private readonly IDataSaver _gestureDataSaver;
        
        private readonly int _bufferSize;
        private readonly int _patternLenght;

        public GestureRecorder(IDataSaver gestureDataSaver, int bufferSize, int patternLenght)
        {
            _gestureDataSaver = gestureDataSaver;
            _bufferSize = bufferSize;
            _patternLenght = patternLenght;
        }

        public void StartRecording(GestureType gesture)
        {
            ClearData();
            _gestureType = gesture;
            IsRecording = true;
        }

        public void StartRecording() //причесать в будущем
        {
            ClearData();
            IsRecording = true;
        }

        public void StopRecording()
        {
            IsRecording = false;
            SaveData();
        }

        public void CancelRecording()
        {
            IsRecording = false;
            ClearData();
        }

        public void Record()
        {
            if (!IsRecording) return;

            var accelerometer = new SimpleVector(Input.acceleration);
            var gyro = new SimpleVector(Input.gyro.rotationRateUnbiased);

            _accelerationData.Add(accelerometer);
            _gyroData.Add(gyro);

            CheckAndyBuffer(_accelerationData);
            CheckAndyBuffer(_gyroData);
        }

        public bool IsAndyBufferFull()
        {
            var accDataCount = _accelerationData.Count;
            var gyroDataCount = _gyroData.Count;

            return accDataCount == _bufferSize && gyroDataCount == _bufferSize;
        }

        private void CheckAndyBuffer(List<SimpleVector> data)
        {
            if (data.Count > _bufferSize)
                data.RemoveAt(0);
        }

        private void SaveData()
        {
            var smoothGestureData = GetSmoothGestureData(_accelerationData, _gyroData, _patternLenght);
            _gestureDataSaver.SaveData(smoothGestureData, _gestureType);
        }

        private void ClearData()
        {
            _accelerationData.Clear();
            _gyroData.Clear();
        }

        private GestureData GetSmoothGestureData(List<SimpleVector> accData, List<SimpleVector> gyroData, int length)
        {
            if (accData.Count <= length && gyroData.Count <= length)
                return new GestureData(accData, gyroData);

            var accSmoothedData = NormalizedData(accData, length);
            var gyroSmoothedData = NormalizedData(gyroData, length);

            return CurrentData = new GestureData(accSmoothedData, gyroSmoothedData);
        }

        private List<SimpleVector> NormalizedData(List<SimpleVector> data, int length)
        {
            var count = data.Count;
            var windowSize = (int) Mathf.Ceil((float) count / length);
            List<SimpleVector> smoothedData = new List<SimpleVector>(length);

            for (int i = 0; i < length; i++)
            {
                // Вычисляем начальный индекс текущего окна
                int start = i * windowSize;

                // Вычисляем конечный индекс текущего окна, не превышающий размер списка data
                int end = Mathf.Min(start + windowSize, count);

                // Вычисляем сумму всех векторов в текущем окне
                SimpleVector sum = new SimpleVector(Vector3.zero);

                for (int j = start; j < end; j++)
                {
                    sum += data[j];
                }

                // Вычисляем усредненный вектор текущего окна
                SimpleVector smoothedAcc = sum / (end - start);

                smoothedData.Add(smoothedAcc);
            }

            return smoothedData;
        }
    }
}