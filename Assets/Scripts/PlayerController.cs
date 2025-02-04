using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour
{
    public Animator animator;
    public SpriteRenderer spriteRenderer;
    public GameObject tome;
    public int maxHealth = 3;
    public float moveSpeed = 5f;
    [Tooltip("Deceleration multiplier for gradual stopping.")]
    public float decelerationRate = 0.99f;

    [Header("Audio Settings")]
    public AudioClip pageSound;
    public AudioClip heartSound;
    public AudioClip evilLaugh;
    public AudioClip[] gruntSounds;
    public float minTimeBetweenLaughs = 15f;
    public float maxTimeBetweenLaughs = 40f;
    private float laughCooldown;
    private bool canPlaySound = true;
    private AudioSource audioSource;


    private Rigidbody2D rb;
    private Vector2 currentVelocity;
    private LayerMask collectibleMask;
    private TomeController tomeController;
    private int health;

    public List<HeartScript> hearts;
    public float Score;
    public TextMeshProUGUI ScoreText;
    public GameObject HighScoreText;
    public bool dead;

    public Animator RetryAnim;

    public GameObject DeathScreen;

    private void Start()
    {
        Score = 0;
        laughCooldown = Random.Range(minTimeBetweenLaughs, maxTimeBetweenLaughs);
        health = maxHealth;
        rb = GetComponent<Rigidbody2D>();
        animator.Play("Anim_Witch_Floating");
        collectibleMask = 1 << LayerMask.NameToLayer("Collectible");
        tomeController = tome.GetComponent<TomeController>();
        audioSource = GetComponent<AudioSource>();
    }

    private void Update()
    {
        if (!dead)
        {
            Score += Time.deltaTime;
            ScoreText.text = "Score: " + Score.ToString("F0");
        }

        // Get movement input for both X and Y axes
        float moveInputX = Input.GetAxisRaw("Horizontal");  // Left/Right (A/D or Arrow Keys)
        float moveInputY = Input.GetAxisRaw("Vertical");    // Up/Down (W/S or Arrow Keys)

        // Check for input
        Vector2 input = new Vector2(moveInputX, moveInputY);

        if (input != Vector2.zero)
        {
            // Apply movement when input is detected
            currentVelocity = input.normalized * moveSpeed;
        }
        else
        {
            // Gradually reduce speed when no input
            currentVelocity *= decelerationRate;
        }

        // Move the character
        rb.velocity = currentVelocity;

        UpdateDirectionAndAnimation(moveInputX);
        TryPlayEvilLaugh();
    }

    private void OnTriggerEnter2D(Collider2D other) {
        if ((collectibleMask.value & (1 << other.gameObject.layer)) > 0)
        {
            if (other.gameObject.CompareTag("Page") && tomeController != null)
            {
                audioSource.clip = pageSound;
                audioSource.Play();
                tomeController.IncreaseFireRate();
            }
            if (other.gameObject.CompareTag("Heart"))
            {
                audioSource.clip = heartSound;
                audioSource.Play();
                AddHealth();
            }
            Destroy(other.gameObject);
        }
    }

    private void UpdateDirectionAndAnimation(float moveInputX)
    {
        // Flip the sprite based on the direction
        if (moveInputX != 0)
        {
            spriteRenderer.flipX = moveInputX < 0;
        }
    }

    public void TakeDamage()
    {
        health--;
        animator.Play("Anim_Witch_Damage", -1, 0f);

        // Select a random grunt sound from the array
        audioSource.clip = gruntSounds[Random.Range(0, gruntSounds.Length)];
        audioSource.Play();

        UpdateHearts();

        StartCoroutine(WaitForDamageAnimation());
    }

    public void AddHealth()
    {
        if (health < maxHealth)
        {
            health++;
        }
        UpdateHearts();

        StartCoroutine(WaitForDamageAnimation());
    }

    public void UpdateHearts()
    {
        for (int i = 0; i < hearts.Count; i++)
        {
            if (i < health)
            {
                hearts[i].FullHeart.SetActive(true);
                hearts[i].EmptyHeart.SetActive(false);
            }
            else
            {
                hearts[i].FullHeart.SetActive(false);
                hearts[i].EmptyHeart.SetActive(true);
            }
        }
    }


    private IEnumerator WaitForDamageAnimation()
    {
        yield return new WaitForSeconds(0.5f);

        if (health <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        // TODO: Handle the player's death (e.g., play animation, show death screen, etc.)

        if (PlayerPrefs.GetInt("HighScore", 0) < Score)
        {
            PlayerPrefs.SetInt("HighScore", Mathf.RoundToInt(Score));
        }

        //Destroy(gameObject);

        DeathScreen.SetActive(true);
        Time.timeScale = 0f;
        RetryAnim.updateMode = AnimatorUpdateMode.UnscaledTime;
        dead = true;
        HighScoreText.SetActive(false);

        //SceneManager.LoadScene("StartScene");
    }

    public void Retry()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("GameLevel");
    }

    private void TryPlayEvilLaugh()
    {
        // Check if player is moving and if sound cooldown allows
        if ((Input.GetAxis("Horizontal") != 0 || Input.GetAxis("Vertical") != 0) && canPlaySound)
        {
            StartCoroutine(PlayEvilLaugh());
        }
    }

    private IEnumerator PlayEvilLaugh()
    {
        canPlaySound = false;

        audioSource.clip = evilLaugh;
        audioSource.Play();

        // Wait until the cooldown or sound clip duration, whichever is longer
        yield return new WaitForSeconds(Mathf.Max(laughCooldown, evilLaugh.length));

        canPlaySound = true;
    }
}
