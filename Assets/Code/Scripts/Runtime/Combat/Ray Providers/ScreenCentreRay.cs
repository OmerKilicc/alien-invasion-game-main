using UnityEngine;

public class ScreenCentreRay : MonoBehaviour, IRayProvider
{
    Camera _cam;

    private void Start()
    {
        SetCam();
    }

    void SetCam() => _cam = Camera.allCameras[0];

    public Ray CreateRay()
    {
        if (_cam == null)
            SetCam();

        return _cam.ViewportPointToRay(new Vector3(.5f, .5f));
    }
}
