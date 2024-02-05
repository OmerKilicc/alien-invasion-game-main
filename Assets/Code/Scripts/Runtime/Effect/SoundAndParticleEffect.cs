using UnityEngine;

public class SoundAndParticleEffect : MonoBehaviour, IEffect
{
    [Header("VFX")]
    [SerializeField] VFXChannelSO _vfxChannel;
    [SerializeField] string _vfxName;
    [SerializeField, Tooltip("Leave empty if you want to spawn particles at own transform.")] Transform _target;

    [Space]
    [Header("Sound")]
    [SerializeField] SoundChannelSO _soundChannel;
    [SerializeField] string _soundName;

    public void PlayEffect()
    {
        Vector3 pos = _target == null ? transform.position : _target.position;

        _vfxChannel.PlayVFX(_vfxName, transform.position);
        _soundChannel.Play(_soundName);
    }
}
