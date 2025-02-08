using UnityEngine;
using TMPro;

public class HouseInteraction : MonoBehaviour
{
    private bool canInteract = false;
    public LockPickingMinigame lockPickingMinigame;
    public GameObject interactionBubble; // Burbuja de interacción
    public TextMeshProUGUI bubbleText; // Texto dentro de la burbuja
    public DialogueWindow Dialogue;
    public string DialogueText;
    

    void Start()
    {

        interactionBubble.SetActive(false); // Asegurarse de que la burbuja está oculta al inicio
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            canInteract = true;
            Dialogue.Show(DialogueText);
            interactionBubble.SetActive(true); // Mostrar la burbuja cuando el jugador esté cerca

        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            canInteract = false;
           
            interactionBubble.SetActive(false); // Ocultar la burbuja cuando el jugador se aleje
            Dialogue.Close();
        }
    }

    void Update()
    {
        if (canInteract && Input.GetKeyDown(KeyCode.E))
        {
            StartLockPickingMinigame();
        }
    }

    void StartLockPickingMinigame()
    {
        Debug.Log("Iniciando minijuego de cerraduras...");
        if (lockPickingMinigame != null)
        {
            interactionBubble.SetActive(false); // Ocultar la burbuja al iniciar el minijuego
            lockPickingMinigame.StartMinigame(); // Pasar directamente el control al minijuego
        }
        else
        {
            Debug.LogError("No se ha asignado el LockPickingMinigame en el Inspector.");
        }
    }

    public void EndMinigame()
    {
        canInteract = true; // Permitir interacción nuevamente cuando el minijuego termine
    }
}
