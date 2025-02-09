using UnityEngine;

public class TimePickup : MonoBehaviour
{
    public CountdownManager countdownManager; // Referencia al script de la cuenta atrás
    public LootZone lootZone; // Referencia al script que maneja los puntos
    public SpriteRenderer spriteRenderer; // Para mostrar el sprite

    private void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>(); // Obtener el SpriteRenderer si está en el mismo objeto
    }

    public void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            // Aumentar +1 segundo al contador
            if (countdownManager != null)
            {
                countdownManager.AddTime(1);
            }

            // Aumentar +1 punto al marcador
            if (lootZone != null)
            {
                lootZone.AddPoint(); // Asegúrate de que LootZone tenga un método AddPoint()
            }

            // Desactivar objeto y volver a generarlo en otro lugar
            gameObject.SetActive(false);
            TimePickupSpawner.Instance.RespawnPickup();
        }
    }
}
