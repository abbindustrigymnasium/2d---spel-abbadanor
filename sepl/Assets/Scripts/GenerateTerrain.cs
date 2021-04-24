using System;
using UnityEngine;
using UnityEditor.Tilemaps;
using UnityEngine.Tilemaps;

public class GenerateTerrain : MonoBehaviour
{
    public RuleTile tile;
    public Tilemap tilemap;

    public double noise1 = 1;
    public double noise2 = 1;
    public double noise3 = 0.7;
    public double noise4 = 0.4;
    // Start is called before the first frame update
    void Start()
    {
        for(int x = -100; x < 100; x++) {
            for(int y = Convert.ToInt32(noise1 * Mathf.Sin(x / 16) + noise2 * Mathf.Sin(x / 8) + noise3 * Mathf.Sin(x / 4) + noise3 * Mathf.Sin(x / 2)); y > -100; y--) {
                tilemap.SetTile(new Vector3Int(x, y, 0), tile);
            }
            
        }   
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
