using System.Collections.Generic;
using UnityEngine;

namespace DelaunayVoronoi
{
    public class Point
    {
        /// <summary>
        /// Used only for generating a unique ID for each instance of this class that gets generated
        /// </summary>
        private static int _counter;

        /// <summary>
        /// Used for identifying an instance of a class; can be useful in troubleshooting when geometry goes weird
        /// (e.g. when trying to identify when Triangle objects are being created with the same Point object twice)
        /// </summary>
        private readonly int _instanceId = _counter++;

        public float X { get; }
        public float Y { get; }
        public HashSet<Triangle> AdjacentTriangles { get; } = new HashSet<Triangle>();

        public Point(float x, float y)
        {
            X = x;
            Y = y;
        }

        public float Distance(Point target)
        {
            return Mathf.Sqrt(Mathf.Pow(X - target.X, 2) + Mathf.Pow(Y - target.Y, 2));
        }

        public override string ToString()
        {
            // Simple way of seeing what's going on in the debugger when investigating weirdness
            return $"{nameof(Point)} {_instanceId} {X:0.##}@{Y:0.##}";
        }

        public Vector3 ToVector3()
        {
            return new Vector3(X, Y, 0);
        }

        public Vector2 ToVector2()
        {
            return new Vector2(X, Y);
        }

        public Vector2 ToDir(Point dst)
        {
            if (Equals(dst))
            {
                return Vector2.zero;
            }
            return new Vector2(dst.X - X, dst.Y - Y).normalized;
        }

        public override bool Equals(object obj)
        {
            if (obj == null) return false;
            if (obj.GetType() != GetType()) return false;
            var point = obj as Point;

            return X == point.X && Y == point.Y;
        }

        public override int GetHashCode()
        {
            int hCode = (int)X ^ (int)Y;
            return hCode.GetHashCode();
        }
    }
}