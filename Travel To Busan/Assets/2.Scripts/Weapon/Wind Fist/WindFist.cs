using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WindFist : Weapon
{
    public override void Attack(float _additionalDmg)
    { 
        FXControl fx = GetInitializedFX((owner as Player).transform, fxOffset.attack);
        fx.AddOnHitEvent((Entity _target) =>
        {
            _target.BeAttacked((owner as Player).info.damage + weaponInfo.damage);
            _target.KnockBack((owner as Enemy).transform.position - (owner as Player).transform.position, weaponInfo.knockBackPower);
        });
        fx.Play(fxTrigger.attack, audioClip.attack);
    }
    public override void CmdAttack1(float _additionalDmg)
    {
        FXControl fx = GetInitializedFX((owner as Player).transform, fxOffset.cmdAttack1);
        fx.Play(fxTrigger.cmdAttack1, audioClip.cmdAttack1);
    }
    public override void CmdAttack2(float _additionalDmg)
    {
        FXControl fx = GetInitializedFX((owner as Player).transform, fxOffset.cmdAttack2);
        fx.Play(fxTrigger.cmdAttack2, audioClip.cmdAttack2);
    }
    public override void CmdAttack3(float _additionalDmg)
    {
        FXControl fx = GetInitializedFX((owner as Player).transform, fxOffset.cmdAttack3);
        fx.Play(fxTrigger.cmdAttack3, audioClip.cmdAttack3);
    }
}
