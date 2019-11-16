using System.Collections;
using System.Collections.Generic;

using UnityEngine;

public class Controller : MonoBehaviour
{
    #region Public Fields
    public PlayerState targetState;
    #endregion

    #region Private Fields
    #endregion

    #region Public Property
    #endregion

    #region Mono Methods
    private void Awake()
    {
        if (targetState == null)
            Debug.LogError("This component requires 'PlayerState' component.");
    }

    private void Update()
    {
        float horAxis = Input.GetAxis("Horizontal");
        float verAxis = Input.GetAxis("Vertical");

        if (horAxis > 0f)
            targetState.Run(new Vector3(1, 0, 0));
        else if (horAxis < 0f)
            targetState.Run(new Vector3(-1, 0, 0));
    }
    #endregion
}
