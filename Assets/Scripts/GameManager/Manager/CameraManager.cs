using Cinemachine;
using UnityEngine;

public class CameraManager : BaseManager<CameraManager>
{
    public CinemachineVirtualCamera killCam;
    public CinemachineVirtualCamera weaponCamera;
    public Camera minimap;
    private Transform player;

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
    }

    public void EnableKillCam()
    {
        killCam.Priority = 20;
    }

    private void LateUpdate()
    {
        if (player)
        {
            Vector3 newPosition = player.position;
            newPosition.y = minimap.transform.position.y;
            minimap.transform.position = newPosition;

            minimap.transform.rotation = Quaternion.Euler(90f, player.eulerAngles.y, 0f);
        }
    }
}