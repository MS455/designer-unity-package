using UnityEngine;

public class LineSegment
{
    [SerializeField]
    Vector3 start;
    public Vector3 StartPoint { get => start; set => start = value; }
    [SerializeField]
    Vector3 end;
    public Vector3 EndPoint { get => end; set => end = value; }

    public LineSegment()
    {
        start = Vector3.zero;
        end = Vector3.zero;
    }

    public LineSegment(Vector3 start, Vector3 end)
    {
        this.start = start;
        this.end = end;
    }

    public void SetEndPoints(Vector3 start, Vector3 end)
    {
        this.start = start;
        this.end = end;
    }

    public Vector3 GetNormalizedDirection()
    {
        return Vector3.Normalize(end - start);
    }

    public void Draw(Color color)
    {
        Gizmos.color = color;
        Gizmos.DrawLine(start, end);
    }
}