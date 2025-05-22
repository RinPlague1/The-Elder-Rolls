using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HoldWeaponScript : MonoBehaviour
{
    /* USE THE PREFAB GANG "WEAPON HOLD MANAGER"
     * (IT SAVES YOU A LOT OF EFFORT FR FR)
     */
    [System.Serializable]
    public enum Weapons
    {
        Longsword,
        Hammer,
        Bow
    }

    [Header("Get Model and Offset")]
    public GameObject[] WeaponModels;
    public Vector3[] Offsets;
    public Vector3[] Rotations;

    [SerializeField]
    private Dictionary<Weapons, (GameObject, Vector3, Vector3)> WeaponHoldInfo = new();

    private void Start()
    {
        WeaponHoldInfo.Add(Weapons.Longsword, (WeaponModels[0],Offsets[0],Rotations[0]));
        WeaponHoldInfo.Add(Weapons.Hammer, (WeaponModels[1], Offsets[1], Rotations[1]));
        WeaponHoldInfo.Add(Weapons.Bow, (WeaponModels[2], Offsets[2], Rotations[2]));
    }

    private void RemoveHeldItem(GameObject Object)
    {
        if (Object.transform.childCount < 1) return;
    }

    private void AddWeaponToObject(GameObject Object, Weapons Weapon)
    {
        GameObject NewHeldWeapon = Instantiate(WeaponHoldInfo[Weapon].Item1, Object.transform);
        NewHeldWeapon.transform.localPosition = WeaponHoldInfo[Weapon].Item2;
        NewHeldWeapon.transform.localEulerAngles = WeaponHoldInfo[Weapon].Item3;
    }

    public void SetNewHeldWeapon(GameObject parent, Weapons Weapon)
    {
        RemoveHeldItem(parent);
        AddWeaponToObject(parent,Weapon);
    }
}
