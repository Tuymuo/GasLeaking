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
        video.Pause();
    }

    private void Update()
    {
        if (hasPlayed) return;

        float distance = Vector3.Distance(player.position, activationObject.position);

        if (distance <= activationRadius && Input.GetKeyDown(interactionKey))
        {
            PlayCinematic();
        }
    }

    private void PlayCinematic()
    {
        hasPlayed = true;
        cinematicCanvas.SetActive(true);
        video.Play();
        video.loopPointReached += OnVideoFinished;
    }

    private void OnVideoFinished(VideoPlayer vp)
    {
        SceneManager.LoadScene(nextScene);
    }
}