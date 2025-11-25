using UnityEngine;

public class FixedCameraZone : MonoBehaviour
{
    public Transform cameraPoint; // unique per zone

    void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("Player")) return;
        CameraManager.Instance.EnableFixed(cameraPoint);
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (!other.CompareTag("Player")) return;
        CameraManager.Instance.EnableFollow();
    }
}
