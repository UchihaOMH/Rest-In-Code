using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Camera_Follow : MonoBehaviour
{
    public Transform camTr;
    public Transform target;

    private void LateUpdate()
    {
        camTr.position = new Vector3(target.position.x, camTr.position.y, camTr.position.z);
    }
}
