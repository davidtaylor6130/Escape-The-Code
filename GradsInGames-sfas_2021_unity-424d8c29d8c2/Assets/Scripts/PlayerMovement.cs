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

[System.Serializable]
public enum WhenToExicute
{
    Instant = 0,
    AfterAnimation
}

public class PlayerMovement : MonoBehaviour
{
    [Header("Game State")]
    [Tooltip("Game State Controller")]                      public GameStates GameState;

    [Space(10)]

    [Header("Universal Player Movement Data")]
    [Tooltip("Setting Player Object Refrance")]             public GameObject Player;
    [Tooltip("CustomLerpScript to move player")]            public CustomLerp lerp;
    [Tooltip("Currently The Players Movement Type")]        public TypeOfMovement currentMovementType;
    private Vector3 rotation;
    private Rigidbody rb;
    private float CenterToGround;
    
    //- Var to save refrance to queued movementType Errors -//
    private bool waitToChangeMovementType;
    private TypeOfMovement desiredMovementType;
    private AudioSource soundEffectQueued;
    private ParticleSystem particleEffectQueued;

    [Space(10)]

    [Header("Normal Player Movement")]
    [Tooltip("Cinemachine Virtual Camera Ref")]             public CinemachineVirtualCamera VirtualCameraControl;
    [Tooltip("Normal Movement Particle Effect")]            public ParticleSystem NormalParticleEffect;
    [Tooltip("Camera Follow and Look Point")]               public Transform TargetPosition;
    [Tooltip("How Fast The Player Moves")]                  public float PlayerMovementSpeed = 1.0f;
    [Tooltip("Mouse Sensitivity")]                          public float rotationSpeed = 100.0f;
    
    [Space(10)]

    [Header("Data Over Power (D.O.P) Movement")]
    [Tooltip("Cinimachine D.O.P virtual camera")]           public CinemachineVirtualCamera DOPVertualCamera;
    [Tooltip("Yellow Electricity Particle Effect")]         public ParticleSystem DOPParticleEffect;
    [Tooltip("Lerp")]                                       public CustomLerp DOPLerp;
    private GameObject StartingSocket;
    private GameObject DestinationSocket;

    // Start is called before the first frame update
    void Start()
    {
        waitToChangeMovementType = false;

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
        //- Update Lerp -//
        lerp.LerpUpdate();
        
        //- If statement to change Movement type after lerp is done -//
        if (waitToChangeMovementType && lerp.IsLerpCompleated())
        {
            SetPlayerMovementType(desiredMovementType, WhenToExicute.Instant, soundEffectQueued, particleEffectQueued);
        }

        //- Run correct Movement code based on current movement -//
        switch(currentMovementType)
        {
            case TypeOfMovement.Normal:
                NormalMovement();
                break;

            case TypeOfMovement.DOP:
                DOPMovement();
                break;

            case TypeOfMovement.NoMovement:
                break;

            default:
                Debug.LogError("Type Of Movement Not attached to Player Movement update Loop Please and and try again");
                break;
        }
    }

    void NormalMovement()
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

    void DOPMovement()
    {
        Debug.Log("DOP Movement Running");




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

    public void SetPlayerMovementType(TypeOfMovement a_movementType, WhenToExicute a_whenToExicute, AudioSource a_audioSourceToPlay, ParticleSystem a_particleEffectToPlay)
    {
        switch (a_whenToExicute)
        {
            case WhenToExicute.Instant:
                //- Stop All Particle Effects -//
                NormalParticleEffect.Stop();
                DOPParticleEffect.Stop();

                //- Set internal movement type -//
                currentMovementType = a_movementType;

                //- Reactivate the correct particle effect -//
                switch (a_movementType)
                {
                    case TypeOfMovement.Normal:
                        GameState.SetPlayerCameraActive();
                        NormalParticleEffect.Play();
                        rb.isKinematic = false;
                        break;

                    case TypeOfMovement.DOP:
                        DOPParticleEffect.Play();
                        rb.isKinematic = true;
                        break;

                    case TypeOfMovement.NoMovement:
                        rb.isKinematic = true;
                        break;

                    default:
                        Debug.LogError("Not Recognised Movement Type Please Enter New type into switch statement");
                        break;
                }

                //- Playing Visual And Audio Effects -//
                a_audioSourceToPlay.Play();
                a_particleEffectToPlay.Play();

                //- if this was a queue change resets the bool for next use -//
                waitToChangeMovementType = false;
                break;

            case WhenToExicute.AfterAnimation:
                waitToChangeMovementType = true; // toggle if statement in update loop to start checking if lerp is done
                desiredMovementType = a_movementType; // save wanted movement type for later
                soundEffectQueued = a_audioSourceToPlay; // save ref to audio for future ref
                particleEffectQueued = a_particleEffectToPlay; // save ref to paticle for future refrances
                break;
        }
    }

    public void PlayerLerpTo(Vector3 endPos, float height = 0.0f)
    {
        lerp.StartLerp(Player.transform.position, endPos, height);
    }

    //- Query Functions -//
    bool isGrounded(float af_leeway)
    {
       //- fire raycast down and uses the distance to fround for length plus leeway to allow check to work on all terrain -//
        return Physics.Raycast(transform.position, -Vector3.up, CenterToGround + af_leeway);
    }
}
