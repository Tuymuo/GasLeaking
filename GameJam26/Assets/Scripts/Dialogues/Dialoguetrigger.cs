using System.Collections;
using TMPro;
using UnityEngine;

public class DialogueTrigger : MonoBehaviour
{
    [Header("Referencias")]
    [SerializeField] private GameObject dialogueBubble;
    [SerializeField] private TMP_Text dialogueTextMesh;
    [SerializeField] private Camera mainCamera;

    [Header("Diálogo")]
    [TextArea(3, 10)]
    [SerializeField] private string dialogueText; // TEXTO ÚNICO (multilínea)
    [SerializeField] private float letterDelay = 0.05f;
    [SerializeField] private float lineDelay = 1.2f;

    [Header("Zoom")]
    [SerializeField] private float zoomDistance = 2f;
    [SerializeField] private float zoomSpeed = 2f;
    [SerializeField] private bool lockZ = true;

    private string[] lines;
    private Coroutine dialogueCoroutine;
    private Coroutine zoomCoroutine;
    private Vector3 originalCameraPosition;
    private bool playerInside = false;

    private void Awake()
    {
        dialogueBubble.SetActive(false);
        dialogueTextMesh.text = "";

        if (mainCamera == null)
            mainCamera = Camera.main;

        originalCameraPosition = mainCamera.transform.position;

        // Divide el texto por líneas (compatible con Windows / Mac)
        lines = dialogueText.Split(new[] { "\r\n", "\n" }, System.StringSplitOptions.RemoveEmptyEntries);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player") || playerInside) return;

        playerInside = true;
        dialogueBubble.SetActive(true);

        dialogueCoroutine = StartCoroutine(PlayDialogue());

        if (zoomCoroutine != null) StopCoroutine(zoomCoroutine);
        zoomCoroutine = StartCoroutine(SmoothZoom(GetZoomPosition(), zoomSpeed));
    }

    private void OnTriggerExit(Collider other)
    {
        if (!other.CompareTag("Player")) return;
        ResetDialogue();
    }

    private IEnumerator PlayDialogue()
    {
        foreach (string line in lines)
        {
            dialogueTextMesh.text = "";

            foreach (char c in line)
            {
                dialogueTextMesh.text += c;
                yield return new WaitForSeconds(letterDelay);
            }

            yield return new WaitForSeconds(lineDelay);
        }

        ResetDialogue();
    }

    private void ResetDialogue()
    {
        playerInside = false;

        if (dialogueCoroutine != null)
            StopCoroutine(dialogueCoroutine);

        dialogueBubble.SetActive(false);
        dialogueTextMesh.text = "";

        if (zoomCoroutine != null) StopCoroutine(zoomCoroutine);
        zoomCoroutine = StartCoroutine(SmoothZoom(originalCameraPosition, zoomSpeed));
    }

    private Vector3 GetZoomPosition()
    {
        Transform target = transform.parent != null ? transform.parent : transform;
        Vector3 direction = (target.position - mainCamera.transform.position).normalized;
        Vector3 pos = mainCamera.transform.position + direction * zoomDistance;

        if (lockZ)
            pos.z = mainCamera.transform.position.z;

        return pos;
    }

    private IEnumerator SmoothZoom(Vector3 targetPos, float speed)
    {
        Vector3 startPos = mainCamera.transform.position;
        float t = 0f;

        while (t < 1f)
        {
            t += Time.deltaTime * speed;
            t = Mathf.SmoothStep(0f, 1f, t);
            mainCamera.transform.position = Vector3.Lerp(startPos, targetPos, t);
            yield return null;
        }

        mainCamera.transform.position = targetPos;
    }
}
