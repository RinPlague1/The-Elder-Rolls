using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HoldWeaponScript : MonoBehaviour
{
    /* To use this make an empty with it. 
     * Drag weapons in order from 
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
    public int[] Z_Offset;
    

    public void AddWeaponToObject(GameObject Object, Weapons Weapon)
    {
        switch (Weapon)
        {
            case Weapons.Longsword:
                Instantiate(WeaponModels[0],Object.transform.position + new Vector3(0,0,Z_Offset[0]),Object.transform.rotation, Object.transform); 
                break;
            case Weapons.Hammer:
                Instantiate(WeaponModels[1], Object.transform.position + new Vector3(0, 0, Z_Offset[1]), Object.transform.rotation, Object.transform);
                break;
            case Weapons.Bow:
                Instantiate(WeaponModels[2], Object.transform.position + new Vector3(0, 0, Z_Offset[2]), Object.transform.rotation, Object.transform);
                break;
            default:
                Debug.Log("No weapon in enum");
                break;
        }
    }

}
