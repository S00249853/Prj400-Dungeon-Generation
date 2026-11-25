using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class Dungeon : MonoBehaviour
{
    public Node root;

    public Tilemap Tilemap;
    public TileBase[] FloorTiles = new TileBase[5];
    public TileBase WallTile;

    List<RectInt> nodes = new List<RectInt>();

    [SerializeField] int _minRoomSize = 30;
    public int dungeonWidth;
    public int dungeonHeight;

    public int maxNoOfRooms;
    int _noOfRooms;

    public GameObject[] basicPrefabs;
    public GameObject[] enemyPrefabs;
    public GameObject[] treasurePrefabs;
    public GameObject[] bossPrefabs;
    public GameObject[] exitPrefabs;

    Dictionary<RoomType, GameObject[]> roomFiller;

    bool splitH = false;
    bool treasureSpawned = false;
    bool bossSpawned = false;
    bool exitSpawned = false;

    public Node BSPGenerator(RectInt size)
    {
        Node root = new Node(size);
        SplitNode(root);
        return root;
    }

    private void SplitNode(Node node)
    {
        _noOfRooms++;
        if (node.rect.width <= _minRoomSize || node.rect.height <= _minRoomSize)
        {
            nodes.Add(node.rect);
            return;
        }
        else if (node.rect.width < _minRoomSize && node.rect.height !< _minRoomSize)
        {
            splitH = true;
        }
        else if (node.rect.width !< _minRoomSize && node.rect.height < _minRoomSize)
        {
            splitH = false;
        }
        else
        {
            splitH = !splitH;
        }
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

    public List<Room> GenerateRooms(Node node)
    {
        roomFiller = new Dictionary<RoomType, GameObject[]>() {
            {RoomType.Basic, basicPrefabs},
            {RoomType.Enemies, enemyPrefabs },
            {RoomType.Treasure, treasurePrefabs },
            {RoomType.Boss, bossPrefabs },
            {RoomType.Exit, exitPrefabs } };
        List<Room> rooms = new List<Room>();
        TraverseAndCreateRooms(node, rooms);
        return rooms;
    }

    private void TraverseAndCreateRooms(Node node, List<Room> rooms)
    {
        if (node == null) return;

        if (node.Left == null && node.right == null)
        {
            int roomWidth = node.rect.width - 10;
            int roomHeight = node.rect.height - 10;
            int roomX = node.rect.x;
            int roomY = node.rect.y;
            if (roomWidth > 6 && roomHeight > 6)
            {
                RoomType type = (RoomType)Random.Range(0, 4);
                Room room = new Room(roomX, roomY, roomWidth, roomHeight, type, roomFiller[type]);
                rooms.Add(room);
            }
         else
            {
                return;
            }
        }
        else
        {
            TraverseAndCreateRooms(node.Left, rooms);
            TraverseAndCreateRooms(node.right, rooms);
        }
    }

    public List<Vector2Int> GenerateCorridors(List<Room> rooms)
    {
        List<Vector2Int> corridors = new List<Vector2Int>();
        for (int i = 0; i < rooms.Count - 1; i++)
        {
            Vector2Int room1Center = new Vector2Int(rooms[i].dimensions.xMin + rooms[i].dimensions.width / 2, rooms[i].dimensions.yMin + rooms[i].dimensions.height / 2);
            Vector2Int room2Center = new Vector2Int(rooms[i + 1].dimensions.xMin + rooms[i + 1].dimensions.width / 2, rooms[i + 1].dimensions.yMin + rooms[i + 1].dimensions.height / 2);
      
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

        for (int y = start.y; y != end.y; y += (start.y < end.y) ? 1 : -1)
        {
            corridor.Add(new Vector2Int(end.x, y));
        }
        return corridor;
    }

    private void OnDrawGizmos()
    {
        foreach (RectInt node in nodes)
        {
            Gizmos.DrawWireCube(new Vector3(node.center.x, node.center.y), new Vector3(node.size.x, node.size.y));
        }
    }

    public void DrawDungeon(List<Room> rooms, List<Vector2Int> corridors)
    {
        BoundsInt bounds = Tilemap.cellBounds;
        bounds.yMax = root.rect.yMax + 10;
        bounds.xMax = root.rect.xMax+ 10;
        Tilemap.ClearAllTiles();

        foreach (Room room in rooms)
        {
            for (int x = room.dimensions.xMin; x < room.dimensions.xMax; x++)
            {
                for (int y = room.dimensions.yMin; y < room.dimensions.yMax; y++)
                {
                    Tilemap.SetTile(new Vector3Int(x, y, 0), FloorTiles[(int)room.type + 1]);  
                }
            }
            room.FillRoom();
        }

        foreach (var corridorPosition in corridors)
        {
            if (Tilemap.GetTile(new Vector3Int(corridorPosition.x,corridorPosition.y, 0)) == null)
            Tilemap.SetTile(new Vector3Int(corridorPosition.x, corridorPosition.y, 0), FloorTiles[1]);
        }

        //for (int x = bounds.xMin; x < bounds.xMax; x++)
        //{
        //    for (int y = bounds.yMin; y < bounds.yMax; y++)
        //    {
        //        if (Tilemap.GetTile(new Vector3Int(x, y, 0)) == null)
        //        {
        //            Tilemap.SetTile(new Vector3Int(x, y, 0), WallTile);
        //        }
        //    }
        //}
    }

}

