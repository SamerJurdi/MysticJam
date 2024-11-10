using UnityEngine;

public class EnemySoundManager : MonoBehaviour
{
    public static EnemySoundManager Instance { get; private set; }
    public float globalCooldownMin = 5f;
    public float globalCooldownMax = 10f;
    private float nextAllowedSoundTime = 0f;

    private void Awake()
    {
        // Singleton setup
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }

    // Check if an enemy is allowed to make a sound
    public bool CanMakeSound()
    {
        return Time.time >= nextAllowedSoundTime;
    }

    // Updates the next allowed sound time when a sound is played
    public void RegisterSound()
    {
        nextAllowedSoundTime = Time.time + Random.Range(globalCooldownMin, globalCooldownMax);
    }
}
