using Euphrates;
using TMPro;
using UnityEngine;

public class LevelDisplayer : MonoBehaviour
{
    const string TEXT_FORMAT = "Level {0}";

    [SerializeField] IntSO _level;
    [SerializeField] TextMeshProUGUI _text;

    private void OnEnable()
    {
        UpdateText(0);

        _level.OnChange += UpdateText;
    }

    private void OnDisable()
    {
        _level.OnChange -= UpdateText;
    }

    void UpdateText(int _) => _text.text = string.Format(TEXT_FORMAT, (_level.Value + 1).ToString());
}
