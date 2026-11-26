using NUnit.Framework;
using System;
using System.Collections.Generic;
using UnityEngine;

public abstract class Node
{
    private List<Node> children;

    public List<Node> Children { get { return children; } }

    public bool Visited { get; set; }
    public Vector2Int BottomLeftAreaCorner { get; set; }
    public Vector2Int BottomRightAreaCorner { get; set; }
    public Vector2Int TopLeftAreaCorner { get; set; }
    public Vector2Int TopRightAreaCorner { get; set; }

    public Node Parent { get; set; }

    public int TreeLayerIndex { get; set; }

    public Node(Node parentNode)
    {
        children = new List<Node>();
        Parent = parentNode;
        if (parentNode != null)
        {
            parentNode.AddChild(this);
        }
    }

    public void AddChild(Node node)
    {
       children.Add(node);
    }

    public void RemoveChild(Node node)
    {
        children.Remove(node);
    }
}