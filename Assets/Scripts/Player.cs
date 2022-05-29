using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

public class Player : MonoBehaviour
{
    public CharacterController cController;
    float gravity = -9.81f;
    Vector3 velocity;

    public Inventory playerInventory;
    InventoryItem item;

    public float speed;
    public float runningSpeed;
    public float currentSpeed;
    public float maxStamina = 100;
    public static float currentStamina;

    public Animator animator;
    bool isAttacking;
    bool isRunning;
    bool isInteracting;
    bool doorIsOpened;

    public GameObject interactButton;

    [SerializeField]
    Image itemImage;

    public float turnSmoothTime = 0.25f;
    float turnSmoothVelocity;

    void Start()
    {
        currentStamina = maxStamina;
    }

    void Update()
    {
        if (!LevelManager.GetInstance().gameOver)
        {
            Movement();
        }

        InventoryItem item = GetComponent<Inventory>().item;

        if (item != null)
        {
            //itemImage.sprite = item.itemIcon;
            UIManager.GetInstance().ShowItemIcon(GetComponent<Inventory>().item.itemIcon);
        }
    }

    void Movement()
    {
        LockPlayerMovemementIfIsInteracting();
        DoGravity();

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

        // Running/walking

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
            RegenerateStamina();
        }

        if (currentStamina > maxStamina) // Para evitar que el player tenga más stamina de la cantidad máxima
        {
            currentStamina = maxStamina;
        }

        // Animator

        if (movement != Vector3.zero) // Player se está moviendo
        {
            animator.SetBool("IsWalking", true);
        }
        else // Player no se está moviendo
        {
            animator.SetBool("IsWalking", false);
        }

        if (isRunning)
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
    }

    void Walk()
    {
        currentSpeed = speed;
        isRunning = false;
    }

    void RegenerateStamina()
    {
        currentStamina += 18f * Time.deltaTime;
    }

    public void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Enemy") || 
            (other.CompareTag("Objective") && !hasObjectiveBeenCompleted(other) && ObjectiveManager.GetInstance().isCurrentObjective(other.gameObject)) || 
            (other.CompareTag("Door") && !doorIsOpened))
        {
            ShowInteractableButton(true);
        }

        if (Input.GetAxisRaw("Interact") > 0)
        {
            if (other.CompareTag("Enemy") && !isAttacking)
            {
                StartCoroutine(KillEnemy(other));
            }
            else if (other.CompareTag("Objective") && !hasObjectiveBeenCompleted(other) && 
                ObjectiveManager.GetInstance().isCurrentObjective(other.gameObject))
            {
                StartCoroutine(InteractWithObjective(other));
            }
            else if (other.CompareTag("Door"))
            {
                OpenDoor(other);
            }
        }
    }

    public IEnumerator KillEnemy(Collider enemyToKill)
    {
        animator.SetTrigger("Kill");
        isAttacking = true;

        yield return new WaitForSeconds(0.3f); // Para que la animación de la kill del player coordine con la animación de muerte del enemigo

        enemyToKill.GetComponent<Enemy>().Kill();
        InventoryItem lootedItem = enemyToKill.GetComponent<Inventory>().LootInventory();
        playerInventory.SetInventory(lootedItem);
        

        InteractableObjective objective = enemyToKill.GetComponent<InteractableObjective>();
        if (objective != null)
            objective.GetComponent<InteractableObjective>().CompleteObjective();

        yield return new WaitForSeconds(0.5f); // Esperar a que termine la animación

        isAttacking = false;
        ShowInteractableButton(false);
    }

    public IEnumerator InteractWithObjective(Collider interactableObjective)
    {       
        animator.SetTrigger("Interact");
        isInteracting = true;
        interactableObjective.GetComponent<InteractableObjective>().ChargeProgressBar();
        ShowInteractableButton(false);

        yield return new WaitForSeconds(7.3f); // Esperar a que termine la animación

        Debug.Log(interactableObjective.name);
        interactableObjective.GetComponent<InteractableObjective>().CompleteObjective();
        isInteracting = false;
    }

    void OpenDoor(Collider door)
    {
        Animator doorAnimator = door.GetComponent<Animator>();
        doorAnimator.SetBool("doorIsOpened", true);
        doorIsOpened = true;
        ShowInteractableButton(false);
        FindObjectOfType<AudioManager>().Play("DoorOpening");
    }

    public bool hasObjectiveBeenCompleted(Collider objective)
    {
        return objective.GetComponent<InteractableObjective>().completed;
    }

    void LockPlayerMovemementIfIsInteracting()
    {
        // Para que cuando esté interactuando/atacando no se esté moviendo

        if (isAttacking || isInteracting)
        {
            currentSpeed = 0;
        }
    }

    void DoGravity()
    {
        velocity.y += gravity * Time.deltaTime;
        cController.Move(velocity * Time.deltaTime);
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
        if (other.CompareTag("Enemy") || other.CompareTag("Objective") || other.CompareTag("Door"))
        {
            ShowInteractableButton(false);
        }
    }

    void ShowInteractableButton(bool value)
    {
        interactButton.SetActive(value);
    }

    #endregion
}
