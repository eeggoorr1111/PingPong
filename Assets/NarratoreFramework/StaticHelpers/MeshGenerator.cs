using UnityEngine;
using System.Collections.Generic;

namespace Narratore.Mesh
{
    public static class MeshGenerator
    {
        public static void AddLine(Line line, List<Vector3> vertices, List<Vector2> uvs, List<int> triangles)
        {
            int startIndexVert = vertices.Count;
            Vector3 halfWidth = line.Width / 2;
            Vector3 vert1 = line.Start + halfWidth;
            Vector3 vert2 = line.End + halfWidth;
            Vector3 vert3 = line.End - halfWidth;
            Vector3 vert4 = line.Start - halfWidth;

            vertices.Add(vert1);
            vertices.Add(vert2);
            vertices.Add(vert3);
            vertices.Add(vert4);

            uvs.Add(new Vector2(0, 1));
            uvs.Add(new Vector2(1, 1));
            uvs.Add(new Vector2(1, 0));
            uvs.Add(new Vector2(0, 0));

            triangles.Add(startIndexVert);
            triangles.Add(startIndexVert + 1);
            triangles.Add(startIndexVert + 2);

            triangles.Add(startIndexVert);
            triangles.Add(startIndexVert + 2);
            triangles.Add(startIndexVert + 3);
        }
        public static void AddTriangle(Triangle triangle, List<Vector3> vertices, List<Vector2> uvs, List<int> triangles)
        {
            int startIndexVert = vertices.Count;

            vertices.Add(triangle.Point1);
            vertices.Add(triangle.Point2);
            vertices.Add(triangle.Point3);

            uvs.Add(new Vector2(0, 1));
            uvs.Add(new Vector2(1, 1));
            uvs.Add(new Vector2(1, 0));

            triangles.Add(startIndexVert);
            triangles.Add(startIndexVert + 1);
            triangles.Add(startIndexVert + 2);
        }

        public struct Line
        {
            public Vector3 Start;
            public Vector3 End;
            public Vector3 Width;
        }

        public struct Triangle
        {
            public Triangle(Vector3 point1, Vector3 point2, Vector3 point3)
            {
                Point1 = point1;
                Point2 = point2;
                Point3 = point3;
            }


            public Vector3 Point1;
            public Vector3 Point2;
            public Vector3 Point3;
        }
    }
}
