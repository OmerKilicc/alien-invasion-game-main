using TMPro;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(IHealth))]
public class Healthbar : MonoBehaviour
{
    IHealth _health;

    [SerializeField] Slider _slider;

    private void Awake() => _health = GetComponent<IHealth>();

    private void OnEnable() => _health.OnHealthChange += OnHealthChange;

    private void OnDisable() => _health.OnHealthChange -= OnHealthChange;

    void OnHealthChange(int change)
    {
        _slider.gameObject.SetActive(_health.Health > 0);

        int baseHealth = _health.BaseHealth;
        int health = _health.Health;

        _slider.value = health / (float)baseHealth;
    }
}
