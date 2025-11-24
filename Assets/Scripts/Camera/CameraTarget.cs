using System.Collections.Generic;
using UnityEngine;

public class CameraTarget : MonoBehaviour
{
    public float minZoom = 5f;
    public float maxZoom = 12f;
    public float zoomLimiter = 5f;
    public float smoothTime = 0.2f;

    private Vector3 velocity;
    private List<Transform> targets = new List<Transform>();
    private Camera cam;

    void Start()
    {
        cam = Camera.main;
    }

    public void Register(Transform target) => targets.Add(target);
    public void Unregister(Transform target) => targets.Remove(target);

    void LateUpdate()
    {
        if (targets.Count == 0)
            return;

        if (targets.Count == 1)
        {
            FollowSingle();
        }
        else
        {
            FollowMultiple();
        }
    }

    void FollowSingle()
    {
        var t = targets[0];
        Vector3 pos = new Vector3(t.position.x, t.position.y, transform.position.z);
        transform.position = Vector3.SmoothDamp(transform.position, pos, ref velocity, smoothTime);
    }

    void FollowMultiple()
    {
        Bounds bounds = GetBounds();

        Vector3 center = bounds.center;
        center.z = transform.position.z;
        transform.position = Vector3.SmoothDamp(transform.position, center, ref velocity, smoothTime);

        float newZoom = Mathf.Lerp(maxZoom, minZoom, bounds.size.magnitude / zoomLimiter);
        cam.orthographicSize = Mathf.Lerp(cam.orthographicSize, newZoom, Time.deltaTime * 5f);

    }

    Bounds GetBounds()
    {
        var bounds = new Bounds(targets[0].position, Vector3.zero);
        foreach (var t in targets) bounds.Encapsulate(t.position);
        return bounds;
    }
}
