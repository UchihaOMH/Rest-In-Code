using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FXControl : MonoBehaviour, IManagedObject
{
    public Transform Pool
    {
        get => pool;
        set => pool = value;
    }
    [SerializeField] private Transform pool;

    public SpriteRenderer spriteRenderer;
    public Animator animator;
    public AudioSource audioSource;

    private Action<Entity> callback;

    private void OnEnable()
    {
        Pool = GameManager.Instance.fxPoolManager.transform;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        var entity = collision.collider.gameObject.GetComponentInChildren<Entity>();

        if (entity != null)
            callback?.Invoke(entity);
    }

    public void Play(string _trigger, AudioClip _clip)
    {
        audioSource.clip = _clip;
        animator.SetTrigger(_trigger);
    }
    public void AddOnHitEvent(Action<Entity> _callback)
    {
        callback = _callback;
    }
    public void ReturnObject2Pool()
    {
        audioSource.clip = null;
        callback = null;
        spriteRenderer.sprite = null;
        transform.SetParent(Pool);
        gameObject.SetActive(false);
    }

    #region Animator Event
    private void PlayeSFX()
    {
        audioSource.Play();
    }
    private void OnAnimationEnd()
    {
        ReturnObject2Pool();
    }
    #endregion
}
