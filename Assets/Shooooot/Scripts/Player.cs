using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.Serialization;

public class Player : MonoBehaviour
{

    public static int returnedBallCount;

    public GameObject Prefab_Ball;
    public TextMesh ballCounterText;
    public GameObject aimLine;
    public Generator obstacleGenerator;
    [HideInInspector] public bool isShooting;
    [HideInInspector] public bool allBallsLaunched = false;

    private float dragDistance;
    private bool isDragging;
    private Vector2 touchStartPosition;
    private Vector2 dragPosition;
    private int countBalls = 1;


    // Initializes game settings at the start of the scene.
    private void Start()
    {
        InitializeGameControls();
        ResetBallCount();
        UpdateBallCounterDisplay();
        ActivateRotator();
    }

    // Sets initial states for user interaction controls.
    private void InitializeGameControls()
    {
        // Flags to control dragging and shooting actions
        isDragging = false;
        isShooting = false;
    }
    
    // Resets the count of balls returned to the starting point to zero.
    private void ResetBallCount()
    {
        returnedBallCount = 0;
    }
    
    // Updates the display of the current number of balls.
    private void UpdateBallCounterDisplay()
    {
        ballCounterText.text = countBalls.ToString();
    }
    
    // Activates the game object used for rotating the launcher or player view.
    private void ActivateRotator()
    {
        // Ensures the rotator is visible and active at game start
        aimLine.SetActive(true);
    }

    private void Update()
    {
        AimAndShoot();
    }

    // Resets the ball counter and prepares the game for a new round.
    public void ResetBallCounter()
    {
        aimLine.SetActive(true);  // Activate the line of sight indicator
        aimLine.transform.rotation = Quaternion.Euler(0, 0, 0); // Reset the rotation of the line of sight indicator

        isShooting = false; // Set shooting status to false
        
        countBalls = returnedBallCount; // Update the count of balls with the number of returned balls
        ballCounterText.text = countBalls.ToString(); // Display the updated count of balls

        returnedBallCount = 0; // Reset the count of returned balls
    }
    
    
    // Handles user input for aiming and shooting.
    private void AimAndShoot()
    {
        
        HandleTouchStart();
        HandleDragging();
        HandleTouchEnd();
        
    }
    
    // Detects the start of a touch and initiates dragging if conditions are met.
    private void HandleTouchStart()
    {
        // Ensures that a touch begins only if not already shooting or touching a UI object.
        if (Input.GetMouseButtonDown(0) && !isShooting && !IsPointerOverUIObject())
        {
            isDragging = true;
            touchStartPosition = Input.mousePosition;  // Records the start position of the touch.
        }
    }

    // Manages the dragging behavior for aiming.
    private void HandleDragging()
    {
        if (isDragging)
        {
            dragPosition = Input.mousePosition;
            float dragOffset = dragPosition.x - touchStartPosition.x;
            dragDistance = -dragOffset / 3f;
            dragDistance = Mathf.Clamp(dragDistance, -70, 70);  // Clamps the drag distance to limit the aim rotation.

            // Applies the calculated rotation to the aimLine based on the drag distance.
            aimLine.transform.rotation = Quaternion.Euler(0, dragDistance, 0);
        }
    }

    // Executes when the touch ends and triggers the shooting mechanism.
    private void HandleTouchEnd()
    {
        if (isDragging && Input.GetMouseButtonUp(0))
        {
            aimLine.SetActive(false);  // Hides the aiming line.

            isShooting = true;
            isDragging = false;

            StartCoroutine(ShootBall());  // Starts the coroutine to shoot the ball.
        }
    }


    // Coroutine for shooting balls at a regular interval until all balls are shot.
    private IEnumerator ShootBall()
    {
        allBallsLaunched = false; // Flag to indicate that shooting process has started but not completed.

        while (countBalls > 0)
        {
            yield return new WaitForSeconds(0.1f);  // Wait for a short interval between shots.
            
            // Instantiate a new ball at the current position.
            GameObject newBall = Instantiate(Prefab_Ball, transform.position + new Vector3(0, 0, 0), Quaternion.identity);
            
            // Set the velocity of the new ball based on the aim line rotation.
            Vector3 newAngle = Quaternion.AngleAxis(aimLine.transform.rotation.eulerAngles.y, Vector3.forward) * Vector3.right;
            newBall.GetComponent<Ball>().SetVelocity(new Vector3(newAngle.y * 20, 0, newAngle.x * 20));
            
            // Decrement the ball count and update the UI.
            countBalls--;
            ballCounterText.text = countBalls.ToString();
        }

        allBallsLaunched = true; // Set the flag indicating all balls have been launched.

        yield break; // Explicitly denote the end of the coroutine.
    }
    
    
    // Determines if the mouse pointer is over any UI element.
    private bool IsPointerOverUIObject()
    {
        // Create a new PointerEventData instance for the current EventSystem, using the current mouse position.
        PointerEventData eventDataCurrentPosition = new PointerEventData(EventSystem.current);
        eventDataCurrentPosition.position = new Vector2(Input.mousePosition.x, Input.mousePosition.y);
        
        // List to hold the results of the raycast.
        List<RaycastResult> results = new List<RaycastResult>();
        
        // Perform a raycast using the event data to determine if any UI elements are under the mouse position.
        EventSystem.current.RaycastAll(eventDataCurrentPosition, results);
        
        // Return true if any UI elements were found under the pointer, otherwise false.
        return results.Count > 0;
    }



}