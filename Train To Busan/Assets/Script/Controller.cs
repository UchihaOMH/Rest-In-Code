using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Controller : MonoBehaviour
{
    public Transform doll;
    public Rigidbody2D rig;
    public Animator animator;

    [Header("Controll"), Tooltip("Move Pixel Per Sec")]
    public float moveSpeed = 1.0f;
    public float jumpHeight = 50.0f;

    private bool isStep = false;

    private Quaternion rot;

    private void Awake()
    {
        doll = GetComponent<Transform>();
        rig = GetComponent<Rigidbody2D>();
    }

    private void Start()
    {
        rot = Quaternion.Euler(0, 180, 0);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.gameObject.layer == LayerMask.NameToLayer("Terrain"))
            isStep = true;
    }

#if UNITY_EDITOR
    private void Update()
    {
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");
        Vector2 dir = Vector2.zero;

        if (horizontal > 0)
        {
            dir.x = moveSpeed;
            rot = Quaternion.Euler(0, 180, 0);
        }
        else if (horizontal < 0)
        {
            dir.x = -moveSpeed;
            rot = Quaternion.Euler(0, 0, 0);
        }

        if (vertical > 0 && isStep)
        {
            rig.AddForce(new Vector2(0, jumpHeight), ForceMode2D.Impulse);
            isStep = false;
        }

        doll.Translate(new Vector3(dir.x, dir.y, 0), Space.World);
        doll.rotation = rot;
    }
#endif

#if UNITY_ANDROID || UNITY_IPHONE

#endif
}
