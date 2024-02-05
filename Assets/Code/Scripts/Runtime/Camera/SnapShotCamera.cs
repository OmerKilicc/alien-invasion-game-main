using System.IO;
using UnityEngine;

[RequireComponent(typeof(Camera))]
public class SnapShotCamera : MonoBehaviour
{
    Camera _camera;

    [SerializeField] int _resW = 256;
    [SerializeField] int _resH = 256;



    void OnEnable()
    {
        _camera = GetComponent<Camera>();
    }

    string GetFilePath() => $"{Application.dataPath}/Snaps/";

    string GenFileName(int w, int h) => $"snap{w}x{h}_{System.DateTime.Now.ToString("dd-MM-yyyy_HH-mm-ss")}.png";

    public void TakeShot()
    {
        if (_camera == null)
            _camera = GetComponent<Camera>();

        RenderTexture rt = new RenderTexture(_resW, _resH, 24);
        _camera.targetTexture = rt;

        Texture2D screenShot = new Texture2D(_resW, _resH, TextureFormat.ARGB32, false);
        _camera.Render();

        RenderTexture.active = rt;
        screenShot.ReadPixels(new Rect(0, 0, _resW, _resH), 0, 0);

        _camera.targetTexture = null;
        RenderTexture.active = null;
        DestroyImmediate(rt);

        byte[] bytes = screenShot.EncodeToPNG();

        string filePath = GetFilePath();
        string filename = GenFileName(_resW, _resH);

        if (!Directory.Exists(filePath))
            Directory.CreateDirectory(filePath);

        File.WriteAllBytes(filePath + filename, bytes);
        Debug.Log(string.Format("Snap {0} taken", filename));
    }
}
