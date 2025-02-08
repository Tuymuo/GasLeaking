using UnityEngine;
using TMPro;

public class LootZone : MonoBehaviour
{
    public TextMeshProUGUI scoreText; // Referencia al UI de puntos
    private int score = 0;
    public Animator thiefAnimator; // Referencia al Animator del ladrón

    private void Start()
    {
        UpdateScoreUI(); // Asegurar que el UI inicia en 0
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player")) // Si el jugador entra en la zona
        {
            score++; // Aumentar el contador de puntos
            UpdateScoreUI(); // Actualizar la UI de puntos

            // Resetear animación del ladrón para que pueda robar otra vez
            if (thiefAnimator != null)
            {
                thiefAnimator.SetTrigger("Reset");
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
