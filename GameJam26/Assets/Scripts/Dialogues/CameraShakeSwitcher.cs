using UnityEngine;
using Unity.Cinemachine;

public class CameraShakeSwitcher : MonoBehaviour
{
    [Header("Camera References")]
    [SerializeField] private CinemachineCamera mainCamera;
    [SerializeField] private CinemachineCamera shakeCamera;
    
    [Header("Player Reference")]
    [SerializeField] private MonoBehaviour playerMovementScript;
    
    [Header("Settings")]
    [SerializeField] private KeyCode toggleKey = KeyCode.E;
    [SerializeField] private float transitionDuration = 1f;
    
    [Header("Shake Settings")]
    [SerializeField] private float shakeAmplitude = 1.5f;
    [SerializeField] private float shakeFrequency = 2f;
    [SerializeField] private NoiseSettings noiseProfile; // Drag "6D Shake" asset here
    
    [Header("Zoom Settings")]
    [SerializeField] private float normalZoom = 10f;
    [SerializeField] private float shakeZoom = 7f;
    [SerializeField] private bool useFOV = false;
    
    private bool isShakeCameraActive = false;
    private CinemachineBasicMultiChannelPerlin shakeComponent;
    
    private void Start()
    {
        // Get or add the shake component
        shakeComponent = shakeCamera.GetComponent<CinemachineBasicMultiChannelPerlin>();
        
        if (shakeComponent == null)
        {
            shakeComponent = shakeCamera.gameObject.AddComponent<CinemachineBasicMultiChannelPerlin>();
        }
        
        // Try to find noise profile if not assigned
        if (noiseProfile == null)
        {
            noiseProfile = FindNoiseProfile();
        }
        
        // Configure shake
        if (noiseProfile != null)
        {
            shakeComponent.NoiseProfile = noiseProfile;
            Debug.Log("✓ Shake noise profile applied!");
        }
        else
        {
            Debug.LogWarning("No Noise Profile assigned! Shake won't work. Assign '6D Shake' in Inspector.");
        }
        
        shakeComponent.AmplitudeGain = shakeAmplitude;
        shakeComponent.FrequencyGain = shakeFrequency;
        
        // Set initial priorities
        mainCamera.Priority.Value = 10;
        shakeCamera.Priority.Value = 5;
    }
    
    private NoiseSettings FindNoiseProfile()
    {
        NoiseSettings profile = Resources.Load<NoiseSettings>("6D Shake");
        if (profile != null) return profile;
        
        #if UNITY_EDITOR
        string[] guids = UnityEditor.AssetDatabase.FindAssets("t:NoiseSettings");
        if (guids.Length > 0)
        {
            string path = UnityEditor.AssetDatabase.GUIDToAssetPath(guids[0]);
            profile = UnityEditor.AssetDatabase.LoadAssetAtPath<NoiseSettings>(path);
            if (profile != null)
            {
                Debug.Log($"Found noise profile: {path}");
                return profile;
            }
        }
        #endif
        
        return null;
    }
    
    private void Update()
    {
        if (Input.GetKeyDown(toggleKey))
        {
            ToggleCamera();
        }
    }
    
    private void ToggleCamera()
    {
        isShakeCameraActive = !isShakeCameraActive;
        
        if (isShakeCameraActive)
        {
            // Switch to shake camera
            mainCamera.Priority.Value = 5;
            shakeCamera.Priority.Value = 10;
            
            // Apply zoom
            SetCameraZoom(shakeCamera, shakeZoom);
            
            // Disable player movement
            DisablePlayerMovement();
        }
        else
        {
            // Switch back to main camera
            mainCamera.Priority.Value = 10;
            shakeCamera.Priority.Value = 5;
            
            // Reset zoom
            SetCameraZoom(shakeCamera, normalZoom);
            
            // Enable player movement
            EnablePlayerMovement();
        }
    }
    
    private void SetCameraZoom(CinemachineCamera cam, float zoomValue)
    {
        if (useFOV)
        {
            // Adjust Field of View
            var lens = cam.Lens;
            lens.FieldOfView = zoomValue;
            cam.Lens = lens;
        }
        else
        {
            // Adjust camera distance using Follow Offset
            var followComponent = cam.GetComponent<CinemachineFollow>();
            if (followComponent != null)
            {
                Vector3 offset = followComponent.FollowOffset;
                offset.z = -zoomValue; // Negative for behind the target
                followComponent.FollowOffset = offset;
            }
            
            // Alternative: If using Third Person Follow
            var thirdPersonFollow = cam.GetComponent<CinemachineThirdPersonFollow>();
            if (thirdPersonFollow != null)
            {
                thirdPersonFollow.CameraDistance = zoomValue;
            }
        }
    }
    
    private void DisablePlayerMovement()
    {
        if (playerMovementScript != null)
        {
            playerMovementScript.enabled = false;
        }
    }
    
    private void EnablePlayerMovement()
    {
        if (playerMovementScript != null)
        {
            playerMovementScript.enabled = true;
        }
    }
    
    public void SetShakeIntensity(float amplitude, float frequency)
    {
        if (shakeComponent != null)
        {
            shakeComponent.AmplitudeGain = amplitude;
            shakeComponent.FrequencyGain = frequency;
        }
    }
}