using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Video;

public class CinematicPlayer : MonoBehaviour
{
    [SerializeField] private GameObject cinematicCanvas;
    [SerializeField] private VideoPlayer video;
    [SerializeField] private string nextScene;

    private void Awake()
    {
        cinematicCanvas.SetActive(false);
        video.Pause();
    }

    public void PlayCinematic()
    {
        cinematicCanvas.SetActive(true);
        video.Play();
        video.loopPointReached += OnVideoFinished;
    }

    private void OnVideoFinished(VideoPlayer vp)
    {
        SceneManager.LoadScene(nextScene);
    }
}