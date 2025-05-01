using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class Open_Inventory : MonoBehaviour
{
    public GameObject Map;
    public GameObject Inventory;
    public bool Checker = true;
    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.I))
        {
            if (Checker == true)
            {
                Map.SetActive(false);
                Inventory.SetActive(true);
                Checker = false;
            }
            else
            {
                Map.SetActive(true);
                Inventory.SetActive(false);
                Checker = true;
            }
        }
    }
}