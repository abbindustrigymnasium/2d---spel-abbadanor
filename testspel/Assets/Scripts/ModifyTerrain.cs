using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor.Tilemaps;
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
        if (selectedTile != GetSelectedTile(groundMap))
        {
            overlayMap.SetTile(selectedTile, null);
            selectedTile = GetSelectedTile(groundMap);
            if (groundMap.GetTile(selectedTile) != null) overlayMap.SetTile(selectedTile, markerTile);
        }


        if (inventoryManager.itemSelected == InventoryManager.Item.PICKAXE)
        {

            if (Input.GetMouseButtonDown(0))
            {
                buttonPressed = true;
                clickedTile = selectedTile;
                if (groundMap.GetTile(clickedTile) != null) breaking = true;
            }

            if (Input.GetMouseButtonUp(0)) buttonPressed = false;
        } else if(inventoryManager.itemSelected == InventoryManager.Item.DRILL) {
            if(Input.GetMouseButtonDown(0)) {
                breaking = false;
                buttonPressed = false;
                groundMap.SetTile(selectedTile, null);
                overlayMap.SetTile(selectedTile, null);
                if(oreMap.GetTile(selectedTile) != null) {
                    oreMap.SetTile(selectedTile, null);
                    ironCounter.IronCount += 1;
                }
            }
        }


        if (Input.GetMouseButtonDown(1))
        {
            groundMap.SetTile(selectedTile, buildingTile);
        }
    }

    void FixedUpdate()
    {
        if (inventoryManager.itemSelected == InventoryManager.Item.PICKAXE)
        {
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
