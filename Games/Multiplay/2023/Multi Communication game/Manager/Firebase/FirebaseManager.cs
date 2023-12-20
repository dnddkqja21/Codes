using UnityEngine;

using System;
using System.Linq;
using System.Collections.Generic;

#if UNITY_ANDROID
using Unity.Notifications.Android;
using Firebase.Extensions;
using Firebase;
#elif UNITY_IOS
using Unity.Notifications.iOS;
using Firebase.Extensions;
using Firebase;
#endif

public class FirebaseManager : MonoBehaviour
{
    //static FirebaseManager instance = null;
    //public static FirebaseManager Instance { get { return instance; } }

    public string appOs = "";
    public string deviceToken = "";

    //void Awake()
    //{
    //    if (instance == null)
    //        instance = this;
    //}

    void Start()
    {
        if(!BuildConfig.isRelease) { return; }


#if UNITY_EDITOR || UNITY_STANDALONE || UNITY_EDITOR_OSX
        //Bridge.Instance.deviceToken = "token_pc";
        appOs = "PC";
#elif UNITY_ANDROID || UNITY_IOS
        // ContinueWithOnMainThread, using Firebase.Extensions; 메인쓰레드에서 해야 한다.

        FirebaseApp.CheckAndFixDependenciesAsync().ContinueWithOnMainThread(task =>
        {
            FirebaseApp app = FirebaseApp.DefaultInstance;

            Firebase.Messaging.FirebaseMessaging.TokenReceived += OnTokenReceived;
            Firebase.Messaging.FirebaseMessaging.MessageReceived += OnMessageReceived;
            // 토큰 얻어오기 요청
            Firebase.Messaging.FirebaseMessaging.GetTokenAsync().ContinueWithOnMainThread(tokenTask =>
            {
                deviceToken = tokenTask.Result;
            });
        });
#endif

#if UNITY_ANDROID
        InitializeAndroidLocalPush();
#endif

    }

    public void OnTokenReceived(object sender, Firebase.Messaging.TokenReceivedEventArgs token)
    {
        Debug.Log("Received Registration Token: >>" + token.Token+"<<");
        deviceToken = token.Token;
    }

    public void OnMessageReceived(object sender, Firebase.Messaging.MessageReceivedEventArgs e)
    {
        try
        {
            IDictionary<string, string> data = e.Message.Data;
            SendNotification(data["title"], data["body"], DateTime.Now.AddSeconds(0));
        }
        catch (Exception ex)
        {
            Debug.Log("firebase ex : " + ex.ToString());
        } 
    }

    public static void SendNotification(string title, string explain, DateTime time)
    {
        try
        {
#if UNITY_ANDROID

            var notification = new AndroidNotification();
            notification.Title = title;
            notification.Text = explain;
            notification.FireTime = time;
            
            notification.FireTime = DateTime.Now;

            notification.SmallIcon = "icon_0";
            notification.LargeIcon = "icon_1";
            notification.ShowInForeground = true;
            notification.ShouldAutoCancel = true;
            if (getAosApiLevel() >= 26)
            {
                AndroidNotificationCenter.SendNotification(notification, "channel_id");
            }
#elif UNITY_IOS
            var timeTrigger = new iOSNotificationTimeIntervalTrigger()
            {
                TimeInterval = time - DateTime.Now,
                Repeats = false
            };

            var notification = new iOSNotification()
            {
                Identifier = "_notification",
                Title = title,
                Body = explain,
                //Subtitle = explain,
                ShowInForeground = false,
                ForegroundPresentationOption = (PresentationOption.Alert | PresentationOption.Sound),
                CategoryIdentifier = "category_a",
                ThreadIdentifier = "thread1",
                Trigger = timeTrigger,
            };

            iOSNotificationCenter.ScheduleNotification(notification);
#endif
        }
        catch (Exception e)
        {
            Debug.Log("gusanaglkyo == eeee");
            Debug.Log(e.ToString());
        }
    }


    private static int getAosApiLevel()
    {
        string androidInfo = SystemInfo.operatingSystem;
        Debug.Log("androidInfo: " + androidInfo);
        int apiLevel = int.Parse(androidInfo.Substring(androidInfo.IndexOf("-") + 1, 2));
        return apiLevel;
    }

    public void InitializeAndroidLocalPush()
    {
        // 디바이스의 안드로이드 api level 얻기
#if UNITY_ANDROID
        int apiLevel = getAosApiLevel();
        if (apiLevel >= 26)
        {
            var c = new AndroidNotificationChannel()
            {
                Id = "channel_id",
                Name = "com.google.firebase.messaging.default_notification_channel_id",
                Importance = Importance.High,
                Description = "Generic notifications",
            };

            AndroidNotificationCenter.RegisterNotificationChannel(c);
        }
#endif
    }

    public static void CancelAllNotifications()
    {
#if UNITY_ANDROID
        AndroidNotificationCenter.CancelAllNotifications();
#elif UNITY_IOS
        iOSNotificationCenter.RemoveAllScheduledNotifications();
#endif
    }
}
