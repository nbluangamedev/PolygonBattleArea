using UnityEngine;

public class DebugDrawLine : MonoBehaviour
{
    public bool showWeaponRaycast = false;

    private void OnDrawGizmos()
    {
        if (showWeaponRaycast)
        {
            Debug.DrawLine(transform.position, transform.position + transform.forward * 50);
        }
    }
}