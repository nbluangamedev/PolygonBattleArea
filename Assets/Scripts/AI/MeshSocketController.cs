using System.Collections.Generic;
using UnityEngine;

public class MeshSocketController : MonoBehaviour
{
    Dictionary<SocketID,MeshSocket> socketMap = new Dictionary<SocketID,MeshSocket>();

    void Start()
    {
        MeshSocket[] sockets = GetComponentsInChildren<MeshSocket>();
        foreach (MeshSocket socket in sockets)
        {
            socketMap[socket.socketID] = socket;
        }
    }

    public void Attach(Transform objectTransform, SocketID socketID)
    {
        socketMap[socketID].Attach(objectTransform);
    }
}