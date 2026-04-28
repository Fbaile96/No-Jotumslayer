using UnityEngine;

public class SpinAura : MonoBehaviour
{
    [Header("Configuración")]
    public float damage = 15f;
    public float damageCooldown = 0.5f;
    public float rotationSpeed = 90f;

    private float damageTimer;

    void Update()
    {
        // Giramos el aura constantemente
        transform.Rotate(0, rotationSpeed * Time.deltaTime, 0);
        damageTimer -= Time.deltaTime;
    }

    void OnTriggerStay(Collider other)
{
    if (other.CompareTag("Enemy") && damageTimer <= 0)
    {
        EnemyBase enemy = other.GetComponent<EnemyBase>();
        if (enemy != null)
        {
            enemy.TakeDamage(damage);
            damageTimer = damageCooldown;
            return;
        }
    }

    BossEnemy boss = other.GetComponent<BossEnemy>();
    if (boss != null && damageTimer <= 0)
    {
        boss.TakeDamage(damage);
        damageTimer = damageCooldown;
    }
}
}