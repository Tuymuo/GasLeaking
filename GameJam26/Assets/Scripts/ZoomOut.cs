using UnityEngine;

public class ZoomEffect : MonoBehaviour
{
    [Header("Target to Zoom")]
    [SerializeField] private Transform target; // Asignas aquí tu logo o menú

    [Header("Zoom Settings")]
    [SerializeField] private float zoomOutTime = 1f;
    [SerializeField] private float zoomInTime = 1f;

    [Header("Automatic Start")]
    [SerializeField] private bool zoomOutOnStart = false;
    [SerializeField] private bool zoomInOnStart = false;

    private void Start()
    {
        if (zoomOutOnStart && target != null)
            StartCoroutine(ZoomOut(target));
        else if (zoomInOnStart && target != null)
            StartCoroutine(ZoomIn(target));
    }

    public System.Collections.IEnumerator ZoomOut(Transform t)
    {
        Vector3 startScale = t.localScale;
        Vector3 endScale = Vector3.zero;
        float elapsed = 0f;

        while (elapsed < zoomOutTime)
        {
            elapsed += Time.deltaTime;
            t.localScale = Vector3.Lerp(startScale, endScale, elapsed / zoomOutTime);
            yield return null;
        }
        t.localScale = endScale;
    }

    public System.Collections.IEnumerator ZoomIn(Transform t)
    {
        Vector3 startScale = Vector3.zero;
        Vector3 endScale = Vector3.one;
        float elapsed = 0f;

        while (elapsed < zoomInTime)
        {
            elapsed += Time.deltaTime;
            t.localScale = Vector3.Lerp(startScale, endScale, elapsed / zoomInTime);
            yield return null;
        }
        t.localScale = endScale;
    }
}