using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public float speed;
    public float runningSpeed;
    float actualSpeed;
    new private Rigidbody rigidbody;
    public Animator animator;

    // Start is called before the first frame update
    void Start()
    {
        rigidbody = GetComponent<Rigidbody>();
        float actualSpeed = speed;
    }

    // Update is called once per frame
    void Update()
    {
        Movement();
    }

    void Movement()
    {
        // Sneaking

        float horizontal = Input.GetAxisRaw("Horizontal");
        float vertical = Input.GetAxisRaw("Vertical");

        Vector3 movement = new Vector3(horizontal, 0, vertical) * actualSpeed;
        rigidbody.MovePosition((transform.position + movement * Time.deltaTime));

        // Running

        if (Input.GetKeyDown(KeyCode.LeftShift)) // Player está corriendo
        {
            actualSpeed = runningSpeed;
        }
        else
        {
             actualSpeed = speed;
        }

        // Animator

        if(movement != Vector3.zero) // Player se está moviendo
        {
            animator.SetBool("IsSneaking", true);
        }

        else // Player no se está moviendo
        {
            animator.SetBool("IsSneaking", false);
        }
    }
}
