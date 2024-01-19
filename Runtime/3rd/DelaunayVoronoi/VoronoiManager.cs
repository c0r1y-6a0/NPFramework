using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace DelaunayVoronoi
{
    public class VoronoiManager
    {
        private HashSet<Polygon> m_allPolygons;
        private List<Point> m_points;
        private List<Triangle> m_triangles;

        private Dictionary<Polygon, List<Polygon>> m_neighborMap;

        public VoronoiManager(HashSet<Polygon> allPolygons, List<Point> points, List<Triangle> delaunayTriangles)
        {
            m_allPolygons = allPolygons;
            m_points = points;
            m_triangles = delaunayTriangles;
        }

        public List<Polygon> GetPolygonsNear(float x, float y)
        {
            float minDis = float.MaxValue;
            Point target = new Point(x, y);
            Point result = null;
            foreach (var p in m_points)
            {
                var d = target.Distance(p);
                if (d < minDis)
                {
                    minDis = d;
                    result = p;
                }
            }

            Debug.Log($"delaunay point is {result}");
            return GetPolygonsByPoint(result);
        }

        public Polygon GetNearestPolygon(float x, float y)
        {
            Point p = new Point(x, y);
            var polygons = GetPolygonsNear(x, y);
            foreach (var polygon in polygons)
            {
                if (polygon.PointInside(p, false))
                    return polygon;
            }

            Debug.Assert(false, "应该出现这种情况吗，我也不知道");
            return null;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="point"> point必须是用来创建Delaunay剖分的点</param>
        /// <returns></returns>
        private List<Polygon> GetPolygonsByPoint(Point point)
        {
            HashSet<Polygon> result = new();

            var triangles = GetTrianglesByPoint(point);

            foreach (var triangle in triangles)
            {
                Debug.Log($"delaunay triangle index is {m_triangles.IndexOf(triangle)}");
            }

            foreach (var polygon in m_allPolygons)
            {
                foreach (var t in triangles)
                {
                    if (polygon.IsVertex(t.Circumcenter))
                    {
                        result.Add(polygon);
                    }
                }
            }

            return result.ToList();
        }

        private List<Triangle> GetTrianglesByPoint(Point p)
        {
            List<Triangle> result = new List<Triangle>();
            foreach (var t in m_triangles)
            {
                if (t.IsVertex(p))
                {
                    result.Add(t);
                }
            }

            return result;
        }

        private void BuildNeighborMap()
        {
            m_neighborMap = new();

            foreach (var p in m_allPolygons)
            {
                var neighbors = GetAllNeighbor(p);
                m_neighborMap.Add(p, neighbors);
            }
        }

        private List<Polygon> GetAllNeighbor(Polygon target)
        {
            List<Polygon> neighbors = new List<Polygon>();
            foreach (var p in m_allPolygons)
            {
                if (p.Equals(target))
                    continue;

                foreach (var e in p.Edges)
                {
                    if (target.Edges.Contains(e))
                    {
                        neighbors.Add(p);
                        break;
                    }
                }
            }

            return neighbors;
        }
    }
}