using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    // Function to load the game scene
    public void PlayGame()
    {
        // Load the next scene (replace "GameScene" with the name of your gameplay scene)
        SceneManager.LoadScene("Endless Climber"); // Ensure this matches the name of your game scene
    }

    // Function to quit the game (won't work in editor)
    public void QuitGame()
    {
        Debug.Log("Quit!"); // This will show in the console during testing
        Application.Quit();
    }
}