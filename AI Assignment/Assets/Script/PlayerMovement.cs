using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;// For using UI elements like Text
using UnityEngine.SceneManagement;
using TMPro; // Import TextMeshPro namespace for UI

public class PlayerMovement : MonoBehaviour
{
    public float moveSpeed = 5f;
    public float jumpForce = 5f;
    public LayerMask groundLayer;
    public GameObject gameOverUI;
    public TextMeshProUGUI scoreText; // Use TextMeshProUGUI for the score text
    public TextMeshProUGUI gameOverScoreText; // TextMeshProUGUI for the score display on the Game Over UI

    private Rigidbody rb;
    private bool isGrounded;
    private float fallTimer = 0f;
    public float fallThreshold = 2f;

    private Vector3 initialPosition = new Vector3(0f, 1.5f, 0f); // Starting position
    private bool isGameOver = false;
    private float highestPoint = 0f; // Track the highest point reached
    private int score = 0; // The player's score

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        isGrounded = false; // Assume the player is not grounded at the start
        gameOverUI.SetActive(false); // Hide Game Over UI initially
        ResetPlayer(); // Reset player position and state at the start

        highestPoint = transform.position.y; // Initialize the highest point
        UpdateScore(); // Update the score at the start
    }

    void Update()
    {
        if (!isGameOver)
        {
            HandleMovement();
            HandleFallTimer();
            CheckFloorStatus();
            UpdateScore(); // Update the score continuously based on height
        }
    }

    void HandleMovement()
    {
        // Horizontal movement
        float moveInput = Input.GetAxis("Horizontal");
        Vector3 moveDirection = new Vector3(moveInput * moveSpeed, rb.velocity.y, 0f);
        rb.velocity = moveDirection;

        // Jumping
        if (isGrounded && Input.GetButtonDown("Jump"))
        {
            rb.velocity = new Vector3(rb.velocity.x, jumpForce, 0f);
            isGrounded = false; // The player is not grounded after jumping
        }
    }

    void HandleFallTimer()
    {
        if (!isGrounded)
        {
            fallTimer += Time.deltaTime;

            if (fallTimer >= fallThreshold)
            {
                GameOver();
            }
        }
        else
        {
            fallTimer = 0f; // Reset fall timer when grounded
        }
    }

    void UpdateScore()
    {
        // Check if the player has reached a new highest point
        if (transform.position.y > highestPoint)
        {
            highestPoint = transform.position.y;
            score = Mathf.FloorToInt(highestPoint); // Convert height to score
            scoreText.text = "Score: " + score.ToString(); // Update score UI
        }
    }

    void GameOver()
    {
        gameOverUI.SetActive(true);
        isGameOver = true; // Prevent further movement and updates
        Time.timeScale = 0; // Pause the game

        // Hide the in-game score text
        scoreText.gameObject.SetActive(false);

        // Display the final score on the Game Over screen
        gameOverScoreText.text = "Final Score: " + score.ToString();
        gameOverScoreText.gameObject.SetActive(true); // Show the score on Game Over UI
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            isGrounded = true;
        }
    }

    void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            isGrounded = false;
        }
    }

    void CheckFloorStatus()
    {
        Collider[] hitColliders = Physics.OverlapSphere(transform.position, 0.5f);
        foreach (Collider hitCollider in hitColliders)
        {
            if (hitCollider.CompareTag("Floor"))
            {
                isGrounded = true; // Assume grounded when touching the starting floor
                return;
            }
        }

        GameObject floor = GameObject.FindWithTag("Floor");
        if (floor != null && !isGrounded)
        {
            Destroy(floor, 2f); // Destroy the starting floor after 2 seconds
        }
    }

    public void RestartGame()
    {
        Time.timeScale = 1; // Reset time scale
        ResetPlayer(); // Reset player position and state
        isGameOver = false; // Enable player movement and updates
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex); // Restart the current scene
    }

    public void GoToMainMenu()
    {
        Time.timeScale = 1; // Reset time scale
        isGameOver = true; // Disable player movement and updates immediately
        SceneManager.LoadScene("Main Menu"); // Load the main menu scene
    }

    void ResetPlayer()
    {
        transform.position = initialPosition; // Reset player to initial position
        isGrounded = true; // Assume grounded state at the start
        fallTimer = 0f; // Reset fall timer
        rb.velocity = Vector3.zero; // Reset any velocity
        highestPoint = transform.position.y; // Reset the highest point
        score = 0; // Reset the score
        UpdateScore(); // Update the score UI at the start
        scoreText.gameObject.SetActive(true); // Ensure the score text is visible when game restarts
        gameOverScoreText.gameObject.SetActive(false); // Hide Game Over score on restart
    }
}