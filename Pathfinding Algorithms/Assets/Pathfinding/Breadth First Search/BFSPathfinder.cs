using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Pathfinding logic for the BFS algorithm, attempts to find a path by going through a queue of nodes, avoiding any nodes it has already visited.
/// </summary>
public class BFSPathfinder : Pathfinder
{
    public override void FindPath(Vector2 startPosition, Vector2 targetPosition)
    {
        // Start stopwatch.
        stopwatch.Restart();
        stopwatch.Start();

        // Find start and end node positions.
        BFSNode startNode = (BFSNode)grid.NodeFromWorldPoint(startPosition);
        BFSNode targetNode = (BFSNode)grid.NodeFromWorldPoint(targetPosition);

        // Initalise Lists & HastSets.
        List<BFSNode> visitedSet = new List<BFSNode>();
        List<BFSNode> order = new List<BFSNode>();
        Queue<BFSNode> openSet = new Queue<BFSNode>();

        startNode.History = new List<BFSNode>();
        visitedSet.Add(startNode);
        openSet.Enqueue(startNode); // Add starting node to open set, we start searching from here.

        while (openSet.Count > 0) // While there are nodes in the open set, loop.
        {
            BFSNode currentNode = openSet.Dequeue();

            if (!order.Contains(currentNode))
            {
                order.Add(currentNode);
            }

            foreach (BFSNode neighbourNode in grid.GetNeighbours(currentNode)) // Loop over each neighbour of the current node.
            {
                if (!neighbourNode.Walkable || visitedSet.Contains(neighbourNode)) // If the neighbour is not walkable, or has already been sorted into the closed set...
                {
                    continue;
                }

                neighbourNode.History = new List<BFSNode>(currentNode.History);
                neighbourNode.History.Add(currentNode);
                visitedSet.Add(neighbourNode);
                openSet.Enqueue(neighbourNode);

                if (!order.Contains(neighbourNode))
                {
                    order.Add(neighbourNode);
                }

                if (neighbourNode == targetNode) // If we have reached the target node...
                {
                    neighbourNode.History.Add(neighbourNode);
                    RetracePath(targetNode, order.ConvertAll(x => (Node)x));
                    openSet.Clear();
                    stopwatch.Stop();
                    break;
                }
            }
        }

        waypoints = RetracePath(targetNode, order.ConvertAll(x => (Node)x));
        unit.PathToPosition(startPosition, targetPosition, waypoints);
    }

    /// <summary>
    /// Retrace the found path from the start to end node, including the order that the path was found in.
    /// </summary>
    protected override Vector2[] RetracePath(Node endNode, List<Node> order)
    {
        List<Node> path = new List<Node>();
        BFSNode currentNode = (BFSNode)endNode;
        path = currentNode.History.ConvertAll(x => (Node)x);
        path[0].IsStartNode = true;
        endNode.IsEndNode = true;
        grid.Path = path;
        grid.Order = order;
        grid.ShowFinalPath();
        nodesVisitedText.text = "Nodes Visited: " + order.Count + " in " + stopwatch.ElapsedMilliseconds + "ms";

        Vector2[] waypoints = ConvertToWaypoints(path);
        return waypoints;
    }
}
