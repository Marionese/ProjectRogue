using UnityEngine;

public class Billboard : MonoBehaviour
{
    void LateUpdate()
    {
        Camera cam = Camera.main;
        if (!cam) return;

        Vector3 camForward = cam.transform.forward;
        camForward.y = 0f;          // ignore vertical tilt
        camForward.Normalize();

        transform.forward = camForward;
    }
}
