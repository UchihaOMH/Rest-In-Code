using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

using UnityEngine;
using UnityEngine.SocialPlatforms;

using GooglePlayGames;
using GooglePlayGames.BasicApi;

using Firebase;
using Firebase.Extensions;
using Firebase.Auth;

/// <summary>
/// 로그인 및 인증 로직
/// </summary>
public class Authentication : MonoBehaviour
{
    #region Property
    public static Authentication Instance
    {
        get => Instance;
        private set => Instance = value;
    }
    public PlayGamesLocalUser User
    {
        get
        {
            if (BAuthenticated)
                return user;
            else
                return null;
        }
        private set => user = value;
    }
    public bool BAuthenticated { get => bAuthenticated; private set => bAuthenticated = value; }
    #endregion

    #region Public Field
    #endregion

    #region Private Field
    private static Authentication instance;
    private FirebaseAuth auth;
    private PlayGamesLocalUser user;
    private bool bAuthenticated = false;
    #endregion

    private void Awake()
    {
        //  싱글톤 구현
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else if (instance != this)
            Destroy(gameObject);

        //  Dependency 체크
        FirebaseApp.CheckAndFixDependenciesAsync().ContinueWithOnMainThread((Task<DependencyStatus> result) =>
        {
            if (result.IsCompleted)
            {
                auth = FirebaseAuth.DefaultInstance;
                TryAuthentication();

                return;
            }
        });
    }

    public void TryAuthentication()
    {
        //  GPGS 초기화
        PlayGamesPlatform.InitializeInstance(new PlayGamesClientConfiguration.Builder()
            .RequestIdToken()
            .RequestServerAuthCode(false)
            .Build());
        PlayGamesPlatform.DebugLogEnabled = true;
        PlayGamesPlatform.Activate();

        if (!Social.localUser.authenticated)
        {
            Social.localUser.Authenticate((bool success) =>
            {
                if (success)
                {
                    // Credential로 로그인
                    Credential credential = PlayGamesAuthProvider.GetCredential(PlayGamesPlatform.Instance.GetServerAuthCode());
                    auth.SignInWithCredentialAsync(credential).ContinueWith((Task<FirebaseUser> result) =>
                    {
                        if (result.IsCompleted)
                        {
                            bAuthenticated = true;

                            user = Social.localUser as PlayGamesLocalUser;
                            DataManager.Instance.ConnectFirebaseDB();
                        }
                    });
                }
                else
                {
                    Debug.LogError("Google Play Game Service Signin Failed");
                }
            });
        }
    }
}
