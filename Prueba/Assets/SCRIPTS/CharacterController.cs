using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterController : MonoBehaviour
{
    // Velocidad de movimiento del personaje
    public float velocidad;

    // Referencia al componente Rigidbody2D para manejar la física
    private Rigidbody2D rigidBody;

    // Variable para saber si el personaje está mirando a la derecha
    private bool mirandoDerecha = true;

    // Referencia al Animator para controlar las animaciones
    private Animator animator;

    // Start se ejecuta una vez al inicio del juego
    private void Start()
    {
        // Obtener el Rigidbody2D del personaje
        rigidBody = GetComponent<Rigidbody2D>();

        // Obtener el Animator del personaje
        animator = GetComponent<Animator>();
    }

    // FixedUpdate se ejecuta en intervalos de tiempo fijos, ideal para la física
    private void FixedUpdate()
    {
        ProcesarMovimiento();
    }

    // Método que gestiona el movimiento del personaje
    void ProcesarMovimiento()
    {
        // Obtener la entrada del jugador (valores entre -1 y 1)
        float inputHorizontal = Input.GetAxisRaw("Horizontal");
        float inputVertical = Input.GetAxisRaw("Vertical");

        // Crear un vector de movimiento y normalizarlo para evitar velocidades mayores en diagonal
        Vector2 movimiento = new Vector2(inputHorizontal, inputVertical).normalized * velocidad;

        // Aplicar la velocidad al Rigidbody2D
        rigidBody.velocity = movimiento;

        // Gestionar animaciones basadas en la dirección del movimiento
        GestionarAnimaciones(inputHorizontal, inputVertical);

        // Ajustar la orientación del personaje (mirar izquierda o derecha)
        GestionarOrientacion(inputHorizontal);
    }

    // Método para controlar las animaciones del personaje
    void GestionarAnimaciones(float inputHorizontal, float inputVertical)
    {
        // Verificar si el personaje se está moviendo
        bool estaMoviendose = Mathf.Abs(inputHorizontal) > 0 || Mathf.Abs(inputVertical) > 0;

        // Si no hay movimiento, activar la animación Idle y salir del método
        if (!estaMoviendose)
        {
            animator.SetBool("IsRunning", false);
            animator.SetBool("IsUp", false);
            animator.SetBool("IsDown", false);
            return; // Salimos para evitar activar otras animaciones
        }

        // Si el personaje se mueve hacia arriba, activar la animación "IsUp" y desactivar "IsDown"
        if (inputVertical > 0)
        {
            animator.SetBool("IsUp", true);
            animator.SetBool("IsDown", false);
        }
        // Si el personaje se mueve hacia abajo, activar la animación "IsDown" y desactivar "IsUp"
        else if (inputVertical < 0)
        {
            animator.SetBool("IsDown", true);
            animator.SetBool("IsUp", false);
        }
        else
        {
            // Si no hay movimiento vertical, desactivar ambas animaciones
            animator.SetBool("IsUp", false);
            animator.SetBool("IsDown", false);
        }

        // Activar la animación "IsRunning" solo si hay movimiento horizontal y no está en "Up" o "Down"
        animator.SetBool("IsRunning", inputHorizontal != 0 && !animator.GetBool("IsUp") && !animator.GetBool("IsDown"));
    }

    // Método para cambiar la dirección en la que mira el personaje
    void GestionarOrientacion(float inputHorizontal)
    {
        // Si el personaje se mueve a la izquierda y estaba mirando a la derecha, o viceversa
        if ((mirandoDerecha && inputHorizontal < 0) || (!mirandoDerecha && inputHorizontal > 0))
        {
            // Cambiar la dirección en la que está mirando
            mirandoDerecha = !mirandoDerecha;

            // Invertir la escala en X para reflejar el personaje
            transform.localScale = new Vector2(-transform.localScale.x, transform.localScale.y);
        }
    }
}