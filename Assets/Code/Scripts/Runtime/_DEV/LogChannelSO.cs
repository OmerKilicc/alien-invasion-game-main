using UnityEngine;

[CreateAssetMenu(menuName = "DEV/Log Channel")]
public class LogChannelSO : ScriptableObject
{
    public void Log(string context) => Debug.Log(context);
}
