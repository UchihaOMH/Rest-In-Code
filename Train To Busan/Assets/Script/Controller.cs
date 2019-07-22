using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Controller : MonoBehaviour
{
    public Transform doll;
    public Animator animator;

    public float runSpeed = 1.0f;

#if UNITY_EDITOR
    private void Awake()
    {
        doll = gameObject.GetComponent<Transform>();
        animator = gameObject.GetComponent<Animator>();
    }

    private void Update()
    {
        if (Input.GetKey)
    }
#endif

#if UNITY_ANDROID || UNITY_IPHONE

#endif
}
