using UnityEngine;

public class TankEnemy : EnemyBase
{
    protected override void Start()
    {
        // Estadísticas de enemigo tanque
        maxHealth = 120f;
        speed = 1.2f;
        damage = 25f;
        damageCooldown = 2f;
        base.Start();
    }
}