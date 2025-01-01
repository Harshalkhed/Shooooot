using System.Collections;
using UnityEngine;
using UnityEngine.UI;
public class Obstacle : MonoBehaviour
{

    public GameObject FX_Bomb_Explosion;
    public GameObject PF_DestroyEffect;
    public GameObject PF_DestroyEffect2;
    public TextMesh lifeCounter;
    [HideInInspector] public int life = 1;

    private Color originalObstacleColor;
    private Color originalTextColor;
    private AudioManager audioManager;
    private GameManager gameManager;
    private bool isAlreadyDeadEffect = false;


    private void Start()
    {
        InitializeManagers();
        
        SetInitialObstacleColor();
        
        InitializeObstacleUI();
        
    }
    
    
    private void InitializeManagers()
    {
        // Retrieve the AudioManager and GameManager from the GameManager object
        GameObject gameManagerObject = GameObject.FindWithTag("GameManager");
        audioManager = gameManagerObject.GetComponent<AudioManager>();
        gameManager = gameManagerObject.GetComponent<GameManager>();
    }
    
    private void SetInitialObstacleColor()
    {
        // Store the original color of the obstacle
        originalObstacleColor = gameObject.GetComponent<Renderer>().material.color;
    }
    
    private void InitializeObstacleUI()
    {
        // If the GameObject is not a Bomb, set up its UI elements
        if (!gameObject.CompareTag("Bomb"))
        {
            // Set the life counter text to the obstacle's life value
            lifeCounter.text = life.ToString();
            // Store the original text color from the first child's TextMesh component
            originalTextColor = gameObject.transform.GetChild(0).GetComponent<TextMesh>().color;
        }
    }

    // Handle the hit event when the obstacle is hit by the ball
    public void HandleHit(int damage)
    {
        ApplyDamage(damage);

        UpdateLifeCounter();

        IncreaseTotalScore(damage);
        
        HandleObstacleDestruction();
    }
    
    
    private void ApplyDamage(int damage)
    {
        // Reduce the life of this obstacle by the damage amount
        life -= damage;
    }
    
    private void UpdateLifeCounter()
    {
        // Update the UI life counter if it exists
        if (lifeCounter != null)
        {
            lifeCounter.text = life.ToString();
        }
    }
    
    private void IncreaseTotalScore(int damage)
    {
        // Add the damage value to the game's total score
        gameManager.AddScore(damage);
    }

    private void HandleObstacleDestruction()
    {
        // Check if the obstacle's life is zero or below and it hasn't been marked as dead yet
        if (life <= 0 && !isAlreadyDeadEffect)
        {
            isAlreadyDeadEffect = true;  // Mark this obstacle as having processed its death effect
            DeadEffect();  // Trigger the visual/audio effects associated with dying

            // Destroy the game object based on its tag
            if (gameObject.CompareTag("Diamond"))
                Destroy(transform.parent.gameObject);  // Destroy the parent object for diamonds
            else if (gameObject.CompareTag("Rectangle"))
                Destroy(gameObject);  // Destroy this object if it's a rectangle
            else if (gameObject.CompareTag("Bomb"))
                DestroyBomb();  // Call a special destruction method for bombs
        }
    }
    

    private void DeadEffect()
    {
        // Trigger camera shake with specific intensity and duration
        Camera.main.GetComponent<CameraManager>().StartShakeEffect(0.4f, 0.15f);
        // Play break obstacle audio if the audio setting is enabled
        if (PlayerPrefs.GetString("AudioSetting") == "On") {audioManager.PlayBreakObstacleAudio();}

        // Instantiate particle effects for the destruction
        GameObject newEffect = Instantiate(PF_DestroyEffect, transform.position, Quaternion.identity);
        newEffect.GetComponent<Renderer>().material.color = originalObstacleColor;
        Destroy(newEffect, 5f);
        
        GameObject newEffect2 = Instantiate(PF_DestroyEffect2, transform.position, Quaternion.identity);
        Destroy(newEffect2, 1f);
    }


    private void DestroyBomb()
    {
        // Trigger a stronger camera shake due to the bomb's explosion
        Camera.main.GetComponent<CameraManager>().StartShakeEffect(1f, 0.5f);
        // Play bomb detonation animation
        GetComponent<Animator>().SetTrigger("Bomb");
        // Instantiate explosion particle effect and schedule for destruction
        Destroy(Instantiate(FX_Bomb_Explosion, transform.position, Quaternion.identity), 0.5f);
        // Schedule destruction of this game object slightly after the animation plays
        Destroy(gameObject, 0.33f);
    }

    // Initiates color change when the obstacle is hit by the ball.
    public void ChangeColor()
    {
        StartCoroutine(ChangeColorCoroutine());
    }

    // Coroutine to flash the obstacle's color when hit, indicating interaction.
    private IEnumerator ChangeColorCoroutine()
    {
        
        // Temporarily change the color of the obstacle and its text to indicate it has been hit.
        SetObstacleColor(Color.black, Color.white);
        
        // Pause the coroutine for a brief moment.
        yield return new WaitForSecondsRealtime(0.05f);
        
        // Revert the color of the obstacle and its text to their original settings.
        ResetObstacleColor();

        yield break;
    }
    
    private void SetObstacleColor(Color obstacleColor, Color textColor)
    {
        // Set the color of the obstacle's material and the text mesh to specified colors.
        gameObject.GetComponent<Renderer>().material.color = obstacleColor;
        gameObject.transform.GetChild(0).GetComponent<TextMesh>().color = textColor;
    }
    
    private void ResetObstacleColor()
    {
        // Reset the color of the obstacle's material and the text mesh to their original colors.
        gameObject.GetComponent<Renderer>().material.color = originalObstacleColor;
        gameObject.transform.GetChild(0).GetComponent<TextMesh>().color = originalTextColor;
    }
    
    
    // Handles collision events when other objects interact with this obstacle.
    private void OnCollisionEnter(Collision other)
    {
        // Check if the collision is with an object tagged as "Bomb"
        if (other.gameObject.CompareTag("Bomb"))
        {
            // Handle the hit by applying damage equal to the obstacle's current life,
            // effectively destroying the obstacle immediately.
            HandleHit(life);
        }
    }


}
