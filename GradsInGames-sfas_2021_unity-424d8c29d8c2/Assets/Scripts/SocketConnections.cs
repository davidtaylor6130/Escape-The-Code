using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct ConnectionInfo
{
    public Transform transform;
    public Direction direction;
}

public class SocketConnections : MonoBehaviour
{
    [Header("Controls")]
    [Tooltip("If Node is continuous the player wont stop there and will carry on in the same direction")]
    public bool continuousNode;

    [Header("Player")]
    public GameObject Player;

    [Header("SocketManager")]
    public SocketManager manager;

    [Header("Current Connections")]
    public ConnectionInfo[] Connections;
    private LineRenderer Renderers;

    public void Start()
    {
        DrawConnections();
    }

    public void DrawConnections()
    {
        Renderers = this.gameObject.GetComponent<LineRenderer>();
        Renderers.positionCount = Connections.Length * 2;

        for (int i = 0; i < Connections.Length; i++)
        {
            Renderers.SetPosition((i * 2), this.gameObject.transform.position);
            Renderers.SetPosition((i * 2) + 1, Connections[i].transform.position);
        }
    }

    public Vector3 GetLocationToTravel(Direction direction)
    {
        foreach (ConnectionInfo connection in Connections)
        {
            if (connection.direction == direction)
            {
                return connection.transform.position;
            }
        }
        return new Vector3(0, 0, 0);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.name == Player.name)
            manager.SetPlayersClosesNode(this);
    }

    //private void OnTriggerExit(Collider other)
    //{
    //    if (other.gameObject.name == Player.name)
    //        manager.SetPlayersClosesNode(null);
    //}
}
