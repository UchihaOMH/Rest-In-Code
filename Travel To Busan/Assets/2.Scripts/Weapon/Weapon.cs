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
public struct _FXOffset
{
    public Vector3 attack;
    public Vector3 cmdAttack1;
    public Vector3 cmdAttack2;
    public Vector3 cmdAttack3;
}
[System.Serializable]
public struct _AttackAudioClip_
{
    public AudioClip attack;
    public AudioClip cmdAttack1;
    public AudioClip cmdAttack2;
    public AudioClip cmdAttack3;
}
[System.Serializable]
public struct _AttackAnimationTrigger_
{
    public string attack;
    public string cmdAttack1;
    public string cmdAttack2;
    public string cmdAttack3;
}
[System.Serializable]
public struct _WeaponInfo_
{
    public string name;
    public float damage;
    public float knockBackPower;
}

public abstract class Weapon : MonoBehaviour
{
    public Entity owner;

    public _WeaponOffset weaponOffset;
    public _FXOffset fxOffset;
    public _WeaponInfo_ weaponInfo;
    public _AttackAnimationTrigger_ fxTrigger; 
    public _AttackAudioClip_ audioClip;

    public abstract void Attack(float _additionalDmg);
    public abstract void CmdAttack1(float _additionalDmg);
    public abstract void CmdAttack2(float _additionalDmg);
    public abstract void CmdAttack3(float _additionalDmg);

    public FXControl GetInitializedFX(Transform _parent, Vector3 offset)
    {
        FXControl fx = GameManager.Instance.fxPoolManager.GetFXObject().GetComponent<FXControl>();
        fx.transform.SetParent(_parent);
        fx.transform.rotation = _parent.rotation;
        fx.transform.localPosition = fxOffset.attack;

        return fx;
    }
}
