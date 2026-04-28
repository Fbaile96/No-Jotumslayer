using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class HUDController : MonoBehaviour
{
    [Header("Vida")]
    public Slider healthBar;
    public TextMeshProUGUI healthText;

    [Header("XP y Nivel")]
    public Slider xpBar;
    public TextMeshProUGUI levelText;

    [Header("Temporizador")]
    public TextMeshProUGUI timerText;

    private PlayerStats playerStats;
    private float elapsedTime;

    void Start()
    {
        playerStats = GameObject.FindWithTag("Player").GetComponent<PlayerStats>();
    }

    void Update()
    {
        if (playerStats == null) return;

        UpdateHealthBar();
        UpdateXPBar();
        UpdateTimer();
    }

    void UpdateHealthBar()
    {
        healthBar.maxValue = playerStats.maxHealth;
        healthBar.value = playerStats.currentHealth;
        healthText.text = $"HP: {Mathf.CeilToInt(playerStats.currentHealth)} / {Mathf.CeilToInt(playerStats.maxHealth)}";
    }

    void UpdateXPBar()
    {
        xpBar.value = playerStats.currentXP / playerStats.xpToNextLevel;
        levelText.text = $"Nivel {playerStats.currentLevel}";
    }

    void UpdateTimer()
    {
        elapsedTime += Time.deltaTime;
        int minutes = Mathf.FloorToInt(elapsedTime / 60f);
        int seconds = Mathf.FloorToInt(elapsedTime % 60f);
        timerText.text = $"{minutes:00}:{seconds:00}";
    }
    public float GetElapsedTime()
    {
        return elapsedTime;
    }
}