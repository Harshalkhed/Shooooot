using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartArea : MonoBehaviour
{

    private GameManager gameManager;

    private void Start()
    {
        gameManager = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>(); ;
    }

    // This method is called when another collider enters this game object's trigger collider
    private void OnTriggerEnter(Collider other)
    {
        // Check if the collider belongs to an object tagged as "Diamond" or "Rectangle"
        if (other.gameObject.CompareTag("Diamond") || other.gameObject.CompareTag("Rectangle"))
        {
            // Trigger the GameOver function in the game manager if collision is with specified tags
            gameManager.GameOver();
        }
    }


}
