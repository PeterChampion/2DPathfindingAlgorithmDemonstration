using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Handles creation and recording of all nodes within the grid, updates node colours to reflect their current state and returns nodes at specific world coordinates.
/// </summary>
public class Grid : MonoBehaviour
{
    #region Variables
    [SerializeField] private LayerMask unwalkableMask;
    [SerializeField] private GameObject node;

    [SerializeField] private Vector2 gridWorldSize;
    public Vector2 GridWorldSize { get { return gridWorldSize; } }

    [SerializeField] private float nodeRadius;

    private float nodeDiameter;
    [SerializeField] private float distanceBetweenNodes;
    private int gridSizeX, gridSizeY;

    private Node[,] grid;

    private List<Node> order;
    public List<Node> Order { get { return order; } set { order = value; } }

    private List<Node> path;
    public List<Node> Path { get { return path; } set { path = value; } }

    private List<Vector2> previousObstaclePoints = new List<Vector2>();

    private Vector2 previousStartPosition;
    private Vector2 previousEndPosition;
    #endregion

    private void Start()
    {
        nodeDiameter = nodeRadius * 2;

        // Determine how many nodes can be fit into the axis of the grid based on nodeDiameter.  
        gridSizeX = Mathf.RoundToInt(gridWorldSize.x / nodeDiameter);
        gridSizeY = Mathf.RoundToInt(gridWorldSize.y / nodeDiameter);

        CreateGrid();
        UpdateNodeColours();
    }

    /// <summary>
    /// Create a new instance of the grid, instantiating the corresponding active algorithms nodes.
    /// </summary>
    public void CreateGrid()
    {
        CheckExistingGrid();

        grid = new Node[gridSizeX, gridSizeY];
        Vector2 bottomLeft = (Vector2)transform.position - Vector2.right * gridWorldSize.x / 2 - Vector2.up * gridWorldSize.y / 2; // Calculate the bottom left position of the grid by starting from the centre (transform.position) and subtracting half the length and height of the grid.

        // Cycle through every position of the grid.
        for (int y = 0; y < gridSizeY; y++)
        {
            for (int x = 0; x < gridSizeX; x++)
            {
                Vector2 worldPoint = bottomLeft + Vector2.right * (x * nodeDiameter + nodeRadius) + Vector2.up * (y * nodeDiameter + nodeRadius); // Calculate position the node is in the world based on position in the grid and node size.
                bool walkable = true; // Flag as not being obstructed by default.

                // CircleCast at the current position of the grid to check if there are any obstacles, useful for adapting to level design changes.
                if (Physics2D.CircleCast(worldPoint, nodeRadius, Vector2.zero, 0.5f, unwalkableMask))
                {
                    walkable = false; // Flag as being obstructed if an obstacle was overalpping the circlecast.
                }

                // Instantiate node game object and scale to set size.
                GameObject _nodeGO = Instantiate(node, worldPoint, Quaternion.identity);
                _nodeGO.transform.localScale = Vector3.one * (nodeDiameter - distanceBetweenNodes);

                // Assign active algorithms node class to the node game object and store at the current grid position.
                switch (PathfinderFactory.ActiveAlgorithm)
                {
                    case PathfinderFactory.Pathfinding.AStar:                        
                        AStarNode _aStarNode = _nodeGO.AddComponent<AStarNode>();
                        _aStarNode.SetValues(walkable, worldPoint, x, y);
                        grid[x, y] = _aStarNode;
                        break;
                    case PathfinderFactory.Pathfinding.Dijkstra:
                        DijkstraNode _dijkstraNode = _nodeGO.AddComponent<DijkstraNode>();
                        _dijkstraNode.SetValues(walkable, worldPoint, x, y);
                        grid[x, y] = _dijkstraNode;
                        break;
                    case PathfinderFactory.Pathfinding.BFS:
                        BFSNode _BFSNode = _nodeGO.AddComponent<BFSNode>();
                        _BFSNode.SetValues(walkable, worldPoint, x, y);
                        grid[x, y] = _BFSNode;
                        break;
                    case PathfinderFactory.Pathfinding.DFS:
                        DFSNode _DFSNode = _nodeGO.AddComponent<DFSNode>();
                        _DFSNode.SetValues(walkable, worldPoint, x, y);
                        grid[x, y] = _DFSNode;
                        break;
                }
            }
        }

        if (previousObstaclePoints != null) // If any previous obstacles are recorded...
        {
            // Loop over each, calculate the node that has taken their position and flag them as not walkable.
            foreach (Vector2 point in previousObstaclePoints)
            {
                Node node = NodeFromWorldPoint(point);
                node.Walkable = false;
            }
        }

        if (previousStartPosition != Vector2.zero) // If a previous start position exists...
        {
            // Calculate the node that has taken its position and flag as start node.
            Node node = NodeFromWorldPoint(previousStartPosition);
            node.IsStartNode = true;
            PathfindingManager.instance.startNode = node.transform;
        }

        if (previousEndPosition != Vector2.zero) // If a previous end position exists...
        {
            // Calculate the node that has taken its position and flag as end node.
            Node node = NodeFromWorldPoint(previousEndPosition);
            node.IsEndNode = true;
            PathfindingManager.instance.endNode = node.transform;
        }

        UpdateNodeColours();
    }

    /// <summary>
    /// Checks if a grid has already been generated, if so record any unwalkable and assigned start/end nodes before destroying all nodes to make way for a new grid to be created.
    /// </summary>
    private void CheckExistingGrid()
    {
        if (grid != null) // If a grid reference exists...
        {
            // Set default variable states.
            previousStartPosition = Vector2.zero;
            previousEndPosition = Vector2.zero;
            previousObstaclePoints.Clear();

            // Loop over each node in the grid...
            foreach (Node node in grid)
            {
                if (!node.Walkable)
                {
                    previousObstaclePoints.Add(node.WorldPosition);
                }

                if (node.IsStartNode)
                {
                    previousStartPosition = node.WorldPosition;
                }

                if (node.IsEndNode)
                {
                    previousEndPosition = node.WorldPosition;
                }

                Destroy(node.gameObject);
            }
            UpdateNodeColours();
        }
    }

    /// <summary>
    /// Return a node at a Vector2 position.
    /// </summary>
    public Node NodeFromWorldPoint(Vector2 worldPosition)
    {
        // Calculate position on the grid
        float percentX = (worldPosition.x + gridWorldSize.x / 2) / gridWorldSize.x;
        float percentY = (worldPosition.y + gridWorldSize.y / 2) / gridWorldSize.y;

        // Clamp values between 0 & 1.
        percentX = Mathf.Clamp01(percentX);
        percentY = Mathf.Clamp01(percentY);

        // Round values to nearest int (node position).
        int x = Mathf.RoundToInt((gridSizeX - 1) * percentX);
        int y = Mathf.RoundToInt((gridSizeY - 1) * percentY);

        return grid[x, y];
    }

    /// <summary>
    /// Return a list of all neighbours of a node.
    /// </summary>
    public List<Node> GetNeighbours(Node node)
    {
        List<Node> neighbours = new List<Node>();

        // Loop over every adjacent node (Including diagonals)
        for (int x = -1; x <= 1; x++)
        {
            for (int y = -1; y <= 1; y++)
            {
                if (x == 0 && y == 0) // If it is the node searching for neighbours...
                {
                    continue;
                }

                // Calculate positions adjacent to the node searching for neighbours.
                int checkX = node.GridX + x;
                int checkY = node.GridY + y;

                if (checkX >= 0 && checkX < gridSizeX && checkY >= 0 && checkY < gridSizeY) // Bounds check.
                {
                    neighbours.Add(grid[checkX, checkY]); // Add neighbour to list.
                }
            }
        }
        return neighbours;
    }

    #region Grid Display
    /// <summary>
    /// Reveals the path found, highlighting all nodes considered.
    /// </summary>
    public void ShowFinalPath()
    {
        StartCoroutine(DisplayOverTime());
    }

    /// <summary>
    /// Reveals the path found, highlighting all nodes considered.
    /// </summary>
    private IEnumerator DisplayOverTime()
    {
        // Loop over each node in order of consideration...
        foreach (Node node in order)
        {
            yield return new WaitForEndOfFrame(); // Wait a single frame
            
            if (path != null) // If a path exists...
            {
                node.UpdateColour(Color.grey); // Set default colour.

                if (path.Contains(node)) // If the node is within the final path...
                {
                    node.UpdateColour(Color.green);
                }

                if (node.IsStartNode || node.IsEndNode) // If the node is the start or end node...
                {
                    node.UpdateColour(Color.magenta);
                }
            }
        }
    }

    /// <summary>
    /// Loop over the entire grid, updating each nodes colour unless they are unwalkable or a start/end node.
    /// </summary>
    public void UpdateNodeColours()
    {
        // Loop over each node in the grid...
        foreach (Node node in grid)
        {
            node.UpdateColour(Color.white); // Set default colour.

            if (node.IsStartNode || node.IsEndNode) // If the node is the start or end node...
            {
                node.UpdateColour(Color.magenta);
            }

            if (!node.Walkable) // If the node is not walkable...
            {
                node.UpdateColour(Color.black);
            }
        }
    }
    #endregion
}
