using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEditor.Tilemaps;

public class ModifyTerrain : MonoBehaviour
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

    Vector3Int defaultTilePos = new Vector3Int(-1, -1, -1);
    Vector3Int selectedTilePos;
    Vector3Int clickedTilePos;

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
            float dst = Vector3.Distance(GameObject.FindGameObjectWithTag("Player").transform.position, earthMap.CellToWorld(hoveredTilePosGrid));
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
                    Vector3 explodePlayerPos = GameObject.FindGameObjectWithTag("Player").transform.position;
                    Vector3 explodeTargetPos = earthMap.CellToWorld(hoveredTilePosGrid);
                    GameObject dyn = Instantiate(dynamite, explodePlayerPos, Quaternion.identity);
                    Explode dynamiteScript = dyn.GetComponent<Explode>();
                    dynamiteScript.throwLocation = explodePlayerPos;
                    dynamiteScript.targetLocation = explodeTargetPos;
                }
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
                            DestroyTile(clickedTilePos);
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

    public void Explode(Vector3 position, int radius)
    {
        Vector3Int explodedTilePos = earthMap.WorldToCell(position);
        for (int x = explodedTilePos.x - radius; x <= explodedTilePos.x + radius; x++)
        {
            for (int y = explodedTilePos.y - radius; y <= explodedTilePos.y + radius; y++)
            {
                Vector3Int p = new Vector3Int(x, y, explodedTilePos.z);
                if (inCircle(p, explodedTilePos, 2.5f)) DestroyTile(p);            }
        }
    }

    bool inCircle(Vector3Int pInt, Vector3Int mInt, float radius)
    {
        Vector3 p = pInt;
        Vector3 m = mInt;

        float dst = Vector3.Distance(p, m);
        return dst < radius;
    }

    void DestroyTile(Vector3Int p)
    {
        earthMap.SetTile(p, null);
        if(oreMap.GetTile(p) != null)
        {
            IronCounter.IronCount += 1;
            oreMap.SetTile(p, null);
        }
    }
}
