using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerMovement : MonoBehaviour
{
    public float moveSpeed = 5f;
    public float jumpForce = 5f;
    public LayerMask groundLayer;
    public GameObject gameOverUI;

    private Rigidbody rb;
    private bool isGrounded;
    private float fallTimer = 0f;
    public float fallThreshold = 2f;

    private Vector3 initialPosition = new Vector3(0f, 1.5f, 0f); // Starting position
    private bool isGameOver = false;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        isGrounded = false; // Assume the player is not grounded at the start
        gameOverUI.SetActive(false); // Hide Game Over UI initially
        ResetPlayer(); // Reset player position and state at the start
    }

    void Update()
    {
        if (!isGameOver)
        {
            HandleMovement();
            HandleFallTimer();
            CheckFloorStatus();
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

    void GameOver()
    {
        gameOverUI.SetActive(true);
        isGameOver = true; // Prevent further movement and updates
        Time.timeScale = 0; // Pause the game
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

    // Check if player is touching the starting floor and destroy it after leaving
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

        // If player leaves the floor, despawn it
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
    }
}