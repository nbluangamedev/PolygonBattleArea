using Cinemachine;
using UnityEngine;

public class CameraManager : BaseManager<CameraManager>
{
    public CinemachineVirtualCamera killCam;
    public CinemachineVirtualCamera weaponCamera;
    public Camera minimap;

    public void EnableKillCam()
    {
        //Vector3 newPosition = transform.position;
        //newPosition.y = minimap.transform.position.y;
        //minimap.transform.SetPositionAndRotation(newPosition, Quaternion.Euler(90f, transform.eulerAngles.y, 0f));

        killCam.Priority = 20;
    }

    public void DisableKillCam()
    {
        killCam.Priority = 0;
    }

    private void LateUpdate()
    {
        Vector3 newPosition = transform.position;
        newPosition.y = minimap.transform.position.y;
        minimap.transform.SetPositionAndRotation(newPosition, Quaternion.Euler(90f, transform.eulerAngles.y, 0f));
    }
}