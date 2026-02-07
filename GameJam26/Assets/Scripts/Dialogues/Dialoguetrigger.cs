using System.Collections;
using TMPro;
using UnityEngine;

public class DialogueTrigger : MonoBehaviour
{
    [Header("Referencias")]
    [SerializeField] private GameObject dialogueBubble; // Burbuja
    [SerializeField] private TMP_Text dialogueTextMesh; // TMP 3D dentro de la burbuja
    [SerializeField] private Camera mainCamera;         // Cámara principal

    [Header("Diálogo")]
    [TextArea]
    [SerializeField] private string dialogueText;
    [SerializeField] private float letterDelay = 0.05f;

    [Header("Zoom")]
    [SerializeField] private float zoomDistance = 2f; // qué tan cerca de la piedra
    [SerializeField] private float zoomSpeed = 2f;    // velocidad del zoom
    [SerializeField] private bool lockZ = true;       // mantener Z original para 2.5D lateral

    private Coroutine typingCoroutine;
    private Coroutine zoomCoroutine;
    private Vector3 originalCameraPosition;

    private void Awake()
    {
        dialogueBubble.SetActive(false);
        dialogueTextMesh.text = "";

        if (mainCamera == null)
            mainCamera = Camera.main;

        originalCameraPosition = mainCamera.transform.position;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player")) return;

        // Mostrar burbuja y typewriter
        dialogueBubble.SetActive(true);
        if (typingCoroutine != null) StopCoroutine(typingCoroutine);
        typingCoroutine = StartCoroutine(TypeText());

        // Zoom hacia la piedra
        if (zoomCoroutine != null) StopCoroutine(zoomCoroutine);
        zoomCoroutine = StartCoroutine(SmoothZoom(GetZoomPosition(), zoomSpeed));
    }

    private void OnTriggerExit(Collider other)
    {
        if (!other.CompareTag("Player")) return;

        // Ocultar burbuja y finalizar typewriter
        dialogueBubble.SetActive(false);
        if (typingCoroutine != null)
        {
            StopCoroutine(typingCoroutine);
            typingCoroutine = null;
        }
        dialogueTextMesh.text = dialogueText;

        // Zoom de regreso
        if (zoomCoroutine != null) StopCoroutine(zoomCoroutine);
        zoomCoroutine = StartCoroutine(SmoothZoom(originalCameraPosition, zoomSpeed));
    }

    private IEnumerator TypeText()
    {
        dialogueTextMesh.text = "";
        foreach (char c in dialogueText)
        {
            dialogueTextMesh.text += c;
            yield return new WaitForSeconds(letterDelay);
        }
    }

   private Vector3 GetZoomPosition()
{
    // Usa la posición de la piedra (parent del TriggerZone)
    Transform stone = transform.parent != null ? transform.parent : transform;

    // Dirección desde cámara hacia piedra
    Vector3 direction = (stone.position - mainCamera.transform.position).normalized;

    // Solo movemos X y Y para 2.5D lateral
    Vector3 target = mainCamera.transform.position + direction * zoomDistance;
    target.z = mainCamera.transform.position.z; // mantiene Z fijo

    return target;
}


    private IEnumerator SmoothZoom(Vector3 targetPos, float speed)
    {
        Vector3 startPos = mainCamera.transform.position;
        float t = 0f;

        while (t < 1f)
        {
            t += Time.deltaTime * speed;
            t = Mathf.SmoothStep(0f, 1f, t); // easing suave
            mainCamera.transform.position = Vector3.Lerp(startPos, targetPos, t);
            yield return null;
        }

        mainCamera.transform.position = targetPos;
    }

    // Opcional: método público para saltar el typewriter
    public void SkipTyping()
    {
        if (typingCoroutine != null)
        {
            StopCoroutine(typingCoroutine);
            typingCoroutine = null;
            dialogueTextMesh.text = dialogueText;
        }
    }
}
