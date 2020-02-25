using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Dont confuse with the GameManagerment.SceneManager
/// </summary>
public abstract class SceneManagerClass : MonoBehaviour
{
    private void Awake()
    {
        GameManager.Instance.Player = GameObject.FindGameObjectWithTag("Player")?.GetComponentInParent<Player>();
    }
}
