using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Level5SceneManager : SceneManagerClass
{
    public EndCinematic endCinematic;

    private void OnLevelWasLoaded(int level)
    {
        GameManager.Instance.GameData.currLevel = level;
        GameManager.Instance.SaveGameData();
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {

    }
    private void Start()
    {
        GameManager.Instance.Player.rb.bodyType = RigidbodyType2D.Dynamic;
        GameManager.Instance.Player.tr.position = startPoint.position;
    }
}
