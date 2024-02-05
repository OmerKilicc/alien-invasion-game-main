using UnityEngine;

public class FaceCanvasToCam : MonoBehaviour
{
    Transform _transform;
    Transform _cam;

    private void Awake() => _transform = transform;

    void Update()
    {
        if (!SetCam())
            return;

        _transform.forward = _cam.transform.forward;
    }

    bool SetCam()
    {
        if (_cam != null)
            return true;

        if (Camera.allCamerasCount == 0)
            return false;

        Camera cam = Camera.allCameras[0];

        if (cam == null)
            return false;

        _cam = cam.transform;
        return true;
    }
}
