using UnityEditor;
using UnityEngine;

public class MeshCombiner : MonoBehaviour
{
    private void Start()
    {
        CombineMeshes();
    }

    void CombineMeshes()
    {
        MeshFilter[] meshes = transform.GetComponentsInChildren<MeshFilter>(false);
        var instances = new CombineInstance[meshes.Length];

        for (int i = 0; i < meshes.Length; i++)
        {
            instances[i].mesh = meshes[i].mesh;
            instances[i].transform = meshes[i].transform.localToWorldMatrix;
        }

        Mesh combined = new Mesh();
        combined.CombineMeshes(instances, false);

#if UNITY_EDITOR
        string path = EditorUtility.SaveFilePanel("Save Generated Mesh", "Assets/", "Generated Mesh", "asset");

        if (string.IsNullOrWhiteSpace(path))
            return;

        path = FileUtil.GetProjectRelativePath(path);

        Mesh saved = Object.Instantiate(combined);
        MeshUtility.Optimize(saved);

        AssetDatabase.CreateAsset(saved, path);
        AssetDatabase.SaveAssets();
#endif
    }
}
