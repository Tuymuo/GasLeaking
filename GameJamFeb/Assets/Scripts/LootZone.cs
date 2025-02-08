using UnityEngine;
using TMPro;

public class LootZone : MonoBehaviour
{
    public TextMeshProUGUI scoreText; // Referencia al UI de puntos
    private int score = 0;
    public Animator thiefAnimator; // Referencia al Animator del ladrón
    public GameObject lootZone; // Referencia al GameObject LootZone
    public CountdownManager countdownManager; // Referencia al script de la cuenta atrás
    

    private void Start()
    {
        UpdateScoreUI(); // Asegurar que el UI inicia en 0
        lootZone.SetActive(false); // Desactivar la LootZone al inicio
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player")) // Si el jugador entra en la zona
        {
            score++; // Aumentar el contador de puntos
            UpdateScoreUI(); // Actualizar la UI de puntos

            // Añadir 15 segundos al contador de la cuenta atrás
            if (countdownManager != null)
            {
                countdownManager.AddTime(15);
            }

            // Resetear animación del ladrón para que pueda robar otra vez
            if (thiefAnimator != null)
            {
                thiefAnimator.SetTrigger("Reset");
            }

            // Desactivar la LootZone después de entregar el loot
            if (lootZone != null)
            {
                lootZone.SetActive(false);
            }

            Debug.Log("Punto agregado. Volver a robar.");
        }
    }

    void UpdateScoreUI()
    {
        if (scoreText != null)
        {
            scoreText.text = "Puntos: " + score;
        }
    }
}
