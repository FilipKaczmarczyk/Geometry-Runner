using UnityEngine;

public class CameraController : MonoBehaviour
{
    public Transform target;

    public float offset;
    public float speed;

    private EdgeCollider2D edge;
    private Vector2[] edgePoints;

    void Awake()
    {
        edge = GetComponent<EdgeCollider2D>() == null ? gameObject.AddComponent<EdgeCollider2D>() : GetComponent<EdgeCollider2D>();

        edgePoints = new Vector2[5];

        AddCollider();
    }

    void AddCollider()
    {
        Vector2 bottomLeft = Camera.main.ScreenToWorldPoint(new Vector3(0, 0, Camera.main.nearClipPlane));
        Vector2 topRight = Camera.main.ScreenToWorldPoint(new Vector3(Camera.main.pixelWidth, Camera.main.pixelHeight, Camera.main.nearClipPlane));
        Vector2 topLeft = new Vector2(bottomLeft.x, topRight.y);
        Vector2 bottomRight = new Vector2(topRight.x, bottomLeft.y);

        edgePoints[0] = bottomLeft;
        edgePoints[1] = topLeft;
        edgePoints[2] = topRight;
        edgePoints[3] = bottomRight;
        edgePoints[4] = bottomLeft;

        edge.points = edgePoints;
    }
    void LateUpdate()
    {
        transform.position = Vector2.Lerp(transform.position, new Vector2(0, target.position.y + offset), speed);
    }
}
