using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FXPoolManager : MonoBehaviour
{
    [SerializeField] private GameObject fxPref;
    [SerializeField] private GameObject hitFxPref;
    [SerializeField, Space(10f)] private List<GameObject> poolFxList = new List<GameObject>();
    [SerializeField, Space(10f)] private List<GameObject> poolHitFxList = new List<GameObject>();

    public GameObject GetFXObject()
    {
        foreach (var data in poolFxList)
        {
            if (!data.gameObject.activeSelf)
            {
                data.gameObject.SetActive(true);
                return data;
            }
        }
        GameObject newFx = Instantiate(fxPref, this.gameObject.transform);
        newFx.GetComponent<FXControl>().Pool = transform;
        poolFxList.Add(newFx);
        return newFx;
    }
    public GameObject GetHitFXObject()
    {
        foreach (var data in poolHitFxList)
        {
            if (!data.gameObject.activeSelf)
            {
                data.gameObject.SetActive(true);
                return data;
            }
        }
        GameObject newHitFx = Instantiate(hitFxPref, this.gameObject.transform);
        newHitFx.GetComponent<HitFXControl>().Pool = transform;
        poolHitFxList.Add(newHitFx);
        return newHitFx;
    }
}
