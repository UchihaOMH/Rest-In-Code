using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PrologueSceneManager : MonoBehaviour
{
    public PrologueCinematic cinematic;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            GameManager.Instance.fade.LoadScene((int)GameManager.eScene.Level1);
            GameManager.Instance.enemyPoolManager.ReturnAllManagedEnemy();
        }
    }
    private void Awake()
    {
        GameManager.Instance.enemyPoolManager.PrepareEnemyInPool("Office Worker", 10);
    }
    private void Start()
    {
        cinematic.StartCinematic();
    }
}
