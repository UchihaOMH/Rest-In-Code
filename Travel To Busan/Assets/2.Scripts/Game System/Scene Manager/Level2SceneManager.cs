using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Level2SceneManager : SceneManagerClass
{
    [Space(15f)]
    public List<Transform> officeWorkerGenPoint = new List<Transform>();

    private void OnLevelWasLoaded(int level)
    {
        GameManager.Instance.GameData.currLevel = level;
        GameManager.Instance.SaveGameData();
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") && AllEnemyDead())
            GameManager.Instance.Fade.LoadScene((int)GameManager.eScene.Level3, fadeCallback);
    }
    private void Start()
    {
        GameManager.Instance.Player.rb.bodyType = RigidbodyType2D.Dynamic;
        GameManager.Instance.Player.tr.position = startPoint.position;

        foreach (var tr in officeWorkerGenPoint)
        {
            enemyList.Add(GameManager.Instance.EnemyPool.SpawnEnemy(tr.position, "Office Worker").GetComponent<Enemy>());
        }

        GameManager.Instance.Conversation.StartConversation(gameObject, "Script/Main/Level2");
    }
}
