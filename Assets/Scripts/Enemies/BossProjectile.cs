using UnityEngine;

public class BossProjectile : MonoBehaviour
{
    public float speed = 6f;
    public float damage = 15f;
    public float lifetime = 4f;

    private Vector3 direction;

    public void SetDirection(Vector3 dir)
    {
        direction = dir.normalized;
    }

    void Start()
    {
        Destroy(gameObject, lifetime);
    }

    void Update()
    {
        transform.position += direction * speed * Time.deltaTime;
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            other.GetComponent<PlayerStats>()?.TakeDamage(damage);
            Destroy(gameObject);
        }
    }
}