using UnityEngine;
using UnityEngine.UI;  // Necesario para trabajar con Slider
using TMPro;
using UnityEngine.SceneManagement;  // Necesario para cargar nuevas escenas

public class CountdownManager : MonoBehaviour
{
    public Slider countdownBar;  // Barra de progreso (Slider)
    private float countdownTime = 30f;  // Tiempo de cuenta atrás inicial (en segundos)
    private bool isCountingDown = true;
    private float speedMultiplier = 1f;  // Multiplicador de velocidad para hacer que el tiempo pase más rápido

    public LootZone lootZone;  // Referencia a la LootZone para añadir tiempo extra

    void Start()
    {
        countdownBar.maxValue = 1;  // Establece el máximo en 1 (barra llena)
        countdownBar.value = 1;  // Comienza con la barra llena
    }

    void Update()
    {
        if (isCountingDown)
        {
            countdownTime -= Time.deltaTime * speedMultiplier;  // Reducir el tiempo más rápido

            // Actualizar el valor de la barra para reflejar el tiempo restante
            countdownBar.value = countdownTime / 30f;  // Rellenar la barra proporcionalmente

            if (countdownTime <= 0)
            {
                countdownTime = 0;
                isCountingDown = false;
                Debug.Log("¡Tiempo agotado!");
                LoadMenuScene();  // Cargar la escena del menú cuando el tiempo se haya agotado
            }
        }
    }

    // Función para añadir tiempo cuando el ladrón entrega el botín
    public void AddTime(int additionalTime)
    {
        countdownTime += additionalTime;  // Añadir tiempo al contador
        if (countdownTime > 30) 
        {
            countdownTime = 30;  // Limitar el máximo a 30 segundos
        }

        // Reiniciar el temporizador si es necesario para continuar contando
        if (!isCountingDown)
        {
            isCountingDown = true;
        }
    }

    // Llamada desde LootZone para aumentar la velocidad de la cuenta atrás
    public void IncreaseSpeed()
    {
        speedMultiplier += 0.2f;  // Aumentar la velocidad de la cuenta atrás (puedes ajustar este valor)
        Debug.Log("La velocidad de la cuenta atrás ha aumentado. Nueva velocidad: " + speedMultiplier);
    }

    void LoadMenuScene()
    {
        SceneManager.LoadScene("MenuScene");  // Cargar la escena llamada 'MenuScene'
    }
}
