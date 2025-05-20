using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//this is a singleton
//it manages the scene and game state - stuff like unlocks
//this doesnt need to be put onto any game object and if youre looking at this youre probably in the wrong place

public class GameManager : MonoBehaviour
{
    public enum relic
    {
        Chudworth,
        Dan,
        Fork,
        Plushie,
        Lobster,
        chair
    }

    private List<relic> relics = new List<relic>();

    private static GameManager _Instance;
    public static GameManager Instance
    {
        get
        {
            if (!_Instance)
            {
                _Instance = new GameObject().AddComponent<GameManager>();
                // name it for easy recognition
                _Instance.name = _Instance.GetType().ToString();
                // mark root as DontDestroyOnLoad();
                DontDestroyOnLoad(_Instance.gameObject);
            }
            return _Instance;
        }
    }

    private void Start()
    {
        Debug.Log("singleton created");
    }
    public void CollectRelic(relic _relic)
    {
        if (!relics.Contains(_relic))
        {
            //add relic to list of relics
            relics.Add(_relic);
            Debug.Log(_relic.ToString() + " collected");
        }
        else
        {
            Debug.Log(_relic.ToString() + " was already collected");
        }
    }

}