using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Btn_Functions : MonoBehaviour
{
    public Canvas Main;
    public Canvas Settings;
    void Open_Settings()
    {
        Main.enabled = false;
        Settings.enabled = true;
    }

}
