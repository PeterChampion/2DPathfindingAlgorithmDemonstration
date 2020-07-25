using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

/// <summary>
/// Manages what happens based on user input, communicates inputs to grid class functionality.
/// </summary>
public class PathfindingManager : MonoBehaviour
{
    public static PathfindingManager instance;

    private List<GameObject> interactedList = new List<GameObject>();

    public Transform startNode, endNode;

    public Grid grid;

    private Pathfinder pathfinder;

    public void Awake()
    {
        // Singleton setup
        if (instance == null)
        {
            instance = this;
        }
        else if (instance != this)
        {
            Destroy(this);
        }

        grid = GetComponent<Grid>();
    }

    private void Update()
    {
        Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition); // Record the mouses current position on screen.
        RaycastHit2D hit = Physics2D.Raycast(mousePosition, Vector2.zero); // Raycast to the position of the mouse.

        if (Input.GetMouseButton(1) && !endNode) // If the right mouse button is held down and there is no target currently set...
        {
            if (hit.collider != null && hit.collider.GetComponent<Node>() && !interactedList.Contains(hit.collider.gameObject)) // If a game object with the node class which is not currently in the interactedList is hit...
            {                
                Node currentNode = grid.NodeFromWorldPoint(hit.collider.gameObject.transform.position); // Return reference to the node at the position of the game object.
                interactedList.Add(currentNode.gameObject); // Add node game object to the interacted list.

                if (currentNode != null) // If there is a node reference found...
                {
                    currentNode.Walkable = !currentNode.Walkable; // Inverse the nodes walkable state.
                }

                grid.UpdateNodeColours(); // Update grid to reflect the change.
            }
        }

        if (Input.GetMouseButtonUp(1)) // If the right mouse button is released...
        {
            interactedList.Clear(); // Clear interacted list, resets for the next time right mouse button is used.
        }

        if (Input.GetMouseButtonDown(0)) // If the left mouse button is pressed...
        {
            if (hit.collider != null && hit.collider.GetComponent<Node>()) // If a game object with the node class is hit...
            {
                Node currentNode = grid.NodeFromWorldPoint(hit.collider.gameObject.transform.position); // Return reference to the node at the position of the game object.

                if (currentNode != null) // If there is a node reference found...
                {
                    if (startNode == null && currentNode.transform != endNode && currentNode.Walkable) // If there is no current start node AND the current node is not the end AND is walkable...
                    {
                        currentNode.IsStartNode = true;
                        startNode = currentNode.transform;
                        currentNode.UpdateColour(Color.magenta);
                        return;
                    }
                    else if (endNode == null && currentNode.transform != startNode && currentNode.Walkable) // Else if there is no end node AND the current node is not the start AND is walkable...
                    {
                        currentNode.IsEndNode = true;
                        endNode = currentNode.transform;
                        currentNode.UpdateColour(Color.magenta);
                        return;
                    }

                    if (currentNode.transform == startNode) // If the current node IS the start node...
                    {
                        currentNode.UpdateColour(Color.white);
                        currentNode.IsStartNode = false;
                        startNode = null;
                        grid.UpdateNodeColours();
                        return;
                    }
                    else if (currentNode.transform == endNode) // If the current node IS the end node...
                    {
                        currentNode.UpdateColour(Color.white);
                        currentNode.IsEndNode = false;
                        endNode = null;
                        grid.UpdateNodeColours();
                        return;
                    }
                }
            }
        }

        if (startNode == null || endNode == null) // If the start OR end node is not set...
        {
            grid.StopAllCoroutines();
            grid.UpdateNodeColours();
        }
    }

    /// <summary>
    /// Called by UI element. Finds and calls the current pathfinding algorithm if a start and end node are both present.
    /// </summary>
    public void VisualisePath()
    {
        if (startNode != null && endNode != null) // If there is a start AND end node...
        {
            if (pathfinder == null) // If there is no pathfinder reference...
            {
                pathfinder = PathfinderFactory.GetPathfinder();
            }

            pathfinder.FindPath(startNode.position, endNode.position);
        }
        else // Otherwise...
        {
            grid.StopAllCoroutines();
            grid.UpdateNodeColours();
        }
    }

    /// <summary>
    /// Resets the current pathfinder.
    /// </summary>
    public void ResetPathFinder()
    {
        pathfinder = null;
    }

    /// <summary>
    /// Stops the any coroutines displaying the pathfinding on the grid and forces the grid to update without a final path shown. 
    /// </summary>
    public void ResetPathfindingVisuals()
    {
        grid.StopAllCoroutines();
        grid.UpdateNodeColours();
    }
    
}
