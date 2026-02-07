using UnityEngine;
using Unity.Cinemachine;
using System.Collections;

public class StaticShakeCamera : MonoBehaviour
{
    [Header("Camera References")]
    [SerializeField] private CinemachineCamera mainCamera;
    [SerializeField] private CinemachineCamera staticShakeCamera;

    [Header("Player Reference")]
    [SerializeField] private MonoBehaviour playerMovementScript;

    [Header("External Condition")]
    [SerializeField] private MonoBehaviour conditionScript;
    [SerializeField] private string conditionFieldName = "matarMarciano";

    [Header("Activation")]
    [SerializeField] private float activationDelay = 1.5f;

    [Header("Shake Settings")]
    [SerializeField] private float shakeAmplitude = 1.5f;
    [SerializeField] private float shakeFrequency = 2f;
    [SerializeField] private NoiseSettings noiseProfile;

    [Header("Zoom Settings")]
    [SerializeField] private float zoomedFOV = 40f;

    private bool isStaticCameraActive = false;
    private bool alreadyTriggered = false;
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

        var lens = staticShakeCamera.Lens;
        lens.FieldOfView = zoomedFOV;
        staticShakeCamera.Lens = lens;
    }

    private void Update()
    {
        if (alreadyTriggered || conditionScript == null) return;

        var field = conditionScript.GetType().GetField(conditionFieldName);
        if (field != null && field.FieldType == typeof(bool))
        {
            bool value = (bool)field.GetValue(conditionScript);
            if (value)
            {
                alreadyTriggered = true;
                StartCoroutine(ActivateAfterDelay());
            }
        }
    }

    private IEnumerator ActivateAfterDelay()
    {
        yield return new WaitForSeconds(activationDelay);
        ActivateStaticCamera();
    }

    private void ActivateStaticCamera()
    {
        isStaticCameraActive = true;

        mainCamera.Priority.Value = 5;
        staticShakeCamera.Priority.Value = 10;

        if (playerMovementScript != null)
            playerMovementScript.enabled = false;
    }
}
