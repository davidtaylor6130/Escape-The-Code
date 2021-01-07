using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class ElectronicInteractable : MonoBehaviour
{
    [Header("Game State")]
    public GameStates GameState;

    [Header("Player Information")]
    public Transform JumpPoint;
    public GameObject Player;
    public Vector3 PlayersPreviousPosition;
    public TypeOfMovement PlayerStateAfterInteraction = TypeOfMovement.NoMovement;
    public TypeOfMovement PlayerStateAfterExiting = TypeOfMovement.Normal;
    private PlayerMovement playerMovement;

    [Header("Object Information")]
    public bool isControllingObject = false;
    public CinemachineVirtualCamera VMCam;

    [Header("Graphic Elements")]
    public GameObject TakeControlGuiPrompt;
    public ParticleSystem SparkParticleEffect;
    
    [Header("Jump Bezier Curve")]
    public float JumpHeight = 0.0f;
    public float JumpMovementSpeed;
    
    [Header("Sound")]
    public AudioSource SparkSoundEffect;

    void Start()
    {
        playerMovement = Player.GetComponent<PlayerMovement>();
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.name == Player.name)
        {
            if (isControllingObject == true && Input.GetButtonDown("Possess")) // if controlling object exit object
            {
                //- Setting Internal Data -//
                isControllingObject = false; // Keeping Track of player controll for object
                TakeControlGuiPrompt.SetActive(true); // Toggle off and on Gui Element when controlling object

                //- Focus On Player -//
                GameState.SetPlayerCameraActive();

                //- Set/Animate Player Movement -//
                playerMovement.SetPlayerMovementType(PlayerStateAfterExiting, WhenToExicute.Instant, SparkSoundEffect, SparkParticleEffect);
                playerMovement.PlayerLerpTo(PlayersPreviousPosition, JumpHeight, JumpMovementSpeed);
            }
            else if(isControllingObject == false && Input.GetButtonDown("Possess")) // if not controlling object enter
            {
                //- Setting Internal Data -//
                isControllingObject = true; // Keeping Track of player control for specific object
                PlayersPreviousPosition = Player.transform.position;

                //- Focus On Object Camera -//
                GameState.SetNewActiveCamera(VMCam);

                //- Set/Animate Player Movement -//
                playerMovement.SetPlayerMovementType(PlayerStateAfterInteraction, WhenToExicute.AfterAnimation, SparkSoundEffect, SparkParticleEffect);
                playerMovement.PlayerLerpTo(JumpPoint.position, JumpHeight, JumpMovementSpeed);
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.name == Player.name)
            TakeControlGuiPrompt.SetActive(true);
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.name == Player.name)
            TakeControlGuiPrompt.SetActive(false);
    }
}