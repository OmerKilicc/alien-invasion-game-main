using Euphrates;
using System;
using TMPro;
using UnityEngine;

public class OffflineEarnings : MonoBehaviour
{
    [SerializeField] TriggerChannelSO _offlineCollected;
    [SerializeField] TriggerChannelSO _showOfflineCollection;

    [SerializeField] IntSO _coins;
    [SerializeField] TextMeshProUGUI _offlineEarningText;

    [SerializeField] FloatSO _offlineMin;
    [SerializeField] FloatSO _offlineMax;

    int _coinsToCollect = 0;

    private void OnEnable()
    {
        _offlineCollected.AddListener(AddCollectedCoins);
    }

    private void OnDisable()
    {
        _offlineCollected.RemoveListener(AddCollectedCoins);
    }

    void Start()
    {
        if (PlayerPrefs.HasKey("LAST_LOGIN"))
            CalculateOfflineEarnings();
    }

    private void CalculateOfflineEarnings()
    {

        DateTime lastLogin = DateTime.Parse(PlayerPrefs.GetString("LAST_LOGIN"));
        TimeSpan timeSpan = DateTime.Now - lastLogin;

        if (timeSpan.TotalMinutes < _offlineMin)
            return;

        double hours = (double)Mathf.Clamp((float)timeSpan.TotalHours, _offlineMin / 60f, _offlineMax / 60f);

        _showOfflineCollection.Invoke();
        _coinsToCollect = (int)hours * 10 + 60;
        _offlineEarningText.text = _coinsToCollect.ToString();
    }

    void AddCollectedCoins() => _coins.Value += _coinsToCollect;

    bool _pasued = false;
    private void OnApplicationPause(bool pause)
    {
        _pasued = pause;

        if (!pause)
            return;

        PlayerPrefs.SetString("LAST_LOGIN", DateTime.Now.ToString());
    }

    private void OnApplicationQuit()
    {
        if (_pasued)
            return;

        PlayerPrefs.SetString("LAST_LOGIN", DateTime.Now.ToString());
    }
}
