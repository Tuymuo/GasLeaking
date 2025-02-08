using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float speed; // Velocidad del movimiento
    public float groundDist; // Distancia desde el suelo (para ajustar la altura del jugador)
    public LayerMask terrainLayer; // Capa del terreno
    public Rigidbody rb; // Referencia al Rigidbody
    public SpriteRenderer sr; // Referencia al SpriteRenderer
    public Animator animator; // Referencia al Animator del jugador (para animaciones)
    
    // Start is called before the first frame update
    void Start()
    {
        rb = gameObject.GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        RaycastHit hit;
        Vector3 castPos = transform.position;
        castPos.y += 1;
        if (Physics.Raycast(castPos, -transform.up, out hit, Mathf.Infinity, terrainLayer))
        {
            if (hit.collider != null)
            {
                Vector3 movePos = transform.position;
                movePos.y = hit.point.y + groundDist;
                transform.position = movePos;
            }
        }

        // Detectar el movimiento del jugador
        float x = Input.GetAxis("Horizontal"); // Movimiento horizontal
        float y = Input.GetAxis("Vertical"); // Movimiento vertical
        Vector3 moveDir = new Vector3(x, 0, y);

        // Actualizar la velocidad del Rigidbody para el movimiento
        rb.velocity = moveDir * speed;

        // Cambiar la animación dependiendo si el jugador se está moviendo
        if (x != 0 || y != 0)
        {
            // Si hay movimiento, activamos la animación de caminar
            animator.SetBool("IsWalking", true);
        }
        else
        {
            // Si no hay movimiento, activamos la animación de reposo
            animator.SetBool("IsWalking", false);
        }

        // Cambiar la dirección del Sprite (si el jugador se mueve a la izquierda o derecha)
        if (x != 0 && x < 0)
        {
            sr.flipX = true; // Girar el sprite si se mueve a la izquierda
        }
        else if (x != 0 && x > 0)
        {
            sr.flipX = false; // No girar el sprite si se mueve a la derecha
        }
    }
}
