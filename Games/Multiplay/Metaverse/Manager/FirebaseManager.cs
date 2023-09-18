using Firebase;
using Firebase.Extensions;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ���̾�̽� (Ǫ�� �˶�)
/// </summary>

public class FirebaseManager : MonoBehaviour
{
    public string deviceToken = "";

    void Start()
    {
#if UNITY_EDITOR || UNITY_STANDALONE || UNITY_EDITOR_OSX
        deviceToken = "";
#elif UNITY_ANDROID
        FirebaseApp.CheckAndFixDependenciesAsync().ContinueWithOnMainThread(task =>
        {
            FirebaseApp app = FirebaseApp.DefaultInstance;

            Firebase.Messaging.FirebaseMessaging.TokenReceived += OnTokenReceived;
            Firebase.Messaging.FirebaseMessaging.MessageReceived += OnMessageReceived;
            // ��ū ������ ��û
            Firebase.Messaging.FirebaseMessaging.GetTokenAsync().ContinueWithOnMainThread(tokenTask =>
            {
                deviceToken = tokenTask.Result;
            });
        });
#endif
    }

    public void OnTokenReceived(object sender, Firebase.Messaging.TokenReceivedEventArgs token)
    {
        Debug.Log("Received Registration Token: >>" + token.Token + "<<");        
    }

    public void OnMessageReceived(object sender, Firebase.Messaging.MessageReceivedEventArgs e)
    {
        Debug.Log("Received a new message from: " + e.Message.From);
        Debug.Log("Received a new message from: " + e.Message.Notification.Title);
        Debug.Log("Received a new message from: " + e.Message.Notification.Body);        
    }    
}
