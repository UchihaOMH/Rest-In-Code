using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Enemy : Entity
{
    [Header("Enemy")]
    public Vector2 wanderOrigin;
    public float wanderRadius = 2f;

    private void Awake()
    {
        gameObject.layer = LayerMask.GetMask(GameConst.LayerDefinition.enemy);
    }

    public abstract void FocusTarget(Entity _target);
}
