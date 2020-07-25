using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Holds all methods used by UI elements such as drop down menus, buttons, etc.
/// </summary>
public class UI : MonoBehaviour
{
    /// <summary>
    /// Change the current algorithm based on the value selected, reset all current pathfinding and display visuals and call for a new grid to be created.
    /// </summary>
    public void ChangeAlgorithm(int val)
    {
        PathfinderFactory.Pathfinding result = 0;

        switch (val)
        {
            case 0:
                result = PathfinderFactory.Pathfinding.AStar;
                break;
            case 1:
                result = PathfinderFactory.Pathfinding.Dijkstra;
                break;
            case 2:
                result = PathfinderFactory.Pathfinding.BFS;
                break;
            case 3:
                result = PathfinderFactory.Pathfinding.DFS;
                break;
            default:
                break;
        }

        PathfindingManager.instance.ResetPathfindingVisuals();
        PathfinderFactory.ActiveAlgorithm = result;
        PathfindingManager.instance.grid.CreateGrid();
        PathfindingManager.instance.ResetPathFinder();
    }

    /// <summary>
    /// Calls for the pathfinding manager to reset any existing pathfinding visuals and then visulaise the current path.
    /// </summary>
    public void VisualisePath()
    {
        PathfindingManager.instance.ResetPathfindingVisuals();
        PathfindingManager.instance.VisualisePath();
    }
}
