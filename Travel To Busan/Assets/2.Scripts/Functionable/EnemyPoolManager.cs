using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyPoolManager : MonoBehaviour
{
    public List<GameObject> enemyPrefList;

    private List<GameObject> pool = new List<GameObject>();

    public GameObject SpawnEnemy(Vector3 _pos, string _enemyName)
    {
        //  일치하는 이름을 가진 몬스터가 있는가
        foreach (var entityData in enemyPrefList)
        {
            if (entityData.GetComponent<Entity>().name == _enemyName)
            {
                //  풀에 비활성화 상태의 몬스터가 있으면 초기화 후, 리턴
                foreach (var objInPool in pool)
                {   
                    if (objInPool.GetComponent<Entity>().name == entityData.GetComponent<Entity>().name && !objInPool.activeSelf)
                    {
                        var enemy = objInPool.GetComponent<Enemy>();
                        enemy.gameObject.SetActive(true);
                        enemy.info.currHP = enemy.info.maxHP;
                        enemy.transform.position = _pos;
                        enemy.GetComponentInChildren<AnyPortrait.apPortrait>().Initialize();

                        return objInPool;
                    }
                }
                //  풀에 비활성화 상태의 몬스터가 없으면 생성 후, 리턴
                var newObj = Instantiate(entityData, transform);
                newObj.GetComponentInChildren<AnyPortrait.apPortrait>().Initialize();
                newObj.transform.position = _pos;
                pool.Add(newObj);
                return newObj;
            }
        }

        return null;
    }

    public void ReturnAllManagedEnemy()
    {
        var objs = transform.GetComponentsInChildren<IManagedObject>();
        for (int i = 0; i < objs.Length; i++)
        {
            objs[i].ReturnObject2Pool();
        }
    }
}
