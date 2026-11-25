using Unity.VisualScripting;
using UnityEngine;

public enum RoomType
{
    Basic,
    Enemies,
    Treasure,
    Boss,
    Exit
}
public class Room : MonoBehaviour
{
    public RectInt dimensions;
    public RoomType type;

    public Room(int x, int y, int w, int h)
    {
        dimensions = new RectInt(x, y, w, h);
        type = (RoomType)Random.Range(0, 4);
    }
}
