using UnityEngine;
using System.Collections.Generic;
using Narratore.Mesh;
using Narratore.Helpers;

namespace Narratore.Solutions
{
    [RequireComponent(typeof(MeshRenderer), typeof(MeshFilter))]
    public class MeshFrame : MonoBehaviour
    {
        [SerializeField] private float _widthBorder;
        [SerializeField] private Vector2 _size;
        [SerializeField] private bool _sizeByOutBorder;
        [SerializeField] private int _smoothlyCorners;


        private List<Vector3> _vertices;
        private List<int> _triangles;
        private List<Vector2> _uvs;
        private MeshFilter _meshFilter;
        private bool _inited;
        private Vector2 _minPoint;
        private Vector2 _maxPoint;


        public void NewFrame(Vector2 size, float widthBorder, bool sizeByOutBorder, int smoothlyCorners)
        {
            UnityEngine.Mesh mesh = _meshFilter.sharedMesh;

            _size = size;
            _widthBorder = widthBorder;
            _sizeByOutBorder = sizeByOutBorder;
            _smoothlyCorners = smoothlyCorners;
           

            _vertices.Clear();
            _triangles.Clear();
            _uvs.Clear();

            mesh.Clear();

            float widthFromBorder = sizeByOutBorder ? 0f : widthBorder;
            float halfWidth = widthBorder / 2;
            float increaseColumn = _smoothlyCorners > 0 ? -halfWidth : halfWidth;
            Vector2 halfSize = size / 2;

            _minPoint = -halfSize - new Vector2(widthFromBorder, widthFromBorder);
            _maxPoint = halfSize + new Vector2(widthFromBorder, widthFromBorder);

            Vector2 leftColStart = new Vector2(-halfSize.x - widthFromBorder, -halfSize.y - widthFromBorder - increaseColumn);
            Vector2 leftColEnd = new Vector2(-halfSize.x - widthFromBorder, halfSize.y + widthFromBorder + increaseColumn);
            Vector2 rightColStart = new Vector2(halfSize.x + widthFromBorder, -halfSize.y - widthFromBorder - increaseColumn);
            Vector2 rightColEnd = new Vector2(halfSize.x + widthFromBorder, halfSize.y + widthFromBorder + increaseColumn);

            AddLine(leftColStart, leftColEnd, true);
            AddLine(rightColStart, rightColEnd, true);

            AddLine( new Vector2(-halfSize.x - widthFromBorder + halfWidth, -halfSize.y - widthFromBorder),
                    new Vector2(halfSize.x + widthFromBorder - halfWidth, -halfSize.y - widthFromBorder), false);

            AddLine( new Vector2(-halfSize.x - widthFromBorder + halfWidth, halfSize.y + widthFromBorder),
                    new Vector2(halfSize.x + widthFromBorder - halfWidth, halfSize.y + widthFromBorder), false);

            if (_smoothlyCorners > 0)
            {
                AddSmoothCorner(leftColStart + new Vector2(halfWidth, 0), widthBorder, Mathf.PI * 1.5f, Mathf.PI);
                AddSmoothCorner(leftColEnd + new Vector2(halfWidth, 0), widthBorder, Mathf.PI, Mathf.PI * 0.5f);
                AddSmoothCorner(rightColStart - new Vector2(halfWidth, 0), widthBorder, 0f, Mathf.PI * 1.5f);
                AddSmoothCorner(rightColEnd - new Vector2(halfWidth, 0), widthBorder, Mathf.PI * 0.5f, 0f);
            }

            mesh.vertices = _vertices.ToArray();
            mesh.triangles = _triangles.ToArray();
            mesh.SetUVs(0, _uvs);

            mesh.name = "Frame";
        }
        public void StartCustom()
        {
            Init();
            NewFrame(_size, _widthBorder, _sizeByOutBorder, _smoothlyCorners);
        }


        private void AddSmoothCorner(Vector2 center2d, float radius, float startRad, float endRad)
        {
            // TODO: bottom right corner not view
            float stepRad = (endRad - startRad) / _smoothlyCorners;
            float currentRad = startRad + stepRad;
            Vector3 center = center2d.To3D();
            Vector3 point1 = center + new Vector3(Mathf.Cos(startRad), Mathf.Sin(startRad), 0) * radius;
            Vector3 point2 = center + new Vector3(Mathf.Cos(currentRad), Mathf.Sin(currentRad), 0) * radius;


            for (int i = 0; i < _smoothlyCorners; i++)
            {
                MeshGenerator.Triangle triangle = new MeshGenerator.Triangle(center, point1, point2);
                MeshGenerator.AddTriangle(triangle, _vertices, _uvs, _triangles);

                currentRad += stepRad;
                point1 = point2;
                point2 = center + new Vector3(Mathf.Cos(currentRad), Mathf.Sin(currentRad), 0) * radius;
            }
        }
        private void AddLine(Vector2 start, Vector2 end, bool isColumn)
        {
            MeshGenerator.Line line = new MeshGenerator.Line();

            line.Start = start.To3D();
            line.End = end.To3D();

            if (isColumn)
                line.Width = new Vector3(-_widthBorder, 0, 0);
            else
                line.Width = new Vector3(0, _widthBorder, 0);

            MeshGenerator.AddLine(line, _vertices, _triangles);

            for (int i = _vertices.Count - 4; i < _vertices.Count; i++)
            {
                float uvX = (_vertices[i].x - _minPoint.x) / (_maxPoint.x - _minPoint.x);
                float uvY = (_vertices[i].y - _minPoint.y) / (_maxPoint.y - _minPoint.y);

                _uvs.Add(new Vector2(uvX, uvY));
            }
        }
        private void Init()
        {
            _meshFilter = GetComponent<MeshFilter>();

            _vertices = new List<Vector3>(16);
            _triangles = new List<int>(24);
            _uvs = new List<Vector2>(16);

            _inited = true;
        }
        private void OnValidate()
        {
            if (!_inited)
                Init();

            if (_meshFilter.sharedMesh == null)
                Debug.LogError("Frame require link on mesh", this);
            else
                NewFrame(_size, _widthBorder, _sizeByOutBorder, _smoothlyCorners);
        }
    }
}
