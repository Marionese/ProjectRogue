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
        // Move fixed camera to the point (XZ plane, keep height)
        fixedCam.transform.position = new Vector3(
            cameraPoint.position.x,
            fixedCam.transform.position.y, // KEEP height
            cameraPoint.position.z
        );

        followCam.Priority = inactivePriority;
        fixedCam.Priority = activePriority;
    }
}
