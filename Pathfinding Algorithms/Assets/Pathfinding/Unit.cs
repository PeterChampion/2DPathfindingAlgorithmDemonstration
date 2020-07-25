using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Stores information regarding the path and movement of a unit.
/// </summary>
public class Unit : MonoBehaviour
{
    public Vector2 target;
    [SerializeField] private float speed = 1;
    private Vector2[] path;
    private int targetIndex;
    [SerializeField] private bool moving;

    /// <summary>
    /// Assigns a new path for the unit to follow.
    /// </summary>
    public void PathToPosition(Vector2 startPosition, Vector2 targetPosition, Vector2[] newPath)
    {
        transform.position = startPosition;
        transform.position += new Vector3(0, 0, -5); // Offset on the Z axis to appear in front of other objects on screen.
        target = targetPosition;
        path = newPath;
        targetIndex = 0;
        StopAllCoroutines();
        StartCoroutine(FollowPath());
    }

    /// <summary>
    /// Has the unit path to the position of the targetIndex, once reached incrementing the index until the end of the path has been reached.
    /// </summary>
    private IEnumerator FollowPath()
    {
        Vector3 currentWaypoint = path[0]; // Look at first position of the path.

        while (true)
        {
            if ((Vector2)transform.position == (Vector2)currentWaypoint) // If the current position of the unit matches the current waypoints...
            {
                targetIndex++; // Increment index
                if (targetIndex >= path.Length) // If the target index reaches or exceeds the length of the path...
                {
                    yield break;
                }
                currentWaypoint = path[targetIndex]; // Update waypoint to path position at index.
            }

            transform.position = Vector3.MoveTowards(transform.position, currentWaypoint + new Vector3(0,0,-5), speed); // Move towards the next waypoint along the path.
            yield return null; // Wait a frame.
        }
    }
}
