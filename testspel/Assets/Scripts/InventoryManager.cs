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
    Button button1;
    Button button2;
    Button button3;

    ColorBlock color1;
    ColorBlock color2;
    ColorBlock color3;

    IronCounter ironCounter;
    GameObject buyMenu;

    public enum Item {
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
        buyMenu.SetActive(false);
        ironCounter = text.GetComponent<IronCounter>();
    }

    void Awake() {
        button1 = slotObject1.GetComponent<Button>();
        button2 = slotObject2.GetComponent<Button>();
        button3 = slotObject3.GetComponent<Button>();

        color1 = slotObject1.GetComponent<Button>().colors;
        color2 = slotObject2.GetComponent<Button>().colors;
        color3 = slotObject3.GetComponent<Button>().colors;
    }

    // Update is called once per frame
    void Update()
    {
        if(itemSelected == Item.DRILL) {

            color2.normalColor = Color.blue;
            color1.normalColor = Color.white;
            color3.normalColor = dynamiteAvailable ? Color.white : Color.gray;
        } else if (itemSelected == Item.DYNAMITE) {
            color3.normalColor = Color.blue;
            color1.normalColor = Color.white;
            color2.normalColor = Color.white;
        } else {
            color1.normalColor = Color.blue;
            color2.normalColor = drillAvailable ? Color.white : Color.gray;
            color3.normalColor = dynamiteAvailable ? Color.white : Color.gray;
        }
        button1.colors = color1;
        button2.colors = color2;
        button3.colors = color3;

        if(Input.GetKeyDown(KeyCode.Alpha1)) {
            itemSelected = Item.PICKAXE;
        }
        if(Input.GetKeyDown(KeyCode.Alpha2)) {
            if(drillAvailable) {
                itemSelected = Item.DRILL;
            } else {
                if(ironCounter.IronCount >= 10) {
                    buyMenu.SetActive(true);
                }
            }
        }
        if(Input.GetKeyDown(KeyCode.Alpha3) && dynamiteAvailable) {
            itemSelected = Item.DYNAMITE;
        }
    }
}
