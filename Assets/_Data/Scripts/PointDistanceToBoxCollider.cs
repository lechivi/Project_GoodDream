using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class PointDistanceToBoxCollider : MonoBehaviour
{
    public BoxCollider2D boxCollider;
    public Transform point;

    private Vector3[] boxCorners;
    private float[] distancesToSides;

    void Start()
    {
        // Get the corners of the Box Collider
        boxCorners = GetBoxCorners(boxCollider);

        // Initialize the distancesToSides array with the same length as the number of sides
        distancesToSides = new float[4];
    }

    void FixedUpdate()
    {
        // Calculate the distance from the point to each side of the Box Collider
        for (int i = 0; i < 4; i++)
        {
            distancesToSides[i] = DistanceToSide(point.position, boxCorners[i], boxCorners[(i + 1) % 4]);
        }
    }

    // Helper function to get the corners of a Box Collider
    private Vector3[] GetBoxCorners(BoxCollider2D boxCollider)
    {
        Vector3 center = boxCollider.bounds.center;
        Vector3 size = boxCollider.bounds.size;
        Vector3 topLeft = new Vector3(center.x - size.x / 2f, center.y + size.y / 2f, 0f);
        Vector3 topRight = new Vector3(center.x + size.x / 2f, center.y + size.y / 2f, 0f);
        Vector3 bottomRight = new Vector3(center.x + size.x / 2f, center.y - size.y / 2f, 0f);
        Vector3 bottomLeft = new Vector3(center.x - size.x / 2f, center.y - size.y / 2f, 0f);
        return new Vector3[] { topLeft, topRight, bottomRight, bottomLeft };
    }

    // Helper function to calculate the distance between a point and a line segment
    private float DistanceToSide(Vector3 point, Vector3 sideStart, Vector3 sideEnd)
    {
        Vector3 v = sideEnd - sideStart;
        Vector3 w = point - sideStart;

        float c1 = Vector3.Dot(w, v);
        if (c1 <= 0f)
        {
            return Vector3.Distance(point, sideStart);
        }

        float c2 = Vector3.Dot(v, v);
        if (c2 <= c1)
        {
            return Vector3.Distance(point, sideEnd);
        }

        float b = c1 / c2;
        Vector3 pb = sideStart + v * b;
        return Vector3.Distance(point, pb);
    }
}
