using Euphrates;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Unity.Services.Authentication;
using Unity.Services.Core;
using Unity.Services.RemoteConfig;
using UnityEngine;

public class RemoteConfigHandler : MonoBehaviour
{
    const int CONNECTION_TIMEOUT_DELAY = 10000;

    [SerializeField] TriggerChannelSO _initChannel;
    [SerializeField] TriggerChannelSO _initDoneChannel;

    public struct userAttributes { }
    public struct appAttributes { }

    private void OnEnable() => _initChannel.AddListener(Init);

    private void OnDisable() => _initChannel.RemoveListener(Init);

    async void Init()
    {
        await Task.WhenAny(InitAsync(), Task.Delay(CONNECTION_TIMEOUT_DELAY));

        _initDoneChannel.Invoke();
    }

    async Task InitAsync()
    {
        if (Utilities.CheckForInternetConnection())
            await InitializeRemoteConfigAsync();

        // RemoteConfigService.Instance.FetchCompleted += ApplyRemoteSettings;
        await RemoteConfigService.Instance.FetchConfigsAsync(new userAttributes(), new appAttributes());
    }

    async Task InitializeRemoteConfigAsync()
    {
        // initialize handlers for unity game services
        await UnityServices.InitializeAsync();

        // remote config requires authentication for managing environment information
        if (!AuthenticationService.Instance.IsSignedIn)
        {
            await AuthenticationService.Instance.SignInAnonymouslyAsync();
        }
    }

    //void ApplyRemoteSettings(ConfigResponse configResponse)
    //{
    //    if (configResponse.requestOrigin != ConfigOrigin.Remote)
    //        return;

    //}
}
