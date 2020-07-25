using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Stores all information of an A Star Node, including G, H & F costs and their assigned parent when they are part of a final path.
/// </summary>
public class AStarNode : Node
{
    private int gCost;
    public int GCost { get { return gCost; } set { gCost = value; } }

    private int hCost;
    public int HCost { get { return hCost; } set { hCost = value; } }

    public int FCost { get { return gCost + hCost; } }

    private AStarNode parent;
    public AStarNode Parent { get { return parent; } set { parent = value; } }
}
