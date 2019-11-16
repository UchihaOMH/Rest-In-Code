using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using System.Text;

using UnityEngine;

using Firebase;
using Firebase.Extensions;
using Firebase.Database;
using TinyJSON;

/// <summary>
/// 파이어베이스와 직접적인 커넥션
/// </summary>
public class DatabaseConnection
{
    private FirebaseDatabase fdb;

    public DatabaseConnection(FirebaseDatabase fdb)
    {
        this.fdb = fdb;
    }

    #region Public Method
    public PlayerInfo LoadPlayerData(string userID, Action<PlayerInfo> callback)
    {
        PlayerInfo playerInfo = new PlayerInfo();

        fdb.GetReference(GameConst.FirebasePath.playerData + userID).GetValueAsync().ContinueWithOnMainThread((Task<DataSnapshot> result) =>
        {
            if (result.IsCompleted)
            {
                DataSnapshot snapshot;
                snapshot = result.Result;

                string data = snapshot.Value.ToString();
                JSON.Load(data).Make<PlayerInfo>(out playerInfo);
                callback(playerInfo);
            }
            else
            {
                NoteRefferenceError();
                callback(null);
            }
        });

        return playerInfo;
    }
    public void SavePlayerData(string userID, PlayerInfo playerInfo, Action<bool> callback)
    {
        string playerData = JSON.Dump(playerInfo);

        fdb.GetReference(GameConst.FirebasePath.playerData).Child(userID).SetValueAsync(playerData).ContinueWithOnMainThread((result) =>
        {
            if (result.IsCompleted)
            {
                callback(true);
            }
            else
            {
                NoteRefferenceError();
                callback(false);
            }
        });
    }
    public void DeletePlayerData(string userID, Action<bool> callback)
    {
        fdb.GetReference(GameConst.FirebasePath.playerData + userID).RemoveValueAsync().ContinueWithOnMainThread((result) =>
        {
            if (result.IsCompleted)
            {
                callback(true);
            }
            else
            {
                NoteRefferenceError();
                callback(false);
            }
        });
    }
    public void DoesDataExist(string userID, Action<bool> callback)
    {
        fdb.GetReference(GameConst.FirebasePath.playerData + userID).GetValueAsync().ContinueWithOnMainThread((Task<DataSnapshot> result) =>
        {
            if (result.IsCompleted)
            {
                DataSnapshot snapshot = result.Result;

                if (snapshot.Exists)
                    callback(true);
                else
                    callback(false);
            }
            else
            {
                NoteRefferenceError();
                callback(false);
            }
        });
    }
    #endregion

    #region Private Method
    private void NoteRefferenceError()
    {
        Debug.LogError("Firebase DB Reference Faulted or Canceled");
    }
    #endregion
}
