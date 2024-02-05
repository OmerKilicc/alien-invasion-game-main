using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(SnapShotCamera))]
public class SnapShotInspector : Editor
{
    SnapShotCamera _target;

    void OnEnable()
    {
        _target = (SnapShotCamera)target;
    }

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        if (GUILayout.Button("Take Snap"))
        {
            _target.TakeShot();
        }
    }
}
