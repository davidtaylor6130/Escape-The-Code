using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float PlayerMovementSpeed = 1.0f;
    public GameObject Player;
    public float rotationSpeed = 100.0f;
    public Transform TargetPosition;

    private Vector3 rotation;
    private Rigidbody rb;


    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        Move();
        Jump();
        Look();
    }

    void Move()
    {
        //- Movement Code -//
        Vector3 movement = new Vector3(Input.GetAxis("Horizontal"), 0.0f, Input.GetAxis("Vertical")); // <---Get Input From Unity Input System
        movement = transform.TransformDirection(movement * PlayerMovementSpeed);
        movement.y = rb.velocity.y;
        rb.velocity = movement;
    }

    void Jump()
    {
        if (Input.GetButtonDown("Jump"))
        {
            GetComponent<Rigidbody>().velocity = (Vector3.up * 5);
        }

        if (rb.velocity.y < 0)
        {
            rb.velocity += Vector3.up * Physics.gravity.y * (2.5f - 1) * Time.deltaTime;
        }
        else if (rb.velocity.y > 0 && !Input.GetButton("Jump"))
        {
            rb.velocity += Vector3.up * Physics.gravity.y * (2.0f - 1) * Time.deltaTime;
        }
    }

    void Look()
    {
        //Vector3 rotationInput = new Vector3(((Input.GetAxis("Mouse Y") * rotationSpeed) * Time.deltaTime), ((Input.GetAxis("Mouse X") * rotationSpeed) * Time.deltaTime), 0.0f);
        //rotation += rotationInput;
        //TargetPosition.eulerAngles = rotation;
    }
}
