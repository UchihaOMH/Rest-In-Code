using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// 게임플로우에 있어서 상위계층에 있는 클래스
/// UI조작시 관련 기능 수행이라거나
/// 씬 전환이라거나
/// 추상적이고 포괄적인 클래스
/// </summary>
public class GameManager : MonoBehaviour
{
    public static GameManager Instance
    {
        get => instance;
        private set => instance = value;
    }
    private static GameManager instance;

    private void Awake()
    {
        //  싱글톤 구현
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else if (instance != this)
            Destroy(gameObject);
    }
}
