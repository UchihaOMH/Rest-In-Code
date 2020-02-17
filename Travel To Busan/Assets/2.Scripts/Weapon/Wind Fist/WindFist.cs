using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WindFist : Weapon
{
    public override void Attack(float _additionalDmg)
    { 
        FXControl fx = GetInitializedFX(owner.transform, fxOffset.attack);
        fx.AddOnHitEvent((Entity _target) =>
        {
            _target.BeAttacked(owner, owner.info.damage + weaponInfo.damage, _target.transform.position.x - owner.transform.position.x < 0f ? Vector2.left : Vector2.right, weaponInfo.knockBackDist);
        });
        fx.transform.localScale = Vector3.one;
        fx.Play(fxTrigger.attack, audioClip.attack);
    }
    public override void SkillAttack1(float _additionalDmg)
    {
        FXControl fx = GetInitializedFX(owner.transform, fxOffset.cmdAttack1);
        fx.AddOnHitEvent((Entity _target) =>
        {
            
        });
        fx.Play(fxTrigger.cmdAttack1, audioClip.cmdAttack1);
    }
    public override void SkillAttack2(float _additionalDmg)
    {
        FXControl fx = GetInitializedFX(owner.transform, fxOffset.cmdAttack2);
        fx.AddOnHitEvent((Entity _target) =>
        {

        });
        fx.Play(fxTrigger.cmdAttack2, audioClip.cmdAttack2);
    }
    public override void SkillAttack3(float _additionalDmg)
    {
        FXControl fx = GetInitializedFX(owner.transform, fxOffset.cmdAttack3);
        fx.AddOnHitEvent((Entity _target) =>
        {

        });
        fx.Play(fxTrigger.cmdAttack3, audioClip.cmdAttack3);
    }
}
