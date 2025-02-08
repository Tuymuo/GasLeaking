using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using TMPro; // 🔹 Asegúrate de importar esto si usas TextMesh Pro

public class LockPickingMinigame : MonoBehaviour
{
    public TextMeshProUGUI sequenceText;
    //public TMP_Text sequenceText;
    public GameObject minigameUI; // Panel de UI del minijuego
    //public Text sequenceText; // Texto donde se muestra la secuencia de teclas
    private string[] possibleKeys = { "A", "S", "D", "F" };
    private List<string> currentSequence = new List<string>();
    private int currentIndex = 0;

    void Start()
    {
        minigameUI.SetActive(false);
    }

    public void StartMinigame()
    {
        minigameUI.SetActive(true);
        GenerateSequence();
    }

    void GenerateSequence()
    {
        currentSequence.Clear();
        for (int i = 0; i < 4; i++)
        {
            currentSequence.Add(possibleKeys[Random.Range(0, possibleKeys.Length)]);
        }
        sequenceText.text = string.Join(" - ", currentSequence);
        currentIndex = 0;
        Debug.Log("Secuencia generada: " + string.Join(" - ", currentSequence));  
    }

    void Update()
    {
        if (minigameUI.activeSelf && currentIndex < currentSequence.Count)
        {
            if (Input.anyKeyDown)
            {
                string keyPressed = Input.inputString.ToUpper();
                if (keyPressed == currentSequence[currentIndex])
                {
                    currentIndex++;
                    if (currentIndex == currentSequence.Count)
                    {
                        Debug.Log("¡Candado abierto!");
                        minigameUI.SetActive(false);
                    }
                }
                else
                {
                    Debug.Log("Error en la combinación.");
                    minigameUI.SetActive(false);
                }
            }
        }
    }
}
