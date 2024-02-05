using System.Collections.Generic;
using Unity.Mathematics;
using UnityEditor;
using UnityEngine;

namespace Euphrates.Path
{
    [AddComponentMenu("Path/Path", 0)]
    public class Path : MonoBehaviour
    {
        [Tooltip("Higher is better but has worse performance.")]
        [SerializeField] int _pathResolution = 20;
        public int PathResolution => _pathResolution;

        [SerializeField, HideInInspector] List<PathNode> _nodes = new List<PathNode>();
        public int NodeCount => _nodes.Count;

        [HideInInspector] public List<Transform> Points = new List<Transform>();

        [SerializeField] bool _looping = false;
        public bool Looping { get => _looping; set => _looping = value; }

        public void AddNode(PathNode node)
        {
            if (_nodes.Contains(node))
                return;

            _nodes.Add(node);

            if (_nodes.Count == 1)
            {
                Points.Add(node.transform);
                return;
            }

            GameObject goc1 = new GameObject("Control");
            GameObject goc2 = new GameObject("Control");

            Transform c1 = goc1.transform;
            Transform c2 = goc2.transform;

            c1.parent = Points[^1];
            c2.parent = node.transform;

            if (_nodes.Count == 2)
            {
                c1.transform.position = Points[0].position + Vector3.up;
                c2.position = node.transform.position + Vector3.down;


                Points.Add(c1);
                Points.Add(c2);
                Points.Add(node.transform);
                return;
            }

            Vector3 displacement = Points[^1].position - Points[^2].position;
            Vector3 newControl1 = Points[^1].position + displacement;

            c1.transform.position = newControl1;
            c2.transform.position = node.transform.position + Vector3.down;

            Points.Add(c1);
            Points.Add(c2);
            Points.Add(node.transform);
        }

        public void RemoveNode(PathNode node)
        {
            if (!_nodes.Contains(node))
                return;

            int indx = _nodes.IndexOf(node);

            if (indx == 0)
                Points.RemoveRange(indx, 2);
            else if (indx == _nodes.Count - 1)
                Points.RemoveRange(indx * 3 - 2, 3);
            else
                Points.RemoveRange(indx * 3 - 1, 3);

            _nodes.Remove(node);

            if (Application.isPlaying)
                Destroy(node.gameObject);
            else
                DestroyImmediate(node.gameObject);
        }


        public PathNode GetNode(int index)
        {
            if (index > _nodes.Count - 1)
                return null;

            return _nodes[index];
        }

        public Vector3[] CreateSegmentVertex(int segmentIndex)
        {
            Vector3 handle1 = Points[segmentIndex * 3].position;
            Vector3 control1 = Points[segmentIndex * 3 + 1].position;

            Vector3 handle2 = Points[segmentIndex * 3 + 3].position;
            Vector3 control2 = Points[segmentIndex * 3 + 2].position;

            Vector3[] vertexData = new Vector3[_pathResolution + 1];

            float step = 1f / (float)_pathResolution;
            for (int i = 0; i < _pathResolution + 1; i++)
                vertexData[i] = GetBezierPoint(handle1, handle2, control1, control2, step * i);

            return vertexData;
        }

        public Vector3 GetBezierPoint(Vector3 handle1, Vector3 handle2, Vector3 control1, Vector3 control2, float t)
        {
            float invT = 1 - t;

            // Implementation of Bezier Algorithm (P(t) = (1-t)^3P0 + 3(1-t)^2tP1 + 3(1-t)t^2P2 + t^3P3).
            Vector3 bezierPoint = math.pow(invT, 3f) * handle1
                + 3f * math.pow(invT, 2f) * t * control1
                + 3f * invT * math.pow(t, 2f) * control2
                + math.pow(t, 3) * handle2;

            return bezierPoint;

            // This is a less efficient implementation.
            //Vector3 h1c1 = Vector3.Lerp(handle1, control1, t);
            //Vector3 c1c2 = Vector3.Lerp(control1, control2, t);
            //Vector3 c2h2 = Vector3.Lerp(control2, handle2, t);

            //Vector3 h1c1_c1c2 = Vector3.Lerp(h1c1, c1c2, t);
            //Vector3 c1c2_c2h2 = Vector3.Lerp(c1c2, c2h2, t);

            //return Vector3.Lerp(h1c1_c1c2, c1c2_c2h2, t);
        }

        public Vector3[] CreateSegmentVertexLocal(int segmentIndex)
        {
            Vector3[] rval = CreateSegmentVertex(segmentIndex);

            for (int i = 0; i < rval.Length; i++)
                rval[i] = transform.TransformPoint(rval[i]);


            return rval;
        }

        public void MoveHandlePoint(int pointIndex, Vector3 position)
        {
            if (pointIndex % 3 != 0)
                return;

            Transform control = Points[pointIndex];
            control.position = position;
        }

        public void MoveControlPoint(int pointIndex, Vector3 position)
        {
            if (pointIndex % 3 == 0)
                return;

            Transform control = Points[pointIndex];
            control.position = position;

            if (pointIndex == 1 || pointIndex == Points.Count - 2)
                return;

            bool firstControl = pointIndex % 3 == 1;
            Transform handle = firstControl ? Points[pointIndex - 1] : Points[pointIndex + 1];
            Transform mirrored = firstControl ? Points[pointIndex - 2] : Points[pointIndex + 2];

            Vector3 dir = (handle.position - control.position).normalized;
            float dist = Vector3.Distance(handle.position, mirrored.position);
            mirrored.position = handle.position + dir * dist;
        }

        public static bool AlwaysDrawPaths = false;

#if UNITY_EDITOR
        private void OnDrawGizmos()
        {
            if (!AlwaysDrawPaths || Points.Count == 0)
                return;

            for (int i = 0; i < NodeCount - 1; i++)
            {
                Vector3 h1 = Points[i * 3].position;
                Vector3 h2 = Points[i * 3 + 3].position;

                Vector3 c1 = Points[i * 3 + 1].position;
                Vector3 c2 = Points[i * 3 + 2].position;

                Handles.DrawBezier(h1, h2, c1, c2, Color.red, null, 2);
            }
        }

        [MenuItem("GameObject/Path/Path")]
        public static Path CreatePath()
        {
            GameObject go = new GameObject("Path");
            Selection.activeGameObject = go;

            var path = go.AddComponent<Path>();
            return path;
        }

        [MenuItem("Tools/Path/Always Shown")]
        static void SetPathAlwaysVisible() => AlwaysDrawPaths = true;

        [MenuItem("Tools/Path/Shown When Selected")]
        static void SetPathNotAlwaysVisible() => AlwaysDrawPaths = false;
#endif
    }
}