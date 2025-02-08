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
        score = PlayerPrefs.GetInt("TotalScore", 0); // Cargar puntuación guardada
        UpdateScoreUI(); // Asegurar que el UI inicia correctamente
        lootZone.SetActive(false); // Desactivar la LootZone al inicio
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player")) // Si el jugador entra en la zona
        {
            score++; // Aumentar el contador de puntos
            PlayerPrefs.SetInt("TotalScore", score); // Guardar puntuación en PlayerPrefs
            PlayerPrefs.Save(); // Asegurar que se guarde correctamente

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
