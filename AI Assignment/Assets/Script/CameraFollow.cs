using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform player; // Drag the Player object here
    public float offsetY = 5f; // Height offset
    public float offsetZ = -10f; // Depth offset

    void Update()
    {
        if (player != null)
        {
            Vector3 newPos = new Vector3(transform.position.x, player.position.y + offsetY, player.position.z + offsetZ);
            transform.position = newPos;
        }
    }
}