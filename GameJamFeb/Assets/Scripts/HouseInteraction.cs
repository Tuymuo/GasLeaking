using UnityEngine;

public class HouseInteraction : MonoBehaviour
{
    private bool canInteract = false;
    public LockPickingMinigame lockPickingMinigame; // 🔹 Añadimos la referencia pública al minijuego

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
        if (lockPickingMinigame != null) // Evita errores si no está asignado
        {
            lockPickingMinigame.StartMinigame();
        }
        else
        {
            Debug.LogError("No se ha asignado el LockPickingMinigame en el Inspector.");
        }
    }
}
