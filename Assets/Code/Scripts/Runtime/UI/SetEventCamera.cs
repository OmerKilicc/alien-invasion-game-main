using UnityEngine;

[RequireComponent(typeof(Canvas))]
public class SetEventCamera : MonoBehaviour
{
    Canvas _canvas;

    private void Start()
    {
        SetCamera();
    }

    void SetCamera()
    {
        _canvas = GetComponent<Canvas>();

        if (Camera.allCamerasCount == 0)
            return;

        Camera cam = Camera.allCameras[0];
        _canvas.worldCamera = cam;
    }
}
