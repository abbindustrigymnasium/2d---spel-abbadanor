using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class ModTerrain : MonoBehaviour
{
    public Tilemap earthMap;
    public Tilemap oreMap;
    public Tilemap overlayMap;
    public Tile selector;
    public Tile buildingTile;
    public GameObject dynamitePrefab;
    public float pickaxeRange;
    public float drillRange;
    public float dynamiteRange;
    public float pickaxeBreakTime;
    public float drillBreakTime;
    public List<Tile> miningTiles;

    InventoryManager inventoryManager;
    InventoryManager.Item itemSelected;

    GameObject player;

    bool mouseDown = false;
    float breakTime = 0;
    Vector3Int hoveredTile;
    Vector3Int miningTile;
    void Start()
    {
        inventoryManager = GameObject.Find("Hotbar").GetComponent<InventoryManager>();
        player = GameObject.FindGameObjectWithTag("Player");
    }

    // Update is called once per frame
    void Update()
    {
        itemSelected = inventoryManager.itemSelected;

        SetSelector();

        CheckMouse();

        Build();

        Mine(itemSelected);
    }

    Vector3Int GetTileFromMouse()
    {
        return earthMap.WorldToCell(Camera.main.ScreenToWorldPoint(Input.mousePosition));
    }

    Vector3 GetWorldFromTile(Vector3Int tile)
    {
        return earthMap.CellToWorld(tile);
    }

    void SetSelector()
    {
        Vector3Int hoveredTile = GetTileFromMouse();

        if (hoveredTile != this.hoveredTile)
        {
            overlayMap.SetTile(this.hoveredTile, null);
            overlayMap.SetTile(hoveredTile, selector);
        }

        this.hoveredTile = hoveredTile;
    }

    bool CheckIfAir(Vector3Int tile)
    {
        return earthMap.GetTile(tile) == null;
    }

    void CheckMouse()
    {
        if (Input.GetMouseButtonDown(0))
        {
            mouseDown = true;
        }
        else if (Input.GetMouseButtonUp(0))
        {
            mouseDown = false;
        }
    }

    void DestroyTile(Vector3Int p)
    {
        earthMap.SetTile(p, null);
        if (oreMap.GetTile(p) != null)
        {
            IronCounter.IronCount += 1;
            oreMap.SetTile(p, null);
        }
    }

    void Build()
    {
        if(Input.GetMouseButtonDown(1))
        {
            if(CheckIfAir(hoveredTile))
            {
                earthMap.SetTile(hoveredTile, buildingTile);
            }
        }
    }

    void SetMiningStage(float toolBreakTime)
    {
        int stage = Mathf.RoundToInt(9 * (breakTime / toolBreakTime)) - 1;
        stage = Mathf.Clamp(stage, 0, 8);
        overlayMap.SetTile(miningTile, miningTiles[stage]);
    }

    bool isInPlayerRange(Vector3 p, float range)
    {
        float dst = Vector3.Distance(player.transform.position, p);
        return dst <= range;
    }

    Vector3 getCentralizedVector(Vector3 p)
    {
        return new Vector3(p.x + 0.5f, p.y + 0.5f, p.z);
    }

    void Mine(InventoryManager.Item itemSelected)
    {
        if (itemSelected == InventoryManager.Item.DYNAMITE)
        {
            if (Input.GetMouseButtonDown(0))
            {
                if(!CheckIfAir(hoveredTile) && isInPlayerRange(getCentralizedVector(earthMap.CellToWorld(hoveredTile)), dynamiteRange))
                {
                    Vector3 playerPos = player.transform.position;
                    Vector3 targetPos = earthMap.CellToWorld(hoveredTile);
                    GameObject dynamite = Instantiate(dynamitePrefab, playerPos, Quaternion.identity);
                    Explode dynamiteScript = dynamite.GetComponent<Explode>();
                    dynamiteScript.throwLocation = playerPos;
                    dynamiteScript.targetLocation = targetPos;
                }
            }
        }
        else
        {
            if (mouseDown)
            {
                if (!CheckIfAir(hoveredTile))
                {
                    if (isInPlayerRange(getCentralizedVector(earthMap.CellToWorld(hoveredTile)), pickaxeRange))
                    {
                        if (breakTime == 0) breakTime = (itemSelected == InventoryManager.Item.PICKAXE) ? pickaxeBreakTime : drillBreakTime;

                        if (miningTile == hoveredTile)
                        {
                            if (breakTime > 0)
                            {
                                breakTime -= Time.deltaTime;
                                SetMiningStage((itemSelected == InventoryManager.Item.PICKAXE) ? pickaxeBreakTime : drillBreakTime);
                            }
                            else
                            {
                                breakTime = 0;
                                overlayMap.SetTile(miningTile, selector);
                                DestroyTile(miningTile);
                            }
                        }
                        else
                        {
                            breakTime = 0;
                            miningTile = hoveredTile;
                        }
                    }
                }
            }
        }
    }
}
