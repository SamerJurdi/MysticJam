using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    public Animator animator;
    public int health = 10;
    public float moveSpeed = 3f;
    public float attackCooldown = 1f; // Time between damage ticks while colliding with the player

    [Header("Collectible Settings")]
    public GameObject page;
    public GameObject heart;
    [Range(0, 100)]
    [Tooltip("The chance of  any item dropping (%).")]
    public int dropChance = 15;
    [Range(0, 100)]
    [Tooltip("Increase percentage to have more pages than hearts dropping.")]
    public int pagesToHeartsRatio = 80;

    [Header("Audio Settings")]
    public AudioClip enemySound;
    public float minSoundInterval = 5f; // Min delay between sound requests
    public float maxSoundInterval = 10f; // Max delay between sound requests
    private AudioSource audioSource;

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
        audioSource = GetComponent<AudioSource>();
        StartCoroutine(SoundCoroutine());
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
            playerController.TakeDamage();
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
        yield return new WaitForSeconds(0.75f);

        SpawnItem();
        Destroy(gameObject);
    }

    private void SpawnItem()
    {
        int randomValue = Random.Range(0, 100);
        if (randomValue < dropChance)
        {
            randomValue = Random.Range(0, 100);
            if (randomValue < pagesToHeartsRatio & page != null) {
                Instantiate(page, transform.position, Quaternion.identity);
            } else if (randomValue >= pagesToHeartsRatio & heart != null) {
                Instantiate(heart, transform.position, Quaternion.identity);
            }
        }
    }

    private IEnumerator SoundCoroutine()
    {
        while (true)
        {
            // Random delay before this enemy tries to make a sound
            float randomDelay = Random.Range(minSoundInterval, maxSoundInterval);
            yield return new WaitForSeconds(randomDelay);

            // Request permission to make a sound
            if (EnemySoundManager.Instance.CanMakeSound())
            {
                // Play sound and register it with the sound manager
                audioSource.PlayOneShot(enemySound);
                EnemySoundManager.Instance.RegisterSound();
            }
        }
    }
}
