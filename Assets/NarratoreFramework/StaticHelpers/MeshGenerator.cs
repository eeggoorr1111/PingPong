using UnityEngine;
using System.Collections.Generic;

namespace Narratore.Mesh
{
    public static class MeshGenerator
    {
        public static void AddLine(LineParams line, List<Vector3> vertices, List<Vector2> uvs, List<int> triangles)
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


        public struct LineParams
        {
            public Vector3 Start;
            public Vector3 End;
            public Vector3 Width;
        }
    }
}
