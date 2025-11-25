using UnityEngine;

public class FixedCameraZone : MonoBehaviour
{
    public Transform cameraPoint;
    private int playersInside = 0;

    void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("Player")) return;

        playersInside++;
        CameraManager.Instance.EnableFixed(cameraPoint);
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (!other.CompareTag("Player")) return;

        playersInside--;

        if (playersInside <= 0)
            CameraManager.Instance.EnableFollow();
    }
}
