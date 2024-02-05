using System;
using UnityEngine;

[CreateAssetMenu(fileName = "New VFX Channel", menuName = "SO Channels/VFX")]
public class VFXChannelSO : ScriptableObject
{
    public event Action<string, Vector3> OnPlayVFX;
    public void PlayVFX(string effectName, Vector3 position) => OnPlayVFX?.Invoke(effectName, position);
}
