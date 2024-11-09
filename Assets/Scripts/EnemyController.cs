using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    public Animator animator;
    public int health = 10;
    public float moveSpeed = 3f;
    public int damageToPlayer = 1;
    public float attackCooldown = 1f; // Time between damage ticks while colliding with the player

    private Transform player;
    private PlayerController playerController;
    private Rigidbody2D rb;
    private BoxCollider2D boxCollider;
    private float attackCooldownTimer = 0f;
    private bool isAlive = true;

    private void Start()
    {
        animator.Play("Anim_Pumpkin");
        rb = GetComponent<Rigidbody2D>();
        boxCollider = GetComponent<BoxCollider2D>();
        player = GameObject.FindWithTag("Player").transform;
        playerController = player.GetComponent<PlayerController>();
    }

    private void Update()
    {
        if (isAlive && player != null) {
            FollowPlayer();
        }
    }

    private void FollowPlayer()
    {
        // Manage Attack Intervals
        if (attackCooldownTimer > 0f)
        {
            attackCooldownTimer -= Time.deltaTime;
        }
        // Follow player logic
        Vector3 direction = (player.position - transform.position).normalized;
        transform.position += direction * moveSpeed * Time.deltaTime;
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player") && attackCooldownTimer <= 0f && isAlive)
        {
            playerController.TakeDamage(damageToPlayer);
            attackCooldownTimer = attackCooldown;
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
        isAlive = false; // Internal state to prevent attacks
        boxCollider.enabled = false; // Prevent collisions
        rb.velocity = Vector2.zero; // Stop them in their place
        animator.Play("Anim_Pumpkin_Death");

        StartCoroutine(WaitForDamageAnimation());
    }

    private IEnumerator WaitForDamageAnimation()
    {
        yield return new WaitForSeconds(1f);

        Destroy(gameObject);
    }

}
