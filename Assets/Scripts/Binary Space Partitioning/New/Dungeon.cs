using UnityEngine;

public class Dungeon : MonoBehaviour
{
    public int dungeonWidth, dungeonLength, roomWidthMin, roomLengthMin, maxIterations, corridorWidth;
    public Material material;
    private void Start()
    {
        DungeonCreator();
    }
    private void DungeonCreator()
    {
        DungeonGenerator generator = new DungeonGenerator(dungeonWidth, dungeonLength);
        var listOfRooms = generator.CalculateRooms(maxIterations,roomWidthMin,roomLengthMin);
        for (int i = 0; i < listOfRooms.Count; i++)
        {
            CreateMesh(listOfRooms[i].BottomLeftAreaCorner, listOfRooms[i].TopRightAreaCorner);
        }
    }

    private void CreateMesh(Vector2 bottomLeftCorner, Vector2 topRightCorner)
    {
        Vector3 BottomLeftCorner = new Vector3(bottomLeftCorner.x, 0, bottomLeftCorner.y);
        Vector3 BottomRightCorner = new Vector3(topRightCorner.x, 0, bottomLeftCorner.y);
        Vector3 TopLeftCorner = new Vector3(bottomLeftCorner.x, 0, topRightCorner.y);
        Vector3 TopRightCorner = new Vector3(topRightCorner.x, 0, topRightCorner.y);

        Vector3[] vertices = new Vector3[]
        {
            TopLeftCorner,
            TopRightCorner,
            BottomLeftCorner,
            BottomRightCorner
        };

        Vector2[] uvs = new Vector2[vertices.Length];
        for (int i = 0; i < uvs.Length; i++)
        {
            uvs[i] = new Vector2(vertices[i].x, vertices[i].z);
        }

        int[] triangles = new int[]
        {
            0,
            1,
            2,
            2,
            1,
            3
        };
        Mesh mesh = new Mesh();
        mesh.vertices = vertices;
        mesh.uv = uvs;
        mesh.triangles = triangles;

        GameObject dungeonFloor = new GameObject("Mesh"+bottomLeftCorner, typeof(MeshFilter), typeof(MeshRenderer));

        dungeonFloor.transform.position = Vector3.zero;
        dungeonFloor.transform.localScale = Vector3.one;
        dungeonFloor.GetComponent<MeshFilter>().mesh = mesh;
        dungeonFloor.GetComponent<MeshRenderer>().material = material;
    }

}
