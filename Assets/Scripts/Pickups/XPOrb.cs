using UnityEngine;

public class XPOrb : MonoBehaviour
{
    [Header("Configuración")]
    public float xpAmount = 10f;
    public float attractRadius = 3f; //TODO verificar distancia
    public float attractSpeed = 8f;

    private Transform player;
    private PlayerStats playerStats;
    private bool isAttracting;

    void Start()
    {
        GameObject playerObj = GameObject.FindWithTag("Player");
        player = playerObj.transform;
        playerStats = playerObj.GetComponent<PlayerStats>();
    }

    void Update()
    {
        if (player == null) return;

        float distance = Vector3.Distance(transform.position, player.position);

        // Si el jugador está cerca, el orbe se mueve hacia él
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
    if (other.CompareTag("Player"))
    {
        if (AudioManager.Instance != null)
            AudioManager.Instance.PlayXPOrb();

        playerStats.GainXP(xpAmount);
        Destroy(gameObject);
    }
    }
}