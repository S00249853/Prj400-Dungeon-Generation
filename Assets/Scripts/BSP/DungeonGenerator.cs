using NUnit.Framework;
using UnityEngine;
using System.Collections.Generic;
using UnityEngine.Tilemaps;

public class DungeonGenerator : MonoBehaviour
{
    public Dungeon dungeon;
    private void Start()
    {
        dungeon.root = dungeon.BSPGenerator(new RectInt(new Vector2Int(0, 0), new Vector2Int(dungeon.dungeonWidth, dungeon.dungeonHeight)));
        List<Room> rooms = dungeon.GenerateRooms(dungeon.root);
        List<Vector2Int> corridors = dungeon.GenerateCorridors(rooms);
        dungeon.DrawDungeon(rooms, corridors);
    }
}
