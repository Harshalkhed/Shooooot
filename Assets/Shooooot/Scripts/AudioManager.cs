using UnityEngine;


/*
 * 
 * This script is responsible for playing audio clips.
 * 
 */

// todo : action 이용해서 audio clip 재생하기 

public class AudioManager : MonoBehaviour {

    // Audio Clips
    public AudioClip collisionClip;
    public AudioClip breakObstacleeClip;
    
    // Prefab
    public GameObject obstacleDestroyPrefab;
    
    // Audio Source
    private AudioSource audioSource;
    
    private void Start()
    {
        // Get the audio source component from the game object
        audioSource = GetComponent<AudioSource>();
    }


    public void PlayCollisionAudio()
    {
        if (audioSource.isPlaying == false) audioSource.PlayOneShot(collisionClip);
    }


    public void PlayBreakObstacleAudio()
    {
        // this is a normal way to play audio
        // audioSource.PlayOneShot(breakObstacleClip);

        // But sometimes audio doesn't play properly, so I decided to use this method.
        Destroy(Instantiate(obstacleDestroyPrefab, Vector3.zero, Quaternion.identity), 0.05f);
    }
}