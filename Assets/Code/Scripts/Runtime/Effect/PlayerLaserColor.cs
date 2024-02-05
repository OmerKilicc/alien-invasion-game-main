using UnityEngine;

public class PlayerLaserColor : MonoBehaviour
{
    const float EMISSION_INTENSITY = 6f;

    [SerializeField] Gradient _colorGradient;
    [SerializeField] int _maxLevel = 1;
    [SerializeField] UpgradeSO _damageUpgrade;
    [SerializeField] Material _playerLaserMaterial;

    void Start()
    {
        SetColor();
    }

    void SetColor()
    {
        int level = Mathf.Clamp(_damageUpgrade.UpgradedAmount, 0, _maxLevel);
        float t = (float)level / (float)_maxLevel;

        Color color = _colorGradient.Evaluate(t);

        _playerLaserMaterial.SetColor("_BaseColor", color);
        _playerLaserMaterial.SetColor("_Color", color);

        color = new Color(color.r, color.g, color.b, 1f) * EMISSION_INTENSITY;

        _playerLaserMaterial.SetColor("_EmissionColor", color);
    }
}
