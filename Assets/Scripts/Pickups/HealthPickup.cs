using UnityEngine;

public class HealthPickup : MonoBehaviour
{
    [Header("Configuración")]
    public float healAmount = 20f;
    public float attractRadius = 3f;
    public float attractSpeed = 8f;

    private Transform player;
    private PlayerStats playerStats;
    private bool isAttracting;

    void Start()
    {
        GameObject playerObj = GameObject.FindWithTag("Player");
        player = playerObj.transform;
        playerStats = playerObj.GetComponent<PlayerStats>();

        // Rotación automática para que se vea bien
        StartCoroutine(RotatePickup());
    }

    void Update()
    {
        if (player == null) return;

        float distance = Vector3.Distance(transform.position, player.position);

        if (distance < attractRadius)
        {
            isAttracting = true;
        }

        if (isAttracting)
        {
            transform.position = Vector3.MoveTowards(
                transform.position,
                player.position,
                attractSpeed * Time.deltaTime
            );
        }
    }
    void OnTriggerEnter(Collider other)
    {
        Heal(other);
    }

    void OnTriggerStay(Collider other)
    {
        Heal(other);
    }

    void Heal(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerStats.currentHealth = Mathf.Min(
                playerStats.currentHealth + healAmount,
                playerStats.maxHealth
            );
            Destroy(gameObject);
        }
    }

    System.Collections.IEnumerator RotatePickup()
    {
        while (true)
        {
            transform.Rotate(0, 90 * Time.deltaTime, 0);
            yield return null;
        }
    }
}