using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using Facebook.Unity;
using Euphrates;

#if UNITY_ANDROID
using Unity.Notifications.Android;
#endif

public class PushNotificationsManager : MonoBehaviour
{
#if UNITY_ANDROID

    public AndroidNotificationChannel defaultNotificationChannel;

    DateTime _retentionWaitTime;
    DateTime _offlineEaringsWaitTime;

    [SerializeField] FloatSO _retentionNotifTime;
    [SerializeField] FloatSO _offlineEarningNotifTime;

    [SerializeField] string _retentionTitleText = "Come back, Pilot!";
    [SerializeField] string _retentionBodyText = "The galaxy is in danger.";
    [SerializeField] string _retentionIconSmall = "default";
    [SerializeField] string _retentionIconLarge = "default";

    [SerializeField] string _offlineEaringTitleText = "Pilot, while you were gone...";
    [SerializeField] string _offlineEaringBodyText = "Our fleet raided the enemy base. Come and collect your pay.";
    [SerializeField] string _offlineEaringIconSmall = "default";
    [SerializeField] string _offlineEaringIconLarge = "default";


    private const string _notificationChannelId = "notification_chanell";
    private string _notificationChannelName = "Default Channel";
    private string _notificationChannelDescription = "For Generic notifications";

    private int _identifierRetention;
    private int _identifierOffline;

    private void Start()
    {
        CreateNotificationChannel();
    }

    private void CreateNotificationChannel()
    {
        defaultNotificationChannel = new AndroidNotificationChannel()
        {
            Id = _notificationChannelId,
            Name = _notificationChannelName,
            Description = _notificationChannelDescription,
            Importance = Importance.Default,
        };

        AndroidNotificationCenter.RegisterNotificationChannel(defaultNotificationChannel);
    }

    public void ScheduleNotifications()
    {
        _retentionWaitTime = DateTime.Now.AddMinutes(_retentionNotifTime.Value);
        _offlineEaringsWaitTime = DateTime.Now.AddMinutes(_offlineEarningNotifTime.Value);

        CreateNotifications();
        NotificationReceive();
    }

    private static void NotificationReceive()
    {
        AndroidNotificationCenter.NotificationReceivedCallback receivedNotificationHandler = delegate (AndroidNotificationIntentData data)
        {
            var msg = "Notification received : " + data.Id + "\n";
            msg += "\n Notification received: ";
            msg += "\n .Title: " + data.Notification.Title;
            msg += "\n .Body: " + data.Notification.Text;
            msg += "\n .Channel: " + data.Channel;
            Debug.Log(msg);
        };

        AndroidNotificationCenter.OnNotificationReceived += receivedNotificationHandler;

        var notificationIntentData = AndroidNotificationCenter.GetLastNotificationIntent();

        if (notificationIntentData != null)
        {
            Debug.Log("App was opened with notification");
        }
    }

    private void CreateNotifications()
    {
        AndroidNotification retentionNotification = new AndroidNotification()
        {
            Title = _retentionTitleText,
            Text = _retentionBodyText,
            SmallIcon = _retentionIconSmall,
            LargeIcon = _retentionIconLarge,
            FireTime = _retentionWaitTime,
        };

        AndroidNotification offlineEarningNotification = new AndroidNotification()
        {
            Title = _offlineEaringTitleText,
            Text = _offlineEaringBodyText,
            SmallIcon = _offlineEaringIconSmall,
            LargeIcon = _offlineEaringIconLarge,
            FireTime = _offlineEaringsWaitTime
        };

        _identifierRetention = AndroidNotificationCenter.SendNotification(retentionNotification, _notificationChannelId);
        _identifierOffline = AndroidNotificationCenter.SendNotification(offlineEarningNotification, _notificationChannelId);
    }


    bool _paused = false;
    private void OnApplicationPause(bool pause)
    {
        _paused = pause;

        if (!_paused)
            return;

        Exited();
    }

    private void OnApplicationQuit()
    {
        if (_paused)
            return;

        Exited();
    }

    void Exited()
    {
        NotificationStatus offlineStats = AndroidNotificationCenter.CheckScheduledNotificationStatus(_identifierOffline);
        NotificationStatus retentionStats = AndroidNotificationCenter.CheckScheduledNotificationStatus(_identifierRetention);

        if ((offlineStats & (NotificationStatus.Delivered | NotificationStatus.Scheduled)) != NotificationStatus.Unknown)
            AndroidNotificationCenter.CancelNotification(_identifierOffline);

        if ((retentionStats & (NotificationStatus.Delivered | NotificationStatus.Scheduled)) != NotificationStatus.Unknown)
            AndroidNotificationCenter.CancelNotification(_identifierOffline);

        ScheduleNotifications();
    }

#endif
}
