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

    //  Component
    public Player Player
    {
        get => player;
        set => player = value;
    }
    public FXPoolManager FxPool
    {
        get => fxPool;
        set => fxPool = value;
    }
    public EnemyPoolManager EnemyPool
    {
        get => enemyPool;
        set => enemyPool = value;
    }
    public DamageCalculator DamageCalculator
    {
        get => damageCalculator;
        set => damageCalculator = value;
    }
    public Fade Fade
    {
        get => fade;
        set => fade = value;
    }

    //  Parameter
    public GameData GameData
    {
        get => gameData;
        set => gameData = value;
    }
    public ConversationManager Conversation
    {
        get => conversation;
        set => conversation = value;
    }
    /// <summary>
    /// Include '\' at last
    /// </summary>
    public string ScriptFolderPath
    {
        get => scriptFolderPath;
        set => scriptFolderPath = value;
    }
    #endregion

    #region Public Field
    public WeaponDefinition weaponDefinition;
    #endregion

    #region Private Field
    private static GameManager instance = null;

    //  Component
    private FXPoolManager fxPool;
    private EnemyPoolManager enemyPool;
    private DamageCalculator damageCalculator;
    private Fade fade;
    private ConversationManager conversation;

    //  Parameter
    private Player player;
    private GameData gameData;

    private string scriptFolderPath;
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

        ScriptFolderPath = Application.persistentDataPath + @"/Resources/Script/";
        Application.targetFrameRate = 30;

        LoadGameData();

        //  Initialize Component
        FxPool = GetComponentInChildren<FXPoolManager>();
        EnemyPool = GetComponentInChildren<EnemyPoolManager>();
        DamageCalculator = GetComponentInChildren<DamageCalculator>();
        Fade = GetComponentInChildren<Fade>();
        Conversation = GetComponentInChildren<ConversationManager>();

        var weapon = weaponDefinition.GetWeaponByWeaponName("바람의 주먹");
        Player = GameObject.FindGameObjectWithTag("Player")?.GetComponentInParent<Player>();
        Player?.EquipWeapon(weapon);
    }
    private void Update()
    {

    }

    public void PopupOptionPanel()
    {

    }
    public void SaveGameData()
    {
        string data = JsonUtility.ToJson(GameData);
        string path = Application.persistentDataPath + @"\Gamedata.json";

        using (StreamWriter writer = new StreamWriter(path, false, Encoding.UTF8))
        {
            writer.Write(data);

            writer.Close();
        }
    }
    public void LoadGameData()
    {
        try
        {
            using (StreamReader reader = new StreamReader(Application.persistentDataPath + @"\Gamedata.json", Encoding.UTF8))
            {
                string data = reader.ReadToEnd();
                GameData = JsonUtility.FromJson<GameData>(data);

                reader.Close();
                return;
            }
        }
        catch (FileNotFoundException)
        {
            GameData = new GameData();
            SaveGameData();
            LoadGameData();
            return;
        }
    }
}
