using Euphrates;
using TMPro;
using UnityEngine;

[RequireComponent(typeof(TextMeshProUGUI))]
public class MissileDelayDisplayer : MonoBehaviour
{
    TextMeshProUGUI _text;
    [SerializeField] MissileButtonEnabler _missileButtonEnabler;

    private void Awake() => _text = GetComponent<TextMeshProUGUI>();

    private void OnEnable()
    {
        _missileButtonEnabler.OnTimerEnd += ButtonEnabled;
        _missileButtonEnabler.OnTimerTick += Count;
    }

    private void OnDisable()
    {
        _missileButtonEnabler.OnTimerEnd -= ButtonEnabled;
        _missileButtonEnabler.OnTimerTick -= Count;
    }

    void ButtonEnabled()
    {
        _text.text = "";
    }

    void Count(TickInfo tick)
    {
        _text.text = $"{tick.TimeLeft.ToString("0.0")}s";
    }
}
