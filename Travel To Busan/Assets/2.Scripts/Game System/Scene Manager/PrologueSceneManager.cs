using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PrologueSceneManager : SceneManagerClass
{
    public PrologueCinematic cinematic;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            GameManager.Instance.Fade.LoadScene((int)GameManager.eScene.Level1);
            GameManager.Instance.EnemyPool.ReturnAllManagedEnemy();
        }
    }
    private void Start()
    {
        Camera.main.GetComponent<CameraControl>().ExplosionShake(1.5f, 1.5f);
        Invoke("StartCine", 3f);
    }
    private void StartCine()
    {
        GameManager.Instance.EnemyPool.PrepareEnemyInPool("Office Worker", 10);
        GameManager.Instance.Conversation.StartConversation(GameManager.Instance.ScriptFolderPath + "Prologue.txt", () =>
        {
            cinematic.StartCinematic();
        });
    }
}
