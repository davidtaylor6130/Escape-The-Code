using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class ElectronicInteractable : MonoBehaviour
{
    [Header("Game State")]
    public GameStates GameState;
    public Game GameRef;
    public outline outlineRef;

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
            GameRef.IsCurrenlyActive = true;
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

                //- Allow Computer to run if its a computer -//
                if (tag == "Computer" && GameRef != null)
                    GameRef.IsCurrenlyActive = false;

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

                if(tag == "Computer" && GameRef != null)
                    GameRef.IsCurrenlyActive = true;

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

            //- Turn outline color to birght green if computer is active or dark green for non interactable objects -//
            outlineRef.IsActiveComputer = (GameRef != null);
            
            //- Set Objects to outline ont the correct layer -//
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

    public void SetGameRef(Game a_GameRef)
    {
        GameRef = a_GameRef;
    }
}