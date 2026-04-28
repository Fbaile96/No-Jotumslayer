using UnityEngine;

public class PlayerStats : MonoBehaviour
{
    [Header("Vida")]
    public float maxHealth = 100f;
    public float currentHealth;

    [Header("XP y Nivel")]
    public float currentXP = 0f;
    public float xpToNextLevel = 50f;
    public int currentLevel = 1;

    [Header("Referencias")]
    public LevelUpUI levelUpUI;
    public GameOverUI gameOverUI; 

    void Start()
    {
        currentHealth = maxHealth;
    }

    public void TakeDamage(float amount)
    {
        currentHealth -= amount;
        Debug.Log($"Vida restante: {currentHealth}");

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    public void GainXP(float amount)
    {
        currentXP += amount;
        Debug.Log($"XP: {currentXP} / {xpToNextLevel}");

        if (currentXP >= xpToNextLevel)
        {
            LevelUp();
        }
    }

    void LevelUp()
    {
        currentLevel++;
        currentXP = 0f;
        xpToNextLevel *= 1.5f; // Cada nivel requiere más XP
        Debug.Log($"¡Nivel {currentLevel}!");

        if (AudioManager.Instance != null)
        AudioManager.Instance.PlayLevelUp();

        levelUpUI.ShowLevelUpMenu();
    }

    void Die()
    {
    Debug.Log("¡Game Over!");
    gameOverUI.ShowGameOver(currentLevel, FindObjectOfType<HUDController>().GetElapsedTime());
    gameObject.SetActive(false);
    }
}