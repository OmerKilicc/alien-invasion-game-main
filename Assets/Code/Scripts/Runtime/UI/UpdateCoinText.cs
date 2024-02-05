using Euphrates;
using TMPro;
using UnityEngine;

public class UpdateCoinText : MonoBehaviour
{
    [SerializeField] IntSO _int;
    [SerializeField] TextMeshProUGUI _text;

    [Space]
    [SerializeField] float _animDuration = .25f;

    int _cval = 0;

    private void OnEnable()
    {
        UpdateVal(0);
        _int.OnChange += UpdateVal;
    }

    private void OnDisable()
    {
        _int.OnChange -= UpdateVal;
    }

    bool _tweening = false;
    TweenData _td;
    void UpdateVal(int change)
    {
        if (_tweening)
            _td?.Stop();

        _td = Tween.DoTween(_cval, _int.Value, _animDuration, Ease.Lerp, UpdateText, () => _tweening = false);
        _tweening = true;
    }

    void UpdateText(int val)
    {
        _text.text = val.ToString();
        _cval = val;
    }
}
