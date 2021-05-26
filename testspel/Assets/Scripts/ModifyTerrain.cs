        using UnityEngine;
using UnityEngine.Tilemaps;

public class ModifyTerrain : MonoBehaviour
{
    public Tilemap groundMap;
    public Tilemap oreMap;
    public Tilemap overlayMap;
    public Tile markerTile;
    public Tile buildingTile;
    public Tile stage1;
    public Tile stage2;
    public Tile stage3;
    public Tile stage4;
    public Tile stage5;
    public Tile stage6;
    public Tile stage7;
    public Tile stage8;
    public Tile stage9;
    float timeUntilBreak = 1.0f;
    Vector3Int clickedTile;
    Vector3Int selectedTile;
    Vector3Int hoveredTile;
    bool breaking = false;

    bool buttonPressed = false;
    IronCounter ironCounter;
    InventoryManager inventoryManager;
    // Start is called before the first frame update
    void Start()
    {
        selectedTile = GetSelectedTile(groundMap);
        if (groundMap.GetTile(selectedTile) != null) overlayMap.SetTile(selectedTile, markerTile);
        GameObject text = GameObject.Find("Text");
        ironCounter = text.GetComponent<IronCounter>();
        GameObject hotbar = GameObject.Find("Hotbar");
        inventoryManager = hotbar.GetComponent<InventoryManager>();
    }

    // Update is called once per frame
    void Update()
    {
        hoveredTile = GetSelectedTile(groundMap);
        if (selectedTile != hoveredTile)
        {
            overlayMap.SetTile(selectedTile, null);
            selectedTile = hoveredTile;
            if (groundMap.GetTile(selectedTile) != null)
            {
                overlayMap.SetTile(selectedTile, markerTile);
            }
        }

        switch (inventoryManager.itemSelected)
        {
            case InventoryManager.Item.PICKAXE:
                if (Input.GetMouseButtonDown(0))
                {
                    if (groundMap.GetTile(selectedTile) != null)
                    {
                        buttonPressed = true;
                        clickedTile = selectedTile;
                        breaking = true;
                    }
                }
                else if (Input.GetMouseButtonUp(0))
                {
                    buttonPressed = false;
                }

                if (breaking)
                {
                    if (buttonPressed)
                    {
                        if (clickedTile != hoveredTile)
                        {
                            timeUntilBreak = 1.0f;
                            overlayMap.SetTile(clickedTile, null);
                            if (groundMap.GetTile(hoveredTile) != null)
                            {
                                clickedTile = selectedTile;
                            }
                        }
                        if (timeUntilBreak > 0)
                        {
                            timeUntilBreak -= Time.deltaTime;

                            if (timeUntilBreak > (float)8 / 9) overlayMap.SetTile(clickedTile, stage1);
                            else if (timeUntilBreak > (float)7 / 9) overlayMap.SetTile(clickedTile, stage2);
                            else if (timeUntilBreak > (float)6 / 9) overlayMap.SetTile(clickedTile, stage3);
                            else if (timeUntilBreak > (float)5 / 9) overlayMap.SetTile(clickedTile, stage4);
                            else if (timeUntilBreak > (float)4 / 9) overlayMap.SetTile(clickedTile, stage5);
                            else if (timeUntilBreak > (float)3 / 9) overlayMap.SetTile(clickedTile, stage6);
                            else if (timeUntilBreak > (float)2 / 9) overlayMap.SetTile(clickedTile, stage7);
                            else if (timeUntilBreak > (float)1 / 9) overlayMap.SetTile(clickedTile, stage8);
                            else overlayMap.SetTile(clickedTile, stage9);
                        }
                        else
                        {
                            if (oreMap.GetTile(clickedTile) != null)
                            {
                                oreMap.SetTile(clickedTile, null);
                                ironCounter.IronCount += 1;
                            }
                            overlayMap.SetTile(clickedTile, null);
                            groundMap.SetTile(clickedTile, null);
                            timeUntilBreak = 1.0f;
                            breaking = false;
                        }

                    }
                    else
                    {
                        overlayMap.SetTile(clickedTile, null);
                        timeUntilBreak = 1.0f;
                        breaking = false;
                    }
                }
                break;
            case InventoryManager.Item.DRILL:
                if (Input.GetMouseButtonDown(0))
                {
                    breaking = false;
                    buttonPressed = false;
                    groundMap.SetTile(selectedTile, null);
                    overlayMap.SetTile(selectedTile, null);
                    if (oreMap.GetTile(selectedTile) != null)
                    {
                        oreMap.SetTile(selectedTile, null);
                        ironCounter.IronCount += 1;
                    }
                }
                break;
            case InventoryManager.Item.DYNAMITE:
                break;
        }

        if (Input.GetMouseButtonDown(1))
        {
            groundMap.SetTile(selectedTile, buildingTile);
        }
    }

    void FixedUpdate()
    {
        switch (inventoryManager.itemSelected)
        {
            case InventoryManager.Item.PICKAXE:
                if (breaking)
                {
                    if (buttonPressed)
                    {
                        if (clickedTile == GetSelectedTile(groundMap))
                        {
                            if (timeUntilBreak > 0)
                            {
                                timeUntilBreak -= Time.deltaTime;

                                if (timeUntilBreak > (float)8 / 9) overlayMap.SetTile(clickedTile, stage1);
                                else if (timeUntilBreak > (float)7 / 9) overlayMap.SetTile(clickedTile, stage2);
                                else if (timeUntilBreak > (float)6 / 9) overlayMap.SetTile(clickedTile, stage3);
                                else if (timeUntilBreak > (float)5 / 9) overlayMap.SetTile(clickedTile, stage4);
                                else if (timeUntilBreak > (float)4 / 9) overlayMap.SetTile(clickedTile, stage5);
                                else if (timeUntilBreak > (float)3 / 9) overlayMap.SetTile(clickedTile, stage6);
                                else if (timeUntilBreak > (float)2 / 9) overlayMap.SetTile(clickedTile, stage7);
                                else if (timeUntilBreak > (float)1 / 9) overlayMap.SetTile(clickedTile, stage8);
                                else overlayMap.SetTile(clickedTile, stage9);
                            }
                            else
                            {
                                if (oreMap.GetTile(clickedTile) != null)
                                {
                                    oreMap.SetTile(clickedTile, null);
                                    ironCounter.IronCount += 1;
                                }
                                overlayMap.SetTile(clickedTile, null);
                                groundMap.SetTile(clickedTile, null);
                                timeUntilBreak = 1.0f;
                                breaking = false;
                            }
                        }
                        else
                        {
                            timeUntilBreak = 1.0f;
                            overlayMap.SetTile(clickedTile, null);
                            clickedTile = GetSelectedTile(groundMap);
                        }
                    }
                    else
                    {
                        overlayMap.SetTile(clickedTile, null);
                        timeUntilBreak = 1.0f;
                        breaking = false;
                    }
                }
                break;
            case InventoryManager.Item.DRILL:
                break;
            case InventoryManager.Item.DYNAMITE:
                break;
            default:
                break;
        }
    }

    Vector3Int GetSelectedTile(Tilemap tm)
    {
        return tm.WorldToCell(Camera.main.ScreenToWorldPoint(Input.mousePosition));
    }
    /* void Update() {
        if(Input.GetMouseButtonDown(0))
            {
                clickedTile = groundMap.WorldToCell(Camera.main.ScreenToWorldPoint(Input.mousePosition));
                Debug.Log(groundMap.GetTile(clickedTile));
            }
    } */
}
