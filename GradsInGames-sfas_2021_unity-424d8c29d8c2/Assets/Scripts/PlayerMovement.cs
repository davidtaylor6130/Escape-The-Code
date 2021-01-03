using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class PlayerMovement : MonoBehaviour
{
    public GameStates GameState;

    public float PlayerMovementSpeed = 1.0f;
    public GameObject Player;
    public float rotationSpeed = 100.0f;
    public Transform TargetPosition;
    public CinemachineVirtualCamera VirtualCameraControl;

    private Vector3 rotation;
    private Rigidbody rb;
    private float CenterToGround;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        CenterToGround = GetComponent<SphereCollider>().bounds.extents.y;

        //- Setting Player for Game Start in Interactable Object -//
        Player.GetComponent<ParticleSystem>().Stop();
    }

    // Update is called once per frame
    void Update()
    {
        if (GameState.IsPlayerActive)
        {
            Move();
            Jump();
            Look();
            Zoom();
        }
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
        Vector3 rotationInput = new Vector3(((Input.GetAxis("Mouse Y") * rotationSpeed) * Time.deltaTime), ((Input.GetAxis("Mouse X") * rotationSpeed) * Time.deltaTime), 0.0f);
        rotation += rotationInput;

        //rotation.x = Mathf.Clamp(rotation.x, 100.0f, 80.0f);

        TargetPosition.eulerAngles = rotation;
        //Player.transform.eulerAngles = rotation;
    }

    void Zoom()
    {
        if (Input.GetAxis("Mouse ScrollWheel") != 0.0f)
        {
            float lf_distance = VirtualCameraControl.GetCinemachineComponent<Cinemachine3rdPersonFollow>().CameraDistance += Input.GetAxis("Mouse ScrollWheel");
            VirtualCameraControl.GetCinemachineComponent<Cinemachine3rdPersonFollow>().CameraDistance = Mathf.Clamp(lf_distance, 1, 4);
        }
    }

    //- Query Functions -//
    bool isGrounded(float af_leeway)
    {
       //- fire raycast down and uses the distance to fround for length plus leeway to allow check to work on all terrain -//
        return Physics.Raycast(transform.position, -Vector3.up, CenterToGround + af_leeway);
    }
}
