using Euphrates;
using UnityEngine;

public class CollectedUpdater : MonoBehaviour
{
    [SerializeField] IntSO _totalCoins;
    [SerializeField] IntSO _collectedCoins;

    private void OnEnable() => _totalCoins.OnChange += UpdateCoins;

    private void OnDisable() => _totalCoins.OnChange -= UpdateCoins;

    void UpdateCoins(int change) => _collectedCoins.Value += change;
}
