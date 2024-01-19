using UnityEngine;

namespace DelaunayVoronoi
{
    public class Edge
    {
        public Point Point1 { get; }
        public Point Point2 { get; }

        public Edge(Point point1, Point point2)
        {
            Point1 = point1;
            Point2 = point2;
        }

        public Vector2 GetVector()
        {
            return new Vector2((float)Point2.Y - (float)Point1.Y, (float)Point2.X - (float)Point1.X);
        }

        public bool InRect(Rect rect)
        {
            return rect.Contains(Point1.ToVector2()) && rect.Contains(Point2.ToVector2());
        }

        public bool IsVertex(Point p)
        {
            return p.Equals(Point1) || p.Equals(Point2);
        }

        public override bool Equals(object obj)
        {
            if (obj == null) return false;
            if (obj.GetType() != GetType()) return false;
            var edge = obj as Edge;

            var samePoints = Point1 == edge.Point1 && Point2 == edge.Point2;
            var samePointsReversed = Point1 == edge.Point2 && Point2 == edge.Point1;
            return samePoints || samePointsReversed;
        }

        public override int GetHashCode()
        {
            int hCode = (int)Point1.X ^ (int)Point1.Y ^ (int)Point2.X ^ (int)Point2.Y;
            return hCode.GetHashCode();
        }
    }
}