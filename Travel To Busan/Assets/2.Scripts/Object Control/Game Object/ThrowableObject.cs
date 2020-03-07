using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThrowableObject : MonoBehaviour, IManagedObject
{
    public float damage = 20f;
    public float knockBackDist = 0.4f;
    public float knockBackDuration = 0.3f;
    public float speed = 0f;
    public float rotationDamp = 0f;
    public float lifeTime = 5f;

    private Vector2 dir;

    private bool onThrow = false;

    public Transform Pool
    {
        get => pool;
        set => pool = value;
    }
    private Transform pool;

    private float timer = 0f;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.GetComponent<Player>() != null)
        {
            collision.gameObject.GetComponent<Player>().BeAttacked(null, damage, collision.gameObject.transform.position - transform.position, knockBackDist, knockBackDuration);
            ReturnObject2Pool();
        }
        else if (LayerMask.LayerToName(collision.gameObject.layer) == GameConst.LayerDefinition.level)
            ReturnObject2Pool();
    }
    private void Update()
    {
        if (onThrow)
        {
            if (Time.time >= timer)
                ReturnObject2Pool();

            transform.Translate(dir * speed * Time.deltaTime, Space.World);
            transform.Rotate(Vector3.forward, rotationDamp * Time.deltaTime);
        }
    }

    public void ThrowTo(Vector2 _dir)
    {
        dir = _dir.normalized;
        timer = Time.time + lifeTime;
        onThrow = true;
    }

    public void ReturnObject2Pool()
    {
        if (Pool != null)
        {
            ResetObjectForPooling();
            transform.SetParent(Pool);
            gameObject.SetActive(false);
            transform.localPosition = Vector3.zero;
        }
        else
            Debug.LogError("Pool Reference is null");
    }
    public void ResetObjectForPooling()
    {
        dir = Vector2.zero;
        onThrow = false;
        timer = 0f;
    }
}
