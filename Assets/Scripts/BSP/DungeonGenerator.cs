using NUnit.Framework;
using UnityEngine;
using System.Collections.Generic;
using UnityEngine.Tilemaps;

public class DungeonGenerator : MonoBehaviour
{
    public Tilemap Tilemap;
    public TileBase FloorTile;
    public TileBase WallTile;


    public void DrawDungeon(List<RectInt> rooms, List<Vector2Int> corridors)
    {
        BoundsInt bounds = Tilemap.cellBounds;
        Tilemap.ClearAllTiles();

        foreach (var room in rooms)
        {
            for (int x = room.xMin; x < room.xMax; x++)
            {
                for (int y = room.yMin; y < room.yMax; y++)
                {
                    Tilemap.SetTile(new Vector3Int(x, y, 0), FloorTile);
                }
            }
        }

        foreach (var corridorPosition in corridors)
        {
            Tilemap.SetTile(new Vector3Int(corridorPosition.x, corridorPosition.y, 0), FloorTile);
        }

        for (int x = bounds.xMin; x < bounds.xMax; x++)
        {
            for(int y = bounds.yMin; y < bounds.yMax; y++)
            {
                if (Tilemap.GetTile(new Vector3Int(x,y,0)) == null)
                {
                    Tilemap.SetTile(new Vector3Int(x, y, 0), WallTile);
                }
            }
        }
    }
}
