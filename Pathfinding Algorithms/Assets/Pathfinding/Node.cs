using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Base Node class that stores walkable state, world/grid position. Used as the base node class that all other nodes derive from.
/// </summary>
public class Node : MonoBehaviour
{
    protected bool walkable;
    public bool Walkable { get { return walkable; } set { walkable = value; } }

    protected bool isStartNode;
    public bool IsStartNode { get { return isStartNode;} set { isStartNode = value; } } 

    protected bool isEndNode;
    public bool IsEndNode { get { return isEndNode; } set { isEndNode = value; } }

    protected Vector2 worldPosition;
    public Vector2 WorldPosition { get { return worldPosition; } }

    protected int gridX;
    public int GridX { get { return gridX; } }

    protected int gridY;
    public int GridY { get { return gridY; } }

    public void SetValues(bool _walkable, Vector2 _worldPosition, int _gridX, int _gridY)
    {
        walkable = _walkable;
        worldPosition = _worldPosition;
        gridX = _gridX;
        gridY = _gridY;
        isStartNode = false;
        isEndNode = false;
    }

    public void UpdateColour(Color color)
    {
        gameObject.GetComponent<SpriteRenderer>().color = color;
    }
}
