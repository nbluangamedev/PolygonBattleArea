using System.Collections.Generic;
using UnityEngine;

public class Waypoint : MonoBehaviour
{
    public Waypoint prevWaypoint;
    public Waypoint nextWaypoint;
    [Range(0, 5f)]
    public float width = 3.0f;

    public List<Waypoint> branches = new List<Waypoint>();

    [Range(0f, 1f)]
    public float branchProbability = 0.5f;

    public Vector3 GetPosition()
    {
        Vector3 minBound = transform.position + transform.right * width / 2;
        Vector3 maxBound = transform.position - transform.right * width / 2;

        return Vector3.Lerp(minBound, maxBound, Random.Range(0, 1f));
    }
}