using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Stores all information of a BFS node, the history of all other nodes reached before this.
/// </summary>
public class BFSNode : Node
{
    private List<BFSNode> history;
    public List<BFSNode> History { get { return history; } set { history = value; } }
}
