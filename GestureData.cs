using System;
using System.Collections.Generic;

namespace NeuralNetwork
{
    [Serializable]
    public class GestureData
    {
        public readonly List<SimpleVector> AccData;
        public readonly List<SimpleVector> GyroData;

        public GestureData(List<SimpleVector> accData, List<SimpleVector> gyroData)
        {
            AccData = accData.CheckForNaNValues();
            GyroData = gyroData.CheckForNaNValues();
        }

        public List<float> TransformDataInToList()
        {
            var resultList = new List<float>();

            resultList.AddRange(AddValuesToList(AccData));
            resultList.AddRange(AddValuesToList(GyroData));

            return resultList;
        }

        private List<float> AddValuesToList(List<SimpleVector> valuesList)
        {
            var result = new List<float>();

            foreach (var vector3 in valuesList)
            {
                result.Add(vector3.X);
                result.Add(vector3.Y);
                result.Add(vector3.Z);
            }

            return result;
        }
    }
}