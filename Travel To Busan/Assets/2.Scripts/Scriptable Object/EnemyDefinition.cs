using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Enemy Difinition", menuName = "Scriptable Object/Enemy Difinition", order = int.MaxValue)]
public class EnemyDefinition : ScriptableObject
{
    public List<GameObject> enemyPrefebList;

    public GameObject GetEnemyByName(string _name)
    {
        foreach (var enemy in enemyPrefebList)
        {
            if (enemy.GetComponent<Enemy>()?.info.name == _name)
                return enemy;
        }
        Debug.LogError("Not Found");
        return null;
    }
}
