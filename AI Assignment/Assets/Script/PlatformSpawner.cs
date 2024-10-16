using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformSpawner : MonoBehaviour
{
    public GameObject staticPlatformPrefab; // The static platform prefab to spawn
    public GameObject movingPlatformPrefab; // The moving platform prefab to spawn
    public float spawnDistance = 10f; // Distance above the player to spawn new platforms
    public int platformCount = 5; // Number of platforms to spawn initially
    public float minX = -3f, maxX = 3f; // Horizontal range for platform spawning
    public float minYGap = 2f, maxYGap = 5f; // Vertical gap range between platforms

    private List<GameObject> platforms = new List<GameObject>(); // List to store active platforms
    private Transform player; // Reference to the player
    private float lastPlatformY; // Track the Y position of the last platform

    void Start()
    {
        // Find the player by tag (make sure the player is tagged as "Player")
        player = GameObject.FindGameObjectWithTag("Player").transform;

        // Spawn the initial set of platforms
        for (int i = 0; i < platformCount; i++)
        {
            // First platform is right above the player, others have a random Y gap
            float spawnY = i == 0 ? player.position.y + 2f : lastPlatformY + Random.Range(minYGap, maxYGap);
            SpawnPlatform(spawnY);
        }
    }

    void Update()
    {
        // Check if we need to spawn a new platform
        if (player.position.y + spawnDistance > lastPlatformY)
        {
            SpawnPlatform(lastPlatformY + Random.Range(minYGap, maxYGap));
        }

        // Optional: Remove platforms far below the player to optimize performance
        RemoveOldPlatforms();
    }

    // Spawn a platform at a given Y position
    void SpawnPlatform(float yPosition)
    {
        float randomX = Random.Range(minX, maxX); // Random X position within range

        // Randomly decide whether to spawn a static or moving platform
        GameObject platformPrefab = Random.Range(0, 2) == 0 ? staticPlatformPrefab : movingPlatformPrefab;

        Vector3 spawnPos = new Vector3(randomX, yPosition, 0f);
        GameObject newPlatform = Instantiate(platformPrefab, spawnPos, Quaternion.identity);
        platforms.Add(newPlatform); // Add to the list of active platforms

        lastPlatformY = yPosition; // Update the Y position of the last platform
    }

    // Remove platforms far below the player
    void RemoveOldPlatforms()
    {
        for (int i = platforms.Count - 1; i >= 0; i--)
        {
            if (platforms[i].transform.position.y < player.position.y - 10f)
            {
                Destroy(platforms[i]);
                platforms.RemoveAt(i);
            }
        }
    }
}