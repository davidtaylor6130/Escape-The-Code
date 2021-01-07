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
    Continuous = 4
};

public class SocketManager : MonoBehaviour
{
    [Header("SocketConnections")]
    public GameObject[] AllSockets;
    private SocketConnections nearestSocketToPlayer;



    // Start is called before the first frame update
    void Start()
    {
        
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
}
