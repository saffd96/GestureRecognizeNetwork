using System;
using System.Runtime.CompilerServices;
using UnityEngine;

namespace NeuralNetwork
{
    [Serializable]
    public readonly struct SimpleVector
    {
        public readonly float X;
        public readonly float Y;
        public readonly float Z;

        public SimpleVector(float x, float y, float z)
        {
            X = x;
            Y = y;
            Z = z;
        }

        public SimpleVector(Vector3 vector3)
        {
            X = vector3.x;
            Y = vector3.y;
            Z = vector3.z;
        }

        public bool Equals(SimpleVector other)
        {
            return X.Equals(other.X) && Y.Equals(other.Y) && Z.Equals(other.Z);
        }

        public override bool Equals(object obj)
        {
            return obj is SimpleVector other && Equals(other);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(X, Y, Z);
        }

        [MethodImpl((MethodImplOptions) 256)]
        public static SimpleVector operator +(SimpleVector a, SimpleVector b) => new(a.X + b.X, a.Y + b.Y, a.Z + b.Z);

        [MethodImpl((MethodImplOptions) 256)]
        public static SimpleVector operator -(SimpleVector a, SimpleVector b) => new(a.X - b.X, a.Y - b.Y, a.Z - b.Z);

        [MethodImpl((MethodImplOptions) 256)]
        public static SimpleVector operator -(SimpleVector a) => new(-a.X, -a.Y, -a.Z);

        [MethodImpl((MethodImplOptions) 256)]
        public static SimpleVector operator *(SimpleVector a, float d) => new(a.X * d, a.Y * d, a.Z * d);

        [MethodImpl((MethodImplOptions) 256)]
        public static SimpleVector operator *(float d, SimpleVector a) => new(a.X * d, a.Y * d, a.Z * d);

        [MethodImpl((MethodImplOptions) 256)]
        public static SimpleVector operator /(SimpleVector a, float d) => new(a.X / d, a.Y / d, a.Z / d);

        [MethodImpl((MethodImplOptions) 256)]
        public static bool operator ==(SimpleVector lhs, SimpleVector rhs)
        {
            float num1 = lhs.X - rhs.X;
            float num2 = lhs.Y - rhs.Y;
            float num3 = lhs.Z - rhs.Z;
            return (double) num1 * (double) num1 + (double) num2 * (double) num2 + (double) num3 * (double) num3 <
                   Double.Epsilon;
        }

        [MethodImpl((MethodImplOptions) 256)]
        public static bool operator !=(SimpleVector lhs, SimpleVector rhs) => !(lhs == rhs);
    }
}