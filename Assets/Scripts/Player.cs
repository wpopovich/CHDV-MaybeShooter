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
        DarInstruccionesDeJuego();
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

        // Movimiento adelante y atrás
        transform.Translate(movement * Time.deltaTime);

        // Rotación izquierda y derecha
        if (Input.GetKey(KeyCode.A))
        {
            RotarIzquierda();
        }
        if (Input.GetKey(KeyCode.D))
        {
            RotarDerecha();
        }

        // Running/walking

        // El player cuenta con una barra de stamina o energía, la cual si está cargada le permite correr por un tiempo determinado.

        if (Input.GetKey(KeyCode.LeftShift) && movement != Vector3.zero && currentStamina > 0) // Player está corriendo
        {
            Run();
        }
        else
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

    void RotarIzquierda()
    {
        transform.Rotate(new Vector3(0, -150 * Time.deltaTime, 0));
    }

    void RotarDerecha()
    {
        transform.Rotate(new Vector3(0, 150 * Time.deltaTime, 0));
    }

    void DarInstruccionesDeJuego()
    {
        Debug.Log("W y S para mover el personaje adelante y atrás. A y D para rotar. E para neutralizar enemigos y para tomar el objetivo. LShift para correr (consume stamina)");
    }

    public void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Objective")) {
            if (Input.GetAxisRaw("Interact") > 0) {
                InteractableObjective objective = other.GetComponent<InteractableObjective>();
                objective.CompleteObjective();
            }
        } else if (other.CompareTag("Enemy")) {
            if (Input.GetAxisRaw("Interact") > 0) {
                Enemy enemy = other.GetComponent<Enemy>();
                enemy.Kill();
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
