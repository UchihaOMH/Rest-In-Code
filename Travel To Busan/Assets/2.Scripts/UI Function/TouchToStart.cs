using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TouchToStart : MonoBehaviour
{
    public Fade fade;

    private void Update()
    {
#if UNITY_EDITOR
        if (Input.anyKeyDown)
        {

        }
#endif

#if UNITY_ANDROID
#endif
    }
}
