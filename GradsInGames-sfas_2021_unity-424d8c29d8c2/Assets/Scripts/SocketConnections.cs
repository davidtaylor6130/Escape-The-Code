using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Direction 
{ 
    Up = 0,
    Down = 1,
    Left = 2,
    Right = 3,
    Continuous = 4
};

public class SocketConnections : MonoBehaviour
{
    [System.Serializable]
    public struct ConnectionInfo
    {
        public Transform transform;
        public Direction direction;
    }

    public ConnectionInfo[] Connections;
    private LineRenderer Renderers;

    public void Start()
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
}
