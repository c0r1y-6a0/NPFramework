#if UNITY_EDITOR

using System.Collections.Generic;
using System.Linq;
using DelaunayVoronoi;
using NP;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEditor;

namespace SevenWonders
{
    public class VoronoiGenerator : MonoBehaviour
    {
        [SerializeField] private int m_pointCount = 10;
        [SerializeField] private Rect m_rect;
        
        [SerializeField] private LineRenderer m_greeTemplate;
        [SerializeField] private LineRenderer m_redTemplate;
        [SerializeField] private Transform m_polygonRoot;
        [SerializeField] private Transform m_voronoiEdgeRoot;
        [SerializeField] private Transform m_voronoiMapPolygonsRoot;

        [SerializeField] private MeshFilter m_delaunayMeshFilter;

        private List<Point> m_points;
        private List<Triangle> m_triangles;
        private HashSet<Polygon> m_allPolygons;
        private VoronoiManager m_voronoiManager;

        private void OnDrawGizmos()
        {
            if (m_triangles == null)
                return;

            for (int i = 0; i < m_triangles.Count; i++)
            {
                Handles.Label(m_triangles[i].Circumcenter.ToVector3(), i.ToString(), new GUIStyle()
                {
                    fontSize = 20,
                    normal = new ()
                    {
                        textColor = Color.green
                    }
                });
            }

            foreach (var p in m_points)
            {
                Handles.Label(p.ToVector3(), $"[{p.X}, {p.Y}]", new GUIStyle()
                {
                    fontSize = 10,
                    normal = new ()
                    {
                        textColor = Color.cyan
                    }
                });
            }

        }

        [Button]
        public void PrepareData()
        {
            var delaunay = new DelaunayTriangulator();
            m_points = delaunay.GeneratePoints(m_pointCount, m_rect.min.x, m_rect.max.x, m_rect.min.y, m_rect.max.y);
            m_triangles = delaunay.BowyerWatson(m_points);

            m_allPolygons = new();
            foreach (var triangle in m_triangles)
            {
                List<Polygon> polygons = Voronoi.GetPolygons(triangle);
                foreach (var p in polygons)
                {
                    if (p.InRect(m_rect))
                    {
                        m_allPolygons.Add(p);
                    }
                }
            }

            m_voronoiManager = new VoronoiManager(m_allPolygons, m_points, m_triangles);

            GenerateDelaunayMesh();

            m_voronoiEdgeRoot.DeleteAllChildren();
            m_polygonRoot.DeleteAllChildren();

            RenderAllPolygons();
        }

        private void GenerateDelaunayMesh()
        {
            var mesh = new Mesh();

            Vector3[] vertices = new Vector3[m_points.Count];
            Dictionary<Point, int> pointIndex = new Dictionary<Point, int>();
            for (int i = 0; i < m_points.Count; i++)
            {
                var point = m_points[i];
                vertices[i] = new Vector3(point.X, point.Y, 0);
                pointIndex.Add(point, i);
            }

            int[] trianglesArray = new int[m_triangles.Count * 3];
            int triangleIndex = 0;
            foreach (var triangle in m_triangles)
            {
                trianglesArray[triangleIndex * 3] = pointIndex[triangle.Vertices[0]];
                trianglesArray[triangleIndex * 3 + 1] = pointIndex[triangle.Vertices[1]];
                trianglesArray[triangleIndex * 3 + 2] = pointIndex[triangle.Vertices[2]];
                triangleIndex++;
            }

            mesh.vertices = vertices;
            mesh.triangles = trianglesArray;

            m_delaunayMeshFilter.mesh = mesh;
        }

        [Button]
        public void PolygonByTriangleIndex(int triangleIndex)
        {
            m_polygonRoot.DeleteAllChildren();

            List<Polygon> polygons = Voronoi.GetPolygons(m_triangles.ElementAt(triangleIndex));
            foreach (var p in polygons)
            {
                RenderPolygon(m_redTemplate, p, m_polygonRoot);
            }
        }

        [Button]
        public void RenderPolygonsNearPoint(float x, float y)
        {
            List<Polygon> polygons = m_voronoiManager.GetPolygonsNear(x, y);
            RenderPolygons(m_redTemplate,polygons, m_polygonRoot);
        }

        [Button]
        public void RenderNearestPolygon(float x, float y, bool deletePrevious)
        {
            if (deletePrevious)
                m_polygonRoot.DeleteAllChildren();

            RenderPolygon(m_redTemplate,m_voronoiManager.GetNearestPolygon(x, y), m_polygonRoot);
        }

        private void RenderAllPolygons()
        {
            RenderPolygons(m_greeTemplate, m_allPolygons, m_voronoiMapPolygonsRoot);
        }


        private void RenderPolygons(LineRenderer template,IEnumerable<Polygon> polygons, Transform root)
        {
            root.DeleteAllChildren();

            foreach (var p in polygons)
            {
                RenderPolygon(template, p, root);
            }
        }

        private void RenderPolygon(LineRenderer template, Polygon polygon, Transform root)
        {
            int totalVertices = polygon.Edges.Count() + 1;

            var render = Instantiate(template, root, true);
            render.positionCount = totalVertices;
            int index = 0;
            foreach (var edge in polygon.Edges)
            {
                render.SetPosition(index++, new Vector3(edge.Point1.X, edge.Point1.Y, 0));
            }

            render.SetPosition(index++, new Vector3(polygon.Edges.Last().Point2.X, polygon.Edges.Last().Point2.Y, 0));
        }
    }
}
#endif