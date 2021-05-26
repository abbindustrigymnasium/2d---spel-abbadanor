using UnityEngine;
using UnityEngine.UI;

public class InventoryManager : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject slotObject1;
    public GameObject slotObject2;
    public GameObject slotObject3;

    public bool drillAvailable;
    public bool dynamiteAvailable;
    Toggle button1;
    Toggle button2;
    Toggle button3;

    ColorBlock color1;
    ColorBlock color2;
    ColorBlock color3;

    IronCounter ironCounter;
    GameObject buyMenu;

    public enum Item
    {
        PICKAXE,
        DRILL,
        DYNAMITE
    }
    public Item itemSelected;
    void Start()
    {
        itemSelected = Item.PICKAXE;
        drillAvailable = false;
        dynamiteAvailable = false;

        GameObject text = GameObject.Find("Text");
        buyMenu = GameObject.Find("BuyMenu");
        //buyMenu.SetActive(false);
        ironCounter = text.GetComponent<IronCounter>();

        button1 = slotObject1.GetComponent<Toggle>();
        button2 = slotObject2.GetComponent<Toggle>();
        button3 = slotObject3.GetComponent<Toggle>();

        color1 = button1.colors;
        color2 = button2.colors;
        color3 = button3.colors;
    }

    // Update is called once per frame
    void Update()
    {
        if (itemSelected == Item.DRILL)
        {

            color2.normalColor = Color.blue;
            color1.normalColor = Color.white;
            color3.normalColor = dynamiteAvailable ? Color.white : Color.gray;
        }
        else if (itemSelected == Item.DYNAMITE)
        {
            color3.normalColor = Color.blue;
            color1.normalColor = Color.white;
            color2.normalColor = Color.white;
        }
        else
        {
            color1.normalColor = Color.blue;
            color2.normalColor = drillAvailable ? Color.white : Color.gray;
            color3.normalColor = dynamiteAvailable ? Color.white : Color.gray;
        }
        button1.colors = color1;
        button2.colors = color2;
        button3.colors = color3;

        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            itemSelected = Item.PICKAXE;
        }
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            if (drillAvailable)
            {
                itemSelected = Item.DRILL;
            }
            else if (ironCounter.IronCount >= 10)
            {
                drillAvailable = true;
                ironCounter.IronCount -= 10;
                itemSelected = Item.DRILL;
            }
        }
        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            if (dynamiteAvailable)
            {
                itemSelected = Item.DYNAMITE;
            }
            else if (ironCounter.IronCount >= 20)
            {
                dynamiteAvailable = true;
                ironCounter.IronCount -= 20;
                itemSelected = Item.DYNAMITE;
            }
        }
    }
}
