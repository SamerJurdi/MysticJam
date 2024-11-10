using System.Collections;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public AudioClip introMusic;
    public AudioClip loopMusic;
    private AudioSource audioSource;
    public float crossfadeDuration = 0.5f;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        PlayIntroMusic();
    }

    private void PlayIntroMusic()
    {
        audioSource.clip = introMusic;
        audioSource.loop = false;
        audioSource.Play();

        // Schedule the crossfade to loop music
        StartCoroutine(PlayLoopMusic(introMusic.length - crossfadeDuration));
    }

    private IEnumerator PlayLoopMusic(float delay)
    {
        yield return new WaitForSeconds(delay);

        audioSource.Stop();
        audioSource.clip = loopMusic;
        audioSource.loop = true;
        audioSource.Play();
    }
}
