using UnityEngine;
using UnityEngine.Tilemaps;

public class GenerateTerrain : MonoBehaviour
{
    public RuleTile groundRuletile;
    public Tilemap groundTilemap;
    public Tile oreTile;
    public Tilemap oreTilemap;
    public float scale;
    public float amplitude;
    public int worldSize = 1024;

    [Range(0.0f, 1.0f)]
    public float rarity;
    void Start() {
        for(int x = -worldSize; x <= worldSize; x++) {
            float xFloat = (float)x / worldSize * amplitude;
            float height = Mathf.PerlinNoise(xFloat, 0.0f) * scale;
            for(int y = (int) height; y > -100; y--) {
                if (y < 10 && Mathf.PerlinNoise(xFloat, y / (height + 100) * 16) > rarity) oreTilemap.SetTile(new Vector3Int(x, y, 0), oreTile);
                
                groundTilemap.SetTile(new Vector3Int(x, y, 0), groundRuletile);
            }
        }
    }
}
