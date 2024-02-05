using Euphrates;
using System.Collections.Generic;
using UnityEngine;

public class Init : MonoBehaviour
{
    [SerializeField] List<InitData> _initializations = new List<InitData>();

    void Start() => DoInit();

    void DoInit(int from = 0)
    {
        for (int i = from; i < _initializations.Count; i++)
        {
            InitData initData = _initializations[i];

            if (initData.InitCompleteChannel != null)
            {
                void OnInitComplete()
                {
                    initData.InitCompleteChannel.RemoveListener(OnInitComplete);
                    DoInit(i + 1);
                }

                initData.InitCompleteChannel.AddListener(OnInitComplete);
                initData.InitChannel.Invoke();

                return;
            }

            initData.InitChannel.Invoke();
        }
    }
}

[System.Serializable]
public struct InitData
{
    public string Name;
    public TriggerChannelSO InitChannel;

    [Tooltip("This event is only required if this initialization needs to await methods. " +
        "If your initialization method can be completed when called leave this empty.")]
    public TriggerChannelSO InitCompleteChannel;
}