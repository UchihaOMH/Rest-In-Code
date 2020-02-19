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
    private Transform pool;

    public SpriteRenderer spriteRenderer;
    public Animator animator;
    public AudioSource audioSource;

    private Action<Entity> callback;

    private bool vfxEnd = false;
    private bool sfxEnd = false;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        var entity = collision.gameObject.GetComponentInChildren<Entity>();

        if (entity != null)
            callback?.Invoke(entity);
    }
    private void Update()
    {
        if (sfxEnd && vfxEnd)
            ReturnObject2Pool();
        else if (vfxEnd)
            CheckSfxIsPlaying();
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
        ResetObjectForPooling();
        transform.SetParent(Pool);
        gameObject.SetActive(false);
    }
    public void ResetObjectForPooling()
    {
        audioSource.clip = null;
        callback = null;
        spriteRenderer.sprite = null;

        vfxEnd = false;
        sfxEnd = false;
    }

    #region Animator Event
    private void PlayeSFX()
    {
        audioSource.Play();
    }
    private void OnAnimationEnd()
    {
        vfxEnd = true;
    }
    #endregion

    private void CheckSfxIsPlaying()
    {
        if (!audioSource.isPlaying)
            sfxEnd = true;
    }
}
