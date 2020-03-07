using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Dont confuse with the GameManagerment.SceneManager
/// </summary>
public abstract class SceneManagerClass : MonoBehaviour
{
    [Header("Scene Manager Class Field")]
    public Transform startPoint;

    protected List<Enemy> enemyList = new List<Enemy>();
    protected Action fadeCallback = () => GameManager.Instance.EnemyPool.ReturnAllManagedEnemy();

    public bool AllEnemyDead()
    {
        foreach (var enemy in enemyList)
        {
            if (enemy.gameObject.activeSelf && !enemy.isDead)
                return false;
        }
        return true;
    }
}
