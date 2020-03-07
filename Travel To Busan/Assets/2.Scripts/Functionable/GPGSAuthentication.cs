using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using GooglePlayGames;
using GooglePlayGames.BasicApi;

using Firebase;
using Firebase.Extensions;
using Firebase.Auth;

public class GPGSAuthentication
{
    public bool IsAuthenticated
    {
        get => isAuthenticated;
        private set => isAuthenticated = value;
    }
    public string ServerAuthCode
    {
        get => serverAuthCode;
        private set => serverAuthCode = value;
    }

    private bool isAuthenticated = false;
    private string serverAuthCode = "";

    public void Authentication(Action _callback = null)
    {
        var config = new PlayGamesClientConfiguration.Builder()
            .RequestServerAuthCode(false)
            .RequestIdToken()
            .Build();   
        PlayGamesPlatform.InitializeInstance(config);
        PlayGamesPlatform.DebugLogEnabled = true;
        PlayGamesPlatform.Activate();

        Social.localUser.Authenticate(success =>
        {
            if (success)
            {
                IsAuthenticated = true;
                serverAuthCode = PlayGamesPlatform.Instance.GetServerAuthCode();

                _callback?.Invoke();
            }
            else
                throw new Firebase.FirebaseException();
        });
    }
}
