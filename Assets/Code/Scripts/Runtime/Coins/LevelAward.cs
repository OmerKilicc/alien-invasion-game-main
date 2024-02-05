using Euphrates;
using UnityEngine;

public class LevelAward : MonoBehaviour
{
    int _randomLevelPrize = 0;

    [SerializeField] int _levelFailMinPrize = 50;
    [SerializeField] int _levelFailMaxPrize = 50;

    [SerializeField] int _levelWonMinPrize = 400;
    [SerializeField] int _levelWonMaxPrize = 500;

    [SerializeField] IntSO _coins;
    [SerializeField] TriggerChannelSO _levelComplete;
    [SerializeField] TriggerChannelSO _levelFail;


    private void OnEnable()
    {
        _levelFail.AddListener(LevelFailed);
        _levelComplete.AddListener(LevelWon);
    }

    private void OnDisable()
    {
        _levelFail.RemoveListener(LevelFailed);
        _levelComplete.RemoveListener(LevelWon);
    }

    void LevelFailed()
    {
        _randomLevelPrize = Random.Range(_levelFailMinPrize, _levelFailMaxPrize + 1);
        _coins.Value += _randomLevelPrize;
    }

    void LevelWon()
    {
        _randomLevelPrize = Random.Range(_levelWonMinPrize, _levelWonMaxPrize + 1);
        _coins.Value += _randomLevelPrize;
    }
}
