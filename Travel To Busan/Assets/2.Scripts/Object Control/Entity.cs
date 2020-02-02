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
    public eEntityType entityType;
    public _EntityInfo_ info;
    
    public bool isDead = false;

    private void Update()
    {
        if (info.currHP <= 0f || isDead)
            OnDeadEvent();
    }

    public abstract void OnDeadEvent();
    public abstract void BeAttacked(float _damage);
    public abstract void KnockBack(Vector2 _knockBackDir, float _knockBackPower);
    public abstract void LookAt(Vector2 _dir);
    public abstract void TransitionProcess(IAnimState _state);
}
