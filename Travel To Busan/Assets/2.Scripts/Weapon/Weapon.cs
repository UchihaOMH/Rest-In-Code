using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct _WeaponOffset
{
    public Vector3 position;
    public Vector3 rotation;
    public Vector3 scale;
}
[System.Serializable]
public struct _AttackAudioClip_
{
    public AudioClip attack;
    public AudioClip skillAttack1;
    public AudioClip skillAttack2;
}
[System.Serializable]
public struct _WeaponInfo_
{
    public string name;
    public float damage;
    public float knockBackDist;
}

public abstract class Weapon : MonoBehaviour
{
    public Entity owner;

    public _WeaponOffset weaponOffset;
    public _WeaponInfo_ weaponInfo;
    public _AttackAudioClip_ audioClip;

    public abstract void Attack(float _additionalDmg);
    public abstract void RizingAttack(float _additionalDmg);
    public abstract void SmashAttack(float _additionalDmg);

    public FXControl GetInitializedFX(Transform _parent)
    {
        FXControl fx = GameManager.Instance.fxPoolManager.GetFXObject().GetComponent<FXControl>();
        fx.transform.SetParent(_parent);
        fx.transform.localPosition = Vector3.zero;
        fx.transform.localScale = Vector3.one;

        return fx;
    }
}
