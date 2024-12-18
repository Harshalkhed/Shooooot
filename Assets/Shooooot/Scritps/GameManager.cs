using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

// todo :  ui 따로 관리하도록 
public class GameManager : MonoBehaviour {

    // ui 부분 
    public GameObject gameOverPanel;
    public GameObject pausePanel;
    public Generator obstacleGenerator;
    
    public Text gameOverPanel_text_score;
    public Text gameOverPanel_text_BestScore;
    public Text text_score;

    public Text pausePanel_Score;
    public Text pausePanel_Best;

    public Toggle audioOnOff;
    private int totalBalls;
    private int currentScore;

    private Player player;


    private void Start()
    {
        InitializeSettings();
        InitializePlayer();
        InitializeUI();
        InitializeAudioSettings();
    }
    
    private void InitializeSettings()
    {
        // Set the target frame rate to 60 frames per second
        Application.targetFrameRate = 60;

        // Set the timescale to 1 (default setting)
        Time.timeScale = 1;
    }
    
    private void InitializePlayer()
    {
        // Retrieve the Player component from the player game object
        player = GameObject.FindWithTag("Player").GetComponent<Player>();
    }

    private void InitializeUI()
    {
        // Initialize score display
        text_score.text = "0";
    }
    
    private void InitializeAudioSettings()
    {
        // Attach ToggleValueChanged method to the audio toggle's onValueChanged event using lambda expression
        audioOnOff.onValueChanged.AddListener((value) => ToggleValueChanged(audioOnOff));

        // Set the audio toggle state based on saved preferences
        audioOnOff.isOn = PlayerPrefs.GetString("AudioSetting") == "On";
    }


    // Open Pause Panel
    public void OpenMenuPanel()
    {
        Time.timeScale = 0; // Stop Gameplay
        
        pausePanel_Score.text = currentScore.ToString(); // Set Score Text
        pausePanel_Best.text = PlayerPrefs.GetInt("BestScore", 0).ToString ( ); // Set Best Score Text
        pausePanel.SetActive(true); // Open Pause Panel 
    }

    // Restart Game
    // this method is called by the Restart Button in the Pause Panel
    public void Restart()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    // Continue Game
    // this method is called by the Continue Button in the Pause Panel
    public void Continue()
    {
        Time.timeScale = 1;
        pausePanel.SetActive(false);
    }
    
    public void GameOver()
    {
        StopGame();
        
        DisplayGameOverUI();
        
        CheckAndUpdateBestScore();
    }
    
    private void StopGame()
    {
        // Pause the game by setting the timescale to zero
        Time.timeScale = 0;
    }
    
    private void DisplayGameOverUI()
    {
        // Display the current score on the Game Over panel
        gameOverPanel_text_score.text = currentScore.ToString();
        
        // Activate the Game Over panel
        gameOverPanel.SetActive(true);
        
    }
    
    private void CheckAndUpdateBestScore()
    {
        // Retrieve the best score from player preferences, default to 0 if not set
        int bestScore = PlayerPrefs.GetInt("BestScore", 0);

        if (currentScore > bestScore)
        {
            // If current score is higher, update and display as the new best score
            gameOverPanel_text_BestScore.text = currentScore.ToString();
            PlayerPrefs.SetInt("BestScore", currentScore);
        }
        else
        {
            // Display the existing best score
            gameOverPanel_text_BestScore.text = bestScore.ToString();
        }
    }

    // this method is called by the obstacle.cs when the ball hits the obstacle
    public void AddScore(int n)
    {
        currentScore += n;
        text_score.text = currentScore.ToString();
    }

    
    public void IncreaseBallCounter()
    {
        totalBalls++;
    }


    // Decreases the count of total balls and checks if all balls have been launched and no balls are left.
    public void DecreaseBallCounter()
    {
        totalBalls--; // Decrement the total number of balls
        
        bool noBallsLeft = totalBalls <= 0; // Check if there are no balls left
        
        // Check if no balls are left and all balls have been launched
        if (noBallsLeft && player.allBallsLaunched)
        {
            player.ResetBallCounter();  // Reset the ball counter for the next round
            StartCoroutine(obstacleGenerator.AnimateObstaclesMovement()); // Initiate moving obstacles to their next positions
        }
    }


    private void ToggleValueChanged(Toggle change)
    {
        if (audioOnOff.isOn)
        {
            PlayerPrefs.SetString("AudioSetting", "On");
            Debug.Log(PlayerPrefs.GetString("AudioSetting", "-"));
        }
        else
        {
            PlayerPrefs.SetString("AudioSetting", "Off");
            Debug.Log(PlayerPrefs.GetString("AudioSetting", "-"));
        }
    }
}