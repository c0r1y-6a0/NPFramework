using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace DelaunayVoronoi
{
    public class Polygon
    {
        public List<Edge> Edges { get; }
        public List<Point> Points { get; }

        public Polygon(List<Edge> edges)
        {
            Edges = edges;

            HashSet<Point> allPoints = new();
            foreach (var edge in Edges)
            {
                allPoints.Add(edge.Point1);
                allPoints.Add(edge.Point2);
            }

            Points = allPoints.ToList();
        }

        public bool IsVertex(Point p)
        {
            foreach (var edge in Edges)
            {
                if (edge.IsVertex(p))
                    return true;
            }

            return false;
        }

        /// <summary>
        /// 检查点p是否位于这个polygon内部, 基于该polygon为凸多边形的前提
        /// </summary>
        /// <param name="p"></param>
        /// <param name="strict"> 当strict为true时，位于顶点或者边界的point不被认为是位于polygon内部 </param>
        /// <returns></returns>
        public bool PointInside(Point p, bool strict)
        {
            float firstCross = GetCross(p, Edges[0]);
            for (int i = 1; i < Edges.Count; i++)
            {
                //点p到该边第一个顶点的向量， 与 该边从第一个顶点到第二个顶点的向量的叉积的正负，与 第一条边的结果不同，说明不在内部
                if (strict)
                {
                    if (firstCross * GetCross(p, Edges[i]) < 0)
                    {
                        return false;
                    }
                }
                else
                {
                    // == 0时说明是顶点或者位于边上
                    if (firstCross * GetCross(p, Edges[i]) <= 0)
                    {
                        return false;
                    }
                }
            }

            return true;
        }

        private float GetCross(Point p, Edge edge)
        {
            var dir1 = p.ToDir(edge.Point1);
            var dir2 = edge.Point1.ToDir(edge.Point2);

            return dir1.x * dir2.y - dir1.y * dir2.x;
        }

        public bool InRect(Rect rect)
        {
            foreach (var edge in Edges)
            {
                if (!edge.InRect(rect))
                    return false;
            }

            return true;
        }

        public override bool Equals(object obj)
        {
            if (obj == null) return false;
            if (obj.GetType() != GetType()) return false;
            var other = obj as Polygon;

            foreach (var edge in Edges)
            {
                if (!other.Edges.Contains(edge))
                {
                    return false;
                }
            }

            foreach (var edge in other.Edges)
            {
                if (!Edges.Contains(edge))
                {
                    return false;
                }
            }

            return true;
        }

        public override int GetHashCode()
        {
            if (Edges == null)
                return 0;

            int hCode = 0;
            foreach (var edge in Edges)
            {
                hCode ^= edge.GetHashCode();
            }

            return hCode.GetHashCode();
        }
    }
}