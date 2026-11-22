using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;

public class Dungeon : MonoBehaviour
{
    private Node _root;
    private List<RectInt> _rooms;
    private List<Vector2Int> _corridors;
    [SerializeField] int _minRoomSize;
    [SerializeField] int _dungeonWidth;
    [SerializeField] int _dungeonHeight;
    [SerializeField] DungeonGenerator _generator;

    public Node BSPGenerator(RectInt size)
    {
        _root = new Node(size);
        SplitNode(_root);
        return _root;
    }

    private void SplitNode(Node node)
    {
        if (node.rect.width < _minRoomSize || node.rect.height < _minRoomSize)
        {
            return;
        }

        bool splitH = Random.Range(0f, 1f) > 0.5f;
        int splitPoint = splitH ? Random.Range(_minRoomSize, node.rect.width - _minRoomSize) : Random.Range(_minRoomSize, node.rect.height - _minRoomSize);

        if (splitH)
        {
            node.Left = new Node(new RectInt(node.rect.x, node.rect.y, splitPoint, node.rect.height));
            node.right = new Node(new RectInt(node.rect.x + splitPoint, node.rect.y, node.rect.width - splitPoint, node.rect.height));
        }

        else
        {
            node.Left = new Node(new RectInt(node.rect.x, node.rect.y, node.rect.width, splitPoint));
            node.right = new Node(new RectInt(node.rect.x, node.rect.y + splitPoint, node.rect.width, node.rect.height - splitPoint));
        }

        SplitNode(node.Left);
        SplitNode(node.right);
    }

    public List<RectInt> GenerateRooms(Node node)
    {
        List<RectInt> rooms = new List<RectInt>();
        TraverseAndCreateRooms(node, rooms);
        return rooms;
    }

    private void TraverseAndCreateRooms(Node node, List<RectInt> rooms)
    {
        if (node == null) return;

        if (node.Left == null && node.right == null)
        {
            int roomWidth = Random.Range(_minRoomSize / 2, node.rect.width - 1);
            int roomHeight = Random.Range(_minRoomSize / 2, node.rect.height - 1);
            int roomX = Random.Range(node.rect.x, node.rect.x + node.rect.width - roomWidth);
            int roomY = Random.Range(node.rect.y, node.rect.y + node.rect.height - roomHeight);

            RectInt room = new RectInt(roomX, roomY, roomWidth, roomHeight);
            rooms.Add(room);
        }
        else
        {
            TraverseAndCreateRooms(node.Left, rooms);
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
      
            foreach(var pos in CreateLShapedCorridor(room1Center, room2Center))
            {
                corridors.Add(pos);
            }
        }
        return corridors;
    }

    private List<Vector2Int> CreateLShapedCorridor(Vector2Int start, Vector2Int end)
    {
        List<Vector2Int> corridor = new List<Vector2Int>();

        for (int x = start.x; x != end.x; x += (start.x < end.x) ? 1 : -1)
        {
            corridor.Add(new Vector2Int(x, start.y));
        }

        for (int y = start.y; y != end.y; y+= (start.y < end.y) ? 1 : -1)
        {
            corridor.Add(new Vector2Int(end.x, y));
        }
        return corridor;
    }

    private void Start()
    {
        RectInt size = new RectInt(0, 1000, _dungeonWidth, _dungeonHeight);
        _root = BSPGenerator(size);
        _rooms = GenerateRooms(_root);
        _corridors = GenerateCorridors(_rooms);

        _generator.DrawDungeon(_rooms, _corridors);
    }

}

