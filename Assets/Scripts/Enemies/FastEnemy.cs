using UnityEngine;

public class FastEnemy : EnemyBase
{
    protected override void Start()
    {
        // Estadísticas de enemigo rápido
        maxHealth = 15f;
        speed = 5f;
        damage = 5f;
        damageCooldown = 0.5f;
        base.Start();
    }
}