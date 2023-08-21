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
        killCam.Priority = 20;
    }

    private void LateUpdate()
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
    }
}