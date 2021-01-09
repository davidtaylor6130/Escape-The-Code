using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public enum Direction
{
    Up = 0,
    Down = 1,
    Left = 2,
    Right = 3
};

public class SocketManager : MonoBehaviour
{
    [Header("SocketConnections")]
    public GameObject[] AllSockets;

    private SocketConnections nearestSocketToPlayer;
    private LineRenderer[] Connections;

    // Start is called before the first frame update
    void Start()
    {
        Connections = this.gameObject.GetComponentsInChildren<LineRenderer>();
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
        foreach (LineRenderer lineRenderer in Connections)
            lineRenderer.enabled = VisualLinesOn;
        
    }
}
