using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public Animator animator;
    public SpriteRenderer spriteRenderer;
    public int health = 3;
    public float moveSpeed = 5f;

    private Rigidbody2D rb;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator.Play("Anim_Witch_Floating");
    }

    private void Update()
    {
        // Get movement input for both X and Y axes
        float moveInputX = Input.GetAxisRaw("Horizontal");  // Left/Right (A/D or Arrow Keys)
        float moveInputY = Input.GetAxisRaw("Vertical");    // Up/Down (W/S or Arrow Keys)

        // Apply movement in both X and Y axes using Rigidbody2D velocity
        rb.velocity = new Vector2(moveInputX, moveInputY).normalized * moveSpeed;

        UpdateDirectionAndAnimation(moveInputX);
    }

    private void UpdateDirectionAndAnimation(float moveInputX)
    {
        // Flip the sprite based on the direction
        if (moveInputX != 0)
        {
            spriteRenderer.flipX = moveInputX < 0;
        }
    }

    public void TakeDamage(int damage)
    {
        health -= damage;
        if (health <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        // TODO: Handle the player's death (e.g., play animation, show death screen, etc.)
        Destroy(gameObject);
    }
}
