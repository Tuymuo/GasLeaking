using UnityEngine;

public class ButtonBounce : MonoBehaviour
{
    private Vector3 originalScale;

    private void OnEnable()
    {
        originalScale = transform.localScale;
        LeanTween.cancel(gameObject);
    }

    public void Bounce()
    {
        LeanTween.cancel(gameObject);

        LeanTween.scale(gameObject, originalScale * 0.9f, 0.08f)
            .setEaseOutQuad()
            .setOnComplete(() =>
            {
                LeanTween.scale(gameObject, originalScale * 1.1f, 0.08f)
                    .setEaseOutQuad()
                    .setOnComplete(() =>
                    {
                        LeanTween.scale(gameObject, originalScale, 0.12f)
                            .setEaseOutBounce();
                    });
            });
    }
}