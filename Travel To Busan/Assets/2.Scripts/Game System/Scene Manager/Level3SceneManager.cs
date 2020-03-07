using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Level3SceneManager : SceneManagerClass
{
    [Space(15f)]
    public List<Transform> officeWorkerPoint = new List<Transform>();
    public List<Transform> underArmourCollectorPoint = new List<Transform>();

    private void OnLevelWasLoaded(int level)
    {
        GameManager.Instance.GameData.currLevel = level;
        GameManager.Instance.SaveGameData();
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") && AllEnemyDead())
            GameManager.Instance.Fade.LoadScene((int)GameManager.eScene.Level4, fadeCallback);
    }
    private void Start()
    {
        GameManager.Instance.Player.rb.bodyType = RigidbodyType2D.Dynamic;
        GameManager.Instance.Player.tr.position = startPoint.position;

        foreach (var point in officeWorkerPoint)
        {
            Enemy enemy = GameManager.Instance.EnemyPool.SpawnEnemy(point.position, "Office Worker").GetComponent<Enemy>();
            enemyList.Add(enemy);
        }
        foreach (var point in underArmourCollectorPoint)
        {
            enemyList.Add(GameManager.Instance.EnemyPool.SpawnEnemy(point.position, "UnderArmour Collector").GetComponent<Enemy>());
        }

        GameManager.Instance.Conversation.StartConversation(gameObject, "Script/Main/Level3");
    }
}
