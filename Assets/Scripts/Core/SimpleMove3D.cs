using UnityEngine;

public class SimpleMove3D : MonoBehaviour
{
    public float speed = 5f;

    void Update()
    {
        float h = Input.GetAxis("Horizontal");
        float v = Input.GetAxis("Vertical");

        Vector3 dir = new Vector3(h, 0f, v);
        transform.position += dir * speed * Time.deltaTime;
    }
}
