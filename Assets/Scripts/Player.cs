using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public float speed;
    public float runningSpeed;
    float currentSpeed;
    public float maxStamina = 100;
    public static float currentStamina;
    // new private Rigidbody rigidbody;
    public Animator animator;
    bool isRunning;
    public CharacterController cController;
    float turnSmoothVelocity;
    float turnSmoothTime = 0.25f;

    void Start()
    {
        // rigidbody = GetComponent<Rigidbody>();
        float currentSpeed = speed;
        currentStamina = maxStamina;
    }

    void FixedUpdate() // FixedUpdate para evitar que el personaje traspase las paredes
    {
        Movement();
    }

    void Movement()
    {
        // Movimiento básico

        float horizontal = Input.GetAxisRaw("Horizontal");
        float vertical = Input.GetAxisRaw("Vertical");
        Vector3 movement = new Vector3(horizontal, 0, vertical).normalized * currentSpeed;

        if(movement.magnitude >= 0.1f) 
        {
            // Todo este bodoque se encarga de la rotación en la que mira el personaje mientras se mueve

            float targetAngle = Mathf.Atan2(movement.x, movement.z) * Mathf.Rad2Deg;
            float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref turnSmoothVelocity, turnSmoothTime); // Suaviza la rotación

            transform.rotation = Quaternion.Euler(0, angle, 0);

            cController.Move(movement * speed * Time.deltaTime);
        }        

        // Running/walking animator

        if (Input.GetAxisRaw("Run") > 0 && movement != Vector3.zero && currentStamina > 0) // Si el player está corriendo
        {
            Run();
        }
        else
        {
            Walk();
        }

        if (movement == Vector3.zero && currentStamina <= maxStamina) // Si el player está quieto, regenera su stamina
        {
            RegenerarStamina();
        }

        if (currentStamina > maxStamina) // Para evitar que el player tenga más stamina de la cantidad máxima
        {
            currentStamina = maxStamina;
        }

        if(currentStamina <= 0)
        {
            Debug.Log("No tienes más stamina. Quédate quieto para regenerar tu stamina.");
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

    public void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Objective")) {
            if (Input.GetAxisRaw("Interact") > 0) {
                InteractableObjective objective = other.GetComponent<InteractableObjective>();
                objective.CompleteObjective();
            }
        } 
        else if (other.CompareTag("Enemy")) {
            if (Input.GetAxisRaw("Interact") > 0) {
                Enemy enemy = other.GetComponent<Enemy>();
                enemy.Kill();
                Debug.Log("Killing");
            }
        }
    }

    public void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("WorldExit")) {
            Debug.Log("Level Completed!");
        }
    }

    #endregion
}
