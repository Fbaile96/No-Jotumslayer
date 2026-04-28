using UnityEngine;
using System.Collections;

public class AreaBomb : MonoBehaviour
{
    [Header("Configuración")]
    public float damage = 50f;
    public float explosionRadius = 4f;
    public float travelSpeed = 8f;
    public float explosionDelay = 0.3f;
    [Header("Efectos")]
    public GameObject explosionSpherePrefab; 


    private Vector3 targetPosition;
    private bool hasArrived;
    private bool hasExploded;

    public void Launch(Vector3 target)
    {
        targetPosition = target;
        StartCoroutine(TravelAndExplode());
    }

    IEnumerator TravelAndExplode()
    {
        // Viaja hacia el objetivo
        while (Vector3.Distance(transform.position, targetPosition) > 0.3f)
        {
            transform.position = Vector3.MoveTowards(
                transform.position,
                targetPosition,
                travelSpeed * Time.deltaTime
            );
            yield return null;
        }

        // Llegó al objetivo — pequeño delay antes de explotar
        yield return new WaitForSeconds(explosionDelay);

        // Explosión
        Explode();
    }

    void Explode()
    {
        if (hasExploded) return;
        hasExploded = true;

        if (AudioManager.Instance != null)
        AudioManager.Instance.PlayBomb();

        // Daña a todos los enemigos en el radio
        Collider[] hits = Physics.OverlapSphere(transform.position, explosionRadius);
        foreach (Collider hit in hits)
        {
            EnemyBase enemy = hit.GetComponent<EnemyBase>();
            if (enemy != null)
            {
                enemy.TakeDamage(damage);
                continue;
            }

            BossEnemy boss = hit.GetComponent<BossEnemy>();
            if (boss != null)
            {
                boss.TakeDamage(damage);
            }
        }

        // Efecto visual de expansión antes de destruirse
        StartCoroutine(ExplosionEffect());
    }


IEnumerator ExplosionEffect()
{
    // Instancia la esfera en la posición de la explosión
    GameObject sphere = Instantiate(
        explosionSpherePrefab,
        transform.position,
        Quaternion.identity
    );

    float elapsed = 0f;
    float duration = 0.3f;
    Vector3 startScale = Vector3.one * 0.1f;
    Vector3 endScale = Vector3.one * explosionRadius * 2f;

    // Obtén el material para animar el alpha
    Material mat = sphere.GetComponent<Renderer>().material;
    Color startColor = mat.color;

    while (elapsed < duration)
    {
        elapsed += Time.deltaTime;
        float t = elapsed / duration;

        // Expande la esfera
        sphere.transform.localScale = Vector3.Lerp(startScale, endScale, t);

        // Desvanece el alpha progresivamente
        Color c = startColor;
        c.a = Mathf.Lerp(0.8f, 0f, t);
        mat.color = c;

        yield return null;
    }

    Destroy(sphere);
    Destroy(gameObject); // destruye la bomba al final
}
}