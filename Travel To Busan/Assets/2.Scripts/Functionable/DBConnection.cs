using System;
using System.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Firebase;
using Firebase.Auth;
using Firebase.Extensions;
using Firebase.Database;

using TinyJSON;

public class DBConnection
{
    public bool Initialized
    {
        get => initialized;
        private set => initialized = value;
    }
    public string UserIdURI
    {
        get => userIdURI;
        private set => userIdURI = value;
    }
    public FirebaseUser User
    {
        get => user;
        set => user = value;
    }

    private bool initialized = false;
    private GPGSAuthentication gpgs = new GPGSAuthentication();
    private FirebaseAuth auth;
    private FirebaseDatabase db;

    private FirebaseUser user;

    private string userIdURI = "Player/ID/";

    public void Initialize(Action _callback = null)
    {
        if (!initialized)
        {
            FirebaseApp.CheckAndFixDependenciesAsync().ContinueWithOnMainThread(success =>
            {
                auth = FirebaseAuth.DefaultInstance;
                db = FirebaseDatabase.DefaultInstance;

                if (success.Result == DependencyStatus.Available)
                {
                    gpgs.Authentication(() =>
                    {
                        var credential = PlayGamesAuthProvider.GetCredential(gpgs.ServerAuthCode);
                        auth.SignInWithCredentialAsync(credential).ContinueWithOnMainThread(result =>
                        {
                            Initialized = true;

                            UserIdURI += FirebaseAuth.DefaultInstance.CurrentUser.UserId;
                            User = result.Result;

                            _callback?.Invoke();

                            Debug.Log("******* DEBUG ******** : SignIn Complete");
                        });
                    });
                }
            });
        }
        else
            _callback?.Invoke();
    }

    public void LoadDataFromDB<T>(string _uri, Action<T> _callback)
    {
        Initialize(() =>
        {
            db.GetReference(_uri).GetValueAsync().ContinueWithOnMainThread((task) =>
            {
                if (task.Result.Exists)
                {
                    string json = task.Result.GetRawJsonValue();
                    T t = JSON.Load(json).Make<T>();

                    _callback?.Invoke(t);
                }
                else
                    _callback?.Invoke(default(T));
            });
        });
    }
    public void SaveDataToDB<T>(string _uri, T value, Action _callback = null)
    {
        Initialize(() =>
        {
            string json = JsonUtility.ToJson(value);
            db.GetReference(_uri).SetRawJsonValueAsync(json).ContinueWithOnMainThread((task) =>
            {
                _callback();
            });
        });
    }
}
