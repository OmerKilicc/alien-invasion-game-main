using Euphrates;

[System.Serializable]
public struct PlayerMessageData
{
    public int ID;
    public string Name;
    public PlayerMessageType MessageType;
    public string Message;
    public float Duration;
    public TriggerChannelSO[] Triggers;
    public bool ExcludeLevels;
    public LevelStageData[] LevelAndStages;
}

[System.Serializable]
public struct LevelStageData
{
    public int Level;
    public int[] Stages;
}

public enum PlayerMessageType { CommanderMessage, CommanderMessageConstant, UpgradeMessage }