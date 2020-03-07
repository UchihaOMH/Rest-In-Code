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
    [SerializeField] public string name;

    [SerializeField] public float damage;
    [SerializeField] public float defense;

    [SerializeField] public float maxHP;
    [SerializeField] public float currHP;

    [SerializeField] public float speed;
    [SerializeField] public float jumpForce;
    [SerializeField] public float knockBackDist;
}

public abstract class Entity : MonoBehaviour
{
    [Header("Entity Info")]
    public eEntityType entityType;
    public _EntityInfo_ info;
    
    public bool isDead = false;

    public abstract void OnDeadEvent();
    public abstract void BeAttacked(Entity _attacker, float _damage, Vector2 _knockBackDir, float _knockBackDist, float _knockBackDuration = 0.3f);
    public abstract void LookAt(Vector2 _dir);
    public abstract void TransitionProcess(IAnimState _state);
}
