using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitFXControl : MonoBehaviour, IManagedObject
{
    public List<ParticleSystem> particleList = new List<ParticleSystem>();

    private AudioSource audioSource;

    public Transform Pool
    {
        get => pool;
        set => pool = value;
    }
    private Transform pool;

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
        Pool = GameObject.FindGameObjectWithTag("FX Pool").transform;
    }
    private void Update()
    {
        foreach (var particle in particleList)
        {
            if (!particle.isPlaying && !audioSource.isPlaying)
                ReturnObject2Pool();
        }
    }

    public void ResetObjectForPooling()
    {
        
    }
    public void ReturnObject2Pool()
    {
        transform.SetParent(Pool);
        gameObject.SetActive(false);
    }
}
