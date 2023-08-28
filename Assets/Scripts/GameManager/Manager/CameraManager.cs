using Cinemachine;
using UnityEngine;

public class CameraManager : BaseManager<CameraManager>
{
    public CinemachineVirtualCamera killCam;
    public CinemachineVirtualCamera weaponCamera;
    public Camera minimap;

    private Transform player;

    public void EnableKillCam()
    {
        if (!player)
        {
            player = GameObject.FindGameObjectWithTag("Player").transform;
        }

        if (player)
        {
            Vector3 newPosition = player.position;
            newPosition.y = minimap.transform.position.y;
            minimap.transform.SetPositionAndRotation(newPosition, Quaternion.Euler(90f, player.eulerAngles.y, 0f));
        }

        killCam.Priority = 20;
    }

    public void DisableKillCam()
    {
        killCam.Priority = 0;
    }

    private void LateUpdate()
    {
        if (player)
        {
            Vector3 newPosition = player.position;
            newPosition.y = minimap.transform.position.y;
            minimap.transform.SetPositionAndRotation(newPosition, Quaternion.Euler(90f, player.eulerAngles.y, 0f));
        }
    }
}