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


        private List<Vector3> _vertices;
        private List<int> _triangles;
        private List<Vector2> _uvs;
        private MeshFilter _meshFilter;
        private bool _inited;


        public void NewFrame(Vector2 size, float widthBorder, bool sizeByOutBorder)
        {
            UnityEngine.Mesh mesh = _meshFilter.sharedMesh;

            _vertices.Clear();
            _triangles.Clear();
            _uvs.Clear();

            mesh.Clear();

            float widthFromBorder = sizeByOutBorder ? 0f : widthBorder;
            float halfWidth = widthBorder / 2;
            Vector2 halfSize = size / 2;

            AddColumn(  new Vector2(-halfSize.x - widthFromBorder, -halfSize.y - widthFromBorder - halfWidth),
                        new Vector2(-halfSize.x - widthFromBorder, halfSize.y + widthFromBorder + halfWidth));

            AddColumn(  new Vector2(halfSize.x + widthFromBorder, -halfSize.y - widthFromBorder - halfWidth),
                        new Vector2(halfSize.x + widthFromBorder, halfSize.y + widthFromBorder + halfWidth));

            AddRow( new Vector2(-halfSize.x - widthFromBorder - halfWidth, -halfSize.y - widthFromBorder),
                    new Vector2(halfSize.x + widthFromBorder + halfWidth, -halfSize.y - widthFromBorder));

            AddRow( new Vector2(-halfSize.x - widthFromBorder - halfWidth, halfSize.y + widthFromBorder),
                    new Vector2(halfSize.x + widthFromBorder + halfWidth, halfSize.y + widthFromBorder));

            mesh.vertices = _vertices.ToArray();
            mesh.triangles = _triangles.ToArray();
            mesh.SetUVs(0, _uvs);

            mesh.name = "Frame";
        }
        public void StartCustom()
        {
            Init();
            NewFrame(_size, _widthBorder, _sizeByOutBorder);
        }


        private void AddColumn(Vector2 start, Vector2 end)
        {
            MeshGenerator.LineParams line = new MeshGenerator.LineParams();

            line.Start = start.To3D();
            line.End = end.To3D();
            line.Width = new Vector3(_widthBorder, 0, 0);

            MeshGenerator.AddLine(line, _vertices, _uvs, _triangles);
        }
        private void AddRow(Vector2 start, Vector2 end)
        {
            MeshGenerator.LineParams line = new MeshGenerator.LineParams();

            line.Start = start.To3D();
            line.End = end.To3D();
            line.Width = new Vector3(0, _widthBorder, 0);

            MeshGenerator.AddLine(line, _vertices, _uvs, _triangles);
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
                NewFrame(_size, _widthBorder, _sizeByOutBorder);
        }
    }
}
