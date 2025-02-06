using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterController : MonoBehaviour
{
    public float velocidad;
    private Rigidbody2D rigidBody;
    private bool mirandoDerecha=true;

    Vector2 movement;

    private Animator animator;

 
    // Start is called before the first frame update
    private void Start()
    {
        rigidBody = GetComponent<Rigidbody2D>();
        animator= GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        ProcesarMovimiento();
        movement.x = Input.GetAxisRaw("Horizontal");
        movement.y = Input.GetAxisRaw("Vertical");
    }
    void ProcesarMovimiento(){
       
        //Logica de movimiento
        float inputhorizontal = Input.GetAxis("Horizontal");
        
        if (inputhorizontal != 0f){
            animator.SetBool("IsRunning",true);
        }
        else {
            animator.SetBool("IsRunning",false);
        }
        
        float inputvertical = Input.GetAxis("Vertical");

        rigidBody.velocity= new Vector2(inputhorizontal * velocidad, rigidBody.velocity.y);
        
        rigidBody.MovePosition(rigidBody.position + movement * velocidad * Time.fixedDeltaTime);
        
        GestionarOrientacion(inputhorizontal);

    }
    void GestionarOrientacion(float inputhorizontal)
    {
        if((mirandoDerecha== true&&inputhorizontal <0) || (mirandoDerecha == false && inputhorizontal >0)){

            mirandoDerecha = !mirandoDerecha;
            transform.localScale = new Vector2(-transform.localScale.x,transform.localScale.y);

        }
    }
}
