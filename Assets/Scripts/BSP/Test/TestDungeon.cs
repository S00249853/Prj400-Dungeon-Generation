using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;

public class TestDungeon : MonoBehaviour
{
    BSPNode root;
    public BSPGenerator g;
    private void Start()
    {
        root = g.GenerateBSP(new RectInt(new Vector2Int(0,0), new Vector2Int(1000,1000)));
        List<RectInt> rooms = g.GenerateRooms(root);
        List<Vector2Int> corridors = g.GenerateCorridors(rooms);
        g.DrawDungeon(rooms, corridors);
    }
}
