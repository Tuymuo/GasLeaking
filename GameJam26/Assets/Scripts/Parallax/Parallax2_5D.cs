using UnityEngine;
//Fondo → 0.2
//Medio → 0.5
//Primer plano → 1
public class Parallax2_5D : MonoBehaviour
{
    public Transform cam; // Cámara principal
    public float parallaxFactor = 0.5f; // Velocidad relativa
    private Vector3 previousCamPos;

    void Start()
    {
        if (!cam)
            cam = Camera.main.transform;
        previousCamPos = cam.position;
    }

    void Update()
    {
        Vector3 delta = cam.position - previousCamPos;

        // Movimiento solo en X e Y
        transform.position += new Vector3(delta.x * parallaxFactor, delta.y * parallaxFactor, 0);

        previousCamPos = cam.position;
    }
}
