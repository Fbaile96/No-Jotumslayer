using UnityEngine;

public class BombLauncher : MonoBehaviour
{
    [Header("Configuración")]
    public GameObject bombPrefab;
    public float cooldown = 5f;
    public float detectionRange = 15f;

    private float timer;

    void Start()
    {
    timer = cooldown;
    }

    void Update()
    {
        timer -= Time.deltaTime;

        if (timer <= 0f)
        {
            GameObject target = FindNearestEnemy();
            if (target != null)
            {
                LaunchBomb(target.transform.position);
                timer = cooldown;
            }
        }
    }

    GameObject FindNearestEnemy()
    {
        GameObject nearest = null;
        float minDistance = detectionRange;

        // Busca enemigos normales
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
        foreach (GameObject enemy in enemies)
        {
            float distance = Vector3.Distance(transform.position, enemy.transform.position);
            if (distance < minDistance)
            {
                minDistance = distance;
                nearest = enemy;
            }
        }

        // Busca al boss si existe en la escena
        BossEnemy boss = FindObjectOfType<BossEnemy>();
        if (boss != null)
        {
            float bossDistance = Vector3.Distance(transform.position, boss.transform.position);
            if (bossDistance < minDistance)
            {
                nearest = boss.gameObject;
            }
        }

        return nearest;
    }

    void LaunchBomb(Vector3 targetPos)
    {
        // Instancia la bomba en la posición del jugador
        GameObject bombObj = Instantiate(bombPrefab, transform.position + Vector3.up, Quaternion.identity);
        AreaBomb bomb = bombObj.GetComponent<AreaBomb>();
        bomb.Launch(targetPos);
    }
}