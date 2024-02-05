using Euphrates;
using Euphrates.Path;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class PlayerStageProgress : MonoBehaviour
{
    ITraveller _movement;

    [SerializeField] TriggerChannelSO _stageSet;
    [SerializeField] TriggerChannelSO _stageStart;

    [Space]
    [SerializeField] List<Path> _stagePaths = new List<Path>();

    [Space]
    [Header("Controls")]
    [SerializeField] TriggerChannelSO _enableControls;
    [SerializeField] TriggerChannelSO _disableControls;

    int _pathIndex = 0;

    private void Awake()
    {
        _movement = GetComponent<ITraveller>();
    }

    private void OnEnable()
    {
        _stageSet.AddListener(SetStage);
        _movement.OnPathEnded += StartStage;
    }

    private void OnDisable()
    {
        _stageSet.RemoveListener(SetStage);
        _movement.OnPathEnded -= StartStage;
    }

    void SetStage()
    {
        _disableControls.Invoke();

        if (_stagePaths.Count == 0 || _pathIndex == _stagePaths.Count)
        {
            StartStage();
            return;
        }

        Path path = _stagePaths[_pathIndex++];

        if (path == null)
        {
            StartStage();
            return;
        }

        _movement.SetPath(path);
    }

    async void StartStage()
    {
        await Task.Yield();

        _stageStart.Invoke();
        _enableControls.Invoke();
    }
}
