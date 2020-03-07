using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Level1SceneManager : SceneManagerClass
{
    [Space(15f)]
    public List<Transform> enemyPoint;

    private void OnLevelWasLoaded(int level)
    {
        GameManager.Instance.GameData.currLevel = level;
        GameManager.Instance.SaveGameData();
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") && AllEnemyDead())
            GameManager.Instance.Fade.LoadScene((int)GameManager.eScene.Level2, fadeCallback);
    }
    private void Start()
    {
        GameManager.Instance.Player.tr.position = startPoint.position;
        foreach (var point in enemyPoint)
            enemyList.Add(GameManager.Instance.EnemyPool.SpawnEnemy(point.position, "Office Worker").GetComponent<Enemy>());

        GameManager.Instance.Conversation.StartConversation(gameObject, "Script/Main/Level1", () =>
        {
            if (!GameManager.Instance.GameData.uiTutorialShown)
                GameManager.Instance.ShowTutorial();
        });
    }
}
