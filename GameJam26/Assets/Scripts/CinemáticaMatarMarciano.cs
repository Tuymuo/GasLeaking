using UnityEngine;
using UnityEngine.Video;
using UnityEngine.SceneManagement;
using System.Collections;
using Unity.Cinemachine;

public class CinematicaLuegoShake : MonoBehaviour
{
    [Header("Referencia al marciano")]
    [SerializeField] private ActivarMatarMarciano marcianoScript; // El script donde matarMarciano se vuelve true

    [Header("Cinemática")]
    [SerializeField] private GameObject canvasCinematica;
    [SerializeField] private VideoPlayer videoPlayer;

    [Header("Shake/Zoom")]
    [SerializeField] private CinemachineCamera mainCamera;
    [SerializeField] private CinemachineCamera staticShakeCamera;
    [SerializeField] private float zoomFOV = 40f;
    [SerializeField] private float shakeAmplitude = 1.5f;
    [SerializeField] private float shakeFrequency = 2f;
    [SerializeField] private float duracionShake = 3f;

    [Header("Escena")]
    [SerializeField] private string sceneName;

    [Header("Delay opcional antes de shake")]
    [SerializeField] private float delayAntesShake = 0f;

    private bool coroutineRunning = false;

    private void Update()
    {
        if (!coroutineRunning && marcianoScript != null && marcianoScript.matarMarciano)
        {
            coroutineRunning = true;
            StartCoroutine(ReproducirCinematicaYLuegoShake());
        }
    }

    private IEnumerator ReproducirCinematicaYLuegoShake()
    {
        // 1️⃣ Activar cinematica
        if (canvasCinematica != null)
            canvasCinematica.SetActive(true);

        if (videoPlayer != null)
        {
            videoPlayer.Stop();
            videoPlayer.Play();
            yield return new WaitForSeconds((float)videoPlayer.clip.length);
        }

        if (canvasCinematica != null)
            canvasCinematica.SetActive(false);

        // 2️⃣ Delay opcional antes del shake
        if (delayAntesShake > 0f)
            yield return new WaitForSeconds(delayAntesShake);

        // 3️⃣ Activar shake/zoom
        var shakeComponent = staticShakeCamera.GetComponent<CinemachineBasicMultiChannelPerlin>();
        if (shakeComponent == null)
            shakeComponent = staticShakeCamera.gameObject.AddComponent<CinemachineBasicMultiChannelPerlin>();

        shakeComponent.AmplitudeGain = shakeAmplitude;
        shakeComponent.FrequencyGain = shakeFrequency;

        mainCamera.Priority.Value = 5;
        staticShakeCamera.Priority.Value = 10;

        var lens = staticShakeCamera.Lens;
        lens.FieldOfView = zoomFOV;
        staticShakeCamera.Lens = lens;

        // 4️⃣ Mantener shake/zoom durante duracionShake
        yield return new WaitForSeconds(duracionShake);

        // 5️⃣ Cambiar escena
        SceneManager.LoadScene(sceneName);
    }
}