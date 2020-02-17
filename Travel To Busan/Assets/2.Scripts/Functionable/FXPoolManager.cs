using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FXPoolManager : MonoBehaviour
{
    [SerializeField] private GameObject fxPref;
    [SerializeField, Space(10f)] private List<GameObject> poolFXList = new List<GameObject>();

    public GameObject GetFXObject()
    {
        foreach (var data in poolFXList)
        {
            if (!data.gameObject.activeSelf)
            {
                data.gameObject.SetActive(true);
                return data;
            }
        }
        GameObject newFX = Instantiate(fxPref, this.gameObject.transform);
        newFX.GetComponent<FXControl>().Pool = transform;
        poolFXList.Add(newFX);
        return newFX;
    }
}
