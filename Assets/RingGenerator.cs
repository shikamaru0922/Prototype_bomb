using UnityEngine;

[RequireComponent(typeof(LineRenderer))]
public class RingGenerator : MonoBehaviour
{
    public int segments = 100;        // 圆环的细分程度
    public float radius = 5f;         // 圆环的半径
    public float width = 0.2f;        // 圆环的线宽

    private LineRenderer lineRenderer;

    void Start()
    {
        lineRenderer = GetComponent<LineRenderer>();
        lineRenderer.useWorldSpace = false;
        lineRenderer.loop = true;

        lineRenderer.startWidth = width;
        lineRenderer.endWidth = width;

        lineRenderer.positionCount = segments + 1;

        CreatePoints();
    }

    void CreatePoints()
    {
        float angle = 0f;
        for (int i = 0; i <= segments; i++)
        {
            float x = Mathf.Cos(Mathf.Deg2Rad * angle) * radius;
            float y = Mathf.Sin(Mathf.Deg2Rad * angle) * radius;

            lineRenderer.SetPosition(i, new Vector3(x, y, 0));

            angle += 360f / segments;
        }
    }
}