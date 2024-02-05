using TMPro;
using UnityEngine;

[RequireComponent(typeof(Upgrader))]
public class UpgradeLocker : MonoBehaviour
{
    const string LOCKED_TEXT = "Reach Level {0}";

    Upgrader _upgrader;

    [SerializeField] GameObject _overlay;
    [SerializeField] TextMeshProUGUI _overlayText;

    private void Awake() => _upgrader = GetComponent<Upgrader>();

    public void CheckUnlock()
    {
        if (_upgrader.Unlocked)
            Unlock();
        else
            Lock();
    }

    void Lock()
    {
        _overlay.SetActive(true);

        _overlayText.text = string.Format(LOCKED_TEXT, _upgrader.UpgradeData.LevelToUnlock + 1);
    }

    void Unlock() => _overlay.SetActive(false);
}
