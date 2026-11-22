using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Tilemaps;

public class BSPGenerator : MonoBehaviour
{
    public int minRoomSize = 10;

    public BSPNode GenerateBSP(RectInt space)
    {
        BSPNode root = new BSPNode(space);
        SplitNode(root);
        return root;
    }

    private void SplitNode(BSPNode node)
    {
        if (node.rect.width < minRoomSize || node.rect.height < minRoomSize)
        {
            return; // Stop splitting if too small
        }

        bool splitH = Random.Range(0f, 1f) > 0.5f; // Randomly choose split direction
        int splitPoint = splitH ? Random.Range(minRoomSize, node.rect.width - minRoomSize) : Random.Range(minRoomSize, node.rect.height - minRoomSize);

        if (splitH)
        {
            node.left = new BSPNode(new RectInt(node.rect.x, node.rect.y, splitPoint, node.rect.height));
            node.right = new BSPNode(new RectInt(node.rect.x + splitPoint, node.rect.y, node.rect.width - splitPoint, node.rect.height));
        }
        else
        {
            node.left = new BSPNode(new RectInt(node.rect.x, node.rect.y, node.rect.width, splitPoint));
            node.right = new BSPNode(new RectInt(node.rect.x, node.rect.y + splitPoint, node.rect.width, node.rect.height - splitPoint));
        }

        SplitNode(node.left);
        SplitNode(node.right);
    }

    public List<RectInt> GenerateRooms(BSPNode node)
    {
        List<RectInt> rooms = new List<RectInt>();
        TraverseAndCreateRooms(node, rooms);
        return rooms;
    }

    private void TraverseAndCreateRooms(BSPNode node, List<RectInt> rooms)
    {
        if (node == null) return;

        if (node.left == null && node.right == null) // Leaf node
        {
            int roomWidth = Random.Range(minRoomSize / 2, node.rect.width - 1);
            int roomHeight = Random.Range(minRoomSize / 2, node.rect.height - 1);
            int roomX = Random.Range(node.rect.x, node.rect.x + node.rect.width - roomWidth);
            int roomY = Random.Range(node.rect.y, node.rect.y + node.rect.height - roomHeight);

            RectInt room = new RectInt(roomX, roomY, roomWidth, roomHeight);
            rooms.Add(room);
        }
        else
        {
            TraverseAndCreateRooms(node.left, rooms);
            TraverseAndCreateRooms(node.right, rooms);
        }
    }

    public List<Vector2Int> GenerateCorridors(List<RectInt> rooms)
    {
        List<Vector2Int> corridors = new List<Vector2Int>();
        for (int i = 0; i < rooms.Count - 1; i++)
        {
            Vector2Int room1Center = new Vector2Int(rooms[i].xMin + rooms[i].width / 2, rooms[i].yMin + rooms[i].height / 2);
            Vector2Int room2Center = new Vector2Int(rooms[i + 1].xMin + rooms[i + 1].width / 2, rooms[i + 1].yMin + rooms[i + 1].height / 2);

            //Create L shaped corridor
            foreach (var pos in CreateLShapedCorridor(room1Center, room2Center))
            {
                corridors.Add(pos);
            }
        }
        return corridors;
    }

    private List<Vector2Int> CreateLShapedCorridor(Vector2Int start, Vector2Int end)
    {
        List<Vector2Int> corridor = new List<Vector2Int>();
        //horizontal portion
        for (int x = start.x; x != end.x; x += (start.x < end.x) ? 1 : -1)
        {
            corridor.Add(new Vector2Int(x, start.y));
        }
        //vertical portion
        for (int y = start.y; y != end.y; y += (start.y < end.y) ? 1 : -1)
        {
            corridor.Add(new Vector2Int(end.x, y));
        }

        return corridor;
    }

    public Tilemap tilemap;
    public TileBase floorTile;
    public TileBase wallTile;

    public void DrawDungeon(List<RectInt> rooms, List<Vector2Int> corridors)
    {
        BoundsInt bounds = tilemap.cellBounds;
        tilemap.ClearAllTiles();

        // Draw Rooms
        foreach (var room in rooms)
        {
            for (int x = room.xMin; x < room.xMax; x++)
            {
                for (int y = room.yMin; y < room.yMax; y++)
                {
                    tilemap.SetTile(new Vector3Int(x, y, 0), floorTile);
                }
            }
        }

        // Draw Corridors
        foreach (var corridorPosition in corridors)
        {
            tilemap.SetTile(new Vector3Int(corridorPosition.x, corridorPosition.y, 0), floorTile);
        }

        // Draw Walls (Simple Example - Improve this!)
        for (int x = bounds.xMin; x < bounds.xMax; x++)
        {
            for (int y = bounds.yMin; y < bounds.yMax; y++)
            {
                if (tilemap.GetTile(new Vector3Int(x, y, 0)) == null)
                {
                    tilemap.SetTile(new Vector3Int(x, y, 0), wallTile);
                }
            }
        }
    }

}