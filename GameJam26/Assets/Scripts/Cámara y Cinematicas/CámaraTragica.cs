using UnityEngine;
using Unity.Cinemachine;
using System.Collections;
using UnityEngine.SceneManagement;

public class StaticShakeCamera : MonoBehaviour
{
    [Header("Cámaras")]
    [SerializeField] private CinemachineCamera mainCamera;
    [SerializeField] private CinemachineCamera staticShakeCamera;

    [Header("Estado del marciano")]
    [SerializeField] private ActivarMatarMarciano marcianoState;

    [Header("Zoom & Shake")]
    [SerializeField] private float zoomedFOV = 40f;
    [SerializeField] private float shakeAmplitude = 1.5f;
    [SerializeField] private float shakeFrequency = 2f;
    [SerializeField] private NoiseSettings noiseProfile;

    [Header("Duración zoom antes de transición")]
    [SerializeField] private float duracionZoom = 3f;

    [Header("Escena siguiente")]
    [SerializeField] private string sceneName;

    private bool triggered = false;
    private CinemachineBasicMultiChannelPerlin shakeComponent;

    private void Start()
    {
        shakeComponent = staticShakeCamera.GetComponent<CinemachineBasicMultiChannelPerlin>();
        if (shakeComponent == null)
            shakeComponent = staticShakeCamera.gameObject.AddComponent<CinemachineBasicMultiChannelPerlin>();

        shakeComponent.NoiseProfile = noiseProfile;
        shakeComponent.AmplitudeGain = shakeAmplitude;
        shakeComponent.FrequencyGain = shakeFrequency;

        mainCamera.Priority.Value = 10;
        staticShakeCamera.Priority.Value = 5;
    }

    private void Update()
    {
        if (!triggered && marcianoState.matarMarciano)
        {
            triggered = true;
            StartCoroutine(ActivateZoomSequence());
        }
    }

    private IEnumerator ActivateZoomSequence()
    {
        // 1️⃣ Cambiar a cámara estática
        mainCamera.Priority.Value = 5;
        staticShakeCamera.Priority.Value = 10;

        // 2️⃣ Aplicar zoom (FOV)
        var lens = staticShakeCamera.Lens;
        lens.FieldOfView = zoomedFOV;
        staticShakeCamera.Lens = lens;

        // 3️⃣ Esperar duración del zoom
        yield return new WaitForSeconds(duracionZoom);

        // 4️⃣ Cambiar escena
        SceneManager.LoadScene(sceneName);
    }
}
