using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

[System.Serializable]
public enum TypeOfMovement
{
    Normal = 0,
    DOP,
    NoMovement
}

public class PlayerMovement : MonoBehaviour
{
    [Header("Game State")]
    [Tooltip("Game State Controller")]                      public GameStates GameState;

    [Header("Universal Player Movement Data")]
    [Tooltip("Setting Player Object Refrance")]             public GameObject Player;
    [SerializeField] [Tooltip("Current Movement Tyoe")]     private TypeOfMovement CurrentMovementType;
    [SerializeField] [Tooltip("Mouse Rotation Input")]      private Vector3 rotation;
    [SerializeField] [Tooltip("RigidBody Refrance")]        private Rigidbody rb;
    [SerializeField] [Tooltip("Distance to ground")]        private float CenterToGround;

    [Header("Normal Player Movement")]
    [Tooltip("Cinemachine Virtual Camera Ref")]             public CinemachineVirtualCamera VirtualCameraControl;
    [Tooltip("Normal Movement Particle Effect")]            public ParticleSystem NormalParticleEffect;
    [Tooltip("Camera Follow and Look Point")]               public Transform TargetPosition;
    [Tooltip("How Fast The Player Moves")]                  public float PlayerMovementSpeed = 1.0f;
    [Tooltip("Mouse Sensitivity")]                          public float rotationSpeed = 100.0f;
    
    [Header("Data Over Power (D.O.P) Movement")]
    [Tooltip("Cinimachine D.O.P virtual camera")]           public CinemachineVirtualCamera DOPVertualCamera;
    [Tooltip("Yellow Electricity Particle Effect")]         public ParticleSystem DOPParticleEffect;
    [Tooltip("Lerp")]                                       public CustomLerp DOPLerp;

    [SerializeField] [Tooltip("Starting Power Socket")]     private GameObject StartingSocket;
    [SerializeField] [Tooltip("Destination Power Socket")]  private GameObject DestinationSocket;

    // Start is called before the first frame update
    void Start()
    {
        //- Get Refrances and Get/Calculate Data -//
        rb = GetComponent<Rigidbody>();
        CenterToGround = GetComponent<SphereCollider>().bounds.extents.y;

        //- Setting Player for Game Start in Interactable Object -//
        NormalParticleEffect.Stop();
        DOPParticleEffect.Stop();
    }

    // Update is called once per frame
    void Update()
    {
        switch(CurrentMovementType)
        {
            case TypeOfMovement.Normal:
                NormalMovement();
                break;

            case TypeOfMovement.DOP:
                DOPMovement();
                break;

            default:
                Debug.LogError("Type Of Movement Not attached to Player Movement update Loop Please and and try again");
                break;
        }
    }

    void NormalMovement()
    {
        if (GameState.IsPlayerActive == TypeOfMovement.Normal)
        {
            //- Movement Code -//
            Vector3 movement = new Vector3(Input.GetAxis("Horizontal"), 0.0f, Input.GetAxis("Vertical"));
            movement = transform.TransformDirection(movement * PlayerMovementSpeed);
            movement.y = rb.velocity.y;
            rb.velocity = movement;

            //- Check For Jump and add velocity upwards -//
            if (Input.GetButtonDown("Jump") && isGrounded(0.1f))
                GetComponent<Rigidbody>().velocity = (Vector3.up * 5);

            //- Gravity Calculations -//
            if (rb.velocity.y < 0)
                rb.velocity += Vector3.up * Physics.gravity.y * (2.5f - 1) * Time.deltaTime;
            //- If jump button isnt pressed slow fall -//
            else if (rb.velocity.y > 0 && !Input.GetButton("Jump"))
                rb.velocity += Vector3.up * Physics.gravity.y * (2.0f - 1) * Time.deltaTime;
            

            //- Camera Movement -//
            Vector3 rotationInput = new Vector3(((Input.GetAxis("Mouse Y") * rotationSpeed) * Time.deltaTime), ((Input.GetAxis("Mouse X") * rotationSpeed) * Time.deltaTime), 0.0f);
            rotation += rotationInput;
            TargetPosition.eulerAngles = rotation;

            //- Camera Zoom -//
            if (Input.GetAxis("Mouse ScrollWheel") != 0.0f)
            {
                float lf_distance = VirtualCameraControl.GetCinemachineComponent<Cinemachine3rdPersonFollow>().CameraDistance += Input.GetAxis("Mouse ScrollWheel");
                VirtualCameraControl.GetCinemachineComponent<Cinemachine3rdPersonFollow>().CameraDistance = Mathf.Clamp(lf_distance, 1, 4);
            }
        }
    }

    void DOPMovement()
    {

    }

    //- Query Functions -//
    bool isGrounded(float af_leeway)
    {
       //- fire raycast down and uses the distance to fround for length plus leeway to allow check to work on all terrain -//
        return Physics.Raycast(transform.position, -Vector3.up, CenterToGround + af_leeway);
    }
}
