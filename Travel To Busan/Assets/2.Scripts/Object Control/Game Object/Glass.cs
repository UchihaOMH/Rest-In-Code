using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Glass : MonoBehaviour
{
    public ParticleSystem particle;
    public AudioSource audioSource;

    [Space(10f)] public Animator anim;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.GetContact(0).normal.x > 0f)
        {
            anim.SetTrigger("tBreak");
            Camera.main.GetComponent<CameraControl>().ExplosionShake(1.2f, 0.7f);
        }
    }

    private void OnGlassBrokenEvent()
    {
        particle.Play();
        audioSource.volume = 0.175f;
        audioSource.Play();
    }
}
