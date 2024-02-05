using UnityEngine;

[RequireComponent(typeof(RectTransform))]
public class TargetIndicator : MonoBehaviour
{
    RectTransform _rectTransform;

    bool _initialized = false;

    Transform _target;
    float _offset = 0f;

    Camera _camera;

    RectTransform _canvasRect;

    [SerializeField] GameObject _onScreenIndicator;
    [SerializeField] GameObject _offScreenIndicator;

    private void Start()
    {
        _rectTransform = GetComponent<RectTransform>();

        SetCamera();
    }

    public void Init(Transform target, float offset, RectTransform canvasRect)
    {
        _initialized = true;

        _target = target;
        _offset = offset;

        _canvasRect = canvasRect;
    }

    public void Stop()
    {
        _initialized = false;
        _target = null;

        _onScreenIndicator.gameObject.SetActive(false);
        _offScreenIndicator.gameObject.SetActive(false);
    }

    bool IsInView(Vector3 pos)
        => pos.x >= 0 && pos.x <= _canvasRect.rect.width * _canvasRect.localScale.x
        && pos.y >= 0 && pos.y <= _canvasRect.rect.height * _canvasRect.localScale.y
        && pos.z >= 0;

    void SetGraphic(bool onScreen)
    {
        _onScreenIndicator.SetActive(onScreen);
        _offScreenIndicator.SetActive(!onScreen);
    }

    Vector3 OutOfViewPos(Vector3 pos)
    {
        Vector3 rval = new Vector3(pos.x, pos.y, 0);

        float halfW = _canvasRect.rect.width * .5f;
        float halfH = _canvasRect.rect.height * .5f;

        Vector3 canvasCenter = new Vector3(halfW, halfH, 0f) * _canvasRect.localScale.x;
        rval -= canvasCenter;

        float divX = (halfW - _offset) / Mathf.Abs(rval.x);
        float divY = (halfH - _offset) / Mathf.Abs(rval.y);

        bool useX = divX < divY;

        float angle = useX ? Vector3.SignedAngle(Vector3.right, rval, Vector3.forward) : Vector3.SignedAngle(Vector3.up, rval, Vector3.forward);

        if (useX)
        {
            rval.x = Mathf.Sign(rval.x) * (halfW - _offset) * _canvasRect.localScale.x;
            rval.y = Mathf.Tan(Mathf.Deg2Rad * angle) * rval.x;
        }
        else
        {
            rval.y = Mathf.Sign(rval.y) * (halfH - _offset) * _canvasRect.localScale.y;
            rval.x = -Mathf.Tan(Mathf.Deg2Rad * angle) * rval.y;
        }

        rval += canvasCenter;

        return rval;
    }

    void SetIndicator()
    {
        if (!SetCamera())
            return;

        Vector3 screenPos = _camera.WorldToScreenPoint(_target.position);
        bool inView = IsInView(screenPos);

        if (inView)
        {
            screenPos.z = 0;

            SetGraphic(true);

            _rectTransform.position = screenPos;
            return;
        }

        screenPos = screenPos.z < 0 ? screenPos * -1 : screenPos;

        SetGraphic(false);
        _rectTransform.position = OutOfViewPos(screenPos);
    }

    bool SetCamera()
    {
        if (Camera.allCamerasCount == 0)
            return false;

        _camera = Camera.allCameras[0];

        return true;
    }

    private void Update()
    {
        if (!_initialized)
            return;

        SetIndicator();
    }
}
