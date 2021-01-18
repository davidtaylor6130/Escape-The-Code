using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public enum Direction
{
    Up = 0,
    Down = 1,
    Left = 2,
    Right = 3,
    Up1,
    Down1,
    Left1,
    Right1
};

public class SocketManager : MonoBehaviour
{
    [Header("SocketConnections")]
    public GameObject[] AllSockets;

    private SocketConnections nearestSocketToPlayer;
    private SocketConnections[] Connections;

    // Start is called before the first frame update
    void Start()
    {
        Connections = this.gameObject.GetComponentsInChildren<SocketConnections>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetPlayersClosesNode(SocketConnections socket)
    {
        nearestSocketToPlayer = socket;
    }

    public Vector3 GetDirectionDestination(Direction a_direction)
    {
        return nearestSocketToPlayer.GetLocationToTravel(a_direction);
    }

    public bool isContinuousNode()
    {
        return nearestSocketToPlayer.continuousNode;
    }

    public void toggleVisualConnectionsOn(bool VisualLinesOn)
    {
        foreach (SocketConnections lineRenderer in Connections)
            lineRenderer.GetComponent<LineRenderer>().enabled = VisualLinesOn;
        
    }

    [ContextMenu("Refresh Connections")]
    public void toggleVisualLinesDebug()
    {
        Connections = this.gameObject.GetComponentsInChildren<SocketConnections>();
        foreach (SocketConnections lineRenderer in Connections)
        {
            lineRenderer.DrawConnections();
            lineRenderer.GetComponent<LineRenderer>().enabled = true;
        }
    }
}
