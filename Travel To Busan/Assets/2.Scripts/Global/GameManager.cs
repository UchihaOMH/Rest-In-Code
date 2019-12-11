using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    #region Property
    public GameManager Instance { get => instance; private set => instance = value; }
    public Option Option { get => option; private set => option = value; }
    #endregion

    #region Public Field
    #endregion

    #region Private Field
    private GameManager instance;
    private Option option;
    #endregion

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(this.gameObject);
        }
        else
            Destroy(this.gameObject);
    }

    private void Update()
    {
        if (Input.anyKeyDown)
            Fade.Instance.LoadScene("Prologue");
    }

    public void PopupOptionPanel()
    {

    }
}
