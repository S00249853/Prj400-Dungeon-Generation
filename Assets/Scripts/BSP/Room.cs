using Unity.VisualScripting;
using UnityEngine;

public enum RoomType
{
    Basic,
    Enemies,
    Treasure,
    Boss
}
public class Room : MonoBehaviour
{
    public RectInt dimensions;
    public RoomType type;
    public GameObject[] items;
    public Room(int x, int y, int w, int h, RoomType Type, GameObject[] Items)
    {
        dimensions = new RectInt(x, y, w, h);
       type = Type;
        items = Items;
    }

    public void FillRoom()
    {
        foreach (GameObject item in items)
        {
            Vector3 pos = new Vector3(Random.Range(dimensions.xMin, dimensions.xMax), Random.Range(dimensions.yMin, dimensions.yMax));
            Instantiate(item, pos, item.transform.rotation);
        }
    }
}
