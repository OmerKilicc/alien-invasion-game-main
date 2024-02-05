using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Save and Load Channel", menuName = "SO Channels/Save and Load")]
public class SaveLoadSO : ScriptableObject
{
    static readonly string SAVE_NAME = "proto_save";

    [SerializeField] SaveData _defaultData;

    bool _cached = false;
    SaveData _cache;

    public void Save(SaveData data)
    {
        string datatStr = JsonUtility.ToJson(data);
        PlayerPrefs.SetString(SAVE_NAME, datatStr);
        PlayerPrefs.Save();
        _cached = false;
    }

    public SaveData Load()
    {
        if (!PlayerPrefs.HasKey(SAVE_NAME))
            return _defaultData;

        if (_cached)
            return _cache;

        _cached = true;

        _cache = JsonUtility.FromJson<SaveData>(PlayerPrefs.GetString(SAVE_NAME));
        return _cache;
    }
}

[System.Serializable]
public struct SaveData
{
    public int Level;
    public int Coins;
    public List<SavedUpgrade> Upgrades;
}

[System.Serializable]
public struct SavedUpgrade
{
    public string Name;
    public int Level;
}
