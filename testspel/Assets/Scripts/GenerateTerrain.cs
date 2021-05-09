using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEditor.Tilemaps;
using UnityEngine.Tilemaps;

public class GenerateTerrain : MonoBehaviour
{
    public RuleTile tile;
    public Tilemap tilemap;
    public float scale;
    public float amplitude;
    public int chunkSize = 128;
    void Start() {
        for(int x = -128; x <= 128; x++) {
            float xFloat = (float)x / chunkSize * amplitude;
            float height = Mathf.PerlinNoise(xFloat, 0.0f) * scale;
            for(int y = (int) height; y > -100; y--) {
                tilemap.SetTile(new Vector3Int(x, y, 0), tile);
            }
        }
    }
}
