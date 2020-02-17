using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Level1SceneManager : MonoBehaviour
{
    public List<Enemy> enemyList;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            //  모든 적이 죽어있으면 씬 로드
            foreach (var enemy in enemyList)
            {
                if (!enemy.isDead)
                {
                    return;
                }
            }
            GameManager.Instance.fade.LoadScene(2);
        }
    }

    private void Update()
    {
        if (enemyList[0].isDead && !enemyList[0].gameObject.activeSelf)
        {
            enemyList[0].isDead = false;
            enemyList[0].gameObject.SetActive(true);
        }
    }
}
