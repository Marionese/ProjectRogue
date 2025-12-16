using UnityEngine;

[RequireComponent(typeof(BoxCollider))]
public class FixedCameraZone : MonoBehaviour
{
    public Transform cameraPoint;
    private int playersInside = 0;

    void Reset()
    {
        GetComponent<BoxCollider>().isTrigger = true;
    }

    void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player"))
            return;

        playersInside++;
        CameraManager.Instance.EnableFixed(cameraPoint);
    }

    void OnTriggerExit(Collider other)
    {
        if (!other.CompareTag("Player"))
            return;

        playersInside--;

        if (playersInside <= 0)
            CameraManager.Instance.EnableFollow();
    }
}
