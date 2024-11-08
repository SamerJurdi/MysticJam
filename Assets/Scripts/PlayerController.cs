using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float moveSpeed = 5f;

    private Rigidbody2D rb;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        // Get movement input for both X and Y axes
        float moveInputX = Input.GetAxisRaw("Horizontal");  // Left/Right (A/D or Arrow Keys)
        float moveInputY = Input.GetAxisRaw("Vertical");    // Up/Down (W/S or Arrow Keys)

        // Apply movement in both X and Y axes using Rigidbody2D velocity
        rb.velocity = new Vector2(moveInputX, moveInputY).normalized * moveSpeed;
    }
}
