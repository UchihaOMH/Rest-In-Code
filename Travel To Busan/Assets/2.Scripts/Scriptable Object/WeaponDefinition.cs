using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Weapon Kind Definition", menuName = "Scriptable Object/Weapon Kind Definition", order = int.MaxValue)]
public class WeaponDefinition : ScriptableObject
{
    [SerializeField] private List<GameObject> weaponPrefebList;

    public GameObject GetWeaponByWeaponName(string _weaponName)
    {
        foreach (var data in weaponPrefebList)
        {
            if (data.GetComponent<Weapon>()?.weaponInfo.name == _weaponName)
                return Instantiate(data);
        }

        return null;
    }
}
