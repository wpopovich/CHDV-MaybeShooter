using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public float speed;
    public float runningSpeed;
    float currentSpeed;
    public float maxStamina = 100;
    float currentStamina;
    new private Rigidbody rigidbody;
    public Animator animator;
    bool isRunning;

    void Start()
    {
        rigidbody = GetComponent<Rigidbody>();
        float currentSpeed = speed;
        currentStamina = maxStamina;
    }

    void FixedUpdate() // FixedUpdate para evitar que el personaje traspase las paredes
    {
        Movement();
    }

    void Movement()
    {

        // Movimiento 

        float horizontal = Input.GetAxisRaw("Horizontal");
        float vertical = Input.GetAxisRaw("Vertical");

        Vector3 movement = new Vector3(0, 0, vertical) * currentSpeed;
        transform.Translate(movement * Time.deltaTime);

        if (Input.GetKey(KeyCode.A))
        {
            transform.Rotate(new Vector3(0, -150 * Time.deltaTime, 0));
        }
        if (Input.GetKey(KeyCode.D))
        {
            transform.Rotate(new Vector3(0, 150 * Time.deltaTime, 0));
        }

        //if(horizontal != 0 && vertical != 0)
        //{
        //    rigidbody.MovePosition(transform.position + movement * Time.deltaTime * 0.707f); // Para evitar que el personaje se mueva más rapido en las diagonales
        //}
        //else
        //{
        //    rigidbody.MovePosition(transform.position + movement * Time.deltaTime);
        //}

        // Running

        if (Input.GetKey(KeyCode.LeftShift) && movement != Vector3.zero && currentStamina > 0) // Player está corriendo
        {
            Run();
            
        }
        else // Player no está corriendo
        {
            Walk();
        }

        if (movement == Vector3.zero && currentStamina != maxStamina) // Si el player está quieto, regenera su stamina
        {
            RegenerarStamina();
        }

        if (currentStamina > maxStamina) // Para evitar que el player tenga más stamina de la cantidad máxima
        {
            currentStamina = maxStamina;
        }

        // Animator

        if (movement != Vector3.zero) // Player se está moviendo
        {
            animator.SetBool("IsSneaking", true);
        }

        else // Player no se está moviendo
        {
            animator.SetBool("IsSneaking", false);
        }

        if (isRunning == true)
        {
            animator.SetBool("IsRunning", true);
        }

        else
        {
            animator.SetBool("IsRunning", false);
        }

        if (Input.GetKey(KeyCode.A))
        {
            animator.SetBool("IsTurningLeft", true);
        }

        if (Input.GetKey(KeyCode.D))
        {
            animator.SetBool("IsTurningRight", true);
        }

        //if (Input.GetKeyUp(KeyCode.A))
        //{
        //    animator.SetBool("IsTurningLeft", false);
        //}
        //
        //if (Input.GetKeyUp(KeyCode.D))
        //{
        //    animator.SetBool("IsTurningRight", false);
        //}
    }

    #region Métodos

    void Run()
    {
        currentSpeed = runningSpeed;
        currentStamina -= 1f;
        isRunning = true;
    }

    void Walk()
    {
        currentSpeed = speed;
        isRunning = false;
    }

    void RegenerarStamina()
    {
        currentStamina += 0.6f;
    }

    #endregion
}