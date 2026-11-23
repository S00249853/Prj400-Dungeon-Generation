using UnityEngine;

public class Node 
{
    public RectInt rect;
    public Node Left;
    public Node right;


    public Node(RectInt size)
    {
        rect = size;
        Left = null;
        right = null;
    }
}
