using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Glass : MonoBehaviour
{
    public ParticleSystem particle;
    public AudioSource audioSource;

    [Space(10f)] public Animator anim;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if ((collision.transform.position - transform.position).x < 0f)
        {
            anim.SetTrigger("tBreak");
            Camera.main.GetComponent<CameraControl>().ExplosionShake(1.2f, 0.7f);
            GetComponent<BoxCollider2D>().enabled = false;
        }
    }

    private void OnGlassBrokenEvent()
    {
        particle.Play();
        audioSource.volume = 0.175f;
        audioSource.Play();
    }
}
