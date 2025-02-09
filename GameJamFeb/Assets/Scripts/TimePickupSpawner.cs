using UnityEngine;

public class TimePickupSpawner : MonoBehaviour
{
    public static TimePickupSpawner Instance; // Singleton
    public Transform[] spawnPoints; // Lugares donde puede aparecer el objeto
    public GameObject pickupPrefab; // Prefab del objeto a generar
    private GameObject currentPickup; // Referencia al objeto actual

    private void Awake()
    {
        // Configurar el Singleton
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        RespawnPickup(); // Generar el primer objeto al inicio
    }

    public void RespawnPickup()
    {
        if (currentPickup != null)
        {
            Destroy(currentPickup); // Destruir el anterior
        }

        Transform spawnPoint = spawnPoints[Random.Range(0, spawnPoints.Length)];
        currentPickup = Instantiate(pickupPrefab, spawnPoint.position, Quaternion.identity);
    }
}
