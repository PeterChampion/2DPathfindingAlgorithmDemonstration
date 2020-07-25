using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Stores all information of a DFS node, the history of all other nodes reached before this.
/// </summary>
public class DFSNode : Node
{
    private List<DFSNode> history;
    public List<DFSNode> History { get { return history; } set { history = value; } }
}
