using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Player : MonoBehaviour
{
    public CharacterController cController;

    public float speed;
    public float runningSpeed;
    public float sneakingSpeed;
    public float currentSpeed;
    public float maxStamina = 100;
    public static float currentStamina;

    public Animator animator;
    bool isAttacking = false;
    bool isInteracting = false;
    bool isCrouching = false;
    bool isRunning;

    public GameObject worldCanvas;
    public GameObject interactButton;

    //public float taskDuration = 7f;
    //float currentTaskTime;
    //bool taskIsFinished = false;

    public float turnSmoothTime = 0.25f;
    float turnSmoothVelocity;

    void Start()
    {
        currentStamina = maxStamina;
        //currentTaskTime = taskDuration;

    }

    void Update()
    {
        if (!LevelManager.GetInstance().gameOver) {
            Movement();
        }
    }

    void Movement()
    {
        LockPlayerMovemementIfIsInteracting();

        // Movimiento básico

        float horizontal = Input.GetAxisRaw("Horizontal");
        float vertical = Input.GetAxisRaw("Vertical");
        Vector3 movement = new Vector3(horizontal, 0, vertical).normalized * currentSpeed;

        if (movement.magnitude >= 0.1f)
        {
            // Todo este bodoque se encarga de la rotación en la que mira el personaje mientras se mueve

            float targetAngle = Mathf.Atan2(movement.x, movement.z) * Mathf.Rad2Deg;
            float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref turnSmoothVelocity, turnSmoothTime); // Suaviza la rotación

            transform.rotation = Quaternion.Euler(0, angle, 0);

            cController.Move(movement * speed * Time.deltaTime);
        }

        // Running/walking/sneaking

        if (Input.GetAxisRaw("Run") > 0 && movement != Vector3.zero && currentStamina > 0) // Si el player está corriendo
        {
            Run();
        }
        else if (isCrouching == false)
        {
            Walk();
        }
        else
        {
            Sneaking();
        }

        if (Input.GetKeyDown(KeyCode.LeftControl))
        {
            if (isCrouching == false)
            {
                isCrouching = true;
            }
            else
            {
                isCrouching = false;
            }
        }

        if (movement == Vector3.zero && currentStamina <= maxStamina) // Si el player está quieto, regenera su stamina
        {
            RegenerateStamina();
        }

        if (currentStamina > maxStamina) // Para evitar que el player tenga más stamina de la cantidad máxima
        {
            currentStamina = maxStamina;
        }

        // Animator

        if (movement != Vector3.zero && isCrouching == false) // Player se está moviendo
        {
            animator.SetBool("IsWalking", true);
        }
        else // Player no se está moviendo
        {
            animator.SetBool("IsWalking", false);
        }

        if (isCrouching == true)
        {
            animator.SetBool("isCrouching", true);
        }
        else
        {
            animator.SetBool("isCrouching", false);
        }

        if (movement != Vector3.zero && isCrouching == true)
        {
            animator.SetBool("isSneaking", true);
        }
        else
        {
            animator.SetBool("isSneaking", false);
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

    #region Métodos y funciones

    void Run()
    {
        currentSpeed = runningSpeed;
        currentStamina -= 32f * Time.deltaTime;
        isRunning = true;
        isCrouching = false;
    }

    void Walk()
    {
        currentSpeed = speed;
        isRunning = false;
    }

    void Sneaking()
    {
        isCrouching = true;
        currentSpeed = sneakingSpeed;
    }

    void RegenerateStamina()
    {
        currentStamina += 18f * Time.deltaTime;
    }

    public void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Enemy") || other.CompareTag("Objective"))
        {
            interactButton.SetActive(true);
        }

        if (Input.GetAxisRaw("Interact") > 0)
        {
            if (other.CompareTag("Enemy") && !isAttacking)
            {
                StartCoroutine(KillEnemy(other));
            }
            else if (other.CompareTag("Objective"))
            {
                StartCoroutine(InteractWithComputer(other));
            }
        }
    }

    public IEnumerator KillEnemy(Collider enemyToKill)
    {
        animator.SetTrigger("Kill");
        isAttacking = true;

        yield return new WaitForSeconds(0.3f); // Para que la animación de la kill del player coordine con la animación de muerte del enemigo

        enemyToKill.GetComponent<Enemy>().Kill();

        yield return new WaitForSeconds(0.5f);

        isAttacking = false;
        interactButton.SetActive(false);
    }

    public IEnumerator InteractWithComputer(Collider interactableObjective)
    {
        animator.SetTrigger("Interact");
        isInteracting = true;
        worldCanvas.SetActive(true);

        yield return new WaitForSeconds(7.3f);

        interactableObjective.GetComponent<InteractableObjective>().CompleteObjective();
        isInteracting = false;
        worldCanvas.SetActive(false);
        interactButton.SetActive(false);
    }

    void LockPlayerMovemementIfIsInteracting()
    {
        // Para que cuando esté interactuando/atacando no se esté moviendo

        if (isAttacking || isInteracting)
        {
            currentSpeed = 0;
        }
    }

    public void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("WorldExit"))
        {
            Debug.Log("Level Completed!");
        }
    }
    
    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Enemy") || other.CompareTag("Objective"))
        {
            interactButton.SetActive(false);
        }
    }

    void ShowInteractableButton()
    {
        interactButton.SetActive(true);
    }

    #endregion
}
