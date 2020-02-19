using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WindFist : Weapon
{
    public override void Attack(float _additionalDmg)
    { 
        FXControl fx = GetInitializedFX(owner.transform);
        fx.AddOnHitEvent((Entity _target) =>
        {
            _target.BeAttacked(owner, owner.info.damage + weaponInfo.damage, _target.transform.position.x - owner.transform.position.x < 0f ? Vector2.left : Vector2.right, weaponInfo.knockBackDist);
        });
        fx.Play("Wind Fist Attack", audioClip.attack);
    }
    public override void RizingAttack(float _additionalDmg)
    {
        FXControl fx = GetInitializedFX(owner.transform);
        fx.AddOnHitEvent((Entity _target) =>
        {
            _target.BeAttacked(owner, owner.info.damage + weaponInfo.damage, _target.transform.position.x - owner.transform.position.x < 0f ? Vector2.left : Vector2.right, weaponInfo.knockBackDist);
            _target.GetComponent<Rigidbody2D>().AddForce(Vector2.up * 30f, ForceMode2D.Impulse);
        });
        fx.Play("Wind Fist Rizing Attack", audioClip.skillAttack1);
    }
    public override void SmashAttack(float _additionalDmg)
    {
        FXControl fx = GetInitializedFX(owner.transform);
        fx.AddOnHitEvent((Entity _target) =>
        {

        });
        fx.Play("Wind Fist Smash", audioClip.skillAttack2);
    }
}
