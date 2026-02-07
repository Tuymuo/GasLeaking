using UnityEngine;
using UnityEngine.UI;

public class ButtonBounce : MonoBehaviour
{
    private Vector3 originalScale;

    private void Awake()
    {
        originalScale = transform.localScale;
    }

    public void Bounce()
    {
        // Cancel any previous tweens (important)
        LeanTween.cancel(gameObject);

        // Shrink a bit
        LeanTween.scale(gameObject, originalScale * 0.9f, 0.1f)
            .setEase(LeanTweenType.easeOutQuad)
            .setOnComplete(() =>
            {
                // Overshoot
                LeanTween.scale(gameObject, originalScale * 1.1f, 0.1f)
                    .setEase(LeanTweenType.easeOutQuad)
                    .setOnComplete(() =>
                    {
                        // Back to normal
                        LeanTween.scale(gameObject, originalScale, 0.1f)
                            .setEase(LeanTweenType.easeOutBounce);
                    });
            });
    }
}