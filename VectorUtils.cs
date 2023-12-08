using System.Collections.Generic;
using NeuralNetwork;

public static class VectorUtils
{
    public static List<SimpleVector> CheckForNaNValues(this List<SimpleVector> data)
    {
        var result = new List<SimpleVector>();

        foreach (var v in data)
        {
            var newV = new SimpleVector(
                float.IsNaN(v.X) ? 0 : v.X,
                float.IsNaN(v.Y) ? 0 : v.Y,
                float.IsNaN(v.Z) ? 0 : v.Z
            );

            result.Add(newV);
        }

        return result;
    }
}