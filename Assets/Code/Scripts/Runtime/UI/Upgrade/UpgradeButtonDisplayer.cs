using Euphrates;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Upgrader))]
public class UpgradeButtonDisplayer : MonoBehaviour
{
    Upgrader _upgrader;

    [SerializeField] TextMeshProUGUI _costText;
    [SerializeField] GameObject _coinIcon;
    [SerializeField] IntSO _currentLevel;

    [Space]
    [SerializeField] TextMeshProUGUI _name;
    [SerializeField] Image _image;

    private void Awake()
    {
        _upgrader = GetComponent<Upgrader>();
    }

    public void UpdateAll()
    {
        _name.text = _upgrader.UpgradeData.Name;
        _image.sprite = _upgrader.UpgradeData.Image;

        UpdateCost();
    }

    public void UpdateCost()
    {
        _coinIcon.SetActive(true);
        _costText.text = _upgrader.UpgradeData.Cost.ToString();
    }
}
