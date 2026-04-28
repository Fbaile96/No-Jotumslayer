using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;

    [Header("Música")]
    public AudioSource musicSource;
    public AudioClip musicGameplay;
    public AudioClip musicBoss;

    [Header("Efectos de sonido")]
    public AudioSource sfxSource;
    public AudioClip sfxSword;
    public AudioClip sfxHit;
    public AudioClip sfxEnemyDeath;
    public AudioClip sfxLevelUp;
    public AudioClip sfxXPOrb;
    public AudioClip sfxBomb;

    void Awake()
    {
        // Singleton
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        PlayGameplayMusic();
    }

    // ── Música ──────────────────────────────

    public void PlayGameplayMusic()
    {
        if (musicGameplay == null) return;
        musicSource.clip = musicGameplay;
        musicSource.loop = true;
        musicSource.Play();
    }

    public void PlayBossMusic()
    {
        if (musicBoss == null) return;
        musicSource.clip = musicBoss;
        musicSource.loop = true;
        musicSource.Play();
    }

    public void StopMusic()
    {
        musicSource.Stop();
    }

    // ── Efectos ─────────────────────────────

    public void PlaySword()
    {
        PlaySFX(sfxSword);
    }

    public void PlayHit()
    {
        PlaySFX(sfxHit);
    }

    public void PlayEnemyDeath()
    {
        PlaySFX(sfxEnemyDeath);
    }

    public void PlayLevelUp()
    {
        PlaySFX(sfxLevelUp);
    }

    public void PlayXPOrb()
    {
        PlaySFX(sfxXPOrb);
    }

    public void PlayBomb()
    {
        PlaySFX(sfxBomb);
    }

    void PlaySFX(AudioClip clip)
    {
        if (clip == null) return;
        sfxSource.PlayOneShot(clip);
    }
}