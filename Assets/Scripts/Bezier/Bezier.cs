using UnityEngine;


public class Bezier : MonoBehaviour
{
    public LineRenderer lineRenderer;
    private int numPoints = 50;
    private Vector3[] positions = new Vector3[50];
    [SerializeField] private Transform p0;
    [SerializeField] private Transform p1;
    [SerializeField] private Transform p2;
    [SerializeField] private Transform p3;

    private void Start()
    {
        lineRenderer = GetComponent<LineRenderer>();
        lineRenderer.positionCount = 50;
        DrawLinerCurve();
    }

    private void Update()
    {
        DrawLinerCurve();
    }

    private void DrawLinerCurve()
    {
        for (int i = 1; i < numPoints + 1; i++)
        {
            var t = i / (float) numPoints;
            positions[i - 1] = CalculateCubicBezierCurve(t, p0.position, p1.position, p2.position, p3.position);
        }

        lineRenderer.SetPositions(positions);
    }

    private Vector3 CalculateLinerBezierCurve(float t, Vector3 p0, Vector3 p1)
    {
        var c = p0 + t * (p1 - p0);
        return c;
    }

    private Vector3 CalculateQuadraticBezierCurve(float t, Vector3 p0, Vector3 p1, Vector3 p2)
    {
        var u = 1 - t;
        var uu = u * u;
        var tt = t * t;
        Vector3 p = uu * p0;
        p += 2 * u * t * p1;
        p += tt * p2;
        return p;
    }

    private Vector3 CalculateCubicBezierCurve(float t, Vector3 p0, Vector3 p1, Vector3 p2, Vector3 p3)
    {
        // (1-t) p0 + 3(1-t)^2 t*p1 +3(1-t)t*p2+t^3 *p3 

        var u = 1 - t;
        var uu = u * u;
        var tt = t * t;
        var uuu = uu * u;
        var ttt = tt * t;

        var p = uuu * p0;
        p += 3 * uu * t * p1;
        p += 3 * u * tt * p2;
        p += ttt * p3;

        return p;
        var c = p0 + t * (p1 - p0);
        return c;
    }
}