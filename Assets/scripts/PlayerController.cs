using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float moveSpeed = 5f;

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out RaycastHit hit))
            {
                if (hit.collider.CompareTag("HexTile"))
                {
                   

                    StopAllCoroutines();
                    StartCoroutine(MoveToTile(hit.transform.position));
                }
                if (hit.collider.CompareTag("Barrier"))
                {
                    return; // Prevent movement onto ocean or mountain tiles
                }

            }
        }
    }

    System.Collections.IEnumerator MoveToTile(Vector3 targetPos)
    {
        while (Vector3.Distance(transform.position, targetPos) > 0.1f)
        {
            transform.position = Vector3.MoveTowards(transform.position, targetPos, moveSpeed * Time.deltaTime);
            yield return null;
        }
    }
}
