using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraManager : MonoBehaviour
{

    // original position of the camera
    private Vector3 originPos;


    private void Start()
    {
        // save the original position of the camera
        originPos = transform.localPosition;
    }


    // Method to initiate the shake effect. It is called from Obstacle.cs.
    public void StartShakeEffect(float intensity, float duration)
    {
        StartCoroutine(Shake(intensity, duration));
    }

    // Coroutine to shake the object's position.
    // It gradually decreases the shake intensity over the duration.
    private IEnumerator Shake(float intensity, float duration)
    {
        float elapsed = 0;  // Time elapsed since the start of the shake.

        while (elapsed < duration)
        {
            // Calculate the current intensity factor based on elapsed time.
            float intensityFactor = 1 - (elapsed / duration);

            // Set the new position with reduced intensity as time progresses.
            transform.localPosition = (Vector3)Random.insideUnitCircle * (intensity * intensityFactor) + originPos;

            // Increment the elapsed time.
            elapsed += Time.deltaTime;

            // Return control until the next frame.
            yield return null;
        }

        // Reset the position to the original after shaking ends.
        transform.localPosition = originPos;

    }










}
