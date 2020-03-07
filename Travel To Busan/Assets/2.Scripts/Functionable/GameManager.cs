using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;

using UnityEngine;
using UnityEngine.SceneManagement;

using TinyJSON;

public class GameManager : MonoBehaviour
{
    public enum eScene
    {
        FirstSceneBridge = 0,
        Prologue,
        Level1,
        Level2,
        Level3,
        Level4,
        Level5
    }

    #region Property
    public static GameManager Instance
    {
        get => instance;
        private set => instance = value;
    }

    public FXPoolManager FxPool
    {
        get => fxPool;
        private set => fxPool = value;
    }
    public EnemyPoolManager EnemyPool
    {
        get => enemyPool;
        private set => enemyPool = value;
    }
    public DamageCalculator DamageCalculator
    {
        get => damageCalculator;
        private set => damageCalculator = value;
    }
    public Fade Fade
    {
        get => fade;
        private set => fade = value;
    }

    //  Parameter
    public GameData GameData
    {
        get => gameData;
        private set => gameData = value;
    }
    public ConversationManager Conversation
    {
        get => conversation;
        private set => conversation = value;
    }
    public DBConnection DbConnection
    {
        get => dbConnection;
        private set => dbConnection = value;
    }
    public Player Player
    {
        get => player;
        private set => player = value;
    }
    public GameObject MainUI
    {
        get => mainUI;
        private set => mainUI = value;
    }
    public UITutorial Tutorial
    {
        get => tutorial;
        set => tutorial = value;
    }
    #endregion

    #region Public Field


    [Space(15f)]
    public WeaponDefinition weaponDefinition;
    #endregion

    #region Private Field
    private static GameManager instance = null;

    //  Component
    private FXPoolManager fxPool;
    private EnemyPoolManager enemyPool;
    private DamageCalculator damageCalculator;
    private Fade fade;
    //  Initialize on FirstSceneBridgeSceneManager class
    private DBConnection dbConnection = new DBConnection();

    //  Parameter
    [Space(15f)]
    private UITutorial tutorial;
    private ConversationManager conversation;
    private GameObject mainUI;
    
    private GameData gameData = null;
    private Player player;
    #endregion

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(this.gameObject);
        }
        else
            Destroy(this.gameObject);

        Application.targetFrameRate = 30;

        //  Initialize Component
        FxPool = GetComponentInChildren<FXPoolManager>();
        EnemyPool = GetComponentInChildren<EnemyPoolManager>();
        DamageCalculator = GetComponentInChildren<DamageCalculator>();
        Fade = GetComponentInChildren<Fade>();
        Tutorial = GetComponentInChildren<UITutorial>();
        Conversation = GetComponentInChildren<ConversationManager>();

        MainUI = GameObject.FindGameObjectWithTag("Main UI");

        Player = GetComponentInChildren<Player>();
        Player.EquipWeapon(weaponDefinition.GetWeaponByWeaponName("바람의 주먹"));

#if UNITY_ANDROID
        DbConnection.Initialize(() => LoadGameData(() =>
        {
            if (!GameData.prologueShown)
                Fade.LoadScene((int)eScene.Prologue);
            else
                Fade.LoadScene(GameData.currLevel, () => MainUI.SetActive(true));
        }));
#endif
    }

    public void PlayerDead()
    {
        Invoke("RespawnPlayer", 2f);
    }
    public void PopupOptionPanel()
    {

    }
    public void SaveGameData(Action _callback = null)
    {
        //  게임데이터가 없는경우
        if (GameData == null)
            GameData = new GameData();

        GameData.playerInfo = Player.info;

        DbConnection.SaveDataToDB(DbConnection.UserIdURI, GameData, _callback);
    }
    public void LoadGameData(Action _callback = null)
    {
        DbConnection.LoadDataFromDB(DbConnection.UserIdURI, (GameData _data) =>
        {
            if (_data == null)
            {
                SaveGameData(_callback);
            }
            else
            {
                GameData = _data;
                player.info = GameData.playerInfo;
                _callback?.Invoke();
            }
        });
    }
    public void PauseGame()
    {
        Time.timeScale = 0f;
        Time.fixedDeltaTime = 0f;
    }
    public void ResumeGame()
    {
        Time.timeScale = 1f;
        Time.fixedDeltaTime = 0.01f;
    }
    public void ShowTutorial()
    {
        GameData.uiTutorialShown = true;
        Tutorial.gameObject.SetActive(true);
        SaveGameData();
    }

    private void RespawnPlayer()
    {
        Fade.LoadScene((int)eScene.Level1, () =>
        {
            player.info.currHP = player.info.maxHP;
            player.isDead = false;
            player.apPortrait.Play(_PlayerAnimTrigger_.idle);
            player.CurrState = player.animationStates.run;
        });
    }
}
