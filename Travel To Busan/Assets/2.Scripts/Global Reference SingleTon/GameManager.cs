using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    #region Property
    public static GameManager Instance
    {
        get => instance;
        private set => instance = value; 
    }
    private static GameManager instance;
    #endregion

    #region Public Field
    [HideInInspector] public Player player;
    [HideInInspector] public FXPoolManager fxPoolManager;
    [HideInInspector] public EnemyPoolManager enemyPoolManager;
    [HideInInspector] public DamageCalculator damageCalculator;
    [HideInInspector] public Fade fade;
    #endregion

    #region Private Field
    [SerializeField] private WeaponDefinition weaponDefinition;
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

        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
        fxPoolManager = GetComponentInChildren<FXPoolManager>();
        enemyPoolManager = GetComponentInChildren<EnemyPoolManager>();
        damageCalculator = GetComponentInChildren<DamageCalculator>();
        fade = GetComponentInChildren<Fade>();

        var weapon = weaponDefinition.GetWeaponByWeaponName("바람의 주먹");
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
        player.EquipWeapon(weapon);
    }
    private void Update()
    {

    }

    public void PopupOptionPanel()
    {

    }
}
