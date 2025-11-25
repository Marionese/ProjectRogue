using UnityEngine;
using Unity.Cinemachine;

public class CameraManager : MonoBehaviour
{
    public static CameraManager Instance;

    public CinemachineCamera followCam;
    public CinemachineCamera fixedCam;

    public int activePriority = 20;
    public int inactivePriority = 10;

    void Awake()
    {
        Instance = this;
    }

    public void EnableFollow()
    {
        followCam.Priority = activePriority;
        fixedCam.Priority = inactivePriority;
    }

    public void EnableFixed(Transform cameraPoint)
    {
        // Assign the camera point dynamically
        fixedCam.transform.position = new Vector3(
            cameraPoint.position.x,
            cameraPoint.position.y,
            fixedCam.transform.position.z // KEEP camera Z
        );

        followCam.Priority = inactivePriority;
        fixedCam.Priority = activePriority;
    }
}
