using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VideoControl : MonoBehaviour
{
    public GameObject VideoEnabled;
    public float TimeSinceLastInput = 0;

    private void Start()
    {
        StartCoroutine(SecondCounter());
    }

    private void Update()
    {
        if (Input.anyKeyDown || Input.GetAxis("Horizontal") > 0 || Input.GetAxis("Vertical") > 0){
            TimeSinceLastInput = 0;
        }

        if (TimeSinceLastInput > 30) { VideoEnabled.SetActive(true); }
        else { VideoEnabled.SetActive(false); }
        
    }

    private IEnumerator SecondCounter()
    {
        while (true)
        {
            yield return new WaitForSeconds(1);
            TimeSinceLastInput += 1;
        }
    }
}
