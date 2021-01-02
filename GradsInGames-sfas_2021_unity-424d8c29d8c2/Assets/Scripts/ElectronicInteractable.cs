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
    public bool isControllingObject;
    public CinemachineVirtualCamera VMCam;

    [Header("Graphic Elements")]
    public GameObject TakeControlGuiPrompt;
    public ParticleSystem SparkParticleEffect;
    
    [Header("Jump Bezier Curve")]
    public int CurveSamplePoints;
    public float TravelSpeed = 1.0f;
    public float JumpHeight = 10.0f;
    public LineRenderer lr;
    public Vector3[] JumpPoints;
    private bool StartLerp;
    private float timer;
    private Vector3 CurrentPositionHolder;
    private Vector3 StartPosition;
    private int Currentpoint;
    private bool Entering;

    [Header("Sound")]
    public AudioSource SparkSoundEffect;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (StartLerp)
        {
            timer += Time.deltaTime * TravelSpeed;
            if (Player.transform.position != CurrentPositionHolder)
            {
                Player.transform.position = Vector3.Lerp(StartPosition, CurrentPositionHolder, timer);
            }
            else if (Currentpoint == JumpPoints.Length - 1 && Vector3.Distance(Player.transform.position, JumpPoint.position) < 1)
            {
                StartLerp = false;
                Player.GetComponent<Rigidbody>().isKinematic = false;
                if (Entering)
                {
                    EnteringExitingEffects();
                    Entering = false;
                }
            }
            else if (Currentpoint < JumpPoints.Length - 1)
            {
                Currentpoint++;
                CurrentPositionHolder = JumpPoints[Currentpoint];  
            }
        }
    }

    void BezierCalcuations(bool exiting)
    {
        StartLerp = true;
        if (exiting)
        {
            //- calculate inbetween point and increase hight by set amount -//
            Vector3 Point2 = new Vector3(((Player.transform.position.x + JumpPoint.position.x) / 2), (((Player.transform.position.y + JumpPoint.position.y) / 2) + JumpHeight), ((Player.transform.position.z + JumpPoint.position.z) / 2));
            //- calculate Bezier Curve with 3 points and save the amount of sample points to lerp character with -//
            JumpPoints = GetQuadraticCurvePointsArray(JumpPoint.position, Point2, PlayersPreviousPosition);
            
            EnteringExitingEffects();
            Debug.Log("Exting Effects Played");
        }
        else
        {
            //- calculate inbetween point and increase hight by set amount -//
            Vector3 Point2 = new Vector3(((Player.transform.position.x + JumpPoint.position.x) / 2), (((Player.transform.position.y + JumpPoint.position.y) / 2) + JumpHeight), ((Player.transform.position.z + JumpPoint.position.z) / 2));
            //- calculate Bezier Curve with 3 points and save the amount of sample points to lerp character with -//
            JumpPoints = GetQuadraticCurvePointsArray(Player.transform.position, Point2, JumpPoint.position);
            Entering = true;
        }

        //- Resetting Lerp Variables -//
        Currentpoint = 0;
        timer = 0;
        StartPosition = Player.transform.position;
        CurrentPositionHolder = JumpPoints[Currentpoint];
        
        lr.positionCount = JumpPoints.Length;
        lr.SetPositions(JumpPoints);

        Player.GetComponent<Rigidbody>().isKinematic = true;
    }

    void EnteringExitingEffects()
    {
        //- Play Particle and sound effects -//
        SparkParticleEffect.Play();
        SparkSoundEffect.Play();

        //- toggle player emmiter and gui promt depending player emmiter status -//
        if (Entering)
        {
            Player.GetComponent<ParticleSystem>().Stop();
            TakeControlGuiPrompt.SetActive(false);
            //- Camera Zoom on the Interactable Object -//
            GameState.SetNewActiveCamera(VMCam);
        }
        else if (!Entering)
        {
            GameState.IsPlayerActive = true;
            isControllingObject = false;

            Player.GetComponent<ParticleSystem>().Play();
            TakeControlGuiPrompt.SetActive(true);
            //- Re-enable Players Camera -//
            GameState.SetPlayerCameraActive();
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.name == Player.name)
        {

            if (Input.GetKeyDown(KeyCode.E) && isControllingObject == false)
            {
                //- Save Players Entry point -//
                PlayersPreviousPosition = Player.transform.position;
                
                //- Stop Player Movement -//
                GameState.IsPlayerActive = false;
                isControllingObject = true;

                //- calculate player jump -//
                BezierCalcuations(false);
            }
            else if (Input.GetKeyDown(KeyCode.E) && isControllingObject == true)
            {
                //- calculate player jump -//
                BezierCalcuations(true);
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

    private Vector3[] GetQuadraticCurvePointsArray(Vector3 a_point0, Vector3 a_point1, Vector3 a_point2)
    {

        Vector3[] curvePositions = new Vector3[CurveSamplePoints + 2];
        curvePositions[0] = a_point0;
        curvePositions[CurveSamplePoints + 1] = a_point2;

        for (int i = 2; i < CurveSamplePoints + 2; i++)
        {
            float t = i / (float)CurveSamplePoints;
            curvePositions[i - 1] = CalculateQuadraticBezierPoint(t, a_point0, a_point1, a_point2);
        }
        return curvePositions;
    }

    private Vector3 CalculateQuadraticBezierPoint(float t, Vector3 a_point0, Vector3 a_point1, Vector3 a_point2)
    {
        float u = 1 - t;
        float tt = t * t;
        float uu = u * u;
        Vector3 CalculatedPoint = uu * a_point0;
        CalculatedPoint += 2 * u * t * a_point1;
        CalculatedPoint += tt * a_point2;
        return CalculatedPoint;
    }
}