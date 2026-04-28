using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class GameOverUI : MonoBehaviour
{
    [Header("Referencias")]
    public GameObject gameOverPanel;
    public TextMeshProUGUI statsText;

    void Start()
    {
        gameOverPanel.SetActive(false);
    }

    public void ShowGameOver(int level, float time)
    {
        // Pausamos el juego
        Time.timeScale = 0f;

        // Mostramos el panel
        gameOverPanel.SetActive(true);

        // Formateamos el tiempo
        int minutes = Mathf.FloorToInt(time / 60f);
        int seconds = Mathf.FloorToInt(time % 60f);
        statsText.text = $"Nivel {level} — {minutes:00}:{seconds:00}";
    }

    public void Retry()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("Gameplay");
    }

    public void GoToMainMenu()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("MainMenu");
    }
}