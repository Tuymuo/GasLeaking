using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;


public class MenuManager : MonoBehaviour
{
    public TextMeshProUGUI finalScoreText; // UI de la puntuación final

    void Start()
    {
        int finalScore = PlayerPrefs.GetInt("TotalScore", 0); // Obtener la puntuación guardada
        finalScoreText.text = "Puntuación Final: " + finalScore; // Mostrarla en la UI
    }

    // Función para reiniciar el juego
    public void RestartGame()
    {
        PlayerPrefs.SetInt("TotalScore", 0); // Reiniciar la puntuación
        PlayerPrefs.Save(); // Guardar los cambios
        SceneManager.LoadScene("SampleScene"); // Cargar la escena del juego
    }
}
