using UnityEngine;
using System.Collections;

public class ActivarMatarMarciano : MonoBehaviour
{
    [Header("Referencia")]
    [SerializeField] private Transform marciano;
    [SerializeField] private Transform jugador;

    [Header("Activación")]
    [SerializeField] private float radio = 2f;
    [SerializeField] private KeyCode tecla = KeyCode.E;
    [SerializeField] private float delayAntesActivar = 2f;
    [SerializeField] private AudioSource audiosource;

    [Header("Opcional: Offset de activación")]
    [SerializeField] private Vector3 offsetMarciano = Vector3.zero;
    [SerializeField] private Vector3 offsetJugador = Vector3.zero;

    [HideInInspector] public bool matarMarciano = false;

    private bool coroutineRunning = false;

    private void Start()
    {
        audiosource = GetComponent<AudioSource>();
    }

    private void Update()
    {
        if (matarMarciano || coroutineRunning) return;

        Vector3 posMarciano = marciano.position + offsetMarciano;
        Vector3 posJugador = jugador.position + offsetJugador;

        float distancia = Vector3.Distance(posMarciano, posJugador);

        if (distancia <= radio)
        {
            Debug.Log($"Distancia: {distancia:F2} / Radio: {radio}");
            if (Input.GetKeyDown(tecla))
            {
                audiosource.Play();
                StartCoroutine(ActivateAfterDelay());
            }
        }
    }

    private IEnumerator ActivateAfterDelay()
    {
        coroutineRunning = true;
        yield return new WaitForSeconds(delayAntesActivar);
        matarMarciano = true;
        coroutineRunning = false;
    }

    private void OnDrawGizmosSelected()
    {
        if (marciano == null) return;
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(marciano.position + offsetMarciano, radio);
    }
}