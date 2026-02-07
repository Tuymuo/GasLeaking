using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class LogoTransition : MonoBehaviour
{
    [Header("Imágenes")]
    [SerializeField] private Image blackScreen; // Pantalla negra
    [SerializeField] private Image devLogo;
    [SerializeField] private Image gameLogo;

    [Header("Tiempos")]
    [SerializeField] private float fadeTime = 1f;
    [SerializeField] private float displayTime = 1.5f;

    [Header("Escena")]
    [SerializeField] private string sceneName;

    private void Start()
    {
        blackScreen.gameObject.SetActive(true);
        devLogo.gameObject.SetActive(true);
        gameLogo.gameObject.SetActive(true);

        blackScreen.color = new Color(0,0,0,1);
        devLogo.color = new Color(1,1,1,0);
        gameLogo.color = new Color(1,1,1,0);

        StartCoroutine(PlaySequence());
    }

    private System.Collections.IEnumerator PlaySequence()
    {
        // 1️⃣ Dev logo fade in
        yield return FadeImage(blackScreen, 0f, 0f, fadeTime/3);
        yield return new WaitForSeconds(displayTime/4);
        yield return FadeImage(blackScreen, 1f, 0f, fadeTime/3);

        yield return FadeImage(devLogo, 0f, 1f, fadeTime);
        yield return new WaitForSeconds(displayTime);
        yield return FadeImage(devLogo, 1f, 0f, fadeTime);

        yield return FadeImage(blackScreen, 0f, 0f, fadeTime/3);
        yield return new WaitForSeconds(0);
        yield return FadeImage(blackScreen, 0f, 0f, fadeTime/3);

        // 2️⃣ Game logo fade in
        yield return FadeImage(gameLogo, 0f, 1f, fadeTime);
        yield return new WaitForSeconds(displayTime);
        yield return FadeImage(gameLogo, 1f, 0f, fadeTime);

        // 3️⃣ Fade final a negro y cambiar escena
        yield return FadeImage(blackScreen, 0f, 1f, fadeTime);
        SceneManager.LoadScene(sceneName);
    }

    private System.Collections.IEnumerator FadeImage(Image img, float from, float to, float time)
    {
        float elapsed = 0f;
        Color c = img.color;
        while (elapsed < time)
        {
            elapsed += Time.deltaTime;
            float alpha = Mathf.Lerp(from, to, elapsed / time);
            img.color = new Color(c.r, c.g, c.b, alpha);
            yield return null;
        }
        img.color = new Color(c.r, c.g, c.b, to);
    }
}
