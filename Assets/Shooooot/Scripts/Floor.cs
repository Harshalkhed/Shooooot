using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Floor : MonoBehaviour
{
    
    private void OnTriggerExit(Collider other)
    {
        // if the ball exits the game area, remove it
        if (other.gameObject.CompareTag("Ball"))
        {
            other.gameObject.GetComponent<Ball>().RemoveBall();
        }
    }


}
