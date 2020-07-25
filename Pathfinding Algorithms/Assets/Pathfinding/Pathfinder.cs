using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Diagnostics;
using TMPro;

/// <summary>
/// The default pathfinder class that all algorithm pathfinders derive from, contains base variables and methods for derived classes to extend.
/// </summary>
public abstract class Pathfinder : MonoBehaviour
{
    protected Grid grid;
    protected TextMeshProUGUI nodesVisitedText;
    protected Unit unit;
    protected Vector2[] waypoints = new Vector2[0];
    protected Stopwatch stopwatch = new Stopwatch();

    private void Awake()
    {
        grid = GetComponent<Grid>();
        nodesVisitedText = GameObject.Find("VisitedNodes").GetComponent<TextMeshProUGUI>();
        unit = FindObjectOfType<Unit>();
    }

    /// <summary>
    /// 
    /// </summary>
    public virtual void FindPath(Vector2 startPosition, Vector2 targetPosition)
    {
        // Will be overridden by derived classes.
    }

    /// <summary>
    /// Retrace the found path from the start to end node, including the order that the path was found in.
    /// </summary>
    protected virtual Vector2[] RetracePath(Node startNode, Node endNode, List<Node> order)
    {
        // Will be overridden by derived classes.
        return null;
    }

    /// <summary>
    /// Retrace the found path from the start to end node, including the order that the path was found in.
    /// </summary>
    protected virtual Vector2[] RetracePath(Node endNode, List<Node> order)
    {
        // Will be overridden by derived classes.
        return null;
    }

    /// <summary>
    /// Convert the path to a series of Vector2 positions that can be used to drive unit movement.
    /// </summary>
    protected virtual Vector2[] ConvertToWaypoints(List<Node> path)
    {
        List<Vector2> waypoints = new List<Vector2>();

        // For every position within the path...
        for (int i = 0; i < path.Count; i++)
        {
            waypoints.Add(path[i].WorldPosition); // Add the nodes world position as a way point.
        }

        return waypoints.ToArray();
    }
}
