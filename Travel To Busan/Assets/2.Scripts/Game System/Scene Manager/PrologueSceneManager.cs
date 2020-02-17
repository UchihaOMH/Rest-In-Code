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
            GameManager.Instance.fade.LoadScene(1);
            GameManager.Instance.enemyPoolManager.ReturnAllManagedEnemy();
        }
    }

    private void Start()
    {
        cinematic.StartCinematic();
    }
}
