using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Video;

public class CinematicInteraction : MonoBehaviour
{
    [Header("Cinematic")]
    [SerializeField] private GameObject cinematicCanvas;
    [SerializeField] private VideoPlayer video;
    [SerializeField] private string nextScene;

    [Header("Interaction")]
    [SerializeField] private Transform player;
    [SerializeField] private Transform activationObject;
    [SerializeField] private float activationRadius = 3f;
    [SerializeField] private KeyCode interactionKey = KeyCode.E;

    private bool hasPlayed = false;

    private void Awake()
    {
        cinematicCanvas.SetActive(false);

        if (video != null)
            video.Pause();
    }

    private void Update()
    {
        if (hasPlayed) return;
        if (player == null || activationObject == null) return;

        float distance = Vector3.Distance(player.position, activationObject.position);

        if (distance <= activationRadius && Input.GetKeyDown(interactionKey))
        {
            Debug.Log($"Interaction key {interactionKey} pressed!");
            PlayCinematic();
        }
    }

    private void PlayCinematic()
    {
        hasPlayed = true;

        if (cinematicCanvas != null)
            cinematicCanvas.SetActive(true);

        if (video != null)
        {
            video.loopPointReached += OnVideoFinished;
            video.Play();
        }
    }

    private void OnVideoFinished(VideoPlayer vp)
    {
        vp.loopPointReached -= OnVideoFinished; // desuscribir
        SceneManager.LoadScene(nextScene);
        Time.timeScale = 1f;
    }
}
