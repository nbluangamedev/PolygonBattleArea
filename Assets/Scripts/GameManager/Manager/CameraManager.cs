using Cinemachine;

public class CameraManager : BaseManager<CameraManager>
{
    public CinemachineVirtualCamera killCam;
    public CinemachineVirtualCamera weaponCamera;

    public void EnableKillCam()
    {
        killCam.Priority = 20;
    }
}