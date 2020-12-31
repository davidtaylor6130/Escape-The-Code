using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float PlayerMovementSpeed = 1.0f;
    public GameObject Player;
    public Rigidbody CC;


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 movement = new Vector3(Input.GetAxis("Horizontal"), 0.0f, Input.GetAxis("Vertical")); // <---Get Input From Unity Input System
        movement = transform.TransformDirection(movement * PlayerMovementSpeed);
        CC.velocity = movement;
    }
}
