using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraControl : MonoBehaviour
{
    public Camera cam;

    public Rect camBox;

    private Transform followTarget;
    private Rect camRect;

    private bool shaking = false;

    private void Awake()
    {
        followTarget = GameObject.FindGameObjectWithTag("Player").transform;

        cam.transform.position = followTarget.position + Vector3.back * 10;
        camRect = new Rect(Vector2.zero, new Vector2(cam.orthographicSize * cam.aspect * 2f, cam.orthographicSize * 2f));
        camRect.center = cam.transform.position;
    }
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(camBox.center, camBox.size);
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireCube(transform.position, new Vector2(cam.orthographicSize * cam.aspect * 2f, cam.orthographicSize * 2f));
    }
    private void LateUpdate()
    {
        if (!shaking)
        {
            FollowTarget();
        }
    }

    #region Camera Move
    public void ExplosionShake(float _power, float _duration)
    {
        StopCoroutine(ExplosionShakeCoroutine(_power, _duration));
        StartCoroutine(ExplosionShakeCoroutine(_power, _duration));
    }
    #endregion

    private void FollowTarget()
    {
        cam.transform.position = followTarget.position + Vector3.back * 10;
        camRect.center = cam.transform.position;
        LockUpCameraInRange();
    }
    private void LockUpCameraInRange()
    {
        Vector2 pos = Vector2.zero;

        if (camRect.xMin < camBox.xMin)
        {
            pos.x = camBox.xMin - camRect.xMin;
        }
        if (camRect.xMax > camBox.xMax)
        {
            pos.x = camBox.xMax - camRect.xMax;
        }
        if (camRect.yMin < camBox.yMin)
        {
            pos.y = camBox.yMin - camRect.yMin;
        }
        if (camRect.yMax > camBox.yMax)
        {
            pos.y = camBox.yMax - camRect.yMax;
        }

        cam.transform.Translate(pos);
        camRect.center = cam.transform.position;
    }

    #region Coroutine
    private IEnumerator ExplosionShakeCoroutine(float _power, float _duration)
    {
        float timer = Time.time + _duration;
        shaking = true;

        while (Time.time <= timer)
        {
            Vector3 pos = new Vector3(Random.insideUnitCircle.x * _power, Random.insideUnitCircle.y * _power, 0f) + new Vector3(followTarget.position.x, followTarget.position.y, cam.transform.position.z);

            cam.transform.position = pos;
            camRect.center = cam.transform.position;
            LockUpCameraInRange();
            yield return null;
        }

        shaking = false;
    }
    #endregion
}
