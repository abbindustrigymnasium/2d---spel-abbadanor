using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEditor.Tilemaps;

public class Mining : MonoBehaviour
{
    public Tilemap earthMap;
    public Tilemap oreMap;
    public Tilemap overlayMap;
    public Tile selectorTile;
    public GameObject dynamite;
    public int maxDst;
    public float breakTimePickaxe;
    public float breakTimeDrill;
    public float explosionTime;
    GameObject thrownDynamite;

    Vector3Int defaultTilePos = new Vector3Int(-1, -1, -1);
    Vector3Int selectedTilePos;
    Vector3Int clickedTilePos;

    Vector3 explodeTargetPos;
    Vector3 explodePlayerPos;
    bool isExploding = false;
    float timeUntilExplosion;
    bool mouseBtnDown = false;
    float timeUntilBreak;
    List<Tile> miningTiles = new List<Tile>();
    InventoryManager inventoryManager;

    void Start()
    {
        selectedTilePos = defaultTilePos;
        clickedTilePos = defaultTilePos;
        timeUntilBreak = breakTimePickaxe;

        for (int i = 8; i >= 0; i--)
        {
            miningTiles.Add(Resources.Load<Tile>("Palette/overlay_tiles/overlay_tileset_" + i.ToString()));
        }

        GameObject hotbar = GameObject.Find("Hotbar");
        inventoryManager = hotbar.GetComponent<InventoryManager>();
    }

    void Update()
    {
        Vector3Int hoveredTilePosGrid = GetSelectedTile(earthMap);
        TileBase hoveredTile = earthMap.GetTile(hoveredTilePosGrid);
        if (hoveredTile == null && selectedTilePos != defaultTilePos)
        {
            overlayMap.SetTile(selectedTilePos, null);
            selectedTilePos = defaultTilePos;
        }
        else if (hoveredTilePosGrid != selectedTilePos && hoveredTile  != null)
        {
            overlayMap.SetTile(selectedTilePos, null);
            float dst = Vector2.Distance(GameObject.FindGameObjectWithTag("Player").transform.position, earthMap.CellToWorld(hoveredTilePosGrid));
            if (dst <= maxDst)
            {
                overlayMap.SetTile(hoveredTilePosGrid, selectorTile);
                selectedTilePos = hoveredTilePosGrid;
            }
            else
            {
                selectedTilePos = defaultTilePos;
            }
        }

        if (Input.GetMouseButtonDown(0))
        {
            mouseBtnDown = true;
            if(inventoryManager.itemSelected == InventoryManager.Item.DYNAMITE)
            {
                if(selectedTilePos != defaultTilePos)
                {
                    explodePlayerPos = GameObject.FindGameObjectWithTag("Player").transform.position;
                    explodeTargetPos = earthMap.CellToWorld(hoveredTilePosGrid);
                    thrownDynamite = Instantiate(dynamite, explodePlayerPos, Quaternion.identity);
                    isExploding = true;
                    timeUntilExplosion = 1.0f;
                    Debug.Log("Explosion at: " + earthMap.CellToWorld(selectedTilePos));
                }
            }
        }

        if(isExploding)
        {
            if (timeUntilExplosion > 0)
            {
                float trajectoryProgress = Mathf.Clamp(timeUntilExplosion / 1.0f, 0.0f, 1.0f);
                Debug.Log(trajectoryProgress);
                timeUntilExplosion -= Time.deltaTime;
            }
            else
            {
                Debug.Log("Exploded!");
                isExploding = false;
            }
        }

        if (Input.GetMouseButtonUp(0))
        {
            mouseBtnDown = false;
        }

        /*  TODO:
            Add mousebtn hold functionality
            Add distance sensitivity
        */  
        if(Input.GetMouseButtonDown(1))
        {
            if(hoveredTile == null)
            {
                earthMap.SetTile(hoveredTilePosGrid, Resources.Load<Tile>("Palette/ground_tiles/tileset_18"));
            }
        }

        if(inventoryManager.itemSelected != InventoryManager.Item.DYNAMITE)
        {
            if (mouseBtnDown)
            {
                if (selectedTilePos != defaultTilePos)
                {
                    if (clickedTilePos == selectedTilePos)
                    {
                        if (timeUntilBreak > 0)
                        {
                            timeUntilBreak -= Time.deltaTime; //
                            if(inventoryManager.itemSelected == InventoryManager.Item.PICKAXE) overlayMap.SetTile(clickedTilePos, GetMiningStage(breakTimePickaxe));
                            else overlayMap.SetTile(clickedTilePos, GetMiningStage(breakTimeDrill));
                        }
                        else
                        {
                            if (inventoryManager.itemSelected == InventoryManager.Item.PICKAXE) timeUntilBreak = breakTimePickaxe;
                            else timeUntilBreak = breakTimeDrill;
                            overlayMap.SetTile(clickedTilePos, null);
                            earthMap.SetTile(clickedTilePos, null);
                        }
                    }
                    else
                    {
                        overlayMap.SetTile(clickedTilePos, null);
                        clickedTilePos = selectedTilePos;
                    }
                }

            }
        }
        
    }
    Vector3Int GetSelectedTile(Tilemap tm)
    {
        return tm.WorldToCell(Camera.main.ScreenToWorldPoint(Input.mousePosition));
    }

    Tile GetMiningStage(float breakTime)
    {
        int stage = Mathf.RoundToInt(9 * (timeUntilBreak / breakTime)) - 1;
        stage = Mathf.Clamp(stage, 0, 8);
        return miningTiles[stage];
    }

    void Explode(Vector3 playerPos, Vector3 targetPos)
    {

        Instantiate(dynamite, playerPos, Quaternion.identity);
    }
}
