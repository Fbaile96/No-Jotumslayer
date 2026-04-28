using UnityEngine;
using System.Collections;

public class BossEnemy : MonoBehaviour
{
    [Header("Estadísticas")]
    public float maxHealth = 1000f;
    public float currentHealth;
    public float moveSpeed = 3f;

    [Header("Embestida")]
    public float chargeSpeed = 15f;
    public float chargeCooldown = 5f;
    public float chargeDuration = 0.4f;
    public float chargeDamage = 30f;

    [Header("Proyectiles")]
    public GameObject projectilePrefab;
    public float projectileCooldown = 3f;
    public int projectileCount = 8;

    private Transform player;
    private PlayerStats playerStats;
    private float chargeTimer;
    private float projectileTimer;
    private bool isCharging;

    void Start()
    {
        currentHealth = maxHealth;
        GameObject playerObj = GameObject.FindWithTag("Player");
        player = playerObj.transform;
        playerStats = playerObj.GetComponent<PlayerStats>();

        // Notifica al HUD de la vida del boss
        BossHealthUI bossUI = FindObjectOfType<BossHealthUI>();
        if (bossUI != null) bossUI.SetBoss(this);
    }

    void Update()
    {
        if (player == null || isCharging) return;

        // Movimiento normal hacia el jugador
        transform.position = Vector3.MoveTowards(
            transform.position,
            player.position,
            moveSpeed * Time.deltaTime
        );

        // Rota hacia el jugador
        Vector3 dir = (player.position - transform.position).normalized;
        if (dir != Vector3.zero)
            transform.rotation = Quaternion.LookRotation(dir);

        // Temporizadores de ataques
        chargeTimer -= Time.deltaTime;
        projectileTimer -= Time.deltaTime;

        if (chargeTimer <= 0f)
        {
            StartCoroutine(Charge());
            chargeTimer = chargeCooldown;
        }

        if (projectileTimer <= 0f)
        {
            ShootProjectiles();
            projectileTimer = projectileCooldown;
        }
    }

    IEnumerator Charge()
    {
        isCharging = true;
        Vector3 chargeDirection = (player.position - transform.position).normalized;

        float elapsed = 0f;
        while (elapsed < chargeDuration)
        {
            elapsed += Time.deltaTime;
            transform.position += chargeDirection * chargeSpeed * Time.deltaTime;
            yield return null;
        }

        isCharging = false;
    }

    void ShootProjectiles()
    {
        if (projectilePrefab == null) return;

        // Dispara proyectiles en todas direcciones
        float angleStep = 360f / projectileCount;
        for (int i = 0; i < projectileCount; i++)
        {
            float angle = i * angleStep;
            Vector3 direction = new Vector3(
                Mathf.Cos(angle * Mathf.Deg2Rad),
                0f,
                Mathf.Sin(angle * Mathf.Deg2Rad)
            );

            GameObject proj = Instantiate(
                projectilePrefab,
                transform.position,
                Quaternion.identity
            );
            proj.GetComponent<BossProjectile>().SetDirection(direction);
        }
    }

    public void TakeDamage(float amount)
    {
        currentHealth -= amount;

        BossHealthUI bossUI = FindObjectOfType<BossHealthUI>();
        if (bossUI != null) bossUI.UpdateHealth(currentHealth, maxHealth);

        if (currentHealth <= 0) Die();
    }

    void OnCollisionStay(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            playerStats.TakeDamage(chargeDamage * Time.deltaTime);
        }
    }

    void Die()
    {
        FindObjectOfType<VictoryUI>()?.ShowVictory();
        Destroy(gameObject);
    }
}