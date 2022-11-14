using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DrawLines : MonoBehaviour
{
    [SerializeField]
    LineSegment line;
    [SerializeField]
    LineSegment perpendicularLine;

    public Vector3 startPosition = new Vector3(0, 0, 0);
    public Vector3 endPosition = new Vector3(5, 0, 0);

    // Start is called before the first frame update
    void Start()
    {
        line = new LineSegment(startPosition, endPosition);
        perpendicularLine = new LineSegment();
    }

    // Update is called once per frame
    void Update()
    {
        line.SetEndPoints(startPosition, endPosition);

        Vector3 direction = line.GetNormalizedDirection();

        Vector3 perpendicularDirection = Vector2.Perpendicular(direction);
        perpendicularLine.StartPoint = line.StartPoint;
        perpendicularLine.EndPoint = line.StartPoint + perpendicularDirection * endPosition.x;
    }

    void OnDrawGizmos()
    {
        if (!Application.isPlaying)
            return;

        line.Draw(Color.green);
        perpendicularLine.Draw(Color.red);
    }
}
