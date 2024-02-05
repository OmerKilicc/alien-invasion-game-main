using System.Text.RegularExpressions;
using Unity.Services.RemoteConfig;
using UnityEngine;

[CreateAssetMenu(menuName = "EnemySO")]
public class EnemySO : ScriptableObject
{

    public int StartHealth;
    public float Speed = -1;
    public float TurnSpeed = -1;

    [Header("Combat")]
    public int Damage;
    public float FireRate;
    public float ShootDuration;
    public float FirePauseDuration;

    #region Remote Config

    [System.Serializable]
    public struct RemoteEnemyData
    {
        public int StartHealth;
        public float Speed;
        public float TurnSpeed;

        public int Damage;
        public float FireRate;
        public float ShootDuration;
        public float FirePauseDuration;
    }

    RemoteConfigService _remoteConfig = RemoteConfigService.Instance;

    private void OnEnable()
    {
        GetConfig();
        _remoteConfig.FetchCompleted += OnFetchComplete;
    }

    private void OnDisable()
    {
        _remoteConfig.FetchCompleted -= OnFetchComplete;
    }

    void GetConfig()
    {
        if (!_remoteConfig.appConfig.HasKey(name))
            return;

        string json = _remoteConfig.appConfig.GetJson(name);
        LoadFromJson(json);
    }

    private static readonly Regex sWhitespace = new Regex(@"\s+");
    void OnFetchComplete(ConfigResponse configResponse)
    {
        string key = sWhitespace.Replace(name, "");

        if (!_remoteConfig.appConfig.HasKey(key) 
            || configResponse.status != ConfigRequestStatus.Success 
            || configResponse.requestOrigin != ConfigOrigin.Remote)
            return;

        string json = _remoteConfig.appConfig.GetJson(key);
        LoadFromJson(json);
    }

    void LoadFromJson(string value)
    {
        try
        {
            RemoteEnemyData data = JsonUtility.FromJson<RemoteEnemyData>(value);
            CopyFrom(data);
        }
        catch { }
    }

    void CopyFrom(RemoteEnemyData data)
    {
        this.StartHealth = data.StartHealth;
        this.Speed = data.Speed;
        this.TurnSpeed = data.TurnSpeed;

        this.Damage = data.Damage;
        this.FireRate = data.FireRate;
        this.ShootDuration = data.ShootDuration;
        this.FirePauseDuration = data.FirePauseDuration;
    }

#if UNITY_EDITOR
    public string GenerateJson() => JsonUtility.ToJson(new RemoteEnemyData()
    {
        StartHealth = this.StartHealth,
        Speed = this.Speed,
        TurnSpeed = this.TurnSpeed,

        Damage = this.Damage,
        FireRate = this.FireRate,
        ShootDuration = this.ShootDuration,
        FirePauseDuration = this.FirePauseDuration
    });
#endif

    #endregion
}
