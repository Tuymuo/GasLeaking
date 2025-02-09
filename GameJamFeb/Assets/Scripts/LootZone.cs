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
                thiefAnimator.SetTrigger("Idle");
            }

            // Desactivar la LootZone después de entregar el loot
            if (lootZone != null)
            {
                lootZone.SetActive(false);
            }
            countdownManager.IncreaseSpeed(); // Aumentar la velocidad de la cuenta atrás

            Debug.Log("Punto agregado. Volver a robar.");
        }
    }

    public void AddPoint()
    {
        score++; // Aumentar puntuación
        PlayerPrefs.SetInt("TotalScore", score); // Guardar la puntuación
        PlayerPrefs.Save(); // Guardar datos

        if (countdownManager != null)
        {
            countdownManager.AddTime(1); // Añadir 1 segundo al contador
            
            Debug.Log("Velocidad de cuenta atrás aumentada.");
        }
        else
        {
            Debug.LogError("countdownManager no está asignado en LootZone.");
        }

        UpdateScoreUI(); // Actualizar UI de puntuación
    }



    void UpdateScoreUI()
    {
        if (scoreText != null)
        {
            scoreText.text = "Puntos: " + score;
        }
    }
}
