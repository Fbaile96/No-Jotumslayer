using UnityEngine;
using System.Collections;

public class SwordWeapon : MonoBehaviour

{
    [Header("Configuración")]
    public float damage = 35f;
    public float attackRate = 1.2f;      // Ataques por segundo
    public float detectionRange = 15f;
    public float slashDuration = 0.25f;   // Duración del hitbox activo

    private float attackTimer;
    private BoxCollider swordCollider;
    private bool isAttacking;

    void Start()
    {
        swordCollider = GetComponent<BoxCollider>();
        swordCollider.enabled = false;
    }

    void Update()
    {
        attackTimer -= Time.deltaTime;

        if (attackTimer <= 0f && !isAttacking)
        {
            GameObject nearestEnemy = FindNearestEnemy();

            if (nearestEnemy != null)
            {
                StartCoroutine(PerformSlash(nearestEnemy.transform));
                attackTimer = 1f / attackRate;
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
            float distance = Vector3.Distance(
                transform.parent.position,
                enemy.transform.position
            );
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
            float bossDistance = Vector3.Distance(
                transform.parent.position,
                boss.transform.position
            );
            if (bossDistance < minDistance)
            {
                minDistance = bossDistance;
                nearest = boss.gameObject;
            }
        }

        return nearest;
    }

    IEnumerator PerformSlash(Transform target)
    {
        isAttacking = true;

        // Rotar el jugador hacia el enemigo
        Vector3 direction = (target.position - transform.parent.position).normalized;
        direction.y = 0;
        transform.parent.rotation = Quaternion.LookRotation(direction);

        // Sonido de espada
        if (AudioManager.Instance != null)
        AudioManager.Instance.PlaySword();

        // Activa el hitbox
        swordCollider.enabled = true;

        // Animación simple: movemos la espada en arco
        float elapsed = 0f;
        Quaternion startRot = Quaternion.Euler(0, -90, -45);
        Quaternion endRot = Quaternion.Euler(0, 90, -45);

        while (elapsed < slashDuration)
        {
            elapsed += Time.deltaTime;
            float t = elapsed / slashDuration;
            transform.localRotation = Quaternion.Lerp(startRot, endRot, t);
            yield return null;
        }

        // Desactiva el hitbox
        swordCollider.enabled = false;

        // Vuelve la espada a posición original
        transform.localRotation = Quaternion.Euler(0, 0, -45);

        isAttacking = false;
    }


    void OnTriggerEnter(Collider other)
{
    // Daña enemigos normales
    EnemyBase enemy = other.GetComponent<EnemyBase>();
    if (enemy != null)
    {
        enemy.TakeDamage(damage);
        return; // ← importante, evita seguir ejecutando
    }

    // Daña al boss
    BossEnemy boss = other.GetComponent<BossEnemy>();
    if (boss != null)
    {
        boss.TakeDamage(damage);
    }
}
    
}