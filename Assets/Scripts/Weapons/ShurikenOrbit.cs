using UnityEngine;

public class ShurikenOrbit : MonoBehaviour
{
    [Header("Configuración")]
    public float damage = 25f;
    public float orbitSpeed = 180f;
    public float orbitRadius = 2.5f;
    public float damageCooldown = 0.5f;

    private float angle;
    private float damageTimer;

    void Update()
    {
        // Orbitamos alrededor del jugador
        angle += orbitSpeed * Time.deltaTime;

        float x = Mathf.Cos(angle * Mathf.Deg2Rad) * orbitRadius;
        float z = Mathf.Sin(angle * Mathf.Deg2Rad) * orbitRadius;

        transform.localPosition = new Vector3(x, 0f, z);

        // Rotamos el hacha sobre sí misma
        transform.Rotate(0, orbitSpeed * Time.deltaTime, 0);

        damageTimer -= Time.deltaTime;
    }


void OnTriggerStay(Collider other)
{
    EnemyBase enemy = other.GetComponent<EnemyBase>();
    if (enemy != null && damageTimer <= 0)
    {
        enemy.TakeDamage(damage);
        damageTimer = damageCooldown;
        return;
    }

    BossEnemy boss = other.GetComponent<BossEnemy>();
    if (boss != null && damageTimer <= 0)
    {
        boss.TakeDamage(damage);
        damageTimer = damageCooldown;
    }
}
}