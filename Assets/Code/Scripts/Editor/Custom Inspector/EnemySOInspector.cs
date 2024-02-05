using UnityEditor;
using UnityEngine;

[CanEditMultipleObjects]
[CustomEditor(typeof(EnemySO))]
public class EnemySOInspector : Editor
{
    EnemySO _target;

    private void OnEnable()
    {
        _target = target as EnemySO;
    }

    string _generatedJson = "";
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        EditorGUILayout.Space(10);

        if (GUILayout.Button("Create Json"))
            UpdateJson();

        EditorStyles.textField.wordWrap = true;
        _generatedJson = EditorGUILayout.TextArea(_generatedJson, GUILayout.ExpandHeight(true));
    }

    void UpdateJson() => _generatedJson = _target.GenerateJson();
}
