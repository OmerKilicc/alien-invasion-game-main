using Euphrates;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UpdateHealthInfo : MonoBehaviour
{
    [SerializeField] Slider _slider;
    [SerializeField] TextMeshProUGUI _text;

    [Space]
    [SerializeField] IntSO _baseHealth;
    [SerializeField] IntSO _currentHealth;

    private void OnEnable()
    {
        _currentHealth.OnChange += UpdateHealth;
        UpdateHealth(0);
    }

    private void OnDisable()
    {
        _currentHealth.OnChange -= UpdateHealth;
    }

    public void UpdateHealth(int change)
    {
        int cHealth = Mathf.Clamp(_currentHealth.Value, 0, _baseHealth.Value);

        float slVal = (float)cHealth / (float)_baseHealth.Value;
        _slider.value = slVal;

        _text.text = $"{cHealth}/{_baseHealth.Value}";
    }
}
