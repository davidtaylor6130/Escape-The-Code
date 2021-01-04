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

    [Header("Object Information")]
    public bool isControllingObject = false;
    public CinemachineVirtualCamera VMCam;

    [Header("Graphic Elements")]
    public GameObject TakeControlGuiPrompt;
    public ParticleSystem SparkParticleEffect;
    
    [Header("Jump Bezier Curve")]
    public float JumpHeight = 10.0f;
    public CustomLerp lerp;
    private bool Entering;
    
    [Header("Sound")]
    public AudioSource SparkSoundEffect;

    // Update is called once per frame
    void Update()
    {
        lerp.LerpUpdate();
        if (lerp.IsLerpCompleated())
        {
            Player.GetComponent<Rigidbody>().isKinematic = false;
            if (Entering)
            {
                EnteringExitingEffects();
                Entering = false;
            }
        }
        else
        {
            Player.GetComponent<Rigidbody>().isKinematic = false;
        }
    }

    void EnteringExitingEffects()
    {
        //- Play Particle and sound effects -//
        SparkParticleEffect.Play();
        SparkSoundEffect.Play();

        //- toggle player emmiter and gui promt depending player emmiter status -//
        if (Entering)
        {
            Player.GetComponent<PlayerMovement>().NormalParticleEffect.Stop();
            TakeControlGuiPrompt.SetActive(false);
            //- Camera Zoom on the Interactable Object -//
            GameState.SetNewActiveCamera(VMCam);
            Player.transform.position = JumpPoint.position;
        }
        else if (!Entering)
        {
            GameState.IsPlayerActive = TypeOfMovement.Normal;
            isControllingObject = false;

            Player.GetComponent<PlayerMovement>().NormalParticleEffect.Play();
            TakeControlGuiPrompt.SetActive(true);
            //- Re-enable Players Camera -//
            GameState.SetPlayerCameraActive();
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.name == Player.name)
        {
            if (Input.GetButtonDown("Possess") && isControllingObject == false)
            {
                //- Save Players Entry point -//
                PlayersPreviousPosition = Player.transform.position;
                
                //- Stop Player Movement -//
                GameState.IsPlayerActive = TypeOfMovement.NoMovement;
                isControllingObject = true;
                Entering = true;
                
                //- turns off Player Gravity -//
                Player.GetComponent<Rigidbody>().isKinematic = true;

                //- calculate player jump -//
                lerp.StartLerp(Player.transform.position, JumpPoint.position, JumpHeight);
            }
            else if (Input.GetButtonDown("Possess") && isControllingObject == true)
            {
                //- Turns off Player Gravity -//
                Player.GetComponent<Rigidbody>().isKinematic = true;
                //- Play Effects -//
                EnteringExitingEffects();
                //- calculate player jump -//
                lerp.StartLerp(JumpPoint.position, PlayersPreviousPosition, JumpHeight);
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