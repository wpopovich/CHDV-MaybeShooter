using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public float speed;
    public float runningSpeed;
    new private Rigidbody rigidbody;

    // Start is called before the first frame update
    void Start()
    {
        rigidbody = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        Move();
    }

    void Move()
    {

        float actualSpeed = speed;
        if((Input.GetAxisRaw("Run") > 0)) {
            Debug.Log("Running");
            actualSpeed = runningSpeed;
        }
        Vector3 movement = new Vector3(Input.GetAxisRaw("Horizontal"), 0, Input.GetAxisRaw("Vertical")) * actualSpeed;
        rigidbody.MovePosition(transform.position + movement * Time.deltaTime);
    }
}
