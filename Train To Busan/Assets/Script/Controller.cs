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
        if (Input.GetKey(KeyCode.W))
        {
            doll.Translate(Vector3.up * runSpeed);
        }
        else if (Input.GetKey(KeyCode.S))
        {
            doll.Translate(Vector3.up * -runSpeed);
        }
        if (Input.GetKey(KeyCode.A))
        {
            doll.Translate(Vector3.right * runSpeed);
        }
        else if (Input.GetKey(KeyCode.D))
        {
            doll.Translate(Vector3.right * -runSpeed);
        }
    }
#endif

#if UNITY_ANDROID || UNITY_IPHONE

#endif
}
