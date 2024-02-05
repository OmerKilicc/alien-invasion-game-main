using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(SoundManager))]
public class SoundManagerInspector : Editor
{
    SoundManager _target;

    private void OnEnable()
    {
        _target = (SoundManager)target;
    }

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        EditorGUILayout.LabelField($"Audio Source Count: {_target.AudioSources.Length}");
    }
}
