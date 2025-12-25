using System.Collections.Generic;
using UnityEngine;

public class CameraTarget : MonoBehaviour
{
    public float minZoom = 5f;
    public float maxZoom = 12f;
    public float zoomLimiter = 5f;
    public float smoothTime = 0.2f;
    [SerializeField] private int pixelsPerUnit = 32;

    [Header("Framing Offset")]
    [SerializeField] private float zOffset = -2.5f;

    private Vector3 velocity;
    private List<Transform> targets = new List<Transform>();
    private Camera cam;

    public static CameraTarget Instance;

    void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        cam = Camera.main;
    }

    public void Register(Transform target)
    {
        if (!targets.Contains(target))
            targets.Add(target);
    }

    public void Unregister(Transform target)
    {
        targets.Remove(target);
    }

    void LateUpdate()
    {
        if (targets.Count == 0)
            return;

        if (targets.Count == 1)
            FollowSingle();
        else
            FollowMultiple();

        transform.position = SnapToPixelGrid(transform.position);
    }


    Vector3 SnapToPixelGrid(Vector3 position)
    {
        float unitsPerPixel = 1f / pixelsPerUnit;

        position.x = Mathf.Round(position.x / unitsPerPixel) * unitsPerPixel;
        position.z = Mathf.Round(position.z / unitsPerPixel) * unitsPerPixel;

        return position;
    }

    void FollowSingle()
    {
        var t = targets[0];

        Vector3 pos = new Vector3(
            t.position.x,
            transform.position.y,   // keep camera height
            t.position.z + zOffset  // <-- backward offset
        );

        transform.position = pos;

    }


    void FollowMultiple()
    {
        Bounds bounds = GetBounds();

        Vector3 center = bounds.center;
        center.y = transform.position.y;     // keep height
        center.z += zOffset;                 // <-- backward offset

        transform.position = Vector3.SmoothDamp(
            transform.position,
            center,
            ref velocity,
            smoothTime
        );

        float spread = Mathf.Max(bounds.size.x, bounds.size.z);

        float newZoom = Mathf.Lerp(
            maxZoom,
            minZoom,
            spread / zoomLimiter
        );

        cam.orthographicSize = Mathf.Lerp(
            cam.orthographicSize,
            newZoom,
            Time.deltaTime * 5f
        );
    }


    Bounds GetBounds()
    {
        Bounds bounds = new Bounds(targets[0].position, Vector3.zero);
        foreach (var t in targets)
            bounds.Encapsulate(t.position);

        return bounds;
    }
}
