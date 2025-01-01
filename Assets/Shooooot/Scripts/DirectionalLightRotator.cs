
using UnityEngine;

/*
 * This script rotates the directional light in the scene.
 * It is attached to the Directional Light GameObject.
 */
public class DirectionalLightRotator : MonoBehaviour
{
    public float rotationSpeed = 2f;  // Exposed rotation speed to adjust from the inspector

    private void FixedUpdate()
    {
        // Increment the Y axis rotation based on deltaTime and rotationSpeed
        transform.Rotate(0, Time.deltaTime * rotationSpeed, 0, Space.World);
    }

}
