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
    private float CenterToGround;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        CenterToGround = GetComponent<SphereCollider>().bounds.extents.y;
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
        Vector3 movement = new Vector3(Input.GetAxis("Horizontal"), 0.0f, Input.GetAxis("Vertical"));
        movement = transform.TransformDirection(movement * PlayerMovementSpeed);
        movement.y = rb.velocity.y;
        rb.velocity = movement;
    }

    void Jump()
    {
        //- jump check -//
        if (Input.GetButtonDown("Jump") && isGrounded(0.1f))
        {
            GetComponent<Rigidbody>().velocity = (Vector3.up * 5);
        }

        //- slow decent when holding jump button and faster decent when space is just tapped -//
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
        Vector3 rotationInput = new Vector3(0.0f , ((Input.GetAxis("Mouse X") * rotationSpeed) * Time.deltaTime), 0.0f);
        rotation += rotationInput;
        TargetPosition.eulerAngles = rotation;
    }

    //- Query Functions -//
    bool isGrounded(float af_leeway)
    {
       //- fire raycast down and uses the distance to fround for length plus leeway to allow check to work on all terrain -//
        return Physics.Raycast(transform.position, -Vector3.up, CenterToGround + af_leeway);
    }
}
