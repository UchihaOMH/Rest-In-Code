using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LogicTest : MonoBehaviour
{
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
            ScreenCapture.CaptureScreenshot(Application.persistentDataPath + "/ui.png");
    }
}
