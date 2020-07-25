using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Pathfinding logic for the Dijkstra algorithm, attempts to find the most optimal path by calculating the total cost of all nodes from start to end, taking the path with the lowest cost in total.
/// </summary>
public class DijkstraPathfinder : Pathfinder
{
    public override void FindPath(Vector2 startPosition, Vector2 targetPosition)
    {
        stopwatch.Restart();
        stopwatch.Start();

        // Find start and end node positions.
        DijkstraNode startNode = (DijkstraNode)grid.NodeFromWorldPoint(startPosition);
        DijkstraNode targetNode = (DijkstraNode)grid.NodeFromWorldPoint(targetPosition);

        // Initalise Lists & HastSets.
        List<DijkstraNode> openSet = new List<DijkstraNode>();
        List<DijkstraNode> order = new List<DijkstraNode>();
        HashSet<DijkstraNode> closedSet = new HashSet<DijkstraNode>();

        openSet.Add(startNode); // Add starting node to open set, we start searching from here.
        startNode.Cost = 0;

        while (openSet.Count > 0) // While there are nodes in the open set, loop.
        {
            DijkstraNode currentNode = openSet[0];

            for (int i = 1; i < openSet.Count; i++) // Loop over each node in the open set.
            {
                if (openSet[i].Cost < currentNode.Cost) // If the node has a lower F cost, or a matching F cost with a lower H cost...
                {
                    currentNode = openSet[i]; // Replace current node with this node, it is closer to the destination.
                }
            }

            openSet.Remove(currentNode);
            closedSet.Add(currentNode);

            if (!order.Contains(currentNode))
            {
                order.Add(currentNode);
            }

            if (currentNode == targetNode) // If we have reached the target node...
            {
                RetracePath(startNode, targetNode, order.ConvertAll(x => (Node)x));
                stopwatch.Stop();
                break;
            }

            foreach (DijkstraNode neighbourNode in grid.GetNeighbours(currentNode)) // Loop over each neighbour of the current node.
            {
                if (!neighbourNode.Walkable || closedSet.Contains(neighbourNode)) // If the neighbour is not walkable, or has already been sorted into the closed set...
                {
                    continue;
                }

                if (!order.Contains(neighbourNode))
                {
                    order.Add(neighbourNode);
                }

                int movementCost = currentNode.Cost + GetDistance(currentNode, neighbourNode); // Calculate movement cost to neighbour.

                if (movementCost < neighbourNode.Cost || !openSet.Contains(neighbourNode)) // If the movement cost is less than the neighbours G cost, or is not in the open set...
                {
                    neighbourNode.Cost = GetDistance(neighbourNode, targetNode); // Update neighbour H cost to the distance between it and the target.
                    neighbourNode.Parent = currentNode; // Set parent to the current node, for when retracing the path later.

                    if (!openSet.Contains(neighbourNode)) // If the open set does not contain the neighbour...
                    {
                        openSet.Add(neighbourNode);
                    }
                }
            }
        }

        waypoints = RetracePath(startNode, targetNode, order.ConvertAll(x => (Node)x));
        unit.PathToPosition(startPosition, targetPosition, waypoints);
    }

    protected override Vector2[] RetracePath(Node startNode, Node endNode, List<Node> order)
    {
        List<Node> path = new List<Node>();

        startNode.IsStartNode = true;
        endNode.IsEndNode = true;

        DijkstraNode currentNode = (DijkstraNode)endNode; // Set current node to the end node, as we retrace from the end.

        while (currentNode != startNode) // Whilst the current node is not the starting node...
        {
            path.Add(currentNode); // Add the current node to the path.
            currentNode = currentNode.Parent; // Set current node to its parent.
        }
        path.Reverse(); // Reverse the contents of the list to be in order from start to end.

        grid.Path = path; // Set grid path to path traced.
        grid.Order = order;
        grid.ShowFinalPath();
        nodesVisitedText.text = "Nodes Visited: " + order.Count + " in " + stopwatch.ElapsedMilliseconds + "ms";

        Vector2[] waypoints = ConvertToWaypoints(path);
        return waypoints;
    }

    private int GetDistance(Node a, Node b)
    {
        // Calculate the distance between both nodes.
        int distanceX = Mathf.Abs(a.GridX - b.GridX);
        int distanceY = Mathf.Abs(a.GridY - b.GridY);

        return Mathf.Abs(distanceX + distanceY);
    }
}
