using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DisappearingPlatform : MonoBehaviour
{
    public float disappearTime = 2f; // Time before the platform disappears
    public float respawnTime = 3f; // Time before the platform respawns
    private Renderer platformRenderer; // Renderer to change visibility

    void Start()
    {
        platformRenderer = GetComponent<Renderer>();
        StartCoroutine(PlatformTimer()); // Start the timer coroutine
    }

    IEnumerator PlatformTimer()
    {
        while (true) // Run indefinitely
        {
            yield return new WaitForSeconds(disappearTime); // Wait for the time before disappearing
            platformRenderer.enabled = false; // Make the platform invisible
            GetComponent<Collider>().enabled = false; // Disable collider to prevent interactions

            yield return new WaitForSeconds(respawnTime); // Wait for the respawn time
            platformRenderer.enabled = true; // Make it visible again
            GetComponent<Collider>().enabled = true; // Re-enable collider for future interactions
        }
    }
}