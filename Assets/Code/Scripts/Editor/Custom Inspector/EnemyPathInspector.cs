using Euphrates.Path;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(Path))]
public class EnemyPathInspector : Editor
{
    readonly Color PATH_HANDLE_COLOR = Color.green;
    const float PATH_HANDLE_SIZE = .5f;

    readonly Color PATH_CONTROL_COLOR = Color.blue;
    const float PATH_CONTROL_SIZE = .25f;

    float _snap = 0f;

    Path _target;
    private void OnEnable()
    {
        _target = (Path)target;
    }

    private void OnSceneGUI()
    {
        Input();
        Draw();
    }

    void Input()
    {
        Event guiEvent = Event.current;

        Vector2 mousePos = guiEvent.mousePosition;
        mousePos.y = Screen.height - mousePos.y;

        if (guiEvent.shift && guiEvent.type == EventType.MouseDown && guiEvent.button == 0)
        {
            Camera cam = SceneView.currentDrawingSceneView.camera;

            Vector3 pos = cam.ScreenToWorldPoint(new Vector3(mousePos.x, mousePos.y, 20f));

            PathNode.CreateNode(pos);
        }

        if (guiEvent.alt && guiEvent.type == EventType.MouseDown && guiEvent.button == 0)
        {
            Camera cam = SceneView.currentDrawingSceneView.camera;

            for (int i = 0; i < _target.NodeCount; i++)
            {
                PathNode node = _target.GetNode(i);

                Vector2 ptScrPos = cam.WorldToScreenPoint(node.transform.position);

                Debug.Log($"{ptScrPos} -- {mousePos}");


                if (Vector2.Distance(ptScrPos, mousePos) > 50)
                    continue;

                _target.RemoveNode(node);

                break;
            }
        }

        if (guiEvent.type == EventType.KeyDown && guiEvent.keyCode == KeyCode.LeftControl)
            _snap = 1f;

        if (guiEvent.type == EventType.KeyUp && guiEvent.keyCode == KeyCode.LeftControl)
            _snap = 0f;
    }

    void Draw()
    {
        if (_target.Points.Count < 4)
            return;

        int cnt = _target.NodeCount - 1;
        for (int i = 0; i < cnt; i++)
        {
            Vector3 h1 = _target.Points[i * 3].position;
            Vector3 h2 = _target.Points[i * 3 + 3].position;

            Vector3 c1 = _target.Points[i * 3 + 1].position;
            Vector3 c2 = _target.Points[i * 3 + 2].position;

            Handles.DrawBezier(h1, h2, c1, c2, Color.red, null, 2);
        }


        for (int i = 0; i < _target.Points.Count; i++)
        {
            if (i % 3 == 0)
            {
                Handles.color = PATH_HANDLE_COLOR;

                Transform handle = _target.Points[i];
                Vector3 hNewPos = Handles.FreeMoveHandle(handle.position, Quaternion.identity, PATH_HANDLE_SIZE, Vector3.one, Handles.SphereHandleCap);


                float hArrowSize = 2f;

                Handles.color = Handles.xAxisColor;
                hNewPos = Handles.Slider(hNewPos, Vector3.right, hArrowSize, Handles.ArrowHandleCap, _snap);

                Handles.color = Handles.yAxisColor;
                hNewPos = Handles.Slider(hNewPos, Vector3.up, hArrowSize, Handles.ArrowHandleCap, _snap);

                Handles.color = Handles.zAxisColor;
                hNewPos = Handles.Slider(hNewPos, Vector3.forward, hArrowSize, Handles.ArrowHandleCap, _snap);

                if (handle.position != hNewPos)
                {
                    Undo.RecordObject(_target, "Moved Handle");
                    _target.MoveHandlePoint(i, hNewPos);
                }

                continue;
            }

            Handles.color = PATH_CONTROL_COLOR;

            Transform connectedHandle = i % 3 == 1 ? _target.Points[i - 1] : _target.Points[i + 1];
            Transform control = _target.Points[i];

            Handles.DrawLine(control.position, connectedHandle.position, 2);

            Vector3 cNewPos = Handles.FreeMoveHandle(control.position, Quaternion.identity, PATH_CONTROL_SIZE, Vector3.one, Handles.SphereHandleCap);

            float cArowSize = 1f;

            Handles.color = Handles.xAxisColor;
            cNewPos = Handles.Slider(cNewPos, Vector3.right, cArowSize, Handles.ArrowHandleCap, _snap);

            Handles.color = Handles.yAxisColor;
            cNewPos = Handles.Slider(cNewPos, Vector3.up, cArowSize, Handles.ArrowHandleCap, _snap);

            Handles.color = Handles.zAxisColor;
            cNewPos = Handles.Slider(cNewPos, Vector3.forward, cArowSize, Handles.ArrowHandleCap, _snap);

            if (control.position != cNewPos)
            {
                Undo.RecordObject(_target, "Moved Control");
                _target.MoveControlPoint(i, cNewPos);
            }
        }
    }
}
