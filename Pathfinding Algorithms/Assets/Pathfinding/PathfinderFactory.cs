using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Hold the currently active algorithm and handles returning the corresponding algorithms pathfinder.
/// </summary>
public class PathfinderFactory
{
    public enum Pathfinding { AStar, Dijkstra, BFS, DFS };
    private static Pathfinding activeAlgorithm;
    public static Pathfinding ActiveAlgorithm { get { return activeAlgorithm; } set { activeAlgorithm = value; } }

    /// <summary>
    /// Return the active algorithms pathfinder.
    /// </summary>
    public static Pathfinder GetPathfinder()
    {
        Pathfinder pathFinding = null;

        switch (activeAlgorithm)
        {
            case Pathfinding.AStar:
                pathFinding = Object.FindObjectOfType<Grid>().GetComponent<AStarPathfinder>();
                if (pathFinding)
                {
                    return pathFinding;
                }
                else
                {
                    return Object.FindObjectOfType<Grid>().gameObject.AddComponent<AStarPathfinder>();
                }
            case Pathfinding.Dijkstra:
                pathFinding = Object.FindObjectOfType<Grid>().GetComponent<DijkstraPathfinder>();
                if (pathFinding)
                {
                    return pathFinding;
                }
                else
                {
                    return Object.FindObjectOfType<Grid>().gameObject.AddComponent<DijkstraPathfinder>();
                }
            case Pathfinding.BFS:
                pathFinding = Object.FindObjectOfType<Grid>().GetComponent<BFSPathfinder>();
                if (pathFinding)
                {
                    return pathFinding;
                }
                else
                {
                    return Object.FindObjectOfType<Grid>().gameObject.AddComponent<BFSPathfinder>();
                }
            case Pathfinding.DFS:
                pathFinding = Object.FindObjectOfType<Grid>().GetComponent<DFSPathfinder>();
                if (pathFinding)
                {
                    return pathFinding;
                }
                else
                {
                    return Object.FindObjectOfType<Grid>().gameObject.AddComponent<DFSPathfinder>();
                }
        }
        return null;
    }
}
