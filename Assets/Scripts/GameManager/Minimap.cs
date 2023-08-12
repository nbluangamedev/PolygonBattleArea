using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Minimap : BaseManager<Minimap>
{
    public Transform player;

    private void LateUpdate()
    {
        Vector3 newPosition = player.position;
        newPosition.y = transform.position.y;
        transform.position = newPosition;
    }
}
