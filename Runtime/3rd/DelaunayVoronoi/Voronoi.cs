using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace DelaunayVoronoi
{
    public static class Voronoi
    {
        public static HashSet<Edge> GenerateEdgesFromDelaunay(IEnumerable<Triangle> triangulation)
        {
            var voronoiEdges = new HashSet<Edge>();
            foreach (var triangle in triangulation)
            {
                foreach (var neighbor in triangle.TrianglesWithSharedEdge)
                {
                    var edge = new Edge(triangle.Circumcenter, neighbor.Circumcenter);
                    voronoiEdges.Add(edge);
                }
            }

            return voronoiEdges;
        }

        public static List<Polygon> GetPolygons(Triangle firstTriangle)
        {
            HashSet<Polygon> result = new();
            foreach (var firstTriangleNeighbor in firstTriangle.TrianglesWithSharedEdge)
            {
                var firstEdge = new Edge(firstTriangle.Circumcenter, firstTriangleNeighbor.Circumcenter);
                var firstEdgeVector = firstEdge.GetVector().normalized;

                foreach (var secondTriangleNeighbor in firstTriangleNeighbor.TrianglesWithSharedEdge)
                {
                    if (secondTriangleNeighbor == firstTriangle)
                    {
                        continue;
                    }

                    List<Edge> edges = new List<Edge>();
                    edges.Add(firstEdge);

                    var secondEdge = new Edge(firstTriangleNeighbor.Circumcenter, secondTriangleNeighbor.Circumcenter);
                    var secondEdgeVector = secondEdge.GetVector().normalized;
                    float clockWise = firstEdgeVector.x * secondEdgeVector.y - firstEdgeVector.y * secondEdgeVector.x;
                    edges.Add(secondEdge);

                    AppendToEdgesByConvexHull(edges, clockWise, secondTriangleNeighbor, firstTriangleNeighbor, firstTriangle);
                    result.Add(new Polygon(edges));
                }
            }

            return result.ToList();
        }

        private static void AppendToEdgesByConvexHull(List<Edge> edges, float clockWise, Triangle currentTriangle, Triangle previousTriangle, Triangle firstTriangle)
        {
            Vector2 lastEdgeVector = edges[^1].GetVector().normalized;

            foreach (var neighbour in currentTriangle.TrianglesWithSharedEdge)
            {
                if (neighbour == previousTriangle)
                {
                    continue;
                }

                var edge = new Edge(currentTriangle.Circumcenter, neighbour.Circumcenter);

                if (neighbour == firstTriangle)
                {
                    edges.Add(edge);
                    return; //mission complete
                }

                var edgeVector = edge.GetVector().normalized;
                float newClockWise = lastEdgeVector.x * edgeVector.y - lastEdgeVector.y * edgeVector.x;
                if (newClockWise * clockWise == 0)
                {
                    Debug.Assert(false, "something is wrong");
                }

                if (newClockWise * clockWise > 0)
                {
                    edges.Add(edge);
                    AppendToEdgesByConvexHull(edges, newClockWise, neighbour, currentTriangle, firstTriangle);
                }
            }
        }
    }
}