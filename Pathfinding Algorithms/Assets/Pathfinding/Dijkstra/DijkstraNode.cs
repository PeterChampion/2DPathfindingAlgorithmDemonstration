using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Stores all information of a Dijkstra node, the total cost it takes to move to and its assigned parent when they are part of a final path.
/// 
public class DijkstraNode : Node
{
    private int cost = 100000;
    public int Cost { get { return cost; } set { cost = value; } }

    private DijkstraNode parent;
    public DijkstraNode Parent { get { return parent; } set { parent = value; } }
}
