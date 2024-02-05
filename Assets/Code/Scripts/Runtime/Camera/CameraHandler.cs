using Cinemachine;
using Euphrates;
using UnityEngine;

public class CameraHandler : MonoBehaviour
{
    [SerializeField] TriggererdCamera[] _cameras;

    private void OnEnable()
    {
        for (int i = 0; i < _cameras.Length; i++)
        {
            _cameras[i].Trigger.AddListener(ResetCameraPirorties);
            _cameras[i].Trigger.AddListener(_cameras[i].OnTriggered);
        }
    }

    private void OnDisable()
    {
        for (int i = 0; i < _cameras.Length; i++)
        {
            _cameras[i].Trigger.RemoveListener(ResetCameraPirorties);
            _cameras[i].Trigger.RemoveListener(_cameras[i].OnTriggered);
        }
    }

    void ResetCameraPirorties()
    {
        for (int i = 0; i < _cameras.Length; i++)
            _cameras[i].VirtualCamera.Priority = 0;
    }
}

[System.Serializable]
struct TriggererdCamera
{
    public string Name;
    public CinemachineVirtualCamera VirtualCamera;
    public TriggerChannelSO Trigger;

    public void OnTriggered()
    {
        VirtualCamera.Priority = 10;
    }
}