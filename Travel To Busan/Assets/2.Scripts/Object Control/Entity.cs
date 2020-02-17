using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public enum eEntityType
{
    NPC,
    Enemy,
    Player,
    Obstacle
}
[System.Serializable]
public struct _EntityInfo_
{
    public string name;

    public float damage;
    public float defense;

    public float maxHP;
    public float currHP;

    public float speed;
}

public abstract class Entity : MonoBehaviour
{
    [Header("Entity Info")]
    public eEntityType entityType;
    public _EntityInfo_ info;
    
    public bool isDead = false;

    private void Update()
    {
        if (info.currHP <= 0f || isDead)
            OnDeadEvent();
    }

    public abstract void OnDeadEvent();
    public abstract void BeAttacked(Entity _attacker, float _damage, Vector2 _knockBackDir, float _knockBackDist, float _knockBackDuration = 0.3f);
    public abstract void LookAt(Vector2 _dir);
    public abstract void TransitionProcess(IAnimState _state);
}
