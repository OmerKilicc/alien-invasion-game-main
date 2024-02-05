using Euphrates;
using UnityEngine;
using UnityEngine.UI;

public class PowerLaserIndicator : MonoBehaviour
{
    [SerializeField] Slider _powerSlider;
    [SerializeField] FloatSO _maxPower;
    [SerializeField] FloatSO _currentPower;

    private void OnEnable() => _currentPower.OnChange += PowerChanged;

    private void OnDisable() => _currentPower.OnChange -= PowerChanged;

    void PowerChanged(float _)
    {
        _powerSlider.value = Mathf.Clamp01(_currentPower.Value / _maxPower.Value);
    }
}
