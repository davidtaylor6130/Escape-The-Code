using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class ElectronicInteractable : MonoBehaviour
{
    [Header("Game State")]
    public GameStates GameState;

    [Header("Player Information")]
    public Transform EntryJumpPoint;
    public Transform ExitJumpPoint;
    public GameObject Player;
    public TypeOfMovement PlayerStateAfterInteraction = TypeOfMovement.NoMovement;
    public TypeOfMovement PlayerStateAfterExiting = TypeOfMovement.Normal;
    private PlayerMovement playerMovement;

    [Header("Object Information")]
    public CinemachineVirtualCamera VMCam;

    [Header("Graphic Elements")]
    public GameObject TakeControlGuiPrompt;
    public ParticleSystem SparkParticleEffect;
    public GameObject[] ElementsToOutline;
    
    [Header("Jump Bezier Curve")]
    public float JumpHeight = 0.0f;
    public float JumpMovementSpeed;
    
    [Header("Sound")]
    public AudioSource SparkSoundEffect;

    [Header("Misc")]
    public bool IsInitalComputer;

    void Start()
    {
        playerMovement = Player.GetComponent<PlayerMovement>();

        if (IsInitalComputer)
        {
            GameState.SetNewActiveCamera(VMCam);
            playerMovement.isControllingObject = true;
            playerMovement.SetPlayerMovementType(TypeOfMovement.NoMovement, WhenToExicute.Instant, null, null);
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.name == Player.name)
        {
            if (playerMovement.isControllingObject == true && Input.GetButtonDown("Possess")) // if controlling object exit object
            {
                //- Setting Internal Data -//
                playerMovement.isControllingObject = false; // Keeping Track of player controll for object
                TakeControlGuiPrompt.SetActive(true); // Toggle off and on Gui Element when controlling object

                //- Focus On Player -//
                GameState.SetPlayerCameraActive();

                //- Set/Animate Player Movement -//
                playerMovement.SetPlayerMovementType(PlayerStateAfterExiting, WhenToExicute.Instant, SparkSoundEffect, SparkParticleEffect);
                playerMovement.PlayerLerpTo(ExitJumpPoint.position, JumpHeight, JumpMovementSpeed);
            }
            else if(playerMovement.isControllingObject == false && Input.GetButtonDown("Possess")) // if not controlling object enter
            {
                //- Setting Internal Data -//
                playerMovement.isControllingObject = true; // Keeping Track of player control for specific object

                //- Focus On Object Camera -//
                GameState.SetNewActiveCamera(VMCam);

                //- Set/Animate Player Movement -//
                playerMovement.SetPlayerMovementType(PlayerStateAfterInteraction, WhenToExicute.AfterAnimation, SparkSoundEffect, SparkParticleEffect);
                playerMovement.PlayerLerpTo(EntryJumpPoint.position, JumpHeight, JumpMovementSpeed);
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.name == Player.name)
        {
            TakeControlGuiPrompt.SetActive(true);

            foreach (GameObject objToOutline in ElementsToOutline)
                objToOutline.layer = LayerMask.NameToLayer("Outline");
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.name == Player.name)
        {
            TakeControlGuiPrompt.SetActive(false);
            
            foreach (GameObject objToOutline in ElementsToOutline)
                objToOutline.layer = LayerMask.NameToLayer("Default");
        }
    }
}