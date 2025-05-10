using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Combat_Turn_Order : MonoBehaviour
{
    public List<GameObject> Turn_Order = new List<GameObject>();
    public List<GameObject> Enemies = new List<GameObject>();


    public void Start()
    {
        Scene Active_Scene = SceneManager.GetActiveScene();
        foreach (GameObject Entity in Active_Scene.GetRootGameObjects())
        {
            if( Entity.CompareTag("Combat_Enemy"))
            {
                Enemies.Add(Entity);
                if (Entity.CompareTag("Combat_Player"))
                {
                    Turn_Order.Add(Entity);    
                }
            }
        }
    }
    public GameObject Next_Turn(GameObject Current_Turn)
    {
        for (int i = 0; i < Turn_Order.Count; i++)
        {
            if (Current_Turn = Turn_Order[i])
            { return Turn_Order[i + 1 % Turn_Order.Count]; }
        }
        Debug.LogError("$ Next Turn Entity NOT Found");
        return null;
    }
}
