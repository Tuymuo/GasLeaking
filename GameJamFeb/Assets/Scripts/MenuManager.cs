using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;


public class MenuManager : MonoBehaviour
{
    public TextMeshProUGUI finalScoreText; // UI de la puntuación final

    void Start()
{
    Cursor.visible = true;   // Asegurar que el cursor sea visible
    Cursor.lockState = CursorLockMode.None;  // Liberar el cursor si estaba bloqueado

    int finalScore = PlayerPrefs.GetInt("TotalScore", 0); // Obtener la puntuación guardada
    finalScoreText.text = "Puntos: " + finalScore; // Mostrarla en la UI
}

    // Función para reiniciar el juego
    public void RestartGame()
    {
        PlayerPrefs.SetInt("TotalScore", 0); // Reiniciar la puntuación
        PlayerPrefs.Save(); // Guardar los cambios
        SceneManager.LoadScene("SampleScene"); // Cargar la escena del juego
    }
}
