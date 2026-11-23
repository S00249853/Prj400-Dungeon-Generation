using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;

public class TestDungeon : MonoBehaviour
{
   
    public BSPGenerator g;
    private void Start()
    {
        g.root = g.GenerateBSP(new RectInt(new Vector2Int(0,0), new Vector2Int(100,100)));
        List<RectInt> rooms = g.GenerateRooms(g.root);
        List<Vector2Int> corridors = g.GenerateCorridors(rooms);
        g.DrawDungeon(rooms, corridors);
    }
}
