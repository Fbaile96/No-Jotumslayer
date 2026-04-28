using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    [Header("Condiciones de activación del boss")]
    public float timeToTriggerBoss = 1200f; // 20 minutos en segundos
    public int killsToTriggerBoss = 300;

    [Header("Estado")]
    public int totalKills = 0;
    public float elapsedTime = 0f;
    public bool bossTriggered = false;

    void Awake()
    {
        // Singleton — solo existe una instancia
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Update()
    {
        if (bossTriggered) return;

        elapsedTime += Time.deltaTime;

        // Verificamos condiciones
        if (elapsedTime >= timeToTriggerBoss || totalKills >= killsToTriggerBoss)
        {
            TriggerBossPhase();
        }
    }

    public void RegisterKill()
    {
        totalKills++;
    }

    void TriggerBossPhase()
    {
        bossTriggered = true;

        // Detenemos el spawner
        EnemySpawner spawner = FindObjectOfType<EnemySpawner>();
        if (spawner != null) spawner.enabled = false;

        // Destruimos todos los enemigos en pantalla
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
        foreach (GameObject enemy in enemies)
            Destroy(enemy);

        // Iniciamos la transición
        StartCoroutine(BossTransition());
    }

    IEnumerator BossTransition()
    {
        if (AudioManager.Instance != null)
        AudioManager.Instance.PlayBossMusic();

        // Mostramos la pantalla de transición
        TransitionUI transition = FindObjectOfType<TransitionUI>();
        if (transition != null)
            transition.ShowTransition();

        // Esperamos que termine la animación
        yield return new WaitForSeconds(3f);

        // Cargamos la escena del boss
        SceneManager.LoadScene("BossRoom");
    }
}