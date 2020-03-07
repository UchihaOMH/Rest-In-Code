using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Level4SceneManager : SceneManagerClass
{
    [Space(15f)]
    public List<Transform> officeWorkerPoint = new List<Transform>();
    public List<Transform> batManPoint = new List<Transform>();

    private void OnLevelWasLoaded(int level)
    {
        GameManager.Instance.GameData.currLevel = level;
        GameManager.Instance.SaveGameData();
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") && AllEnemyDead())
            GameManager.Instance.Fade.LoadScene((int)GameManager.eScene.Level5, fadeCallback);
    }
    private void Start()
    {
        GameManager.Instance.Player.rb.bodyType = RigidbodyType2D.Dynamic;
        GameManager.Instance.Player.tr.position = startPoint.position;

        foreach (var point in officeWorkerPoint)
        {
            var officeWorker = GameManager.Instance.EnemyPool.SpawnEnemy(point.position, "Office Worker");
            officeWorker.transform.position = point.position;
            enemyList.Add(officeWorker.GetComponent<Enemy>());
        }
        foreach (var point in batManPoint)
        {
            var batMan = GameManager.Instance.EnemyPool.SpawnEnemy(point.position, "Bat Man");
            batMan.transform.position = point.position;
            enemyList.Add(batMan.GetComponent<Enemy>());
        }

        GameManager.Instance.Conversation.StartConversation(gameObject, "Script/Main/Level4");
    }
}
