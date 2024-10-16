using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingPlatform : MonoBehaviour
{
    public float movementDistance = 3f; // Distance to move in each direction
    public float speed = 2f; // Movement speed

    private Vector3 pointA; // Start position
    private Vector3 pointB; // End position
    private Vector3 targetPosition;

    void Start()
    {
        // Set pointA to the current position of the platform
        pointA = transform.position;
        // Set pointB to be movementDistance units to the right of pointA
        pointB = pointA + new Vector3(movementDistance, 0f, 0f);
        targetPosition = pointB; // Start by moving towards point B
    }

    void Update()
    {
        // Move the platform towards the target position
        transform.position = Vector3.MoveTowards(transform.position, targetPosition, speed * Time.deltaTime);

        // Check if the platform has reached the target position
        if (Vector3.Distance(transform.position, targetPosition) < 0.1f)
        {
            // Switch the target position
            targetPosition = (targetPosition == pointA) ? pointB : pointA;
        }
    }
}