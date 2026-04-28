using UnityEngine;

public class EnemyBase : MonoBehaviour
{
    [Header("Estadísticas")]
    public float maxHealth = 30f;
    public float currentHealth;
    public float speed = 2.5f;
    public float damage = 10f;
    public float damageCooldown = 1f;

    [Header("Drop")]
    public GameObject xpOrbPrefab;
    public GameObject healthPickupPrefab;
    [Range(0f, 1f)]
    public float healthDropChance = 0.1f;

    protected Transform player;
    protected PlayerStats playerStats;
    protected Animator animator;
    private float damageTimer;

    protected virtual void Start()
    {
        currentHealth = maxHealth;
        GameObject playerObj = GameObject.FindWithTag("Player");
        player = playerObj.transform;
        playerStats = playerObj.GetComponent<PlayerStats>();

        // Buscamos el Animator en el objeto o en sus hijos
        animator = GetComponentInChildren<Animator>();
    }

    protected virtual void Update()
    {
        if (player == null) return;

        MoveTowardsPlayer();
        damageTimer -= Time.deltaTime;

        // Activamos animación de movimiento si tiene Animator
        if (animator != null)
            animator.SetBool("isMoving", true);
    }

    protected virtual void MoveTowardsPlayer()
    {
        transform.position = Vector3.MoveTowards(
            transform.position,
            player.position,
            speed * Time.deltaTime
        );
    }

    public virtual void TakeDamage(float amount)
    {
        currentHealth -= amount;
        if (currentHealth <= 0) Die();
    }

    protected virtual void Die()
    {    
        // Registra el kill en el GameManager
        if (GameManager.Instance != null)
        GameManager.Instance.RegisterKill();
        
        // Activamos animación de muerte si tiene Animator
        if (animator != null)
            animator.SetTrigger("isDead");

        if (AudioManager.Instance != null)
        AudioManager.Instance.PlayEnemyDeath();  

        // Soltamos orbe de XP
        if (xpOrbPrefab != null)
        {
            Vector3 randomOffset = new Vector3(
                Random.Range(-0.5f, 0.5f),
                0f,
                Random.Range(-0.5f, 0.5f)
            );
            Instantiate(xpOrbPrefab, transform.position + randomOffset, Quaternion.identity);
        }

        // Soltamos pickup de vida con probabilidad
        if (healthPickupPrefab != null && Random.value < healthDropChance)
            Instantiate(healthPickupPrefab, transform.position, Quaternion.identity);


        // Delay para que se vea la animación de muerte antes de destruirse
        Destroy(gameObject, 0.5f);
    }

    void OnCollisionStay(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player") && damageTimer <= 0)
        {
            playerStats.TakeDamage(damage);
            damageTimer = damageCooldown;
        }
    }
}