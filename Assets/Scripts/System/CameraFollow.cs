using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    [Header("Configuración")]
    public Transform target;
    public Vector3 offset = new Vector3(0, 15, -8);
    public float smoothSpeed = 8f;

    void LateUpdate()
    {
        if (target == null) return;

        // Calculamos la posición deseada
        Vector3 desiredPos = target.position + offset;

        // Movimiento suave hacia esa posición
        transform.position = Vector3.Lerp(
            transform.position,
            desiredPos,
            smoothSpeed * Time.deltaTime
        );
    }
}