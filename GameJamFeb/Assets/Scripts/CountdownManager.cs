using UnityEngine;
using UnityEngine.UI;  // Necesario para trabajar con Slider
using TMPro;

public class CountdownManager : MonoBehaviour
{
    public Slider countdownBar;  // Barra de progreso (Slider)
    private float countdownTime = 30f;  // Tiempo de cuenta atrás inicial (en segundos)
    private bool isCountingDown = true;

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
            countdownTime -= Time.deltaTime;  // Reducir el tiempo cada frame

            // Actualizar el valor de la barra para reflejar el tiempo restante
            countdownBar.value = countdownTime / 30f;  // Rellenar la barra proporcionalmente

            if (countdownTime <= 0)
            {
                countdownTime = 0;
                isCountingDown = false;
                Debug.Log("¡Tiempo agotado!");
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
    }
}
