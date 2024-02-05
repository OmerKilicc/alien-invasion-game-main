using Euphrates;
using UnityEngine;

public class PlayerControls : MonoBehaviour
{
    bool _enabled = true;

    Transform _transform;

    [SerializeField] float _speed = 5f;
    [SerializeField] float _lerp = 2f;

    [Space]
    [Header("Constraints")]
    [SerializeField] float _lookDownConstraint = -45f;
    [SerializeField] float _lookUpConstraint = 45f;

    [Space]
    [Header("Controls")]
    [SerializeField] TriggerChannelSO _enableControls;
    [SerializeField] TriggerChannelSO _disableControls;
    [SerializeField] InputsSO _inputs;

    Quaternion _targetRotation;

    private void Awake() => _transform = transform;

    private void Start()
    {
        _targetRotation = transform.rotation;
    }

    private void OnEnable()
    {
        _enableControls.AddListener(EnableControls);
        _disableControls.AddListener(DisableControls);

        _inputs.OnChange += HandleInput;
    }

    private void OnDisable()
    {
        _enableControls.RemoveListener(EnableControls);
        _disableControls.RemoveListener(DisableControls);

        _inputs.OnChange -= HandleInput;
    }

    public void SetTargetRotaion(Quaternion rotation) => _targetRotation = rotation;

    void HandleInput()
    {
        if (!_enabled)
            return;

        float inputX = _inputs.Horizontal;
        float inputY = _inputs.Vertical;

        float rotateX = -inputY * _speed * .1f;
        float rotateY = inputX * _speed * .1f;

        _targetRotation *= Quaternion.Euler(rotateX, rotateY, 0);
    }

    float ClampAngle(float a)
    {
        float relative = (_lookUpConstraint - _lookDownConstraint) * .5f;

        float offset = _lookUpConstraint - relative;

        float b = ((a + 540) % 360) - 180 - offset;

        return Mathf.Abs(b) > relative ? relative * Mathf.Sign(b) + offset : a;
    }

    void EnableControls() => _enabled = true;

    void DisableControls() => _enabled = false;

    private void Update()
    {
        if (!_enabled)
            return;

        _transform.rotation = Quaternion.Lerp(_transform.rotation, _targetRotation, _lerp * Time.deltaTime);

        Vector3 euler = _transform.eulerAngles;
        _transform.eulerAngles = new Vector3(ClampAngle(euler.x), euler.y, 0);
    }
}