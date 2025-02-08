using UnityEngine;

public class HouseInteraction : MonoBehaviour
{
    private bool canInteract = false;

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            canInteract = true;
            Debug.Log("Presiona 'E' para forzar la cerradura.");
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            canInteract = false;
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
        // Aquí debes activar la UI del minijuego
    }
}
