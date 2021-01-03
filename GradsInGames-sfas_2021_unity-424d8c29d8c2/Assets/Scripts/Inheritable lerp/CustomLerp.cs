using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CustomLerp : MonoBehaviour
{
    //- Start Lerp by Calling StartLerp() Pass in the start end points then the high of a curve if one is wanted -//
    //- For Lerp in straight line pass in 0 height for the curve -//

    //- Public Variables -//
    [Header("Calculated Node Position")]
    [Tooltip("Larger the number Smoother The curve in the line, Recomended is 10")] public int m_lineSamplePoints = 10;
    [Tooltip("Speed At which the Character Object Moves")]                          public float m_movementSpeed;
    [Tooltip("This is the object that moves due to the points given")]              public GameObject m_Character;

    //- Private Variables -//
    [SerializeField] [Tooltip("Storing the calculated Points")]                     private Vector3[] m_nodesToTravel;
    [SerializeField] [Tooltip("Storing the calculated Points")]                     private Vector3 m_nextnodeToTravel;
    [SerializeField] [Tooltip("If The Lerp Is Active")]                             private bool m_isLerpActive;
    [SerializeField] [Tooltip("If The Lerp Is Active")]                             private bool m_lerpIsFinished;
    [SerializeField] [Tooltip("Stores Current Progress")]                           private int m_currentPoint;
    [SerializeField] [Tooltip("Time Since Start Lerp was called")]                  private float m_timer;


    // Update is called once per frame
    public void LerpUpdate()
    {
        if (m_isLerpActive)
        {
            m_timer += Time.deltaTime * m_movementSpeed;
            if (m_Character.transform.position != m_nextnodeToTravel)
            {
                m_Character.transform.position = Vector3.Lerp(m_nodesToTravel[0], m_nextnodeToTravel, m_timer);
            }
            else if (m_currentPoint == m_nodesToTravel.Length - 1 && Vector3.Distance(m_Character.transform.position, m_nodesToTravel[m_nodesToTravel.Length - 1]) < 2f)
            {
                m_isLerpActive = false;
                m_lerpIsFinished = true;
            }
            else if (m_currentPoint < m_nodesToTravel.Length - 1)
            {
                m_currentPoint++;
                m_nextnodeToTravel = m_nodesToTravel[m_currentPoint];
            }
        }
    }

    public void StartLerp(Vector3 a_positionOne, Vector3 a_positionTwo, float af_curveHeight)
    {
        m_lerpIsFinished = false;
        m_isLerpActive = true;
        //- calculate inbetween point and increase hight by set amount -//
        Vector3 Point2 = new Vector3(((a_positionOne.x + a_positionTwo.x) / 2), (((a_positionOne.y + a_positionTwo.y) / 2) + af_curveHeight), ((a_positionOne.z + a_positionTwo.z) / 2));
        //- calculate Bezier Curve with 3 points and save the amount of sample points to lerp character with -//
        m_nodesToTravel = GetQuadraticCurvePointsArray(a_positionOne, Point2, a_positionTwo);

        //- Resetting Lerp Variables -//
        m_currentPoint = 0;
        m_timer = 0;
        m_nextnodeToTravel = m_nodesToTravel[m_currentPoint];
    }

    public bool IsLerpCompleated()
    {
        return m_lerpIsFinished;
    }

    public bool IsLerpRunning()
    {
        return m_isLerpActive;
    }

    private Vector3[] GetQuadraticCurvePointsArray(Vector3 a_point0, Vector3 a_point1, Vector3 a_point2)
    {

        Vector3[] curvePositions = new Vector3[m_lineSamplePoints + 2];
        curvePositions[0] = a_point0;
        curvePositions[m_lineSamplePoints + 1] = a_point2;

        for (int i = 2; i < m_lineSamplePoints + 2; i++)
        {
            float t = i / (float)m_lineSamplePoints;
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
