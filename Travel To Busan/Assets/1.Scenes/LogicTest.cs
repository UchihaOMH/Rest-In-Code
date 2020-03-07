using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LogicTest : MonoBehaviour
{
    public Rigidbody2D player;
    public float power = 10f;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.A))
            player.AddForce(Vector2.left * power, ForceMode2D.Impulse);
        else if (Input.GetKeyDown(KeyCode.D))
            player.AddForce(Vector2.right * power, ForceMode2D.Impulse);
    }
}
