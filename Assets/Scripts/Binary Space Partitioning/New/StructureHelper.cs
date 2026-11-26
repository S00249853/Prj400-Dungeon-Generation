using System;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public static class StructureHelper
{
    public static List<Node> TraverseGraphToExtractLowestLeafs(RoomNode parentNode)
    {
        Queue<Node> nodesToCheck = new Queue<Node>();
        List<Node> listToReturn = new List<Node>();
        if(parentNode.Children.Count == 0)
        {
            return new List<Node>() { parentNode };
        }
        foreach (var child in parentNode.Children)
        {
            nodesToCheck.Enqueue(child);
        }
        while (nodesToCheck.Count > 0) 
        {
            var currentNode = nodesToCheck.Dequeue();
            if(currentNode.Children.Count == 0)
            {
                listToReturn.Add(currentNode);
            }
            else
            {
                foreach(var child in currentNode.Children)
                {
                    nodesToCheck.Enqueue(child);
                }
            }
        }
        return listToReturn;
    }

    public static Vector2Int GenerateBottomLeftCornerBetween(Vector2Int boundaryLeftPoint, Vector2Int boundaryRightPoint, float pointModifier, int offset)
    {
        int xMin = boundaryLeftPoint.x + offset;
        int xMax = boundaryRightPoint.x - offset;
        int yMin = boundaryLeftPoint.y + offset;
        int yMax = boundaryRightPoint.y - offset;
        return new Vector2Int(Random.Range(xMin, (int)(xMin + (xMax - xMin) * pointModifier)),
            Random.Range(yMin, (int)(yMin + (yMax - yMin) * pointModifier)));
    }

    public static Vector2Int GenerateTopRightCornerBetween(Vector2Int boundaryLeftPoint, Vector2Int boundaryRightPoint, float pointModifier, int offset)
    {
        int xMin = boundaryLeftPoint.x + offset;
        int xMax = boundaryRightPoint.x - offset;
        int yMin = boundaryLeftPoint.y + offset;
        int yMax = boundaryRightPoint.y - offset;
        return new Vector2Int(Random.Range((int)(xMin + (xMax-xMin) * pointModifier),xMax),
            Random.Range((int)(yMin + (yMax - yMin) * pointModifier),yMax));
    }
}