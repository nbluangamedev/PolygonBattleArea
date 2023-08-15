using UnityEngine;

public class WorldBounds : MonoBehaviour
{
    public Transform max;
    public Transform min;

    public Vector3 RandomPosition()
    {
        return new Vector3(
            Random.Range(min.position.x, max.position.x),
            1,
            Random.Range(min.position.z, max.position.z)
            );
        //Random.Range(min.position.y, max.position.y)
    }
}