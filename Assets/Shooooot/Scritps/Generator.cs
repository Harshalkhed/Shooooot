using System;
using System.Collections;
using UnityEngine;
using Random = UnityEngine.Random;


public class Generator : MonoBehaviour
{

    public GameObject PF_obstacle_Rectangle;
    public GameObject PF_obstacle_Diamond;
    public GameObject PF_obstacle_Bomb;
    public GameObject PF_item_PlusOneBall;
    public GameObject obstacleParent;

    private int obsLife = 1;
    private bool[] oldObjects;
    private bool[] newObjects;
    private Vector3[] generatePosition;


    private void Start()
    {
        InitializePositionArrays();
        InitializeObjectArrays();
        StartCoroutine(AnimateObstaclesMovement());
    }

    // Initializes positions for obstacle generation
    private void InitializePositionArrays()
    {
        generatePosition = new Vector3[7];
        generatePosition[0] = new Vector3(-4.2f, 0, 9.5f);
        generatePosition[1] = new Vector3(-2.8f, 0, 9.5f);
        generatePosition[2] = new Vector3(-1.4f, 0, 9.5f);
        generatePosition[3] = new Vector3(-0f, 0, 9.5f);
        generatePosition[4] = new Vector3(1.4f, 0, 9.5f);
        generatePosition[5] = new Vector3(2.8f, 0, 9.5f);
        generatePosition[6] = new Vector3(4.2f, 0, 9.5f);
    }


    // Initializes boolean arrays to track state of old and new objects
    private void InitializeObjectArrays()
    {
        oldObjects = new bool[7];
        newObjects = new bool[7];
        Array.Fill(oldObjects, false);  // Fill the array with 'false' using Array.Fill for clarity
        Array.Fill(newObjects, false);
    }

    // Move down all obstacles
    public IEnumerator AnimateObstaclesMovement()
    {
        float distanceToNextPosition = 1.4f;
        float oldZ = obstacleParent.transform.localPosition.z;
        
        Vector3 startPosition = obstacleParent.transform.localPosition;
        Vector3 endPosition = new Vector3(startPosition.x, startPosition.y, startPosition.z - distanceToNextPosition);

        // Move the obstacle parent smoothly to the next position
        while (Vector3.Distance(obstacleParent.transform.localPosition, endPosition) > 0.1f)
        {
            obstacleParent.transform.localPosition = Vector3.MoveTowards(obstacleParent.transform.localPosition, endPosition, 0.1f);
            yield return null;
        }

        // Snap to the exact final position to avoid small discrepancies
        obstacleParent.transform.localPosition = endPosition;

        // Generate new obstacles after moving
        GenerateNewObstacle();
    }


    private void GenerateNewObstacle()
    {
        // Set all container for new obstacle object to false. 
        newObjects = new bool[7];
        
        for (int i = 0; i < newObjects.Length; i++) newObjects[i] = false;


        for (int i = 0; i < generatePosition.Length; i++)
        {
            
            // Index mapping for the array: 0 1 2 3 4 5 6
            // If the index is either 1 or 4, check the corresponding positions in the oldObjects array.
            // Create a Diamond obstacle only if the current and the next positions in the oldObjects array are both empty (false).

            // For example:
            // If the newObjects and oldObjects arrays look like this:
            // new: 0 1 2 3 4 5 6
            // old: T F F T T T T
            // ---> A Diamond obstacle will be created between indices 1 and 2.

            // Another example:
            // If the arrays are as follows:
            // new: 0 1 2 3 4 5 6
            // old: T T T T F F T
            // ---> A Diamond obstacle will be created between indices 4 and 5.
            
            // Check if the current index is 1 or 4 and both the current and the next slot in the oldObjects array are empty (false).
            if ((i == 1 || i == 4) && (oldObjects[i] == false && oldObjects[i + 1] == false))
            {
                // Determine the position for the diamond obstacle based on the index
                Vector3 pos;
                if (i == 1)
                {
                    pos = new Vector3(-2.1f, 0, 8.8f); // Position for the first special case
                } 
                else
                {
                    pos = new Vector3(2.1f, 0, 8.8f); // Position for the second special case
                } 

                // Generate the diamond obstacle at the determined position
                GenerateDiamondObstacle(pos);

                // Mark the current and the next indices as occupied in the newObjects array
                newObjects[i] = true;
                newObjects[i + 1] = true;
            }

            // There's a 60% chance to create an obstacle or a '+1 ball' item if the current slot is not already occupied.
            if (Random.Range(0, 100) < 60 && newObjects[i] == false)
            {
                GameObject newObj; // Variable to hold the new obstacle or item
                
                // Generate a random integer to decide which type of obstacle/item to create
                int randomInt = Random.Range(0, 100);
                
                // There's a 90% chance to create a normal rectangle obstacle.
                if (randomInt < 90)
                {
                    newObj = Instantiate(PF_obstacle_Rectangle, generatePosition[i], Quaternion.identity);
                    // Set the life of the new obstacle, ensuring it's at least 1 and no more than the current maximum obstacle life
                    int newObsLife = Mathf.Clamp(Random.Range((int)obsLife / 2, obsLife), 1, obsLife);
                    newObj.GetComponent<Obstacle>().life = newObsLife;
                }
                // There's an 8% chance to create a Bomb.
                else if (randomInt >= 85 && randomInt < 93)
                {
                    newObj = Instantiate(PF_obstacle_Bomb, generatePosition[i], Quaternion.identity);
                }
                // There's a 7% chance to create a '+1 ball' item.
                else
                {
                    newObj = Instantiate(PF_item_PlusOneBall, generatePosition[i], Quaternion.identity);
                }
                // Mark the current index as occupied
                newObjects[i] = true;
                // Set the parent of the new object to keep the hierarchy organized
                newObj.transform.SetParent(obstacleParent.transform);
            }
        }

        oldObjects = newObjects;

        // 50% chance : increase the life of the obstacle
        if (Random.Range(0, 100f) < 45f) obsLife++;
    }

    private void GenerateDiamondObstacle(Vector3 pos)
    {
        GameObject newDiamond = Instantiate(PF_obstacle_Diamond, pos, Quaternion.identity);
        newDiamond.transform.SetParent(obstacleParent.transform);
        int newObsLife = Random.Range(obsLife * 1, obsLife * 2);
        newDiamond.transform.GetChild(0).GetComponent<Obstacle>().life = newObsLife;
    }



}
