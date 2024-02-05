using Euphrates;
using UnityEngine;

public class PartUnlocker : MonoBehaviour
{
    [SerializeField] IntSO _level;
    [SerializeField] GameObject _missileScreen;
    [SerializeField] int _requiredLevel = 2;

    void Start()
    {
        if (_level.Value >= _requiredLevel)
            return;

        _missileScreen.SetActive(false);
    }
}
