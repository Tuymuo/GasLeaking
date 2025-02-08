using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CountdownManager : MonoBehaviour
{
    public Slider countdownBar;  // Barra de progreso (Slider)
    public TextMeshProUGUI timerText; // Texto  que mostrará los segundos restantes
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
            countdownBar.value = countdownTime / 30f;  // Actualizar el valor de la barra

            if (countdownTime <= 0)
            {
                countdownTime = 0;
                isCountingDown = false;
                
            }

            
        }
    }

    public void AddTime(int additionalTime)
    {
        countdownTime += additionalTime;  // Añadir tiempo al contador
        if (countdownTime > 30) 
        {
            countdownTime = 30;  // Limitar el máximo a 30 segundos
        }
    }
}
