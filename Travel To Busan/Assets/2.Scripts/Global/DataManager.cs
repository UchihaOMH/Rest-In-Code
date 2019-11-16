using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using System.Text;

using UnityEngine;
using UnityEngine.UI;

using Firebase;
using Firebase.Extensions;
using Firebase.Database;

/// <summary>
/// 싱글톤을 통해 전역으로 로컬 데이터 제공
/// </summary>
public class DataManager : MonoBehaviour
{
    #region Property
    public static DataManager Instance
    {
        get => instance;
        private set => instance = value;
    }
    /// <summary>
    /// 웬만하면, 복사하거나 할당하지말고 DataManager.Instance.playerInfo 로 접근하길 바람
    /// </summary>
    public PlayerInfo PlayerInfo
    {
        get
        {
            if (BInitialized)
                return playerInfo;
            else
                return null;
        }
        set => playerInfo = value;
    }
    public bool BInitialized { get => bInitialized; private set => bInitialized = value; }
    #endregion

    #region Public Field
    public float autoSaveInterval = 5f;
    #endregion

    #region Private Field
    private static DataManager instance;
    private FirebaseDatabase fdb;
    private DatabaseConnection dbConnection;
    private bool bInitialized = false;

    private PlayerInfo playerInfo = new PlayerInfo();
    #endregion

    #region Mono
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
    }
    #endregion

    #region Method
    public void ConnectFirebaseDB()
    {
        // 파이어베이스 DB 연결
        FirebaseApp.CheckAndFixDependenciesAsync().ContinueWithOnMainThread((Task<DependencyStatus> result) =>
        {
            if (result.IsCompleted)
            {
                fdb = FirebaseDatabase.DefaultInstance;
                bInitialized = true;

                dbConnection = new DatabaseConnection(fdb);

                dbConnection.DoesDataExist(Authentication.Instance.User.id, (bool success) =>
                {
                    if (success)
                    {
                        Load();
                    }
                    else
                    {
                        playerInfo.CopyPlayerInfo(GameConst.newPlayerInfo);
                        Save();
                    }
                });

                InvokeRepeating("Save", autoSaveInterval, autoSaveInterval);
            }
        });
    }
    public void Load()
    {
        dbConnection.LoadPlayerData(Authentication.Instance.User.id, (PlayerInfo info) =>
        {
            if (info != null)
            {
                playerInfo = info;
                Debug.Log("**Debug : PlayerData => \n" + playerInfo.ToString());
            }
            else
            {
                Debug.LogError("**Debug : Player Data Load Faild Cancle Auto Save Function");
                Application.Quit();
            }
        });
    }
    public void Save()
    {
        dbConnection.SavePlayerData(Authentication.Instance.User.id, playerInfo, (bool success) =>
        {
            if (success)
            {
                Debug.Log("**Debug : Save Successed");
            }
            else
            {
                Debug.Log("**Debug : Save Faild");
            }
        });
    }
    public void Delete()
    {
        dbConnection.DeletePlayerData(Authentication.Instance.User.id, (bool success) =>
        {
            if (success)
            {
                Debug.Log("**Debug : Delete PlayerData From DB");
                Application.Quit();
            }
            else
            {
                Debug.LogError("**Debug : Faild Delete PlayerData From DB");
            }
        });
    }
    #endregion
}
