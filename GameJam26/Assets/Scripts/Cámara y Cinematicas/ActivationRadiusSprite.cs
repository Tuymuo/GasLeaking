using UnityEngine;
using System.Collections;

public class ActivationRadiusSprite : MonoBehaviour
{
    [SerializeField] private Transform player;
    [SerializeField] private float activationRadius = 3f;
    [SerializeField] private SpriteRenderer activationSprite;

    [Header("Animación")]
    [SerializeField] private float fadeTime = 0.25f;
    [SerializeField] private float scaleTime = 0.25f;

    private bool isVisible;
    private Coroutine currentRoutine;
    private Vector3 originalScale;

    private void Awake()
    {
        originalScale = activationSprite.transform.localScale;

        activationSprite.color = new Color(1, 1, 1, 0);
        activationSprite.transform.localScale = Vector3.zero;
    }

    private void Update()
    {
        if (player == null) return;

        float distance = Vector3.Distance(player.position, transform.position);

        if (distance <= activationRadius && !isVisible)
            Show();
        else if (distance > activationRadius && isVisible)
            Hide();
    }

    private void Show()
    {
        isVisible = true;

        if (currentRoutine != null)
            StopCoroutine(currentRoutine);

        currentRoutine = StartCoroutine(FadeAndScale(1f, originalScale));
    }

    private void Hide()
    {
        isVisible = false;

        if (currentRoutine != null)
            StopCoroutine(currentRoutine);

        currentRoutine = StartCoroutine(FadeAndScale(0f, Vector3.zero));
    }

    private IEnumerator FadeAndScale(float targetAlpha, Vector3 targetScale)
    {
        float startAlpha = activationSprite.color.a;
        Vector3 startScale = activationSprite.transform.localScale;

        float t = 0f;
        float duration = Mathf.Max(fadeTime, scaleTime);

        while (t < duration)
        {
            t += Time.deltaTime;
            float lerp = t / duration;

            float a = Mathf.Lerp(startAlpha, targetAlpha, lerp);
            activationSprite.color = new Color(1, 1, 1, a);

            activationSprite.transform.localScale =
                Vector3.Lerp(startScale, targetScale, lerp);

            yield return null;
        }

        activationSprite.color = new Color(1, 1, 1, targetAlpha);
        activationSprite.transform.localScale = targetScale;
    }
}
