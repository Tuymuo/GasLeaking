using UnityEngine;
using TMPro;
using System.Collections.Generic;

public class LockPickingMinigame : MonoBehaviour
{
    public TextMeshProUGUI sequenceText; // El texto donde se muestra la secuencia
    public GameObject minigameUI; // UI del minijuego que incluye la secuencia de teclas
    private string[] possibleKeys = { "A", "S", "D", "F" };
    private List<string> currentSequence = new List<string>();
    private int currentIndex = 0;
    private bool isPlaying = false;

    public TextMeshProUGUI bubbleText; // Texto dentro de la burbuja

    void Start()
    {
        minigameUI.SetActive(false); // Inicialmente ocultamos el minijuego
    }

    public void StartMinigame()
    {
        if (isPlaying) return;

        isPlaying = true;
        minigameUI.SetActive(true); // Mostrar el minijuego
        GenerateSequence();
    }

    void GenerateSequence()
    {
        currentSequence.Clear();
        for (int i = 0; i < 4; i++)
        {
            currentSequence.Add(possibleKeys[Random.Range(0, possibleKeys.Length)]);
        }

        string sequenceString = string.Join(" - ", currentSequence);
        sequenceText.text = sequenceString; // Mostrar la secuencia en el UI del minijuego
        bubbleText.text = "Combinación: " + sequenceString; // Mostrar la secuencia en la burbuja

        currentIndex = 0;
        Debug.Log("Secuencia generada: " + sequenceString);
    }

    void Update()
    {
        if (!isPlaying) return;

        if (Input.anyKeyDown)
        {
            foreach (string key in possibleKeys)
            {
                if (Input.GetKeyDown(key.ToLower()))
                {
                    CheckInput(key);
                    return;
                }
            }
        }
    }

    void CheckInput(string keyPressed)
    {
        if (keyPressed == currentSequence[currentIndex])
        {
            currentIndex++;
            if (currentIndex == currentSequence.Count)
            {
                Debug.Log("¡Candado abierto!");
                EndMinigame(true);
            }
        }
        else
        {
            Debug.Log("Fallaste.");
            EndMinigame(false);
        }
    }

    void EndMinigame(bool success)
    {
        isPlaying = false;
        minigameUI.SetActive(false); // Ocultar el UI del minijuego

        if (success)
        {
            bubbleText.text = "¡Puerta desbloqueada!";
        }
        else
        {
            bubbleText.text = "Fallaste, intenta de nuevo.";
        }
    }
}
