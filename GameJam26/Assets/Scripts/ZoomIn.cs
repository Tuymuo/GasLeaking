using UnityEngine;

public class ZoomIn : MonoBehaviour
{
    [SerializeField] private RectTransform menuUI; // Tu menú principal
    [SerializeField] private float zoomTime = 1.2f; // Duración
    [SerializeField] private float startScale = 0f; // Escala inicial del menú
    [SerializeField] private float moveZ = -500f; // Comienza más atrás en Z

    private void Start()
    {
        menuUI.localScale = Vector3.one * startScale;
        menuUI.localPosition += new Vector3(0, 0, moveZ);
    }

    public void PlayZoomIn(System.Action onComplete = null)
    {
        StartCoroutine(ZoomInCoroutine(onComplete));
    }

    private System.Collections.IEnumerator ZoomInCoroutine(System.Action onComplete)
    {
        float elapsed = 0f;
        Vector3 targetScale = Vector3.one;
        Vector3 startPos = menuUI.localPosition;
        Vector3 endPos = startPos - new Vector3(0, 0, moveZ);

        while (elapsed < zoomTime)
        {
            elapsed += Time.deltaTime;
            float t = elapsed / zoomTime;

            menuUI.localScale = Vector3.Lerp(Vector3.one * startScale, targetScale, t);
            menuUI.localPosition = Vector3.Lerp(startPos, endPos, t);

            yield return null;
        }

        menuUI.localScale = targetScale;
        menuUI.localPosition = endPos;

        onComplete?.Invoke();
    }
}