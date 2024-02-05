using Euphrates;
using UnityEngine;
using UnityEngine.UI;

public class ProgressionBar : MonoBehaviour
{
    const float SLIDER_ANIM_DURATION = 1f;

    [SerializeField] Slider _slider;

    [Space]
    [SerializeField] IntSO _stage;
    [SerializeField] IntSO _stageCount;

    [Space]
    [SerializeField] IntSO _loadedLevel;
    [SerializeField] int[] _disabledLevels;

    private void OnEnable()
    {
        _slider.gameObject.SetActive(true);

        UpdateSlider();

        for (int i = 0; i < _disabledLevels.Length; i++)
        {
            if (_disabledLevels[i] == _loadedLevel)
                _slider.gameObject.SetActive(false);
        }
    }

    public void UpdateSlider()
    {
        float val = _stage.Value / (float)(_stageCount.Value);

        Tween.DoTween(_slider.value, val, SLIDER_ANIM_DURATION, Ease.Lerp, SetVal);
    }

    void SetVal(float v)
    {
        if (_slider == null)
            return;

        _slider.value = v;
    }

    public void Finish() => Tween.DoTween(_slider.value, 1f, SLIDER_ANIM_DURATION, Ease.Lerp, SetVal);
}
