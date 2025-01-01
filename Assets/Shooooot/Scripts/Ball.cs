
using System;
using UnityEngine;
using Random = UnityEngine.Random;

public class Ball : MonoBehaviour
{
    public GameObject FX_collision;
    public GameObject FX_destroy;

    private Rigidbody rb;
    private GameManager gameManager;
    private AudioManager audioManager;
    private int counterWallCollision;

    private float velocityNormal = 20;
    private float velocityMax = 40;


    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    private void Start()
    {
        audioManager = GameObject.FindWithTag("GameManager").GetComponent<AudioManager>();
        gameManager = GameObject.FindWithTag("GameManager").GetComponent<GameManager>();
        
        counterWallCollision = 0;
        gameManager.IncreaseBallCounter();
    }


    private void Update()
    {
        NormalizeVelocity();
    }


    // Normalize Ball Velocity
    private void NormalizeVelocity()
    {
        // If the current normal velocity is less than the maximum velocity,
        // increase the normal velocity slightly.
        if (velocityNormal < velocityMax)
        {
            velocityNormal += 0.02f;
        }

        // Check if the Rigidbody's velocity magnitude is not equal to the normal velocity.
        // If it is not, adjust the Rigidbody's velocity to match the normal velocity.
        if (rb.velocity.magnitude < velocityNormal || rb.velocity.magnitude > velocityNormal)
        {
            var velocity = rb.velocity; // Store the current velocity of the Rigidbody.
            float speedAdjustmentFactor = velocityNormal / velocity.magnitude; // Calculate the factor to adjust the speed.
            velocity = velocity * speedAdjustmentFactor; // Apply the adjustment factor to the velocity.
            rb.velocity = velocity; // Update the Rigidbody's velocity to the adjusted velocity.
        }
    }

    // Set Velocity of this ball
    // This method is called by Player.cs when the ball is created.
    // * this method is called before the Start() method.
    public void SetVelocity(Vector3 velocity)
    {
        rb.velocity = velocity;
    }
    
    
    private void OnCollisionEnter(Collision other)
    {
        PlayCollisionAudio();
        
        CreateAndDestroyCollisionEffect(other); 
        
        HandleCollisionWithObstacles(other); 
        
        HandleCollisionWithWall(other); 
    }
    
    
    private void PlayCollisionAudio()
    {
        // if audio setting is on, play collision audio
        if (PlayerPrefs.GetString("AudioSetting") == "On")
        {
            audioManager.PlayCollisionAudio();
        }
    }
    
    private void CreateAndDestroyCollisionEffect(Collision other)
    {
        // Create Collision Effect and Destroy it after 1 second
        GameObject collisionEffect = Instantiate(FX_collision, other.contacts[0].point, Quaternion.identity);
        Destroy(collisionEffect, 1f);
    }
    
    private void HandleCollisionWithObstacles(Collision other)
    {
        // If the ball hits the obstacle, decrease the obstacle's health by 1 and change its color.
        if (other.gameObject.CompareTag("Rectangle") || other.gameObject.CompareTag("Diamond"))
        {
            Obstacle obstacle = other.gameObject.GetComponent<Obstacle>();
            obstacle.HandleHit(1);
            obstacle.ChangeColor();
        }
        // If the ball hits the bomb, decrease the bomb's health by 1.
        else if (other.gameObject.CompareTag("Bomb"))
        {
            other.gameObject.GetComponent<Obstacle>().HandleHit(1);
        }
    }
    
    
    private void HandleCollisionWithWall(Collision other)
    {
        // If the ball hits the wall, increase the counterWallCollision by 1.
        if (other.gameObject.CompareTag("Wall"))
        {
            counterWallCollision++;
            
            // If the counterWallCollision is greater than 10, push the ball to the starting point.
            if (counterWallCollision > 10)
            {
                rb.AddForce(new Vector3(0, 0, -700)); 
            }
        }
        else
        {
            // If the ball does not hit the wall, reset the counterWallCollision to 0.
            counterWallCollision = 0;
        }
    }


    private void OnTriggerEnter(Collider other)
    {
        // When hit "AddBall" Item
        if (other.gameObject.CompareTag("AddBall"))
        {
            // Create new ball at current position
            CreateNewBallAtCurrentPosition();
            
            // '+1' text effect
            other.GetComponent<PlusOneBall>().plusOneText();

            // Destroy the "AddBall" Item
            Destroy(other.gameObject);
        }
    }
    
    private void CreateNewBallAtCurrentPosition()
    {
        // Create new ball at current position
        GameObject newBall = Instantiate(gameObject, transform.position, Quaternion.identity);
        // Set Random Velocity to new ball
        Vector3 randomDirection = GetRandomDirection();
        newBall.GetComponent<Ball>().SetVelocity(new Vector3(randomDirection.y * 20, 0, randomDirection.x * 20));
    }
    
    private Vector3 GetRandomDirection()
    {
        // Get random rotation
        Quaternion randomRotation = Quaternion.AngleAxis(Random.Range(0, 360), Vector3.forward);
        return randomRotation * Vector3.right;
    }

    
    // this method is called by Floor.cs when the ball is out of the floor
    public void RemoveBall()
    {
        CreateDestroyEffect();  
        UpdateBallCount(); 
        DestroyCurrentBall(); 
    }
    
    
    // Create Destroy Effect and Destroy it after 1 second
    private void CreateDestroyEffect()
    {
        GameObject destroyEffect = Instantiate(FX_destroy, transform.position, Quaternion.identity);
        Destroy(destroyEffect, 1f);
    }
    
    // Update Ball Count
    private void UpdateBallCount()
    {
        Player.returnedBallCount++;
        gameManager.DecreaseBallCounter();
    }

    
    private void DestroyCurrentBall()
    {
        Destroy(gameObject);
    }



}