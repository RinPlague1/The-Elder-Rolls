using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    [Header("Follow Settings")]
    public Transform target;
    public float smoothSpeed = 0.125f;
    public Vector3 offset = new Vector3(0f, 10f, -5f);
    public float rotationAngle = 45f;

    [Header("Transition Settings")]
    public float transitionDuration = 0.5f;
    private bool isTransitioning = false;
    private Vector3 transitionStartPos;
    private Quaternion transitionStartRot;
    private float transitionTime;

    void LateUpdate()
    {
        if (target == null) return;

        if (isTransitioning)
        {
            // Smooth transition to new target
            transitionTime += Time.deltaTime;
            float t = Mathf.Clamp01(transitionTime / transitionDuration);

            Vector3 desiredPosition = target.position + offset;
            Quaternion desiredRotation = Quaternion.Euler(rotationAngle, 0f, 0f);

            transform.position = Vector3.Lerp(transitionStartPos, desiredPosition, t);
            transform.rotation = Quaternion.Slerp(transitionStartRot, desiredRotation, t);

            if (t >= 1f)
            {
                isTransitioning = false;
            }
        }
        else
        {
            // Normal follow
            Vector3 desiredPosition = target.position + offset;
            Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed);
            transform.position = smoothedPosition;

            Quaternion desiredRotation = Quaternion.Euler(rotationAngle, 0f, 0f);
            transform.rotation = Quaternion.Slerp(transform.rotation, desiredRotation, smoothSpeed);
        }
    }

    public void SetTarget(Transform newTarget)
    {
        if (target == newTarget) return;

        // Start transition
        transitionStartPos = transform.position;
        transitionStartRot = transform.rotation;
        transitionTime = 0f;
        isTransitioning = true;
        target = newTarget;
    }
}