using UnityEngine;

[ExecuteInEditMode]
[RequireComponent(typeof(MeshFilter), typeof(MeshRenderer))]
public class TerrainConformingPlane : MonoBehaviour
{
    public Terrain terrain;
    public Material InvisiblePlane;
    public float yOffset = 0f;

    [ContextMenu("Update Mesh")] // Adds a right-click context menu
    public void GenerateAndConformMesh()
    {
        if (!terrain) terrain = Terrain.activeTerrain;
        if (!terrain)
        {
            Debug.LogError("No terrain assigned or found!");
            return;
        }

        TerrainData tData = terrain.terrainData;
        Vector3 terrainSize = tData.size;
        int resolution = tData.heightmapResolution;

        Mesh mesh = new Mesh();
        mesh.indexFormat = UnityEngine.Rendering.IndexFormat.UInt32; // allows >65k vertices

        Vector3[] vertices = new Vector3[resolution * resolution];
        Vector2[] uvs = new Vector2[vertices.Length];
        int[] triangles = new int[(resolution - 1) * (resolution - 1) * 6];

        float step = terrainSize.x / (resolution - 1);
        Vector3 terrainPos = terrain.transform.position;

        for (int z = 0; z < resolution; z++)
        {
            for (int x = 0; x < resolution; x++)
            {
                int i = z * resolution + x;

                float worldX = terrainPos.x + x * step;
                float worldZ = terrainPos.z + z * step;

                float normX = (float)x / (resolution - 1);
                float normZ = (float)z / (resolution - 1);
                float height = tData.GetInterpolatedHeight(normX, normZ) + yOffset;

                vertices[i] = new Vector3(worldX, height, worldZ);
                uvs[i] = new Vector2(normX, normZ);
            }
        }

        int t = 0;
        for (int z = 0; z < resolution - 1; z++)
        {
            for (int x = 0; x < resolution - 1; x++)
            {
                int i = z * resolution + x;

                triangles[t++] = i;
                triangles[t++] = i + resolution;
                triangles[t++] = i + 1;

                triangles[t++] = i + 1;
                triangles[t++] = i + resolution;
                triangles[t++] = i + resolution + 1;
            }
        }

        mesh.vertices = vertices;
        mesh.uv = uvs;
        mesh.triangles = triangles;
        mesh.RecalculateNormals();
        mesh.RecalculateBounds();

        // Assign mesh to MeshFilter
        MeshFilter mf = GetComponent<MeshFilter>();
        mf.sharedMesh = mesh;

        // Assign material
        MeshRenderer mr = GetComponent<MeshRenderer>();
        if (InvisiblePlane != null)
            mr.sharedMaterial = InvisiblePlane;
        else
            mr.sharedMaterial = new Material(Shader.Find("Standard"));

        // Add or update MeshCollider
        MeshCollider mc = GetComponent<MeshCollider>();
        if (!mc) mc = gameObject.AddComponent<MeshCollider>();
        mc.sharedMesh = null; // force refresh
        mc.sharedMesh = mesh;

        Debug.Log("Mesh updated in editor.");
    }

    void OnValidate()
    {
        if (Application.isPlaying) return;
        GenerateAndConformMesh();
    }
}


