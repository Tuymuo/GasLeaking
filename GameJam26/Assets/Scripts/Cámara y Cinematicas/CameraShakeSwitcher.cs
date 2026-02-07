using UnityEngine;
using Unity.Cinemachine;

public class CameraShakeSwitcher : MonoBehaviour
{
    [Header("Camera References")]
    [SerializeField] private CinemachineCamera mainCamera;
    [SerializeField] private CinemachineCamera shakeCamera;
    
    [Header("Player Reference")]
    [SerializeField] private Transform playerTransform;
    [SerializeField] private MonoBehaviour playerMovementScript;
    
    [Header("Settings")]
    [SerializeField] private KeyCode toggleKey = KeyCode.E;
    [SerializeField] private float transitionDuration = 1f;
    
    [Header("Shake Settings")]
    [SerializeField] private float shakeAmplitude = 1.5f;
    [SerializeField] private float shakeFrequency = 2f;
    [SerializeField] private NoiseSettings noiseProfile;
    
    [Header("Zoom Settings")]
    [SerializeField] private float normalZoom = 10f;
    [SerializeField] private float shakeZoom = 7f;
    [SerializeField] private bool useFOV = false;
    
    [Header("NPC Proximity Check")]
    [SerializeField] private string npcTag = "NPC";
    [SerializeField] private float detectionRadius = 3f;
    
    [Header("NPC Camera Framing")]
    [SerializeField] private Vector3 cameraOffset = new Vector3(0, 1.5f, -3f); // Offset from NPC (X, Y, Z)
    [SerializeField] private Vector3 npcLookOffset = new Vector3(0, 1.5f, 0); // Look at NPC's head
    [SerializeField] private float positionSmoothSpeed = 5f; // How smoothly camera moves to NPC
    
    private bool isShakeCameraActive = false;
    private CinemachineBasicMultiChannelPerlin shakeComponent;
    private bool isNearNPC = false;
    private Transform currentNPC = null;
    
    private void Start()
    {
        Debug.Log($"=== CameraShakeSwitcher initialized ===");
        Debug.Log($"Detection Radius: {detectionRadius} units");
        
        // Auto-find player if not assigned
        if (playerTransform == null && mainCamera.Follow != null)
        {
            playerTransform = mainCamera.Follow;
            Debug.Log($"✓ Auto-assigned player from main camera Follow: {playerTransform.name}");
        }
        
        if (playerTransform == null)
        {
            Debug.LogError("⚠️ Player Transform not assigned!");
        }
        
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
            Debug.LogWarning("No Noise Profile assigned! Shake won't work.");
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
    
    private void LateUpdate()
    {
        if (mainCamera == null || shakeCamera == null) return;
        
        if (isShakeCameraActive && currentNPC != null)
        {
            // SHAKE CAMERA: Position based on NPC, not main camera
            Vector3 targetPosition = currentNPC.position + cameraOffset;
            
            // Smoothly move to target position
            shakeCamera.transform.position = Vector3.Lerp(
                shakeCamera.transform.position,
                targetPosition,
                Time.deltaTime * positionSmoothSpeed
            );
            
            // Look at NPC (with offset for head/face)
            Vector3 lookAtPoint = currentNPC.position + npcLookOffset;
            Vector3 direction = lookAtPoint - shakeCamera.transform.position;
            Quaternion targetRotation = Quaternion.LookRotation(direction);
            
            shakeCamera.transform.rotation = Quaternion.Slerp(
                shakeCamera.transform.rotation,
                targetRotation,
                Time.deltaTime * positionSmoothSpeed
            );
        }
        else
        {
            // When not active, just follow main camera (for smooth transition)
            shakeCamera.transform.position = mainCamera.transform.position;
            shakeCamera.transform.rotation = mainCamera.transform.rotation;
        }
    }
    
    private void Update()
    {
        // Check NPC proximity
        if (playerTransform != null)
        {
            CheckNPCProximity();
        }
        
        if (Input.GetKeyDown(toggleKey))
        {
            if (isNearNPC || isShakeCameraActive)
            {
                ToggleCamera();
            }
            else
            {
                Debug.Log($"❌ Cannot activate - not near NPC (need to be within {detectionRadius} units)");
            }
        }
        
        // DEBUG: Press P to check current status
        if (Input.GetKeyDown(KeyCode.P))
        {
            Debug.Log($"=== DEBUG INFO ===");
            Debug.Log($"Player Position: {(playerTransform != null ? playerTransform.position.ToString() : "NULL")}");
            Debug.Log($"Main Camera Position: {mainCamera.transform.position}");
            Debug.Log($"Shake Camera Position: {shakeCamera.transform.position}");
            Debug.Log($"Detection Radius: {detectionRadius}");
            Debug.Log($"Is Near NPC: {isNearNPC}");
            Debug.Log($"Current NPC: {(currentNPC != null ? currentNPC.name : "None")}");
            Debug.Log($"Shake Camera Active: {isShakeCameraActive}");
            
            if (playerTransform != null)
            {
                GameObject[] npcs = GameObject.FindGameObjectsWithTag(npcTag);
                Debug.Log($"NPCs found in scene: {npcs.Length}");
                foreach (var npc in npcs)
                {
                    float distance = Vector3.Distance(playerTransform.position, npc.transform.position);
                    string status = distance <= detectionRadius ? "✓ IN RANGE" : "✗ TOO FAR";
                    Debug.Log($"  - {npc.name} at distance: {distance:F2} {status}");
                }
            }
        }
    }
    
    private void CheckNPCProximity()
    {
        GameObject[] npcs = GameObject.FindGameObjectsWithTag(npcTag);
        
        Transform closestNPC = null;
        float closestDistance = float.MaxValue;
        
        // Find the closest NPC to the PLAYER
        foreach (var npc in npcs)
        {
            float distance = Vector3.Distance(playerTransform.position, npc.transform.position);
            if (distance < closestDistance)
            {
                closestNPC = npc.transform;
                closestDistance = distance;
            }
        }
        
        // Check if we're within detection radius of the closest NPC
        bool shouldBeNear = closestNPC != null && closestDistance <= detectionRadius;
        
        if (shouldBeNear && currentNPC == null)
        {
            // Just entered range
            currentNPC = closestNPC;
            isNearNPC = true;
            Debug.Log($"✓ PLAYER entered NPC range: {currentNPC.name} - Distance: {closestDistance:F2}");
        }
        else if (!shouldBeNear && currentNPC != null)
        {
            // Just left range
            Debug.Log($"PLAYER left NPC range: {currentNPC.name} - Distance: {closestDistance:F2}");
            currentNPC = null;
            isNearNPC = false;
            
            if (isShakeCameraActive)
            {
                ToggleCamera();
                Debug.Log("Auto-disabled shake camera (left NPC range)");
            }
        }
        else if (shouldBeNear)
        {
            // Still in range, update which NPC
            currentNPC = closestNPC;
            isNearNPC = true;
        }
    }
    
    private void ToggleCamera()
    {
        isShakeCameraActive = !isShakeCameraActive;
        
        if (isShakeCameraActive)
        {
            mainCamera.Priority.Value = 5;
            shakeCamera.Priority.Value = 10;
            SetCameraZoom(shakeCamera, shakeZoom);
            DisablePlayerMovement();
            Debug.Log($"→ SHAKE CAMERA ACTIVE - Centered on {currentNPC.name}");
        }
        else
        {
            mainCamera.Priority.Value = 10;
            shakeCamera.Priority.Value = 5;
            SetCameraZoom(shakeCamera, normalZoom);
            EnablePlayerMovement();
            Debug.Log("→ MAIN CAMERA ACTIVE");
        }
    }
    
    private void SetCameraZoom(CinemachineCamera cam, float zoomValue)
    {
        if (useFOV)
        {
            var lens = cam.Lens;
            lens.FieldOfView = zoomValue;
            cam.Lens = lens;
        }
        else
        {
            var followComponent = cam.GetComponent<CinemachineFollow>();
            if (followComponent != null)
            {
                Vector3 offset = followComponent.FollowOffset;
                offset.z = -zoomValue;
                followComponent.FollowOffset = offset;
            }
            
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