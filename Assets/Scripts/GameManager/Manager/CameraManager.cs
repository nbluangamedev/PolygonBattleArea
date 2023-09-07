using Cinemachine;
using UnityEngine;

public class CameraManager : BaseManager<CameraManager>
{
    //public CinemachineVirtualCamera weaponCamera;
    public CinemachineVirtualCamera killCam;
    public Camera minimap;
    public CinemachineTargetGroup targetGroup;
    //private CinemachineTargetGroup.Target target;

    [SerializeField]
    private GameObject player;
    private Vector3 newPosition;


    private void Update()
    {
        if (player && killCam && minimap)
        {
            return;
        }

        ScreenGame screenGame = UIManager.Instance.GetExistScreen<ScreenGame>();
        if (screenGame)
        {
            if (screenGame.CanvasGroup.alpha == 1)
            {
                player = GameObject.FindGameObjectWithTag("Player");
                killCam = player.transform.Find("KillCamera").GetComponent<CinemachineVirtualCamera>();
                minimap = player.transform.Find("MinimapCamera").GetComponent<Camera>();

                targetGroup.AddMember(minimap.transform, 1f, 3f);
                //target.target = minimap.transform;
                //target.weight = 1;
                //target.radius = 3;
            }
        }
    }

    public void EnableKillCam()
    {
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
            newPosition = player.transform.position;
            newPosition.y = minimap.transform.position.y;
            minimap.transform.SetPositionAndRotation(newPosition, Quaternion.Euler(90f, transform.eulerAngles.y, 0f));
        }
    }
}