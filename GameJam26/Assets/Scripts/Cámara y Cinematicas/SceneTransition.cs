using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.Events;

public class SceneTransition : MonoBehaviour
{
    public static SceneTransition Instance;

    [Header("Fade Image")]
    [SerializeField] private Image fadeImage; // La imagen que hará el fade
    [Header("Duración de Fade")]
    [SerializeField] private float fadeInDuration = 30f;
    [SerializeField] private float fadeOutDuration = 20f;

    [Header("Botón de transición (opcional)")]
    [SerializeField] private UnityEngine.UI.Button transitionButton;
    [SerializeField] private string targetSceneName;

    private void Awake()
    {
        // Singleton simple
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);

        // Asegurarse de que la imagen cubre toda la pantalla y está visible
        if (fadeImage != null)
        {
            fadeImage.gameObject.SetActive(true);
            fadeImage.color = new Color(fadeImage.color.r, fadeImage.color.g, fadeImage.color.b, 1f);
        }

        // Asignar evento al botón si se ha puesto
        if (transitionButton != null && !string.IsNullOrEmpty(targetSceneName))
        {
            transitionButton.onClick.AddListener(() => LoadScene(targetSceneName));
        }
    }

    private void Start()
    {
        // Fade in al inicio de la escena
        if (fadeImage != null)
            StartCoroutine(Fade(1f, 0f, fadeInDuration));
    }

    /// <summary>
    /// Llama a esta función para cargar otra escena con fade out
    /// </summary>
    /// <param name="sceneName"></param>
    public void LoadScene(string sceneName)
    {
        StartCoroutine(FadeAndLoad(sceneName));
    }

    private System.Collections.IEnumerator FadeAndLoad(string scene)
    {
        // Fade out
        if (fadeImage != null)
            yield return StartCoroutine(Fade(0f, 1f, fadeOutDuration));

        // Cargar escena de forma asíncrona
        AsyncOperation op = SceneManager.LoadSceneAsync(scene);
        while (!op.isDone)
            yield return null;

        // Fade in en la nueva escena
        if (fadeImage != null)
            yield return StartCoroutine(Fade(1f, 0f, fadeInDuration));
    }

    private System.Collections.IEnumerator Fade(float startAlpha, float endAlpha, float duration)
    {
        float t = 0f;
        Color c = fadeImage.color;

        while (t < duration)
        {
            t += Time.deltaTime;
            float alpha = Mathf.Lerp(startAlpha, endAlpha, t / duration);
            fadeImage.color = new Color(c.r, c.g, c.b, alpha);
            yield return null;
        }

        fadeImage.color = new Color(c.r, c.g, c.b, endAlpha);
    }
}
