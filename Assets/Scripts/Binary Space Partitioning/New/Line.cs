using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class Line
{
    Orientation orientation;
    Vector2Int coordinates;

    public Line(Orientation orientation, Vector2Int coordinates)
    {
        this.orientation = orientation;
        this.coordinates = coordinates;
    }
    public Orientation Orientation { get { return orientation; } set { orientation = value; } }
    public Vector2Int Coordinates { get { return coordinates; } set { coordinates = value; } }
}