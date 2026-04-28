using UnityEngine;

public class MainMenuAudio : MonoBehaviour
{
    [Header("Música")]
    public AudioClip menuMusic;
    public float volume = 0.4f;

    private AudioSource audioSource;

    void Start()
    {
        // Si ya hay un AudioManager de otra escena, destrúyelo
        if (AudioManager.Instance != null)
        {
            Destroy(AudioManager.Instance.gameObject);
        }

        audioSource = gameObject.AddComponent<AudioSource>();
        audioSource.clip = menuMusic;
        audioSource.loop = true;
        audioSource.volume = volume;
        audioSource.Play();
    }
}