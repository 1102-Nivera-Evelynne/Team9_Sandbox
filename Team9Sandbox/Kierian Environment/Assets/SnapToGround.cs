using UnityEngine;

public class GroundAligner : MonoBehaviour
{
    public Terrain terrain;
    public float yOffset = 0.05f;

    void Start()
    {
        if (!terrain) terrain = Terrain.activeTerrain;
        Vector3 pos = transform.position;
        float terrainHeight = terrain.SampleHeight(pos) + terrain.GetPosition().y + yOffset;
        pos.y = terrainHeight;
        transform.position = pos;
    }
}